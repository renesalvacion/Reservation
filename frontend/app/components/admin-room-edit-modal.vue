<template>
  <!-- Modal Overlay and Login Card -->
  <div
    v-if="isModalOpen"
    class="fixed inset-0 bg-opacity-50 flex justify-center items-center z-50"
    style="background-color: rgba(0, 0, 0, 0.5)"
  >

    <!-- Modal content -->
    <div
      class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm relative z-60"
    >
      <button
        @click="closeModal()"
        class="cursor-pointer absolute top-4 right-4 text-gray-500 hover:text-gray-700 focus:outline-none"
      >
        <span class="material-icons">close</span>
      </button>
      <h1 class="text-center font-semibold">Update Room Details</h1>


    <form @submit.prevent="submitUpdateRoomCatalog()"
      
      class="pt-5 grid grid-cols-1 md:grid-cols-2 gap-6 "
    >
      <!-- Room Name -->
      <div>
        <label for="name" class="block mb-1 font-medium text-gray-700"
          >Room Name</label
        >
<!-- Room Name -->
<input
  type="text"
  id="name"
  name="name"
  placeholder="Enter room name"
  v-model="formData.name"
  required
  class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
/>
      </div>

      <!-- Room Number -->
      <div>
        <label for="roomnumber" class="block mb-1 font-medium text-gray-700"
          >Room Number</label
        >
<!-- Room Number -->
<input
  type="text"
  id="roomnumber"
  name="roomnumber"
  placeholder="Enter room number"
  v-model="formData.roomnumber"
  required
  min="1"
  class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
/>

      </div>

      <!-- Description -->
      <div class="md:col-span-2">
        <label for="description" class="block mb-1 font-medium text-gray-700"
          >Description</label
        >
<!-- Description -->
<textarea
  id="description"
  name="description"
  rows="2"
  placeholder="Enter room description"
  v-model="formData.description"
  required
  class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
></textarea>
      </div>

      <!-- Price -->
      <div>
        <label for="price" class="block mb-1 font-medium text-gray-700"
          >Price</label
        >
<!-- Price -->
<input
  type="number"
  id="price"
  name="price"
  placeholder="Enter price"
  v-model="formData.price"
  min="1"
  required
  class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
/>
      </div>

      <!-- Room Type -->
      <div>
        <label for="price" class="block mb-1 font-medium text-gray-700"
          >Room Type</label
        >
<!-- Room Type -->
<input
  type="text"
  id="roomType"
  name="roomType"
  placeholder="Enter Room Type"
  v-model="formData.roomType"
  required
  class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
/>
      </div>

      <!-- Max Occupancy -->
      <div>
        <label for="maxoccupancy" class="block mb-1 font-medium text-gray-700"
          >Max Occupancy</label
        >
<!-- Max Occupancy -->
<input
  type="number"
  id="maxoccupancy"
  name="maxoccupancy"
  placeholder="Max guests"
  v-model="formData.maxoccupancy"
  min="1"
  required
  class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
/>
      </div>

      <!-- Room Status -->
      <div>
        <label for="roomstatus" class="block mb-1 font-medium text-gray-700"
          >Room Status</label
        >
<!-- Room Status -->
<select
  id="roomstatus"
  name="roomstatus"
  v-model="formData.roomstatus"
  required
  class="cursor-pointer w-full border border-gray-300 p-2 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
>
  <option value="">Select status</option>
  <option value="Available">Available</option>
  <option value="Booked">Booked</option>
  <option value="Maintenance">Maintenance</option>
</select>
      </div>

      <!-- Room Image -->
      <div>
        <label for="roomimage" class="block mb-1 font-medium text-gray-700"
          >Room Image</label
        >
<!-- Room Image -->
<input
  type="file"
  id="roomimage"
  name="roomimage"
  @change="onFileChange"
  class="cursor-pointer w-full border border-gray-300 p-1 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
/>
      </div>

      <!-- Submit Button (span full width) -->
      <div class="md:col-span-2 pt-">
        <button
          type="submit"
          class="w-full bg-yellow-600 text-white p-2 rounded-md hover:bg-yellow-700 transition duration-200"
        >
          Update Room
        </button>

        <h1>{{ localRoomId }}</h1>
      </div>
    </form>
    </div>
  </div>

  <loading/>
</template>

<script setup>
import { ref, defineProps, onMounted } from "vue";
import { useRoomCatalogStore } from "#imports";
import loading from "./loading.vue";
// Define emit
const emit = defineEmits(["close", "update:loading"]);
// Track modal visibility state
const isModalOpen = ref(true);

const props = defineProps({
  loading: Boolean,
  show: Boolean,
  roomId: Number,
});

const localRoomId = ref(props.roomId)

const isLoading = ref(false)

const roomCatalogStore = useRoomCatalogStore()

// Create a reference for the form data
const formData = ref({
  name: "",
  description: "",
  price: "",
  maxoccupancy: "",
  roomnumber: "",
  roomstatus: "",
  roomType: "",
  roomimage: null, // This will hold the file
});

// Handle file input change
const onFileChange = (event) => {
  formData.value.roomimage = event.target.files[0];
};

const submitUpdateRoomCatalog = async () => {
  try {
    closeModal()
    isLoading.value = true
    const payload = new FormData();
    payload.append("name", formData.value.name);
    payload.append("description", formData.value.description);
    payload.append("price", formData.value.price);
    payload.append("maxOccupancy", formData.value.maxoccupancy);
    payload.append("roomNumber", formData.value.roomnumber);
    payload.append("roomStatus", formData.value.roomstatus);
    payload.append("roomType", formData.value.roomType)
    if (formData.value.roomimage) {
      payload.append("roomImage", formData.value.roomimage);
    }

    const response = await roomCatalogStore.AdminUpdateRoom(localRoomId.value, payload);
    console.log("Room catalog response:", response);
    alert("Successfully update the room.")
    // Optional: reset form
    formData.value = {
      name: "",
      description: "",
      price: "",
      maxoccupancy: "",
      roomnumber: "",
      roomstatus: "",
      roomimage: null,
    };
    

    isLoading.value = false
  } catch (error) {
    console.error("Error adding room:", error);
    alert("Error adding room:", error)
  }
};


// Close modal function
const closeModal = () => {
  emit("close");
};
</script>
