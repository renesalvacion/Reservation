<!--login modal.vue-->

<template>
  <div v-if="isModalOpen" class="fixed inset-0 bg-opacity-50 flex justify-center items-center z-50" style="background-color: rgba(0,0,0,0.5);">
    
    <div class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm relative z-60">
      <button 
        @click="closeModal"
        class="cursor-pointer absolute top-4 right-4 text-gray-500 hover:text-gray-700 focus:outline-none"
      >
        <span class="material-icons">close</span>
      </button>
      
      <h1 class="text-2xl font-semibold text-center text-blue-500 mb-6">Login</h1>
      <!-- Message -->
<div v-if="message"
     class="text-sm text-center py-2 rounded"
     :class="messageType === 'success'
       ? 'bg-green-100 text-green-700'
       : 'bg-red-100 text-red-700'">
  {{ message }}
</div>

      <form @submit.prevent="submitData" class="space-y-4">
        <!-- Email Input -->
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">Email:</label>
<input 
  type="email" 
  id="email" 
  name="email"
  v-model="formData.email"
  ref="emailInput"
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
          Don't have and existing account? <NuxtLink to="/register" class="underline font-semibold"> Sign up here.</NuxtLink>
        </p>
      </div>



    </div>
  </div>
</template>


<script setup lang="ts">
import { Icon } from '@iconify/vue';
import { useProfileStore } from '~/stores/profile'
import { useRouter } from 'vue-router';
import { ref, defineProps, watch, nextTick } from 'vue'
import { ro } from '@nuxt/ui/runtime/locale/index.js';

const emit = defineEmits(['close', 'update:loading'])
const router = useRouter();
const profileStores = useProfileStore()

const runtime = useRuntimeConfig()

const isModalOpen = ref(true)



const emailInput = ref<HTMLInputElement | null>(null);

const props = defineProps({
  loading : Boolean,
  show : Boolean
})


const message = ref('')
const messageType = ref<'success' | 'error'>('success')
let messageTimer: any = null

const formData = ref({
  email:'',
  password:''
})


const showMessage = (text: string, type: 'success' | 'error' = 'success', duration = 3000) => {
  message.value = text
  messageType.value = type

  if (messageTimer) clearTimeout(messageTimer)

  messageTimer = setTimeout(() => {
    message.value = ''
  }, duration)
}

const submitData = async () => {
  if (!formData.value.email || !formData.value.password) {
    profileStores.error = 'Please fill in both email and password.'
    return
  }

  emit('update:loading', true)

  try {
    const success = await profileStores.Login(formData.value, router)
    emit('update:loading', false)

    if (success){
      showMessage('Login successful! Redirecting...', 'success', 2000)
      setTimeout(() => router.push('/dashboard'), 2000)
    } 
    else {
      showMessage('Invalid credentials.', 'error')
    }

  } catch (err) {
     emit('update:loading', false)
    showMessage('An error occurred. Try again.', 'error')
  }
}

const onSuccess = (e: any) => {
  console.log('success:', e.claims, e.credential.slice(0, 20) + '…');

  // Send the ID token to the backend for verification
  fetch(`${runtime.public.backend}/api/Profile/Google-Login-Google`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ IdToken: e.credential }), // Send the Google ID token
  })
  .then(response => response.json()) // Parse the JSON response
  .then(data => {
    // Check if the response contains the expected properties (e.g., message and token)
    if (data.message && data.token) {
      const token = data.token;  // Assuming 'data.token' is the JWT token returned from the backend
      console.log('Login Function: ' + token);

      // Save token in localStorage or Pinia state (if using state management)
      localStorage.setItem('token', token);  // Persist token in localStorage

      showMessage('Google login successful! Redirecting...', 'success', 2000)
      setTimeout(() => router.push('/dashboard'), 2000)

    } else {
      console.error('Login failed:', data.message);  // Handle error if any
      showMessage('Google login failed. Please try again.', 'error')

    }
  })
  .catch(error => {
    console.error('Error sending to backend:', error);
    alert('An error occurred during login. Please try again.');
  });
};

  // eslint-disable-next-line no-console
  const onVerified = (data: any) => {
    console.log('verified:', data)
  }
  // eslint-disable-next-line no-console
  const onError = (err: any) => {
    console.error('error:', err)
  }

watch(isModalOpen, async (newVal) => {
  if (newVal) {
    await nextTick(); // wait for DOM
    emailInput.value?.focus();
  }
});

const closeModal = () => emit('close')
</script>

<style scoped>
.material-icons { font-size: 24px; }
.card { position: relative; }
</style>
