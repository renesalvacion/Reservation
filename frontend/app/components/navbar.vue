<script setup lang="ts">
import { ref, onMounted, onUnmounted, computed } from "vue";
import register from "./register.vue";
import loadingSpinner from "./loading.vue";
import { useSessionStore, useProfileStore } from "#imports";
import { SetAppointment } from "#imports";
import { useRouter } from "vue-router";
import * as signalR from "@microsoft/signalr";
import { useNotifications } from "#imports";

/* ---------------- STATE ---------------- */

interface SessionType {
  email: string;
  role: string;
  name: string;
  userId: number;
}

interface NotificationItem {
  id: number;
  title: string;
  message: string;
  time: string;
  read: boolean;
}

const session = ref<SessionType | null>(null);
const notifCount = ref(0);
const notifications = ref<NotificationItem[]>([]);
const notifOpen = ref(false);

const isOpen = ref(false);
const loading = ref(false);
const mobileMenu = ref(false);

let connection: signalR.HubConnection | null = null;

/* ---------------- STORES ---------------- */

const sessionStore = useSessionStore();
const profileStore = useProfileStore();
const notificationStore = useNotifications();
const notif = SetAppointment();
const router = useRouter();

/* ---------------- COMPUTED ---------------- */

const sessionName = computed(() =>
  session.value?.email ? session.value.email.split("@")[0] : ""
);

/* ---------------- HELPERS ---------------- */

const mapNotifications = (arr: any[]) =>
  arr.map((n: any) => ({
    id: n.id,
    title: n.title,
    message: n.message,
    read: n.isRead,
    time: new Date(n.createdAt).toLocaleString(),
  }));

/* ---------------- LIFECYCLE ---------------- */

onMounted(async () => {
  const token = localStorage.getItem("token");
  if (!token) return router.push("/login");

  const s = await sessionStore.getSession();
  if (!s?.userId) return;

  session.value = s;

  // Initial fetch
  const result = await notif.getCountNotif(s.userId);
  notifCount.value = result.countNotif;
  notifications.value = mapNotifications(result.notifArry);

  // SignalR setup
connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5080/hubs/notifications", { accessTokenFactory: () => token })
    .withAutomaticReconnect()
    .build();


connection.on("ReceiveNotification", async (data) => {
  console.log("📡 SIGNALR RECEIVED:", data)

  const result = await notif.getCountNotif(session.value!.userId)
  console.log("📦 COUNT FROM API:", result.countNotif)

  notifCount.value = result.countNotif
  notifications.value = mapNotifications(result.notifArry)

    // 🔥 LIVE update
  notifCount.value++
})


});

onUnmounted(() => {
  if (connection) {
    connection.stop();
    connection = null;
  }
});

/* ---------------- ACTIONS ---------------- */

const logout = async () => {
  const success = await profileStore.Logout();
  if (!success) return alert("Logout failed");

  sessionStore.session = null;
  session.value = null;
  localStorage.removeItem("token");
  router.push("/");
};

const closeNotif = () => (notifOpen.value = false);
</script>



<template>
  <nav class="backdrop-blur-md bg-black/60 shadow-lg fixed top-0 left-0 w-full z-50 ">
    <div class="max-w-7xl mx-auto px-4 md:px-6 lg:px-8">
      <div class="flex justify-between h-16 items-center">
        <!-- LOGO -->
        <div class="flex items-center">
          <a href="/" class="flex items-center gap-2">
            <img src="https://tailwindcss.com/plus-assets/img/logos/mark.svg" alt="Logo" class="h-8 w-auto drop-shadow-lg" />
            <span class="text-white font-semibold text-lg tracking-wide">HotelEase</span>
          </a>
        </div>

<div class="hidden sm:flex space-x-6 text-sm font-medium">
    <template v-if="session?.email">
      <NuxtLink to="/dashboard" class="nav-item text-white" active-class="text-blue-500 font-bold">
        Dashboard
      </NuxtLink>

      <NuxtLink to="/reservation" class="nav-item text-white" active-class="text-blue-500 font-bold">
        Reservations
      </NuxtLink>

      <NuxtLink to="/catalog" class="nav-item text-white" active-class="text-blue-500 font-bold">
        Catalog
      </NuxtLink>
      <NuxtLink to="/message" class="nav-item text-white" active-class="text-blue-500 font-bold">
        Message
      </NuxtLink>

      <div class="relative">
  <!-- Notification Button -->
  <button
    @click="notifOpen = !notifOpen"
    class="cursor-pointer nav-item text-white flex items-center gap-2 relative"
  >
    <svg
      xmlns="http://www.w3.org/2000/svg"
      class="h-5 w-5 text-white hover:text-blue-400 transition"
      fill="none"
      viewBox="0 0 24 24"
      stroke="currentColor"
      stroke-width="1.8"
    >
      <path
        stroke-linecap="round"
        stroke-linejoin="round"
        d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V4a2 2 0 10-4 0v1.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0a3 3 0 01-6 0"
      />
    </svg>

    <!-- Badge -->
    <span
      v-if="notifCount > 0"
      class="absolute -top-1 -right-2 bg-red-500 text-white text-[10px]
             rounded-full h-4 w-4 flex items-center justify-center"
    >
      {{ notifCount }}
    </span>
  </button>

  <!-- Dropdown -->
  <transition name="fade-slide">
    <div
      v-if="notifOpen"
      class="absolute right-0 mt-3 w-80 bg-white rounded-xl shadow-2xl
             border border-gray-200 z-50 overflow-hidden"
    >
      <!-- Header -->
      <div class="px-4 py-3 border-b flex justify-between items-center">
        <span class="font-semibold text-gray-800">Notifications</span>
        <button
          class="text-xs text-blue-500 hover:underline"
          @click="notifications.forEach(n => n.read = true)"
        >
          Mark all as read
        </button>
      </div>

      <!-- List -->
      <div class="max-h-80 overflow-y-auto">
        <div
          v-for="n in notifications"
          :key="n.id"
          class="px-4 py-3 flex gap-3 hover:bg-gray-50 cursor-pointer
                 border-b last:border-b-0"
          :class="!n.read ? 'bg-blue-50' : ''"
        >
          <!-- Dot -->
          <span
            v-if="!n.read"
            class="mt-2 h-2 w-2 rounded-full bg-blue-500 flex-shrink-0"
          ></span>

          <div class="flex-1">
            <p class="text-sm font-medium text-gray-800">
              {{ n.title }}
            </p>
            <p class="text-xs text-gray-600">
              {{ n.message }}
            </p>
            <p class="text-[10px] text-gray-400 mt-1">
              {{ n.time }}
            </p>
          </div>
        </div>

        <!-- Empty state -->
        <div
          v-if="notifications.length === 0"
          class="px-4 py-6 text-center text-sm text-gray-500"
        >
          No notifications
        </div>
      </div>

      <!-- Footer -->
      <div class="px-4 py-2 border-t text-center">
        <NuxtLink
          to="/notifications"
          class="text-sm text-blue-600 hover:underline"
          @click="closeNotif"
        >
          View all notifications
        </NuxtLink>
      </div>
    </div>
  </transition>
</div>




 
    </template>
</div>



  <div class="hidden sm:flex items-center gap-4">
  <template v-if="session">
<span class="text-white text-sm">Hi, {{ sessionName }}</span>




    <NuxtLink
      to="/profile"
      class="px-4 py-2 text-sm rounded-md bg-gray-700 hover:bg-gray-600 text-white transition"
    >
      Profile
    </NuxtLink>

    <button
      @click="logout"
      class="px-4 py-2 text-sm rounded-md bg-red-500 hover:bg-red-600 text-white transition"
    >
      Logout
    </button>
  </template>

  <template v-else>
    <NuxtLink
      to="/login"
      class="px-4 py-2 text-sm rounded-md bg-blue-600 hover:bg-blue-700 text-white transition"
    >
      Login
    </NuxtLink>

    <button
      @click="openModal"
      class="px-4 py-2 text-sm rounded-md bg-green-600 hover:bg-green-700 text-white transition"
    >
      Sign Up
    </button>
  </template>
</div>


        <!-- MOBILE MENU BUTTON -->
        <button
          @click="mobileMenu = !mobileMenu"
          class="sm:hidden text-white p-2 rounded hover:bg-white/10 transition"
        >
          <span class="material-icons text-3xl">{{ mobileMenu ? "close" : "menu" }}</span>
        </button>
      </div>
    </div>

<!-- MOBILE MENU -->
<div
  v-if="mobileMenu"
  class="md:inline-block hidden bg-black/90 backdrop-blur-xl text-white px-4 py-5 
         space-y-4 animate-slideDown rounded-b-xl shadow-lg 
         flex flex-col"
>
  <template v-if="session">
    <NuxtLink to="/dashboard" class="mobile-link">Dashboard</NuxtLink>
    <NuxtLink to="/reservation" class="mobile-link">Reservations</NuxtLink>
    <NuxtLink to="/catalog" class="mobile-link">Catalog</NuxtLink>
    <NuxtLink to="/admin-catalog" v-if="session.role === 'Admin'" class="mobile-link">Admin</NuxtLink>
    <NuxtLink to="/profile" class="mobile-link">Profile</NuxtLink>

    <button @click="logout" class="mobile-link bg-red-600/60 text-white rounded-lg">
      Logout
    </button>
  </template>

  <template v-else>
    <NuxtLink to="/" class="mobile-link">Home</NuxtLink>
    <NuxtLink to="/about" class="mobile-link">About</NuxtLink>
    <NuxtLink to="/contact" class="mobile-link">Contact</NuxtLink>
    <NuxtLink to="/login" class="mobile-link">Login</NuxtLink>

    <button @click="openModal" class="mobile-link bg-green-600/70 text-white rounded-lg">
      Sign Up
    </button>
  </template>
</div>



  </nav>

  <!-- SIGN UP MODAL -->
  <register v-if="isOpen" :loading="loading" @update:loading="loading = $event" @close="isOpen = false" />
  <loadingSpinner :show="loading" />
</template>

<style scoped>
.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: all 0.2s ease;
}
.fade-slide-enter-from {
  opacity: 0;
  transform: translateY(-8px);
}
.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
</style>
