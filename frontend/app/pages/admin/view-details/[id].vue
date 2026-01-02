<template>
  <sidebarAdmin :open="sidebarOpen" @toggle="sidebarOpen = !sidebarOpen"/>
  <div class="flex-1 p-8 transition-all duration-300"
  :class="sidebarOpen ? 'ml-64' : 'ml-20'">
    <h1 class="text-2xl font-bold mb-6">Reservation Details</h1>

<div
  v-if="setAppDetails?.length"
  class="grid grid-cols-1 sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6"
>
  <div
  
    v-for="app in setAppDetails"
    :key="app.appointmentId"
    class="bg-blue-50 shadow-md rounded-lg p-4 flex flex-col justify-between break-words"
  >
    <h2 class="text-lg font-semibold mb-2 break-words">{{ app.crudsName }}</h2>
    <p class="break-words"><strong>Room Type:</strong> {{ app.roomType }}</p>
    <p class="break-words"><strong>Number of Beds:</strong> {{ app.numberBed }}</p>
    <p class="break-words"><strong>Number of Heads:</strong> {{ app.headPerson }}</p>
    <p class="break-words"><strong>Price:</strong> ${{ app.price }}</p>
    <p class="break-words"><strong>Status:</strong> {{ app.status || "N/A" }}</p>
    <p class="break-words"><strong>Date:</strong> {{ new Date(app.created).toLocaleString() }}</p>
    <p class="break-words"><strong>Reservations:</strong> {{ app.reservationCount }}</p>

    <div class="flex flex-col sm:flex-row sm:flex-wrap gap-3 pt-5 w-full">
<button
  v-if="!app?.status || app?.status.toLowerCase().trim() === 'active'"
  @click="cancelModal(app.appointmentId)"
  class="cursor-pointer flex-1 bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition"
>
  Cancel Reservation
</button>

<button
  @click="adminEditModal(app.appointmentId)"
  class="cursor-pointer flex-1 bg-yellow-500 text-white px-4 py-2 rounded hover:bg-yellow-600 transition"
>
  Edit Reservation
</button>

<button
  @click="approveReservation(app.crudId)"
  class="cursor-pointer flex-1 bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600 transition"
>
  Approve Reservation
</button>

    </div>
  </div>
</div>


    <div v-else class="text-gray-500">No reservation details found.</div>
  </div>

  <admin_cancel_modal
    v-if="isCancel"
    :appointment-id="appointmentIdToCancel || 0"
    @close="isCancel = false"
  />

  <user_edit_modal v-if="isEditAdmin" :appointment-id="appointmentIdToUpdate || 0" @close="isEditAdmin = false"/>

  <loading :show="isLoading"/>
</template>
<script setup lang="ts">
    import sidebarAdmin from "~/components/sidebar-admin.vue";
    import { ref, onMounted } from "vue";
    import { SetAppointment } from "#imports";
    import { useRoute } from "vue-router";
    import admin_cancel_modal from "~/components/admin_cancel_modal.vue";

    import user_edit_modal from "~/components/admin_edit_modal.vue";


    import loading from "~/components/loading.vue";
import { tree } from "#build/ui";

const isCancelled = ref(false)

    const setApp = SetAppointment();
    const setAppDetails = ref<any[] | null>([]);

    const sidebarOpen = ref(true);

    const route = useRoute(); // <-- Get route params
    const userId = Number(route.params.id); // convert to number
    const appointmentIdToCancel = ref<number | null>(null);
    const appointmentIdToUpdate = ref<number | null>(null);

    const isCancel = ref(false);
    const isEditAdmin = ref(false);

    const isLoading = ref(false)

    const cancelModal = (appointmentId: number) => {
        appointmentIdToCancel.value = appointmentId;
        isCancel.value = true;
    };

    const adminEditModal = (appointmentId: number) => {
      isEditAdmin.value = true;
      appointmentIdToUpdate.value = appointmentId
    }

const approveReservation = async (userId: number) => {
  isLoading.value = true
  try {
    await setApp.approvedReservation(userId)
    // Optionally refresh appointment list here
    setAppDetails.value = await setApp.AdminViewUserAppointment(userId)
  } finally {
    isLoading.value = false
  }
}


    onMounted(async () => {
        if (!isNaN(userId)) {
            setAppDetails.value = await setApp.AdminViewUserAppointment(userId);
            //isCancel.value = true;
        } else {
            console.error("Invalid userId in route");
        }
    });
</script>
