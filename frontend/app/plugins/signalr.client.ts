import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { defineNuxtPlugin } from "#app";
import { useSessionStore } from "~/stores/session";

export default defineNuxtPlugin(async (nuxtApp) => {
  // ✅ Call composables here, inside plugin
  const sessionStore = useSessionStore();
  const session = await sessionStore.getSession();

  if (!session) {
    console.warn("No session found, SignalR will not connect");
    return;
  }

  const token = localStorage.getItem("token") ?? "";

  const connection = new HubConnectionBuilder()
    .withUrl(`http://localhost:5080/hubs/notifications?userId=${session.userId}`, {
      accessTokenFactory: () => token,
    })
    .configureLogging(LogLevel.Information)
    .withAutomaticReconnect()
    .build();

  connection.start()
    .then(() => console.log("SignalR connected as user", session.userId))
    .catch(err => console.error("SignalR connection error:", err));

  nuxtApp.provide("signalR", connection);
});
