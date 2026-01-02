import { useSessionStore } from "../stores/session";
import { navigateTo } from "#app";

declare const process: {
  client: boolean;
  server: boolean;
};

export default defineNuxtRouteMiddleware(async (to, from) => {
  const sessionStore = useSessionStore();
  const s = await sessionStore.getSession();
  if (!s?.email) {
    localStorage.removeItem("token");
    return navigateTo('/');
  }

  if (process.client) {
    await sessionStore.getSession();

    if (!sessionStore.session?.email) {
      localStorage.removeItem("token");
      return navigateTo('/');
    }


  }
});
