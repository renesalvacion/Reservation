<template>
  <div class="flex justify-center items-center min-h-screen bg-gray-100">
    <div class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm">
      <h1 class="text-2xl font-semibold text-center text-blue-500 mb-6">Register</h1>

      <!-- Error Message -->
      <div v-if="profileStore.error" class="text-red-600 text-sm mb-4">
        <p>Error: {{ profileStore.error }}</p>
      </div>

      <!-- Success Message -->
      <div v-if="profileStore.data" class="text-green-600 text-sm mb-4">
        <p>Profile created successfully!</p>
      </div>

      <form @submit.prevent="submitProfile" class="space-y-4">
        <!-- Name Input -->
        <div>
          <label for="name" class="block text-sm font-medium text-gray-700">Name:</label>
          <input
            type="text"
            v-model="formData.name"
            id="name"
            ref="nameInput"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter your name"
            required
          />
        </div>

        <!-- Email Input -->
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">Email:</label>
          <input
            type="email"
            v-model="formData.email"
            id="email"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter your email"
            required
          />
        </div>

        <!-- Password Input -->
        <div>
          <label for="password" class="block text-sm font-medium text-gray-700">Password:</label>
          <input
            type="password"
            v-model="formData.password"
            id="password"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            placeholder="Enter your password"
            required
          />
        </div>

        <!-- Profile Picture Input -->
        <div>
          <label for="profile" class="block text-sm font-medium text-gray-700">Profile Picture:</label>
          <input
            type="file"
            @change="onFileChange"
            id="profile"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
        </div>

        <!-- Submit Button -->
        <div>
          <button
            type="submit"
            class="w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600 focus:outline-none"
          >
            Signup
          </button>
        </div>

        <div class="flex items-center my-4">
          <hr class="flex-grow border-gray-300" />
          <span class="mx-2 text-gray-500">or</span>
          <hr class="flex-grow border-gray-300" />
        </div>

        <!-- Google Login Button -->
        <div class="mt-4">
          <GoogleLoginButton
            :verify-on-server="true"
            :options="{ theme: 'filled_blue', size: 'large', text: 'signup_with' }"
            @success="onSuccess"
            @verified="onVerified"
            @error="onError"
          />
        </div>
      </form>

      <!-- Footer / Login link -->
      <div class="text-center mt-4">
        <p class="text-sm text-gray-500">
          Already have an account?
          <NuxtLink to="/login" class="underline font-semibold">Log in here.</NuxtLink>
        </p>
      </div>
    </div>

    <loading :show="isLoading" />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, nextTick } from "vue";
import { useProfileStore } from "~/stores/profile";
import loading from "~/components/loading.vue";
import { navigateTo } from "#app"; // Nuxt navigation helper

// Form Data
const formData = ref({
  name: "",
  email: "",
  password: "",
  profile: null
});

// Input refs
const nameInput = ref<HTMLInputElement | null>(null);

// Store
const profileStore = useProfileStore();

// Loading state
const isLoading = ref(false);

// Auto-focus the Name input on page load
onMounted(async () => {
  await nextTick();
  nameInput.value?.focus();
});

// Handle file input
const onFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement;
  formData.value.profile = target.files?.[0] || null;
};

// Reset form
const resetForm = () => {
  formData.value.name = "";
  formData.value.email = "";
  formData.value.password = "";
  formData.value.profile = null;
};

// Submit form
const submitProfile = async () => {
  try {
    isLoading.value = true;
    await profileStore.postProfile({
      name: formData.value.name,
      email: formData.value.email,
      password: formData.value.password,
      profile: formData.value.profile
    });

    if (profileStore.error) {
      alert(`Error: ${profileStore.error}`);
    } else if (profileStore.data) {
      alert("Profile created successfully!");
      resetForm();
      await navigateTo("/login");
    }
  } catch (err) {
    console.error(err);
    alert("An error occurred. Please try again.");
  } finally {
    isLoading.value = false;
  }
};

// Google Login handlers
const onSuccess = (e: { credential: string; claims: any }) => {
  console.log("success:", e.claims, e.credential.slice(0, 20) + "…");

  fetch("http://localhost:5080/api/Profile/User-SignIn-Google", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ IdToken: e.credential })
  })
    .then((res) => res.json())
    .then((data) => onVerified(data))
    .catch((err) => console.error("Error sending to backend:", err));
};

const onVerified = (data: any) => console.log("verified:", data);
const onError = (err: any) => console.error("Google login error:", err);
</script>

<style scoped>
.error {
  color: red;
}
</style>
