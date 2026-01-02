<template>
  <div class="grid grid-cols-1 gap-4 w-full p-3">
    <h3 class="text-3xl font-semibold">Available Rooms:</h3>

    <!-- 🔄 LOADING STATE -->
    <div v-if="isLoading" class="grid gap-4">
      <div
        v-for="i in 3"
        :key="i"
        class="animate-pulse flex flex-col lg:flex-row gap-4 p-4 border rounded-lg shadow"
      >
        <div class="bg-gray-300 h-60 w-full lg:w-96 rounded"></div>

        <div class="flex flex-col gap-3 w-full">
          <div class="h-6 bg-gray-300 rounded w-1/2"></div>
          <div class="h-6 bg-gray-300 rounded w-1/3"></div>
          <div class="h-4 bg-gray-300 rounded w-2/3"></div>
          <div class="h-4 bg-gray-300 rounded w-full"></div>
          <div class="h-10 bg-gray-300 rounded w-40 mt-2"></div>
        </div>
      </div>
    </div>

    <!-- 🚫 NO DATA -->
    <div
      v-else-if="!roomCatalogStore.catalog.rooms.length"
      class="text-center text-gray-500 py-10"
    >
      No rooms available.
    </div>

    <!-- ✅ ROOM LIST -->
    <div
      v-else
      class="card border-t-gray-50 rounded-lg shadow-xxl shadow-md overflow-hidden"
      v-for="(hotel, idx) in roomCatalogStore.catalog.rooms"
      :key="hotel?.roomId || idx"
    >
      <div class="flex flex-col lg:flex-row">

        <!-- Image -->
        <img
          :src="`http://localhost/backend/RoomImage/${hotel.room.roomImage}`"
          alt="Hotel Image"
          class="w-full lg:w-96 h-60 object-cover shrink-0"
        />

        <!-- Text container -->
        <div class="flex flex-col justify-center p-4 gap-3 w-full">
          <h3 class="text-2xl font-semibold">Name: {{ hotel.room.name }}</h3>
          <h3 class="text-2xl font-semibold">Room Type: {{ hotel.room.roomType }}</h3>
          <h3 class="text-sm text-gray-400 font-semibold">
            Room Number: {{ hotel.room.roomNumber }}
          </h3>

          <p class="text-gray-500">
            Description:
            <span>
              {{
                expandedDescriptions[idx]
                  ? hotel.room.description
                  : hotel.room.description.slice(0, 120) +
                    (hotel.room.description.length > 120 ? "..." : "")
              }}
            </span>

            <button
              v-if="hotel.room.description.length > 120"
              @click="toggleDescription(idx)"
              class="text-blue-600 underline ml-2"
            >
              {{ expandedDescriptions[idx] ? "Show less" : "Show more" }}
            </button>
          </p>

          <p class="text-gray-500">
            Max Occupancy: {{ hotel.room.maxOccupancy }}
          </p>

          <button
            class="cursor-pointer bg-blue-600 hover:bg-blue-700 text-white font-bold py-2 rounded mt-2 w-full lg:w-40"
            @click="ViewDetails(hotel.room.roomId)"
          >
            Check Details
          </button>
        </div>

        <!-- Right section -->
        <div class="flex flex-row lg:flex-col justify-between items-center p-3 gap-2 w-full lg:w-auto">
          <div class="text-center">
            <p class="text-green-500">Excellent</p>
            <button class="bg-green-100 p-1 font-bold rounded mt-1">
              {{ hotel.averageStars.toFixed(1) }}
            </button>
          </div>

          <h5 class="text-2xl font-bold">${{ hotel.room.price }}</h5>

          <button
            @click="reserveRoom(idx)"
            class="cursor-pointer bg-blue-200 hover:bg-blue-400 font-bold rounded p-3 w-full lg:w-40"
          >
            Start Booking
          </button>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal -->
  <appointment_modal
    v-if="showModal"
    @close="closeModal"
    :room="selectedRoom"
  />
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoomCatalogStore } from '#imports'
import { SetAppointment } from '../stores/appointment'
import appointment_modal from './appointment_modal.vue'
import { useRouter } from 'vue-router'

const showModal = ref(false)
const selectedRoom = ref(null)
const expandedDescriptions = ref({})
const isLoading = ref(true)

const store = SetAppointment()
const roomCatalogStore = useRoomCatalogStore()
const router = useRouter()

const runtime = useRuntimeConfig()
const localhost = runtime.public.backend

function toggleDescription(index) {
  expandedDescriptions.value[index] = !expandedDescriptions.value[index]
}

onMounted(async () => {
  isLoading.value = true
  try {
    await roomCatalogStore.GetCatalog(1, 10)
  } finally {
    isLoading.value = false
  }
})

function reserveRoom(idx) {
  const selected = roomCatalogStore.catalog.rooms[idx].room
  selectedRoom.value = selected
  store.setSelectedRoom(selected)
  showModal.value = true
}

function closeModal() {
  showModal.value = false
}

function ViewDetails(roomId) {
  router.push(`/room-catalog/${roomId}`)
}
</script>
