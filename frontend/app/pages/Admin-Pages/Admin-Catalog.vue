<template>
  <div class="flex min-h-screen bg-gray-100">

    <!-- Sidebar -->
    <sidebarAdmin :open="sidebarOpen" @toggle="sidebarOpen = !sidebarOpen" />

    <!-- Main Content Wrapper -->
    <div
      class="flex-1 flex justify-center items-start p-4 sm:p-6 md:p-10 transition-all duration-300"
      :class="sidebarOpen ? 'ml-64' : 'ml-20'"
    >

      <!-- Main Content -->
      <main class="w-full max-w-6xl flex flex-col items-center space-y-10" >

        <!-- Toggle Button -->
        <div class="w-full flex justify-center">
          <button
            @click="showForm = !showForm"
            class="cursor-pointer bg-blue-600 text-white px-6 py-2 rounded-md
                   hover:bg-blue-700 transition flex items-center gap-2"
          >
            {{ showForm ? 'Hide Form' : 'Add New Room' }}
            <span
              class="transition-transform duration-300"
              :class="showForm ? 'rotate-180' : ''"
            >▼</span>
          </button>
        </div>

        <!-- Animated Form -->
        <transition
          enter-active-class="transition-all duration-300 ease-out"
          enter-from-class="opacity-0 max-h-0"
          enter-to-class="opacity-100 max-h-[2000px]"
          leave-active-class="transition-all duration-300 ease-in"
          leave-from-class="opacity-100 max-h-[2000px]"
          leave-to-class="opacity-0 max-h-0"
        >
          <div
            v-show="showForm"
            class="overflow-hidden w-full bg-white p-6 sm:p-10
                   rounded-lg shadow-md max-w-4xl mx-auto"
          >
            <h2 class="text-2xl font-bold mb-6 text-center text-gray-800">
              Add New Room
            </h2>

            <form
              @submit.prevent="submitRoomCatalog"
              class="grid grid-cols-1 md:grid-cols-3 gap-6"
            >

              <!-- Room Name -->
              <div>
                <label class="block mb-1 font-medium text-gray-700">Room Name</label>
                <input
                  type="text"
                  v-model="formData.name"
                  class="w-full border p-2 rounded-md focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>

              <!-- Room Number -->
              <div>
                <label class="block mb-1 font-medium text-gray-700">Room Number</label>
                <input
                  type="text"
                  v-model="formData.roomnumber"
                  class="w-full border p-2 rounded-md focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>

              <!-- Price -->
              <div>
                <label class="block mb-1 font-medium text-gray-700">Price</label>
                <input
                  type="number"
                  v-model="formData.price"
                  class="w-full border p-2 rounded-md focus:ring-2 focus:ring-blue-500"
                  min="1"
                  required
                />
              </div>

              <!-- Description -->
              <div class="md:col-span-3">
                <label class="block mb-1 font-medium text-gray-700">Description</label>
                <textarea
                  v-model="formData.description"
                  rows="3"
                  class="w-full border p-2 rounded-md focus:ring-2 focus:ring-blue-500"
                  required
                ></textarea>
              </div>

              <!-- Room Type -->
              <div>
                <label class="block mb-1 font-medium text-gray-700">Room Type</label>
                <input
                  type="text"
                  v-model="formData.roomType"
                  class="w-full border p-2 rounded-md focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>

              <!-- Max Occupancy -->
              <div>
                <label class="block mb-1 font-medium text-gray-700">Max Occupancy</label>
                <input
                  type="number"
                  v-model="formData.maxoccupancy"
                  class="w-full border p-2 rounded-md focus:ring-2 focus:ring-blue-500"
                  min="1"
                  max="7"
                  required
                />
              </div>

              <!-- Room Status -->
              <div>
                <label class="block mb-1 font-medium text-gray-700">Room Status</label>
                <select
                  v-model="formData.roomstatus"
                  class="w-full border p-2 rounded-md focus:ring-2 focus:ring-blue-500"
                  required
                >
                  <option value="">Select status</option>
                  <option value="Available">Available</option>
                  <option value="Booked">Booked</option>
                  <option value="Maintenance">Maintenance</option>
                </select>
              </div>

              <!-- Image -->
              <div>
                <label class="block mb-1 font-medium text-gray-700">Room Image</label>
                <input type="file" @change="onFileChange" class="w-full border p-1 rounded-md"/>
              </div>

              <!-- Submit -->
              <div class="md:col-span-3">
                <button
                  type="submit"
                  class="w-full bg-blue-600 text-white p-3 rounded-md hover:bg-blue-700 transition"
                >
                  Add Room
                </button>
              </div>

            </form>
          </div>
        </transition>

<div

  class="grid gap-2 justify-center mx-auto p-5"
  style="grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));"
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

      </main>
    </div>

    <Loading :show="isLoading"/>
  </div>

  <adminRoomEditModal v-if="isRoomEditModal" @close="isRoomEditModal = false"/>
</template>

<script setup lang="ts">
import sidebarAdmin from '~/components/sidebar-admin.vue';
import Loading from '~/components/loading.vue';
import { ref, onMounted, watch, computed } from "vue";
import { useRouter } from "vue-router";
import { useRoomCatalogStore } from '~/stores/room_catalog';
import { useSessionStore } from '#imports';
import adminRoomEditModal from '~/components/admin-room-edit-modal.vue';

interface SessionType { role?: string; }
interface Room { roomId: number; name: string; description: string; price: number; maxOccupancy: number; roomStatus: string; roomImage?: string; }
interface RoomWrapper { room: Room; }

const rooms = ref<RoomWrapper[]>([]);
const pageNumber = ref(1);
const pageSize = ref(12);
const totalCount = ref(0);
const isLoading = ref(false);
const sidebarOpen = ref(true);

const router = useRouter();
const roomCatalogStore = useRoomCatalogStore();
const sessionStore = useSessionStore();
const session = ref<SessionType | null>(null);

  const showForm = ref(false)

const isRoomEditModal = ref(false) 

const totalPages = computed(() => Math.ceil(totalCount.value / pageSize.value));

const fetchRooms = async () => {
  const result = await roomCatalogStore.GetCatalog(pageNumber.value, pageSize.value);
  rooms.value = roomCatalogStore.catalog.rooms || [];
  totalCount.value = roomCatalogStore.catalog.totalCount || rooms.value.length;
};

const nextPage = () => { if(pageNumber.value < totalPages.value) pageNumber.value++; };
const prevPage = () => { if(pageNumber.value > 1) pageNumber.value--; };
const goToPage = (page) => { pageNumber.value = page; };

const adminEditRoomModal = (roomId: number) => { 
  isRoomEditModal.value = true
 };
const addReservation = (idx: number) => { /* Your reservation logic */ };
const viewRoomDetails = (roomId: number) => { router.push(`/room-catalog/${roomId}`); };

watch(pageNumber, fetchRooms);

onMounted(async () => {
  await fetchRooms();
  session.value = await sessionStore.getSession();
});

const formData = ref({
  name: "", description: "", price: "", maxoccupancy: "", roomnumber: "",
  roomstatus: "", roomType: "", roomimage: null,
});

const onFileChange = (event) => { formData.value.roomimage = event.target.files[0]; };

const submitRoomCatalog = async () => {
  try {
    isLoading.value = true;
    const payload = new FormData();
    Object.entries(formData.value).forEach(([key, value]) => { if(value) payload.append(key, value as any); });
    await roomCatalogStore.CreateCatalog(payload);
    showForm.value = false
    Object.keys(formData.value).forEach(key => formData.value[key] = key === 'roomimage' ? null : '');
    isLoading.value = false;
    fetchRooms();
  } catch (error) { console.error(error); isLoading.value = false; }
};
</script>
