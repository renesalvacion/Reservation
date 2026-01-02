<template>
  <Navbar />
  <div class="max-w-2xl mx-auto mt-10">
    <div
      v-if="profile"
      class="bg-white shadow-lg rounded-lg p-6 flex flex-col items-center"
    >
      <h1 class="text-xl font-bold p-3">Profile Information:</h1>
      <!-- Profile Image -->
      <img
        v-if="profile.profile"
        :src="`http://10.0.13.21:4000/profile/${profile.profile}`"
        alt="Profile Image"
        class="w-32 h-32 rounded-full object-cover mb-4"
      />

      <!-- Profile Info -->
      <h2 class="text-xl font-bold mb-2">{{ profile.name }}</h2>
      <p class="text-gray-600 mb-1">Email: {{ profile.email }}</p>
      <p class="text-gray-600 mb-1">Role: {{ profile.role || "N/A" }}</p>
      <p class="text-gray-500 text-sm mb-4">ID: {{ profile.id }}</p>

      <!-- Appointments (if any) -->
      <div
        v-if="profile.appointments && profile.appointments.length"
        class="w-full mt-4"
      >
        <h3 class="text-lg font-semibold mb-2">Appointments</h3>
        <ul class="divide-y divide-gray-200">
          <li v-for="appt in profile.appointments" :key="appt.id" class="py-2">
            <span class="font-medium">{{ appt.date }}</span> -
            {{ appt.description }}
          </li>
        </ul>
      </div>
      <div class="btn flex justify-around w-full mt-4 gap-4">
        <!-- Edit Button -->
        <button @click="accountUpdate()"
          class="bg-fuchsia-400 hover:bg-fuchsia-600 text-white font-semibold py-2 px-4 rounded shadow-md hover:shadow-lg transition duration-200 ease-in-out"
        >
          Edit
        </button>

        <!-- Deactivate Button -->
        <button @click="accountDeactivate()"
          class="bg-red-500 hover:bg-red-700 text-white font-semibold py-2 px-4 rounded shadow-md hover:shadow-lg transition duration-200 ease-in-out"
        >
          Deactivate
        </button>
      </div>
    </div>

    <!-- Fallback if no profile -->
    <div v-else class="text-center text-gray-500 mt-10">No profile found.</div>
  </div>

  <userUpdateAccountModal v-if="isOpen" @close="isOpen = false"/>
  <DeactivateUserProfile v-if="isDeactivate" @close="isDeactivate = false"/>
</template>

<script setup>
import { useSessionStore } from "#imports";
import { useProfileStore } from "#imports";
import { ref, onMounted } from "vue";
import NavBar from "~/components/navbar.vue";
import userUpdateAccountModal from "~/components/user-update-account-modal.vue";
import DeactivateUserProfile from "~/components/Deactivate-User-Profile.vue";

const sessionStore = useSessionStore();
const profileStore = useProfileStore();
const profile = ref(null);
const session = ref(null);

const isOpen = ref(false)
const isDeactivate = ref(false)

const accountUpdate = () => {
  isOpen.value = true
}

const accountDeactivate = () => {
  isDeactivate.value = true
}

onMounted(async () => {
  session.value = await sessionStore.getSession();

  if (session.value?.userId) {
    const result = await profileStore.ViewProfile(session.value.userId);
    profile.value = result.profile; // assign the profile object
    console.log("Profile:", profile.value);
  }
});
</script>
