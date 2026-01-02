import { defineStore } from 'pinia'
import axios from 'axios'
import { useSessionStore } from '#imports'
import { tr } from '@nuxt/ui/runtime/locale/index.js'
import { user } from '#build/ui'

const runtime = useRuntimeConfig()

interface NotificationItem {
  id: number
  title: string
  message: string
  read: boolean
  time: string
}

export const SetAppointment = defineStore('SetAppointment', {
  state: () => ({
    loading: false,
    message: '',
    error: null as string | null,
    selectedRoom: null as any | null,
    Appointments : [] as any [],
    AdminViewAppointment : [] as any [],
    totalCount: 0,
    pageNumber: 1,
    pageSize: 10,
    totalPages: 1,
    adminApp : [] as any [],
    countNotif : [] as any [],
    notifArry: [] as any []
  }),

  //http://localhost:5080/
  //http://10.0.13.21/backend/
  actions: {
    setSelectedRoom(room: any) {
      this.selectedRoom = room
    },

    async SetApp(payload: any, userId: number) {
      try {
        this.loading = true
        const token = localStorage.getItem("token");
        const response = await axios.post(
          `${runtime.public.backend}/api/Appointment/${userId}`,
          payload,
          {
            withCredentials: true,
            headers: { 'Content-Type': 'application/json',Authorization: `Bearer ${token}` }
          }
        )

        this.loading = false

        if (response.status === 200) {
          this.message = 'Set Appointment Successfully'
          return true
        } else {
          this.error = 'Failed to set appointment'
          return false
        }
      } catch (err: any) {
        this.loading = false
        this.error = err.message
        return false
      }
    },

    async getAppointment(userId : number) {
      try {

        const response = await axios.get(`${runtime.public.backend}/api/Appointment/${userId}`)

        this.Appointments = response.data.appointments || []
        console.log(this.Appointments)
      } catch (error: any) {
        alert(error.message || 'Failed to fetch appointments')
        this.Appointments = []
      }
    },


async ViewAdminCatalog(pageNumber = 1, pageSize = 10) {
  try {
    const res = await axios.get(
      `${runtime.public.backend}/api/Appointment/view-appointment-admin?pageNumber=${pageNumber}&pageSize=${pageSize}`,
      { withCredentials: true }
    );

    console.log("Backend response:", res.data);

    // Use the correct field returned by your backend
    const appointments = res.data.appointments || res.data.results || [];
    console.log("Appointments data:", appointments);

    this.AdminViewAppointment = appointments;
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
    this.totalCount = res.data.totalCount || appointments.length;
    this.totalPages = Math.ceil(this.totalCount / this.pageSize);

  } catch (err) {
    console.error("Error fetching paginated appointments:", err);
  }
},



    async AdminViewUserAppointment(userId: number) {
      try {
        const token = localStorage.getItem("token");
        const response = await axios.get(`${runtime.public.backend}/api/Appointment/view-appointment-admin/${userId}`, {
          headers:{Authorization: `Bearer ${token}`},withCredentials : true
        })

        if (response.status === 200) {
          this.adminApp = response.data.results
          console.log("Viewe Admin Appointment: " + response.data.results);
          
          return this.adminApp
        } else {
          this.adminApp = []
          return this.adminApp
        }
      } catch (error: any) {
        alert("Error: " + error.Message)
        this.adminApp = []
        return this.adminApp
      }
    },

async AdminUpdateStatus(appointmentId: number, payload: object) {
  try {
    const token = localStorage.getItem("token"); // check if token exists
    const response = await axios.post(
      `${runtime.public.backend}/api/Appointment/Admin-Update-Status-Appointment/${appointmentId}`,
      payload,
      {
        headers: { 
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        withCredentials: true,
      }
    );

    if (response.status === 200) {
      alert("Update Status Successfully");
      return true;
    } else {
      alert("Update Status Failed");
      return response.status;
    }
  } catch (error: any) {
    const msg = error.response?.data?.message || error.message || "Unknown error";
    alert("Error Admin Update Status: " + msg);
    return error;
  }
},

    async cancelReservation(userId: number, appId: number){
      try {
         const token = localStorage.getItem("token"); // should match what backend expects
        const response = await axios.post(`${runtime.public.backend}/api/Appointment/UserCancelReservation/${userId}`,{ appointmentId: appId }, {
          withCredentials:true, headers:{Authorization: `Bearer ${token}`,'Content-Type' : 'application/json'}
        })

        if(response.status === 200){
          alert("Cancel Reservation Successfully")
          return true
        }else{
          alert("Cancel Reservation Failed")
          return false
        }
      } catch (error: any) {
        alert("Error Message: " + error.Message)
        return false;
      }
    },

    async AdminUpdateReservation(adminId : number, payload : []){
        try {
          const response = await axios.post(`${runtime.public.backend}/api/Appointment/Admin-Update-Reservation/${adminId}`, payload, {
            headers : {'Content-Type' : 'application/json'}, withCredentials : true
          })

          if(response.status === 200){
            alert("Successfully Update")
            return true;
          }else{
            alert(response.data.Message)
            return false;
          }
        } catch (error: any) {
          alert("Error: " + error.Message )
          return false;
        }
    },

    async UserUpdateReservation(userId : number, payload : []){
      try {
        const response = await axios.post(`${runtime.public.backend}/api/Appointment/UserUpdateReservation/${userId}`, 
          payload, {headers : {'Content-Type' : 'application/json'}, withCredentials:true}
        )

        if(response.status === 200){
          alert("You Successfully Update Your Reservation")
          return true;
        }else{
          alert("Update Reservation Failed, Theres Something wrong On Updating Reservation")
          return false;
        }
      } catch (error: any) {
        alert("Error: " + error.Message)
        return false;
      }
    },

    async getCountNotif(userId: number): Promise<{
      countNotif: number
      notifArry: any[]
    }> {
      try {
        const response = await axios.get(
          `http://localhost:5080/api/Appointment/notif/${userId}`,
          { withCredentials: true }
        )

        return {
          countNotif: response.data.count,
          notifArry: response.data.notifications
        }

      } catch (error: any) {
        console.error(error.message)
        throw error
      }
    },

    async approvedReservation(userId: number){
      try {
        const response = await axios.post(
        `http://localhost:5080/approve/${userId}`,
        {},
        {
          withCredentials: true, 
          headers:{
            'Content-Type' : 'application/json'
          }
      })

      if(response.status === 200){
        alert("Approve Reservation Successfully")
        return true
      }else{
        alert("Approve Reservation Failed")
        return false
      }
      } catch (error: any) {
        alert("Error Message: " + error.Message)
        return false
      }
    }

  }
})
