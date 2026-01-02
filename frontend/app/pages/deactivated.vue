<template>
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="bg-white shadow-lg rounded-lg p-8 max-w-md text-center">
      <h1 class="text-2xl font-bold text-gray-800 mb-4">Account Deactivated</h1>
      <p class="text-gray-600 mb-6">
        Your account is currently deactivated. Do you want to activate it?
      </p>

      <div class="flex justify-center gap-4">
        <!-- Activate Button -->
        <button
          @click="activateAccount"
          class="bg-green-600 hover:bg-green-700 text-white font-semibold py-2 px-4 rounded transition"
        >
          Activate
        </button>

        <!-- Back Button -->
        <button
          @click="goBack"
          class="bg-gray-300 hover:bg-gray-400 text-gray-800 font-semibold py-2 px-4 rounded transition"
        >
          Back
        </button>
      </div>
    </div>
  </div>

  <activateAccountModal v-if="isOpen" @close="isOpen = false"/>
</template>

<script setup lang="ts">
import { useRouter } from "vue-router";

import activateAccountModal from "~/components/activate-account-modal.vue";

const router = useRouter();

const isOpen = ref(false)


const activateAccount = () => {
  // Call your API to activate the account
  console.log("Activate account clicked");
  // Example: router.push('/activate') or call activation API

  isOpen.value = true
};

const goBack = () => {
  router.push("/dashboard"); // Go back to dashboard
};
</script>
