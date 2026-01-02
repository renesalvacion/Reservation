<template>
  <div class="mt-3 p-4 pt-12">
    <h2 class="text-2xl font-bold mb-4 p-3 text-center">Room Catalog</h2>

    <div v-if="rooms.length === 0" class="text-center text-gray-500">
      No rooms available.
    </div>

    <!-- Centered Responsive Grid -->
<!-- Centered Responsive Grid -->
<div
  v-else
  class="grid gap-2 justify-center mx-auto p-10"
  style="grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));"
>
  <div
    v-for="(room, idx) in rooms"
    :key="room.room.roomId"
    class="bg-white border border-gray-200 rounded-xl shadow-md 
           hover:shadow-xl hover:scale-[1.02] transition 
           cursor-pointer flex flex-col mx-1"
  >
    <!-- Image -->
    <div class="w-full aspect-[3/2] overflow-hidden rounded-t-xl">
      <img
        v-if="room.room.roomImage"
        :src="`http://localhost/backend/RoomImage/${room.room.roomImage}`"
        alt="Room Image"
        class="w-full h-full object-cover"
        loading="lazy"
      />
      <div
        v-else
        class="w-full h-full bg-gray-200 flex items-center justify-center text-gray-500"
      >
        No Image Available
      </div>
    </div>

    <!-- Content -->
    <div class="p-3 flex flex-col flex-grow">

      <!-- Title -->
      <h3 class="font-bold text-base sm:text-lg mb-1 truncate text-center">
        {{ room.room.name }}
      </h3>

      <!-- Description -->
      <p class="text-gray-600 text-sm sm:text-base mb-2 line-clamp-2 text-center">
        {{ room.room.description }}
      </p>

      <!-- Details -->
      <div class="text-sm sm:text-base space-y-1 mb-3 text-center">
        <p><span class="font-medium">Price:</span> ${{ room.room.price }}</p>
        <p><span class="font-medium">Max Occupancy:</span> {{ room.room.maxOccupancy }}</p>
        <p><span class="font-medium">Status:</span> {{ room.room.roomStatus }}</p>
      </div>

      <!-- Buttons -->
      <div class="flex flex-row justify-center gap-1 flex-wrap">
        <button
          v-if="session?.role !== 'Admin'"
          @click="addReservation(idx)"
          class="cursor-pointer bg-green-500 hover:bg-green-600 text-white 
                 text-xs sm:text-sm font-medium py-1 px-2 rounded-md shadow"
        >
          Add Reservation
        </button>

        <button
          @click="viewRoomDetails(room.room.roomId)"
          class="cursor-pointer bg-blue-600 hover:bg-blue-700 text-white 
                 text-xs sm:text-sm font-medium py-1 px-2 rounded-md shadow"
        >
          Check Details
        </button>

        <button
          v-if="session?.role === 'Admin'"
          @click="adminEditRoomModal(room.room.roomId)"
          class="cursor-pointer bg-yellow-500 hover:bg-yellow-600 text-white 
                 text-xs sm:text-sm font-medium py-1 px-2 rounded-md shadow"
        >
          Update Details
        </button>
      </div>

    </div>
  </div>
</div>


    <!-- Pagination -->
    <div class="flex justify-center mt-6 space-x-2 flex-wrap">
      <button
        @click="prevPage"
        :disabled="pageNumber === 1"
        class="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
      >
        Prev
      </button>

      <button
        v-for="page in totalPages"
        :key="page"
        @click="goToPage(page)"
        :class="[ 'px-4 py-2 rounded hover:bg-gray-400', pageNumber === page ? 'bg-blue-600 text-white' : 'bg-gray-300']"
      >
        {{ page }}
      </button>

      <button
        @click="nextPage"
        :disabled="pageNumber === totalPages"
        class="px-4 py-2 bg-gray-300 rounded hover:bg-gray-400 disabled:opacity-50"
      >
        Next
      </button>
    </div>
  </div>

  <appointment_modal v-if="isOpen" @close="isOpen = false"/>
  <adminRoomEditModal v-if="isadminRoomOpen" @close="isadminRoomOpen = false" :room-id="appId"/>
</template>


<script setup lang="ts">
import { ref, onMounted, watch, computed } from "vue";
import { useRouter } from "vue-router";
import { useRoomCatalogStore } from "~/stores/room_catalog";
import appointment_modal from "./appointment_modal.vue";
import adminRoomEditModal from "./admin-room-edit-modal.vue";

interface SessionType {
  role?: string;
}

interface Room {
  roomId: number;
  name: string;
  description: string;
  price: number;
  maxOccupancy: number;
  roomStatus: string;
  roomImage?: string;
}

interface RoomWrapper {
  room: Room;
}

const runtime = useRuntimeConfig()
const localhost = runtime.public.backend




const roomCatalogStore = useRoomCatalogStore();
const rooms = ref<RoomWrapper[]>([]);

const pageNumber = ref(1);
const pageSize = ref(12);
const totalCount = ref(0);
const router = useRouter();

const isadminRoomOpen = ref(false);
const isOpen = ref(false);
const appId = ref(0);

const selectedRoom = ref(null);
const store = SetAppointment();

const session = ref<SessionType | null>(null);
const sessionStore = useSessionStore();

const totalPages = computed(() =>
  Math.ceil(totalCount.value / pageSize.value)
);

const fetchRooms = async () => {
  const result = await roomCatalogStore.GetCatalog(
    pageNumber.value,
    pageSize.value
  );

  rooms.value = roomCatalogStore.catalog.rooms || [];
  totalCount.value = roomCatalogStore.catalog.totalCount || rooms.value.length;
};

const adminEditRoomModal = (roomId: number) => {
  isadminRoomOpen.value = true;
  appId.value = roomId;
};

const addReservation = (idx: number) => {
  const selected = roomCatalogStore.catalog.rooms[idx].room;
  selectedRoom.value = selected;
  store.setSelectedRoom(selected);
  isOpen.value = true;
};

const nextPage = () => {
  if (pageNumber.value < totalPages.value) pageNumber.value++;
};

const prevPage = () => {
  if (pageNumber.value > 1) pageNumber.value--;
};

const goToPage = (page) => {
  pageNumber.value = page;
};

watch(pageNumber, () => {
  fetchRooms();
});

onMounted(async () => {
  fetchRooms();
  session.value = await sessionStore.getSession();
});

const viewRoomDetails = (roomId: number) => {
  router.push(`/room-catalog/${roomId}`);
};
</script>
