<!--index.vue-->

<template>

<header :style="headerStyle">
  <div class="overlay"></div>

  <div
    class="section_t flex flex-col justify-center items-center"
    style="position: absolute; width: 100%; height: 100%;"
  >
    <div class="text_h flex flex-col justify-center items-center p-5">
      <div class="s_h text-center">
        <h1
          class="text-white text-6xl font-bold text-center text-shadow-strong"
          style="line-height: 5rem;"
        >
          AFFORDABLE BUT QUALITY
        </h1>
        <h5 class="text-white">Quality stay. Exceptional comfort. Fair price.</h5>
      </div>

      <div class="btn pt-10 flex gap-4">
        <button
          type="button"
          class="cursor-pointer rounded text-body text-white bg-neutral-secondary-medium box-border border border-default-medium hover:bg-neutral-tertiary-medium hover:text-heading focus:ring-4 focus:ring-neutral-tertiary shadow-xs font-medium leading-5 rounded-base text-sm px-4 py-2.5 focus:outline-none"
          @click="openModal"
        >
          LOGIN
        </button>
        <button
          type="button"
          class="cursor-pointer rounded text-body text-white bg-neutral-secondary-medium box-border border border-default-medium hover:bg-neutral-tertiary-medium hover:text-heading focus:ring-4 focus:ring-neutral-tertiary shadow-xs font-medium leading-5 rounded-base text-sm px-4 py-2.5 focus:outline-none"
        >
          Discover
        </button>
      </div>
    </div>

    <div class="socia_btn flex flex-col items-center gap-3 pt-10" >
      <h5 class="text-white text-lg font-medium">
        You can reach us on our social media :
      </h5>

<div class="flex gap-4">

  <!-- Facebook -->
  <button class="p-2 cursor-pointer rounded-full hover:opacity-80 transition">
    <svg width="24" height="24" viewBox="0 0 24 24" fill="white">
      <path d="M22.675 0h-21.35C.597 0 0 .597 0 1.325v21.351C0 
      23.403.597 24 1.325 24h11.495v-9.294H9.691V11.01h3.129V8.41c0-3.1 
      1.893-4.788 4.659-4.788 1.325 0 2.463.099 2.795.143v3.24l-1.918.001c-1.504 
      0-1.795.715-1.795 1.763v2.311h3.587l-.467 3.696h-3.12V24h6.116C23.403 
      24 24 23.403 24 22.676V1.325C24 .597 23.403 0 22.675 0z"/>
    </svg>
  </button>

  <!-- Instagram -->
  <button class="p-2 cursor-pointer rounded-full hover:opacity-80 transition">
    <svg width="24" height="24" viewBox="0 0 24 24" fill="white">
      <path d="M7.75 2h8.5A5.75 5.75 0 0 1 22 7.75v8.5A5.75 5.75 0 0 1 
      16.25 22h-8.5A5.75 5.75 0 0 1 2 16.25v-8.5A5.75 5.75 0 0 1 
      7.75 2zm0 1.5A4.25 4.25 0 0 0 3.5 7.75v8.5A4.25 4.25 0 0 0 
      7.75 20.5h8.5A4.25 4.25 0 0 0 20.5 16.25v-8.5A4.25 4.25 0 0 
      0 16.25 3.5h-8.5zM12 7a5 5 0 1 1 0 10a5 5 0 0 1 0-10zm0 
      1.5a3.5 3.5 0 1 0 0 7a3.5 3.5 0 0 0 0-7zm5.25-.75a1.25 
      1.25 0 1 1-2.5 0a1.25 1.25 0 0 1 2.5 0z"/>
    </svg>
  </button>

  <!-- X (Twitter) -->
  <button class="p-2 cursor-pointer rounded-full hover:opacity-80 transition">
    <svg width="24" height="24" viewBox="0 0 24 24" fill="white">
      <path d="M18.146 2H21l-6.52 7.455L22 22h-7.383l-4.6-6.606L4.8 
      22H2l7.028-8.04L2 2h7.495l4.194 5.873L18.146 2zM16.865 
      20h1.62L7.273 4h-1.67l11.262 16z"/>
    </svg>
  </button>

  <!-- TikTok -->
  <button class="p-2 cursor-pointer rounded-full hover:opacity-80 transition">
    <svg width="24" height="24" viewBox="0 0 24 24" fill="white">
      <path d="M12.819 2.001h3.356c.15 1.487.79 2.734 1.875 3.818c1.084 
      1.084 2.332 1.725 3.82 1.875v3.356c-1.39-.05-2.71-.36-3.96-.93v6.77c0 
      2.09-.74 3.86-2.22 5.32c-1.48 1.46-3.26 2.18-5.34 2.18c-2.08 
      0-3.86-.72-5.34-2.18c-1.48-1.46-2.22-3.23-2.22-5.32c0-2.09.74-3.86 
      2.22-5.32c1.48-1.46 3.26-2.19 5.34-2.19c.63 0 1.27.08 
      1.92.24v3.48a4.18 4.18 0 0 0-1.92-.48c-1.16 0-2.15.4-2.97 
      1.21c-.82.81-1.23 1.79-1.23 2.96c0 1.17.41 2.15 1.23 
      2.96c.82.81 1.81 1.21 2.97 1.21c1.16 0 2.15-.4 2.97-1.21c.82-.81 
      1.23-1.79 1.23-2.96V2.001z"/>
    </svg>
  </button>

</div>

    </div>
  </div>
</header>


  <section class="p-10">
    <slider />
  </section>

  <SavedModal
    v-if="isOpen"
    :loading="loading"
    @update:loading="loading = $event"
    @close="isOpen = false"
  />

  <loadingSpinner :show="loading"/>

  <SectionH />
  <contact />


</template>

<script setup lang="ts">
import navbar from "~/components/navbar.vue";
import slider from "~/components/slider.vue";
import SavedModal from "~/components/modal.vue";
import { ref, watch, nextTick } from 'vue';


import headerImage from "../../public/images/hotel_header.avif";
import SectionH from "~/components/section.vue";
import contact from "~/components/contact.vue";
import loadingSpinner from "~/components/loading.vue";

const isOpen = ref();
const loading = ref(false);

const emailInput = ref<HTMLInputElement | null>(null);

const headerStyle = `
  background-image: url(${headerImage});
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat; 
  height: 40rem;
  
  position: relative;
`;

// For testing, keep spinner always running
// To hide it after 3 seconds:
setTimeout(() => {
  loading.value = false;
}, 3000);

const openModal = () => {
  isOpen.value = true;
};


watch(isOpen, async (newVal) => {
  if (newVal) {
    // Wait for DOM to render
    await nextTick();
    emailInput.value?.focus();
  }
});
</script>

<style scoped>
.overlay {
  position: absolute;
  inset: 0; /* top:0; right:0; bottom:0; left:0 */
  background-color: rgba(0, 0, 0, 0.4); /* semi-transparent dark background */
  display: flex;
  justify-content: center;
  align-items: center;
}

.text-shadow-strong {
  text-shadow: 0 3px 6px rgba(0, 0, 0, 0.8);
}

</style>
