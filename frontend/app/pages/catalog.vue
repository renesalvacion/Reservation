<template>
  <div>
    <template v-if="session.role === 'Admin'">
      <AdminCatalog />
    </template>

    <template v-else>
      <Navbar />
      <userRoom />
    </template>
  </div>
</template>


<script setup lang="ts">
import Navbar from "~/components/navbar.vue";
import { reactive,onMounted, ref, computed } from "vue";
import { useRouter } from "vue-router";
import { useSessionStore } from "#imports";
import { useRoomCatalogStore } from "../stores/room_catalog";
import AdminCatalog from "./Admin-Pages/Admin-Catalog.vue";
import userRoom from "./User-Pages/user-room.vue";
import view_catalog from "~/components/view_catalog.vue";

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


const sessionEmail = computed(() => session?.email);
const sessionRole = computed(() => session?.role);

onMounted(async () => {
  const s = await sessionStore.getSession();
  if (!s?.email) {
    router.push("/login");
    return;
  }

  // Assign properties to reactive object
  session.email = s.email;
  session.role = s.role ?? s["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  session.userId = s.userId;
});
</script>



