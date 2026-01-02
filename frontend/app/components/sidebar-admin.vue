<script setup>
import { useProfileStore } from "#imports";
import { useRouter } from "vue-router";

const props = defineProps({
  open: Boolean,
});

const emit = defineEmits(["toggle"]);

const profile = useProfileStore();
const router = useRouter();

const submitLogout = async () => {
  await profile.Logout();
  router.push("/");
};
</script>

<template>
  <!-- Sidebar -->
  <aside
    class="bg-white shadow-xl fixed left-0 top-0 h-full p-5 flex flex-col transition-all duration-300 border-gray-100"
    :class="props.open ? 'w-64' : 'w-20'"
  >
    <!-- Toggle Button INSIDE SIDEBAR -->
    <button
      class="cursor-pointer absolute -right-4 top-6 bg-white p-2 shadow rounded-full flex items-center justify-center"
      @click="emit('toggle')"
    >
      <svg
        v-if="!props.open"
        class="w-6 h-6 text-gray-700"
        fill="none"
        stroke="currentColor"
        stroke-width="2"
        viewBox="0 0 24 24"
      >
        <!-- Hamburger Icon -->
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M4 6h16M4 12h16M4 18h16"
        />
      </svg>

      <svg
        v-else
        class="w-6 h-6 text-gray-700"
        fill="none"
        stroke="currentColor"
        stroke-width="2"
        viewBox="0 0 24 24"
      >
        <!-- Close Icon -->
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          d="M6 18L18 6M6 6l12 12"
        />
      </svg>
    </button>

    <!-- Title -->
    <h2
      class="text-2xl font-bold mt-10 whitespace-nowrap transition-all duration-300"
      :class="props.open ? 'opacity-100 ml-0' : 'opacity-0 ml-[-150px]'"
    >
      Admin Panel
    </h2>

    <!-- Navigation -->
    <nav class="flex flex-col gap-10 mt-8 flex-1">
      <SidebarItem
        icon="M3 13h8V3H3v10zM13 21h8v-6h-8v6zM13 3v6h8V3h-8zM3 21h8v-6H3v6z"
        label="Dashboard"
        link="/dashboard"
        :open="props.open"
      />

      <!-- Example Menu Items -->
      <a
        href="/dashboard"
        class="cursor-pointer flex items-center gap-3 p-2 rounded-md hover:bg-gray-100"
      >
        <svg
          :class="props.open ? 'w-6 h-6' : 'w-10 h-10'"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          viewBox="0 0 24 24"
          class="transition-all duration-300"
        >
          <path
            d="M3 13h8V3H3v10zM13 21h8v-6h-8v6zM13 3v6h8V3h-8zM3 21h8v-6H3v6z"
          />
        </svg>
        <span v-show="props.open" class="font-semibold">Dashboard</span>
      </a>

      <a
        href="/reservation"
        class="cursor-pointer flex items-center gap-3 p-2 rounded-md hover:bg-gray-100"
      >
        <svg
          :class="props.open ? 'w-6 h-6' : 'w-10 h-10'"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          viewBox="0 0 24 24"
          class="transition-all duration-300"
        >
          <!-- Calendar / Reservation Icon -->
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            d="M8 7V3m8 4V3M3 11h18M5 5h14a2 2 0 012 2v14a2 2 0 01-2 2H5a2 2 0 01-2-2V7a2 2 0 012-2z"
          />
        </svg>
        <span v-show="props.open" class="font-semibold">Reservation</span>
      </a>

      <a
        href="/catalog"
        class="cursor-pointer flex items-center gap-3 p-2 rounded-md hover:bg-gray-100"
      >
        <svg
          :class="props.open ? 'w-6 h-6' : 'w-10 h-10'"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          viewBox="0 0 24 24"
          class="transition-all duration-300"
        >
          <!-- Bed / Room Icon -->
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            d="M3 7h18v10H3V7z M3 17v2h18v-2 M7 7v10"
          />
        </svg>
        <span v-show="props.open" class="font-semibold">Room</span>
      </a>

      <a
        href="/message"
        class="cursor-pointer flex items-center gap-3 p-2 rounded-md hover:bg-gray-100"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="24"
          height="24"
          viewBox="0 0 24 24"
        >
          <g fill="none" fill-rule="evenodd">
            <path
              d="m12.593 23.258l-.011.002l-.071.035l-.02.004l-.014-.004l-.071-.035q-.016-.005-.024.005l-.004.01l-.017.428l.005.02l.01.013l.104.074l.015.004l.012-.004l.104-.074l.012-.016l.004-.017l-.017-.427q-.004-.016-.017-.018m.265-.113l-.013.002l-.185.093l-.01.01l-.003.011l.018.43l.005.012l.008.007l.201.093q.019.005.029-.008l.004-.014l-.034-.614q-.005-.018-.02-.022m-.715.002a.02.02 0 0 0-.027.006l-.006.014l-.034.614q.001.018.017.024l.015-.002l.201-.093l.01-.008l.004-.011l.017-.43l-.003-.012l-.01-.01z"
            />
            <path
              fill="currentColor"
              d="M12 2c5.523 0 10 4.477 10 10s-4.477 10-10 10a9.96 9.96 0 0 1-4.863-1.26l-.305-.178l-3.032.892a1.01 1.01 0 0 1-1.28-1.145l.026-.109l.892-3.032A9.96 9.96 0 0 1 2 12C2 6.477 6.477 2 12 2M9.793 9.793l-3 3a1 1 0 1 0 1.414 1.414l2.293-2.293l2.293 2.293a1 1 0 0 0 1.414 0l3-3a1 1 0 0 0-1.414-1.414L13.5 12.086l-2.293-2.293a1 1 0 0 0-1.414 0"
            />
          </g>
        </svg>
        <span v-show="props.open" class="font-semibold">Message</span>
      </a>

      <!-- Logout -->
      <button
        @click="submitLogout"
        class="cursor-pointer flex items-center gap-3 p-2 rounded-md hover:bg-gray-100 mt-auto transition-all duration-300"
      >
        <svg
          :class="props.open ? 'w-6 h-6' : 'w-10 h-10'"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          viewBox="0 0 24 24"
        >
          <path
            d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a2 2 0 01-2 2H5a2 2 0 01-2-2V7a2 2 0 012-2h6a2 2 0 012 2v1"
          />
        </svg>
        <span v-show="props.open" class="font-semibold">Logout</span>
      </button>
    </nav>
  </aside>
</template>
