<template>
  <transition name="notif">
    <div
      v-if="isNotif"
      class="fixed top-6 right-6 z-50 w-96 bg-white border-l-4 border-blue-500 shadow-lg rounded-lg p-4 flex gap-3"
    >
      <!-- Bell SVG Icon -->
      <svg
        class="w-6 h-6 text-blue-500 mt-1 animate-pulse"
        fill="none"
        stroke="currentColor"
        stroke-width="2"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11
           a6.002 6.002 0 00-4-5.659V4a2 2 0 10-4 0v1.341
           C7.67 6.165 6 8.388 6 11v3.159
           c0 .538-.214 1.055-.595 1.436L4 17h5
           m6 0a3 3 0 11-6 0h6z"
        />
      </svg>

      <!-- Content -->
      <div class="flex-1">
        <h3 class="font-semibold text-gray-800">Notifications</h3>

        <ul class="mt-2 space-y-1">
          <li
            v-for="n in notifications"
            :key="n.id"
            class="text-sm text-gray-600"
          >
            <span class="font-medium text-gray-800">{{ n.title }}</span>
            <br />
            <span>{{ n.message }}</span>
            <div class="text-xs text-gray-400">
              {{ n.createdAt }}
            </div>
          </li>
        </ul>
      </div>

      <!-- Close button SVG -->
      <button
        @click="isNotif = false"
        class="text-gray-400 hover:text-gray-600 transition"
      >
        <svg
          class="w-5 h-5"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            d="M6 18L18 6M6 6l12 12"
          />
        </svg>
      </button>
    </div>
  </transition>
</template>

<script setup lang="ts">

    import { useNotifications } from "#imports";
const isNotif = ref(false);

let showTimer: ReturnType<typeof setTimeout> | null = null;
let hideTimer: ReturnType<typeof setTimeout> | null = null;

const { notifications } = useNotifications();

const clearTimers = () => {
  if (showTimer) {
    clearTimeout(showTimer);
    showTimer = null;
  }
  if (hideTimer) {
    clearTimeout(hideTimer);
    hideTimer = null;
  }
};

watch(
  () => notifications.value.length,
  (length) => {
    clearTimers();

    // Only start timer if there are notifications
    if (length > 0) {
      showTimer = setTimeout(() => {
        isNotif.value = true;

        hideTimer = setTimeout(() => {
          isNotif.value = false;
        }, 5000);
      }, 3000);
    } else {
      isNotif.value = false;
    }
  },
  { immediate: true }
);

onUnmounted(() => {
  clearTimers();
});
</script>

<style>
.notif-enter-active,
.notif-leave-active {
  transition: all 0.35s ease;
}

.notif-enter-from {
  opacity: 0;
  transform: translateX(20px) scale(0.95);
}

.notif-leave-to {
  opacity: 0;
  transform: translateX(20px) scale(0.95);
}
</style>
