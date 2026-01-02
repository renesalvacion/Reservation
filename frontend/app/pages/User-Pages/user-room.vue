<!--pages/catalog.vue-->

<template>
  <notificationApproval/>
  <view_catalog />
</template>

<script setup>
import { useRoomCatalogStore } from "~/stores/room_catalog";
import { useSessionStore } from "#imports";
import NavBar from "~/components/navbar.vue";
import view_catalog from "../../components/view_catalog.vue";

import notificationApproval from "~/components/notification-approval.vue";

const roomCatalogStore = useRoomCatalogStore();
const rooms = ref([]);

const sessionStore = useSessionStore();
const session = ref(null);

const loading = ref(true);

// Create a reference for the form data
const formData = ref({
  name: "",
  description: "",
  price: "",
  maxoccupancy: "",
  roomnumber: "",
  roomstatus: "",
  roomType: "",
  roomimage: null, // This will hold the file
});

// Handle file input change
const onFileChange = (event) => {
  formData.value.roomimage = event.target.files[0];
};

const submitRoomCatalog = async () => {
  try {
    const payload = new FormData();
    payload.append("name", formData.value.name);
    payload.append("description", formData.value.description);
    payload.append("price", formData.value.price);
    payload.append("maxOccupancy", formData.value.maxoccupancy);
    payload.append("roomNumber", formData.value.roomnumber);
    payload.append("roomStatus", formData.value.roomstatus);
    payload.append("roomType", formData.value.roomType)
    if (formData.value.roomimage) {
      payload.append("roomImage", formData.value.roomimage);
    }

    const response = await roomCatalogStore.CreateCatalog(payload);
    console.log("Room catalog response:", response);

    // Optional: reset form
    formData.value = {
      name: "",
      description: "",
      price: "",
      maxoccupancy: "",
      roomnumber: "",
      roomstatus: "",
      roomimage: null,
    };
  } catch (error) {
    console.error("Error adding room:", error);
  }
};

onMounted(async () => {
  const result = await roomCatalogStore.GetCatalog();
  rooms.value = roomCatalogStore.catalog.rooms || [];

  // Fetch session
  session.value = await sessionStore.getSession();
  
  console.log("Session:", session.value); // Check role here
});
</script>
