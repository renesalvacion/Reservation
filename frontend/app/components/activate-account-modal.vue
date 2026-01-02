<template>
  <!-- Modal Overlay -->
  <div
    class="fixed inset-0 bg-opacity-50 flex justify-center items-center z-50"
    style="background-color: rgba(0, 0, 0, 0.5)"
  >
    <div
      class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm relative"
    >
      <button
        @click="closeModal"
        class="cursor-pointer absolute top-4 right-4 text-gray-500 hover:text-gray-700 focus:outline-none"
      >
        <span class="material-icons">close</span>
      </button>

      <h1 class="text-center font-semibold pt-4">Are You Sure You Want To Activate Your Profile?</h1>

      <form @submit.prevent="submitData">
            <input type="text" name="isActive" v-model="formData.isActive" id="">
        <div class="btn flex justify-around pt-10">
          <button
            type="submit"
            class="cursor-pointer bg-yellow-500 text-white px-4 py-2 rounded hover:bg-yellow-600 transition"
          >
            Activate Profile
          </button>
          <button
            type="button"
            @click="closeModal"
            class="cursor-pointer bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition"
          >
            Close
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps, watch } from "vue";
import { useRouter } from "vue-router";
import { useProfileStore } from "#imports";
import { useSessionStore } from "#imports";
import session from "~/middleware/session";
import { tr } from "@nuxt/ui/runtime/locale/index.js";

const profile = useProfileStore();
const sessionStore = useSessionStore();
const router = useRouter()
const emit = defineEmits(["close"]);

const sessions = ref(null)
const userId = ref(0)
const formData = ref({
    isActive : true
})

// Submit function
const submitData = async () => {
    console.log("Submitting for userId:", userId.value);

    if (!userId.value) {
        alert("User session not loaded!");
        return;
    }

    const response = await profile.ActivateUser(userId.value, formData.value);
    if(response) router.push('/dashboard')
};


onMounted(async () => {
  sessions.value = await sessionStore.getSession();
  userId.value = sessions.value.userId

  console.log("Onmount Activate Modal: " + userId.value);
  
  // You can check if the session is loaded
  console.log("Session Loaded:", sessions.value);
});


// Close modal
const closeModal = () => {
  emit("close");
};
</script>
