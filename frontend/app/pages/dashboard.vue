<template>
  


  <!-- Loading state -->
  <div v-if="loading" class="text-center p-10">Loading...</div>
<template v-if="session.role === 'Admin'">
  <AdminDashboard/>
</template>
<template v-else>
  <Navbar />
  <userDashboardPages/>
</template>

</template>

<script setup lang="ts">
import Navbar from "~/components/navbar.vue";
import { reactive,onMounted, ref, computed } from "vue";
import { useRouter } from "vue-router";
import { useSessionStore } from "#imports";
import { useRoomCatalogStore } from "../stores/room_catalog";
import AdminDashboard from "./Admin-Pages/Admin-Dashboard.vue";
import userDashboardPages from "~/components/user-dashboard-pages.vue";

const sessionStore = useSessionStore();
const roomCatalogStore = useRoomCatalogStore();
const router = useRouter();

 // ✅ Implicit type, TypeScript infers `any | null`
const rooms = ref([]);

const session = reactive({
  email: "",
  role: "",
  userId: 0
});
const loading = ref(true);

const sessionEmail = computed(() => session?.email);
const sessionRole = computed(() => session?.role);

onMounted(async () => {
  const s = await sessionStore.getSession();
  if (!s?.email) {
    router.push("/login");
    return;
  }

  session.email = s.email;
  session.role =
    s.role ?? s["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  session.userId = s.userId;

  console.log("USER ROLE:", session.role); // 👈 LOG HERE

  loading.value = false;
});

</script>



