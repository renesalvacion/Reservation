<template>
  <section 
    class="relative pt-16 flex flex-col items-center p-8 bg-cover bg-center"
    style="background-image: url('/images/contact.avif');"
  >
    <!-- Overlay -->
    <div class="absolute inset-0 bg-black/40"></div>

    <!-- Content -->
    <div class="relative z-10 w-full max-w-lg bg-white rounded-xl p-8 flex flex-col gap-6">
      <h2 class="text-3xl font-bold text-center text-gray-900">Contact Us</h2>

      <!-- Message -->
      <div v-if="message.text"
           class="text-center py-2 rounded"
           :class="message.type === 'success' ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'">
        {{ message.text }}
      </div>

      <!-- Form -->
      <form @submit.prevent="sendEmail" ref="contactForm" class="flex flex-col gap-4">

        <input 
          ref="subjectInput"
          type="text" 
          name="subject" 
          v-model="form.subject" 
          placeholder="Subject"
          class="border rounded-lg p-3 focus:ring-2 focus:ring-blue-500 outline-none" 
          required 
        />

        <input type="email" name="email" v-model="form.email" placeholder="Email"
               class="border rounded-lg p-3 focus:ring-2 focus:ring-blue-500 outline-none" required />

        <input type="text" name="name" v-model="form.name" placeholder="Name"
               class="border rounded-lg p-3 focus:ring-2 focus:ring-blue-500 outline-none" required />

        <textarea name="message" v-model="form.message" rows="4" placeholder="Message"
                  class="border rounded-lg p-3 focus:ring-2 focus:ring-blue-500 outline-none resize-none" required></textarea>

        <button type="submit" 
                class="w-full bg-blue-600 text-white py-3 rounded-lg font-semibold hover:bg-blue-700 transition">
          Send Message
        </button>
      </form>
    </div>

    <!-- Loading Spinner -->
    <loading :show="isloading"/>
  </section>
</template>

<script lang="ts">
import { ref, onMounted } from 'vue';
import emailjs from '@emailjs/browser';
import loading from './loading.vue';

export default {
  components: { loading },
  setup() {
    const form = ref({ subject: '', name: '', email: '', message: '' });
    const isloading = ref(false);
    const message = ref({ text: '', type: '' as 'success' | 'error' | '' });
    const contactForm = ref<HTMLFormElement | null>(null);
    const subjectInput = ref<HTMLInputElement | null>(null);

    // Auto-focus first input
    onMounted(() => {
      subjectInput.value?.focus();
    });

    const showMessage = (text: string, type: 'success' | 'error' = 'success', duration = 3000) => {
      message.value = { text, type };
      setTimeout(() => message.value = { text: '', type: '' }, duration);
    };

    const sendEmail = () => {
      const serviceId = 'service_qyg0l6l';
      const templateId = 'template_0aqo1rf';
      const userId = 'rlboW4GzRw5Glalhl';
      const formEl = contactForm.value;

      if (!formEl) return;

      isloading.value = true;

      emailjs.sendForm(serviceId, templateId, formEl, userId)
        .then(() => {
          showMessage('Message sent successfully!', 'success');
          form.value = { subject: '', name: '', email: '', message: '' };
          formEl.reset();
          subjectInput.value?.focus(); // refocus after reset
        })
        .catch((err) => {
          console.error(err);
          showMessage('Failed to send message', 'error');
        })
        .finally(() => isloading.value = false);
    };

    return { form, isloading, message, contactForm, subjectInput, sendEmail };
  }
};
</script>

<style scoped>
section {
  min-height: 70vh;
  display: flex;
  justify-content: center;
  align-items: center;
}
.loading {
  position: absolute;
}
</style>
