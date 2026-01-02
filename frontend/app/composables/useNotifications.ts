import { ref, onMounted, onUnmounted } from "vue";
import { useNuxtApp } from "#app";
import { HubConnection } from "@microsoft/signalr";

export const useNotifications = () => {
  const notifications = ref<{ userId: number; title: string; message: string; createdAt: string }[]>([]);
  const nuxtApp = useNuxtApp();

  // Cast nuxtApp.$signalR to HubConnection
  const connection = nuxtApp.$signalR as HubConnection | undefined;

  const handleNotification = (data: any) => {
    console.log("SignalR notification received:", data);
    notifications.value.push(data);
  };

  onMounted(() => {
    if (connection) {
      connection.on("ReceiveNotification", handleNotification);
      console.log("SignalR connection state:", connection.state);
    }
  });

  onUnmounted(() => {
    if (connection) {
      connection.off("ReceiveNotification", handleNotification);
    }
  });

  return { notifications };
};
