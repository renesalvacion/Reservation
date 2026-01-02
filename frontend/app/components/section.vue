<template>
  <section class="flex justify-center items-center gap-10 pt-10 p-10">

    <!-- LEFT CARD -->
    <div v-if="leftImage"
      class="cardh bg-blue-50 flex items-center gap-3 rounded-xl p-3 shadow-xl"
    >
      <img
        :src="imageUrl(leftImage.imagepath)"
        class="rounded-lg"
        style="height: 20rem; width: 20rem; filter: brightness(70%);"
        alt=""
      />

      <div class="txt" style="width: 70%;">
        <p class="lg:text-lg font-bold text-center">
          Dream Stay at Our Luxury Hotel
        </p>

        <div class="tc pt-10">
          <p class="text-sm text-gray-500">
            Enjoy the serene ocean views from our Cozy Deluxe Room. This spacious
            room is designed to offer the ultimate relaxation experience.
          </p>
        </div>
      </div>
    </div>

    <!-- RIGHT CARD -->
    <div v-if="rightImage"
      class="cardh bg-gray-50 flex flex-col gap-3 p-3 rounded-xl shadow-xl w-full"
    >
      <div class="relative w-full">
        <img
          :src="imageUrl(rightImage.imagepath)"
          class="rounded-lg object-cover w-full h-50"
          style="filter: brightness(70%);"
          alt=""
        />
      </div>

      <div class="txt">
        <p class="text-center lg:text-lg font-bold">
          Cozy Deluxe Room - Ocean View
        </p>

        <div class="tc pt-1">
          <p class="text-sm text-gray-500">
            Enjoy a luxurious stay in our spacious Deluxe Room with a breathtaking
            view of the ocean. Perfect for couples or solo travelers.
          </p>
        </div>
      </div>
    </div>

  </section>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useImageViewStore } from '~/stores/imageView';

const imageStore = useImageViewStore();
const { leftImage, rightImage } = storeToRefs(imageStore);

const imageUrl = (path: string) =>
  `http://localhost:5080/uploadViewProfile/${path}`;

onMounted(() => {
  imageStore.fetchImages();
});

useSeoMeta({
  ogImage: '/images/section_h.avif',
});
</script>
