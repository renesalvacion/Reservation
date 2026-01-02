<template>
  <div class="pt-4 px-6">
    <h1 class="font-bold text-2xl mb-4">Available Reservation:</h1>

    <div class="cards pt-2 p-10 relative">
      <!-- Swiper -->
      <swiper
        ref="swiperRef"
        :slides-per-view="1"
        :loop="true"
        class="w-full"
      >
        <swiper-slide v-for="(slideSet, index) in slides" :key="index">
          <div class="flex justify-center gap-6 w-full">
            <!-- Card -->
            <div
              v-for="roomWrapper in slideSet"
              :key="roomWrapper.room.roomId"
              class="bg-gray-50 shadow-lg rounded-lg w-72 flex flex-col items-center p-4"
            >
              <img
                v-if="roomWrapper.room.roomImage"
                :src="`http://localhost/backend/RoomImage/${roomWrapper.room.roomImage}`"
                :alt="roomWrapper.room.name"
                class="w-full h-40 object-cover rounded-md"
              />
              <div class="mt-2 text-center flex-1 flex flex-col justify-between w-full">
                <h3 class="font-semibold text-lg truncate">{{ roomWrapper.room.name }}</h3>
                <p class="text-gray-600 text-sm line-clamp-2">{{ roomWrapper.room.description }}</p>
                <p class="text-gray-800 font-medium text-sm mt-1">Price: ${{ roomWrapper.room.price }}</p>

                <div class="mt-3 flex flex-col gap-2">
                  <button
                    @click="reserveRoom(roomWrapper.room.roomId)"
                    class="bg-green-500 hover:bg-green-600 text-white font-medium py-1 px-2 rounded"
                  >
                    Reserve
                  </button>
                  <button
                    @click="checkRoomDetails(roomWrapper.room.roomId)"
                    class="bg-blue-600 hover:bg-blue-700 text-white font-medium py-1 px-2 rounded"
                  >
                    Details
                  </button>
                </div>
              </div>
            </div>
          </div>
        </swiper-slide>
      </swiper>

      <!-- Navigation Arrows -->
      <button
        @click="goPrev"
        class="absolute left-0 top-1/2 transform -translate-y-1/2 p-2 bg-white rounded-full shadow-lg z-10"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor">
          <path d="M15.41 7.41L14 6l-6 6 6 6 1.41-1.41L10.83 12z"/>
        </svg>
      </button>

      <button
        @click="goNext"
        class="absolute right-0 top-1/2 transform -translate-y-1/2 p-2 bg-white rounded-full shadow-lg z-10"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor">
          <path d="M8.59 16.59L13.17 12 8.59 7.41 10 6l6 6-6 6z"/>
        </svg>
      </button>
    </div>

    <modal v-if="isOpen" @close="isOpen = false" />
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import { Swiper, SwiperSlide } from "swiper/vue";
import "swiper/css";

import modal from "./modal.vue";
import { useRoomCatalogStore } from "#imports";

const roomCatalogStore = useRoomCatalogStore();
const isOpen = ref(false);
const swiperRef = ref(null);

// Arrow navigation
const goPrev = () => {
  swiperRef.value?.swiper?.slidePrev();
};

const goNext = () => {
  swiperRef.value?.swiper?.slideNext();
};

// Room actions
const reserveRoom = (roomId) => {
  isOpen.value = true;
};

const checkRoomDetails = (roomId) => {
  console.log("View details for room:", roomId);
};

// Pagination / rooms
const pageNumber = ref(1);
const pageSize = ref(12);
const availableRooms = computed(() =>
  roomCatalogStore.catalog.rooms.filter(r => r.room.roomStatus === "Available")
);

const slides = computed(() => {
  const chunkSize = 3;
  const result = [];
  for (let i = 0; i < availableRooms.value.length; i += chunkSize) {
    result.push(availableRooms.value.slice(i, i + chunkSize));
  }
  return result;
});

onMounted(async () => {
  await roomCatalogStore.GetCatalog(pageNumber.value, pageSize.value);
  // Optional: confirm swiper instance
  console.log("Swiper instance:", swiperRef.value?.swiper);
});
</script>
