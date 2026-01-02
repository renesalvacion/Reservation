import { defineStore } from 'pinia';

import axios from 'axios'

export interface ImageView {
  imageId: number;
  imagepath: string;
}

export const useImageViewStore = defineStore('imageView', {
  state: () => ({
    images: [] as ImageView[],
    loading: false,
    error: '' as string
  }),

  actions: {
    async fetchImages() {
      this.loading = true;
      this.error = '';

      try {
        const res = await axios.get<ImageView[]>('http://localhost:5080/api/ImageUpload');
        this.images = res.data;
      } catch (err) {
        this.error = 'Failed to load images';
        console.error(err);
      } finally {
        this.loading = false;
      }
    }
  },

  getters: {
    // First image (left card)
    leftImage: (state) => state.images[0],

    // Second image (right card)
    rightImage: (state) => state.images[1],
  }
});
