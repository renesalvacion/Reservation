import axios from "axios";
import { defineStore } from "pinia";

interface SessionType {
  email: string;
  role: string;
  userId: number;
  name: string
}

export const useSessionStore = defineStore('session', {
  state: () => ({
    session: null as SessionType | null
  }),
  actions: {
    async getSession(): Promise<SessionType | null> {
      const runtime = useRuntimeConfig(); // ✅ move inside the action
      const token = localStorage.getItem("token");
      if (!token) return null;

      try {
        const response = await axios.get(`${runtime.public.backend}/api/Profile/get-session`, {
          headers: { Authorization: `Bearer ${token}` }
        });

        const data = response.data;

        this.session = {
          email: data.email,
          role: data.role,
          userId: data.userId,
          name: data.name
        };

        return this.session;
      } catch (err) {
        console.log("Failed to fetch session", err);
        return null;
      }
    }
  }
});
