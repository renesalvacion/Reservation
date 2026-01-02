<script setup>
import { ref } from "vue";
import SidebarAdmin from "~/components/sidebar-admin.vue"; // adjust path if different

import messenger from "~/components/messenger.vue";

const sidebarOpen = ref(true);
const activeTab = ref("popular"); // <-- the missing piece
</script>

<template>
  <div class="flex min-h-screen bg-gray-100 relative">
    <!-- Sidebar -->
    <SidebarAdmin :open="sidebarOpen" @toggle="sidebarOpen = !sidebarOpen" />

    <!-- Main Content -->
    <main
      class="flex-1 p-8 transition-all duration-300"
      :class="sidebarOpen ? 'ml-64' : 'ml-20'"
    >
      <h1 class="text-3xl font-bold">Admin Dashboard</h1>

      <header>
        <h1 class="text-xl font-bold mt-4">Lodging Available</h1>

        <!-- Horizontal Card Row -->
        <div class="card-row flex gap-4 overflow-x-auto py-5">
          <div class="w-60 flex-shrink-0 bg-white p-3 rounded-xl shadow-md">
            <img src="/images/prisc.avif" class="w-full h-32 object-cover rounded-lg" />
            <h5 class="text-xl font-bold mt-2">Shikara Hotel</h5>
            <p class="text-gray-500 font-semibold">Room: 304f</p>
            <p class="font-semibold">$45.40</p>
          </div>
          <div class="w-60 flex-shrink-0 bg-white p-3 rounded-xl shadow-md">
            <img src="/images/contact.avif" class="w-full h-32 object-cover rounded-lg" />
            <h5 class="text-xl font-bold mt-2">Makati Suites</h5>
            <p class="text-gray-500 font-semibold">Room: 201A</p>
            <p class="font-semibold">$80.00</p>
          </div>
          <div class="w-60 flex-shrink-0 bg-white p-3 rounded-xl shadow-md">
            <img src="/images/prisc.avif" class="w-full h-32 object-cover rounded-lg" />
            <h5 class="text-xl font-bold mt-2">Vista Inn</h5>
            <p class="text-gray-500 font-semibold">Room: 109B</p>
            <p class="font-semibold">$52.30</p>
          </div>
        </div>
      </header>

      <section class="mt-8">
        <div class="flex justify-between items-center mb-4">
          <div class="flex gap-6">
            <button
            class="cursor-pointer"
              @click="activeTab = 'popular'"
              :class="activeTab === 'popular'
                ? 'font-semibold border-b-2 border-blue-500 pb-1'
                : 'font-semibold text-gray-400'"
            >
              Most Popular
            </button>

            <button
            class="cursor-pointer"
              @click="activeTab = 'reviews'"
              :class="activeTab === 'reviews'
                ? 'font-semibold border-b-2 border-blue-500 pb-1'
                : 'font-semibold text-gray-400'"
            >
              Reviews
            </button>

            <button
            class="cursor-pointer"
              @click="activeTab = 'reserve'"
              :class="activeTab === 'reserve'
                ? 'font-semibold border-b-2 border-blue-500 pb-1'
                : 'font-semibold text-gray-400'"
            >
              Reserve Room
            </button>
          </div>

          <a href="#" class="text-blue-500 font-semibold">View All</a>
        </div>

        <!-- TABBED CONTENT -->
        <div v-if="activeTab === 'popular'" class="grid grid-cols-2 gap-4">
          <div v-for="i in 4" :key="'popular-'+i" class="flex items-center justify-between bg-white p-3 rounded-xl shadow-md">
            <img src="/images/prisc.avif" class="w-16 h-16 object-cover rounded-lg" />
            <div class="flex-1 px-3">
              <h5 class="font-semibold text-gray-800">Popular Hotel {{ i }}</h5>
              <p class="text-sm text-gray-400 flex items-center gap-1">
                <svg class="w-4 h-4 text-gray-400" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5S10.62 6.5 12 6.5s2.5 1.12 2.5 2.5S13.38 11.5 12 11.5z"/>
                </svg>
                Country {{ i }}
              </p>
            </div>
            <span class="font-semibold">$4{{ i }} / night</span>
          </div>
        </div>

        <div v-if="activeTab === 'reviews'" class="grid grid-cols-2 gap-4">
          <div v-for="i in 4" :key="'review-'+i" class="bg-white p-4 rounded-xl shadow-md">
            <h4 class="font-bold text-gray-800">Guest Review {{ i }}</h4>
            <p class="text-gray-500 text-sm mt-2">“This hotel was amazing! The service was excellent.”</p>
            <p class="text-yellow-500 mt-2">⭐⭐⭐⭐⭐</p>
          </div>
        </div>

        <div v-if="activeTab === 'reserve'" class="grid grid-cols-2 gap-4">
          <div v-for="i in 4" :key="'reserve-'+i" class="bg-white p-4 rounded-xl shadow-md">
            <img src="/images/prisc.avif" class="w-full h-24 object-cover rounded-md" />
            <h4 class="font-bold text-gray-800 mt-2">Room Type {{ i }}</h4>
            <p class="text-gray-500 text-sm">Capacity: {{ i + 1 }} persons</p>
            <button class="mt-2 w-full bg-blue-500 text-white py-1 rounded-md">Reserve Now</button>
          </div>
        </div>
      </section>
    </main>
  </div>
  <messenger/>
</template>
