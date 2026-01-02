te<template>
  <!-- Modal Overlay and Login Card -->
  <div
    v-if="isModalOpen"
    class="fixed inset-0 bg-opacity-50 flex justify-center items-center z-50"
    style="background-color: rgba(0, 0, 0, 0.5)"
  >
    <h1>Edit Modal</h1>
    <!-- Modal content -->
    <div
      class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm relative z-60"
    >
      <button
        @click="closeModal"
        class="cursor-pointer absolute top-4 right-4 text-gray-500 hover:text-gray-700 focus:outline-none"
      >
        <span class="material-icons">close</span>
      </button>
      <h1 class="text-center font-semibold">Edit Reservation?</h1>

      <form @submit.prevent="onSubmit" class="space-y-4 bg-white p-6 rounded-lg shadow">

        <!-- Room Type -->
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">
            Name
          </label>
          <input
            type="text"
            v-model="formData.name"
            name="name"
            placeholder="Enter your name"
            class="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring focus:ring-indigo-200"
          />
        </div>

        <!-- Room Type -->
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">
            Profile
          </label>
          <input
            type="file"
            @change="handleFileUpload"
            name="profile"
            placeholder="Enter your profile"
            class="w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring focus:ring-indigo-200"
          />
        </div>


        <!-- Submit Button -->
        <button
          type="submit"
          class="w-full bg-indigo-600 text-white py-2 rounded-md hover:bg-indigo-700 transition"
        >
          Update Reservation
        </button>

      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, onMounted, watch } from "vue";
import { useProfileStore } from "#imports";
import { useSessionStore } from "#imports";
// Define emit
const emit = defineEmits(["close", "update:loading"]);

const profile = useProfileStore();
const sessionStore = useSessionStore();

const session = ref(null);
// Track modal visibility state
const isModalOpen = ref(true);

const props = defineProps({
  loading: Boolean,
  show: Boolean,
  appointmentId: Number,
});

// Local reactive copy of appointmentId
const appId = ref(props.appointmentId ?? 0);


const userId = ref(0)
// Directly use props.appointmentId
console.log(props.appointmentId); // Access appointmentId directly

const formData = ref({name : '', profile : null})




const handleFileUpload = (event) => {
    formData.value.profile = event.target.files[0];
}

const onSubmit = async() => {
    const payload = {
        name : formData.value.name,
        profile : formData.value.profile
    }
    await profile.UserUpdateAccount(session.value.userId, payload)
}



onMounted(async () => {
  session.value = await sessionStore.getSession();
  userId.value = session.value.userId
  // You can check if the session is loaded
  console.log("Session Loaded:", session.value);
});

// Close modal function
const closeModal = () => {
  emit("close");
};
</script>
