<template>
  <!-- Modal Overlay and Login Card -->
  <div
    v-if="isModalOpen"
    class="fixed inset-0 bg-opacity-50 flex justify-center items-center z-50"
    style="background-color: rgba(0, 0, 0, 0.5)"
  >

    <!-- Modal content -->
    <div
      class="card bg-white rounded-lg overflow-hidden shadow-lg p-6 w-full max-w-sm relative z-60"
    >
      <button
        @click="closeModal"
        class="cursor-pointer absolute top-4 right-4 text-gray-500 hover:text-gray-700 focus:outline-none"
      >
        <span class="material-icons">close</span>
      </button>
      <h1 class="text-center font-semibold">Review Your Room Reservation?</h1>


      <form @submit.prevent="submitReview()" class="space-y-4">



        <!-- Stars -->
        <div>
          <label class="block mb-1 font-medium">Stars (0-5)</label>
          <input
            type="number"
            name="stars"
            min="0"
            max="5"
            v-model.number="form.stars"
            class="w-full border border-gray-300 p-2 rounded"
            required
          />
        </div>

        <!-- Title -->
        <div>
          <label class="block mb-1 font-medium">Review Title</label>
          <input
            type="text"
            name="reviewTitle"
            v-model="form.reviewTitle"
            class="w-full border border-gray-300 p-2 rounded"
            required
          />
        </div>

        <!-- Message -->
        <div>
          <label class="block mb-1 font-medium">Review Message</label>
          <textarea
            name="reviewMessage"
            v-model="form.reviewMessage"
            rows="3"
            class="w-full border border-gray-300 p-2 rounded"
            required
          ></textarea>
        </div>

        <div class="flex justify-end space-x-2">
          <button
            type="button"
            @click="closeModal"
            class="cursor-pointer px-4 py-2 bg-gray-300 rounded hover:bg-gray-400"
          >
            Cancel
          </button>
          <button
            type="submit"
            class="cursor-pointer px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Submit
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, defineProps } from "vue";

// Define emit
const emit = defineEmits(["close", "update:loading"]);

// Track modal visibility state
const isModalOpen = ref(true);

const props = defineProps({
  loading: Boolean,
  show: Boolean,
  isOpen: Boolean,
  appointmentId: Number
});

console.log("Appointment ID in Review Modal:", props.appointmentId);

const form = reactive({
  stars: 0,
  reviewTitle: "",
  reviewMessage: "",
  roomId: props.appointmentId
})

const roomCatalogStore = useReviewStore()
const sessionStore = useSessionStore()



const submitReview = async () => {
  const session = await sessionStore.getSession()
  console.log("User ID from session:", session.userId);

  if (!session?.userId) {
    alert("You must be logged in to post a review")
    return
  }

  form.roomId = props.appointmentId  // Make sure this is correct
  await roomCatalogStore.PostReview(session.userId, form)

  closeModal()
}


// Close modal function
const closeModal = () => {
  emit("close");
};
</script>
