<template>
  <div class="flex h-screen bg-gray-100">
    <!-- Sidebar -->
    <div class="w-64 bg-white border-r flex flex-col">
      <div class="p-4 border-b font-bold text-xl">Chats</div>

      <div class="flex-1 overflow-y-auto">
        <div
          v-for="contact in contacts"
          :key="contact.id"
          class="p-3 cursor-pointer hover:bg-gray-100 flex items-center gap-3"
          @click="selectContact(contact)"
        >
          <img :src="contact.avatar" class="w-10 h-10 rounded-full" />
          <div>
            <p class="font-semibold">{{ contact.name }}</p>
            <p class="text-gray-500 text-sm truncate">
              {{ contact.lastMessage }}
            </p>
          </div>
        </div>
      </div>
    </div>

    <!-- Chat Area -->
    <div class="flex-1 flex flex-col">
      <div class="p-4 border-b font-bold text-lg">
        {{ selectedContact?.name || "Select a chat" }}
      </div>

      <!-- Messages -->
      <div class="flex-1 p-4 overflow-y-auto space-y-3 messages-container">
        <div
          v-for="(msg, i) in messages"
          :key="i"
          :class="{
            'flex justify-end': msg.sender === 'me',
            'flex justify-start': msg.sender === 'other',
            'flex justify-center': msg.sender === 'server'
          }"
        >
          <div
            class="max-w-xs px-4 py-2 rounded-lg"
            :class="msg.sender === 'me'
              ? 'bg-blue-500 text-white'
              : msg.sender === 'server'
              ? 'bg-green-100 text-green-600'
              : 'bg-gray-200 text-gray-900'"
          >
            <div
              v-if="msg.isAdmin && msg.sender === 'other'"
              class="text-[11px] font-semibold text-gray-600 mb-0.5"
            >
              {{ msg.displaySender }}
            </div>

            <div class="text-sm">{{ msg.content }}</div>

            <div
              v-if="msg.createdAt"
              class="text-[10px] opacity-70 mt-1 text-right"
            >
              {{ formatTime(msg.createdAt) }}
            </div>
          </div>
        </div>
      </div>

      <!-- Input -->
      <div class="p-4 border-t flex gap-3">
        <input
          v-model="newMessage"
          @keyup.enter="sendMessage"
          placeholder="Type a message..."
          class="flex-1 border rounded-full px-4 py-2"
        />
        <button
          @click="sendMessage"
          class="bg-blue-500 text-white px-4 py-2 rounded-full"
        >
          Send
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, nextTick } from "vue"
import * as signalR from "@microsoft/signalr"
import { useSessionStore } from "#imports"

interface ChatMessage {
  sender: "me" | "other" | "server"
  content: string
  createdAt?: string
  displaySender?: string
  isAdmin?: boolean
}

const sessionStore = useSessionStore()
const session = ref(sessionStore.session)

const contacts = ref([
  { id: 87, name: "Admin", avatar: "https://i.pravatar.cc/40?img=12", lastMessage: "" }
])

const selectedContact = ref<any>(null)
const messages = ref<ChatMessage[]>([])
const newMessage = ref("")

let connection: signalR.HubConnection | null = null

/* ---------------- HELPERS ---------------- */

const scrollToBottom = () => {
  nextTick(() => {
    const el = document.querySelector(".messages-container") as HTMLElement
    if (el) el.scrollTop = el.scrollHeight
  })
}

const formatTime = (date: string) =>
  new Date(date).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })

/* ---------------- CHAT ---------------- */

const selectContact = async (contact: any) => {
  selectedContact.value = contact
  messages.value = []

  // 🔥 OPTIONAL: load history via API here
}

const sendMessage = async () => {
  if (!newMessage.value.trim() || !selectedContact.value) return
  if (connection?.state !== signalR.HubConnectionState.Connected) return

  await connection.invoke(
    "SendMessage",
    selectedContact.value.id,
    newMessage.value
  )

  newMessage.value = ""
}

/* ---------------- SIGNALR ---------------- */

onMounted(async () => {
  const token = localStorage.getItem("token")
  if (!token) return

  connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5080/hubs/messenger", {
      accessTokenFactory: () => token
    })
    .withAutomaticReconnect()
    .build()

  connection.on("ReceiveMessage", (msg) => {
    const myId = session.value?.userId?.toString()
    const isMe = msg.senderId === myId
    const isAdmin = msg.senderRole === "Admin"

    messages.value.push({
      sender: isMe ? "me" : "other",
      content: msg.content,
      createdAt: msg.createdAt,
      displaySender: msg.senderName,
      isAdmin
    })

    scrollToBottom()
  })

  await connection.start()

  messages.value.push({
    sender: "server",
    content: "💬 Connected"
  })
})

onBeforeUnmount(() => {
  connection?.stop()
})
</script>

<style scoped>
.messages-container::-webkit-scrollbar {
  width: 6px;
}
.messages-container::-webkit-scrollbar-thumb {
  background: rgba(0, 0, 0, 0.2);
  border-radius: 3px;
}
</style>
