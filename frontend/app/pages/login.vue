<template>
  <div class="flex items-center justify-center min-h-screen bg-gray-100">
    <div class="card bg-white rounded-lg shadow-lg p-6 w-full max-w-sm">

      <h1 class="text-2xl font-semibold text-center text-blue-500 mb-6">Login</h1>

      <!-- Message -->
      <div v-if="message.text"
           class="text-sm text-center py-2 rounded"
           :class="message.type === 'success'
             ? 'bg-green-100 text-green-700'
             : 'bg-red-100 text-red-700'">
        {{ message.text }}
      </div>

      <!-- Login Form -->
      <form @submit.prevent="submitData" class="space-y-4">
        <!-- Email Input -->
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">Email:</label>
          <input 
            type="email" 
            id="email" 
            name="email"
            ref="emailInput"
            v-model="formData.email"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Enter your email" 
            required
          />
        </div>

        <!-- Password Input -->
        <div>
          <label for="password" class="block text-sm font-medium text-gray-700">Password:</label>
          <input 
            type="password" 
            id="password" 
            name="password"
            v-model="formData.password"
            class="mt-1 block w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Enter your password" 
            required
          />
        </div>

        <!-- Submit Button -->
        <div>
          <button 
            type="submit" 
            class="w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600 focus:outline-none">
            Log In
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
            :options="{ theme: 'filled_blue', size: 'large' }"
            @success="onSuccess"
            @verified="onVerified"
            @error="onError"
          />
        </div>
      </form>

      <!-- Footer -->
      <div class="text-center mt-4">
        <p class="text-sm text-gray-500">
          Don't have an existing account? <NuxtLink to="/register" class="underline font-semibold"> Sign up here.</NuxtLink>
        </p>
      </div>
    </div>

    <loading :show="isLoading"/>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, nextTick } from 'vue';
import { useProfileStore } from '~/stores/profile';
import { useRouter } from 'vue-router';
import loading from '~/components/loading.vue';

// Store & router
const profileStores = useProfileStore();
const router = useRouter();

// Reactive states
const isLoading = ref(false);
const formData = ref({ email: '', password: '' });
const message = ref<{ text: string; type: 'error' | 'success' | '' }>({ text: '', type: '' });

// Ref for auto-focus
const emailInput = ref<HTMLInputElement | null>(null);

// Auto-focus email field on mount
onMounted(async () => {
  await nextTick();
  emailInput.value?.focus();
});

// Show temporary messages
const showMessage = (text: string, type: 'error' | 'success' = 'error', duration = 3000) => {
  message.value = { text, type };
  setTimeout(() => message.value = { text: '', type: '' }, duration);
};

// Login form submission
const submitData = async () => {
  if (!formData.value.email || !formData.value.password) {
    showMessage('Please fill in both email and password.', 'error');
    return;
  }

  try {
    isLoading.value = true;
    const success = await profileStores.Login(formData.value, router);
    if (success) {
      showMessage('Login successful!', 'success');
      router.push('/dashboard');
    } else {
      showMessage('Invalid credentials.', 'error');
    }
  } catch (err) {
    console.error(err);
    showMessage('An error occurred. Try again.', 'error');
  } finally {
    isLoading.value = false;
  }
};

// Google login handlers
const onSuccess = (e: any) => {
  console.log('Google login success:', e.claims, e.credential.slice(0, 20) + '…');
  fetch('http://localhost:5080/api/Profile/Google-Login-Google', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ IdToken: e.credential }),
  })
  .then(res => res.json())
  .then(data => {
    if (data.token) {
      localStorage.setItem('token', data.token);
      router.push('/dashboard');
    } else {
      showMessage('Google login failed.', 'error');
    }
  })
  .catch(err => console.error('Google login error:', err));
};

const onVerified = (data: any) => console.log('Google verified:', data);
const onError = (err: any) => console.error('Google login error:', err);
</script>

<style scoped>
.material-icons { font-size: 24px; }
.card { position: relative; }
</style>
