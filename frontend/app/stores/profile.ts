import { defineStore } from 'pinia'
import axios from 'axios'
import { tr } from '@nuxt/ui/runtime/locale/index.js'
import { header } from '#build/ui'
import { useRouter } from 'vue-router';


  //http://localhost:5080/
  //http://10.0.13.21:4000/
const runtime = useRuntimeConfig()

const router = useRouter(); // declare here, at top level
export const useProfileStore = defineStore('profile', {
  state: () => ({
    profiles: [] as any[],
    data: null as any, 
         // To store API response
    error: '',    // To store any error messages
    loading: false
  }),

  

  actions: {
    async postProfile(payload:any) {
      try {
        const formData = new FormData()
        
        // Append data from the payload to FormData
        formData.append("name", payload.name)
        formData.append("email", payload.email)
        formData.append("password", payload.password)
        formData.append("isActive", "true"); // must append as string

        
        // If a profile file is provided, append it to the form
        if (payload.profile) {
          formData.append("profile", payload.profile)  // Expecting a file (e.g., image)
        }

        // Send POST request to the API
        const response = await axios.post(`${runtime.public.backend}/api/Profile`, formData, {
          headers: {
            'Content-Type': 'multipart/form-data',  // Ensures the API receives form data properly
          },
        })

        console.log(response);
        

        // Set the response data to the state
        this.data = response.data
      } catch (error:any) {
        // Handle any errors and store the error message
        this.error = error.response?.data || error.message
      }
    },


    async getData() {
      this.loading = true
      this.error = ''
      try {
        const token = localStorage.getItem("token"); // should match what backend expects
        const response = await axios.get(`${runtime.public.backend}/api/Profile`,{
          headers:{Authorization: `Bearer ${token}`}
        })
        this.profiles = response.data
      } catch (error: any) {
        this.error = error.response?.data || error.message
      } finally {
        this.loading = false
      }
    },

async Login(payload: any, router: any) {  // pass router
  this.loading = true
  this.error = ''

  try {
    const response = await axios.post(
      `${runtime.public.backend}/api/Profile/login`,
      payload,
      { headers: { 'Content-Type': 'application/json' }, withCredentials:true}
    );

    if (response.status === 200) {
      const token = response.data.token;
      localStorage.setItem('token', token);
      this.data = response.data;

      this.loading = false;

      // Redirect if user is inactive
      if (!response.data.isActive) {
        router.push('/deactivated'); // ✅ works now
        return false;
      }

      return true;
    } else {
      this.loading = false;
      return false;
    }
  } catch (error: any) {
    this.error = error.response?.data?.message || error.message;
    localStorage.removeItem("token");
    this.loading = false;
    return false;
  }
},

    async Logout() {
      this.loading = true
      this.error = ''

      try {
        const response = await axios.post(
          `${runtime.public.backend}/api/Profile/logout`,
          {},
          { withCredentials: true }
        )

        if (response.status === 200) {
          this.loading = false
          return true
        } else {
          this.error = "Logout failed"
          this.loading = false
          return false
        }
      } catch (error: any) {
        this.error = error.message
        this.loading = false
        return false
      }
    },

    async ViewProfile(userId: number){
      try {
        const token = localStorage.getItem("token"); // should match what backend expects
        const response = await axios.get(`${runtime.public.backend}/api/Profile/${userId}`,{
          headers:{Authorization: `Bearer ${token}`}
        })
        if(response.status === 200){
          console.log("User Profile:", JSON.stringify(response.data, null, 2));

          
          return response.data
        }else{
          alert("Theres something wrong on the fetching.")
        }
      } catch (error:any) {
        alert("error: "+ error.Message)
      }
    },

    async UserUpdateAccount(userId: number, payload: []){
      try {
        const token = localStorage.getItem("token"); // should match what backend expects
        const response = await axios.put(`${runtime.public.backend}/api/Profile/User-Update-Account/${userId}`, payload, {
          headers:{Authorization: `Bearer ${token}`,'Content-Type': 'multipart/form-data',}
        })

        if(response.status === 200){
          alert("User Update Successfully: " + response.status)
          return true
        }else{
          alert("User Update Failed: " + response.status)
          return false
        }
      } catch (error: any) {
        alert("User Update Failed: " + error.Message)
        return false;
      }
    },

    async DeactivateUser(userId: number, payload: []){
      try {
        const token = localStorage.getItem("token"); // should match what backend expects
        const response = await axios.patch(`${runtime.public.backend}/api/Profile/User-Deactivate-Profile/${userId}`, payload,{
          headers:{Authorization: `Bearer ${token}`,'Content-Type': 'application/json'}
        })

        if(response.status === 200){
          alert("Deactivate Profile Successfully")
          this.Logout()
          return true
        }else{
          alert("Deactivate Profile Failed")
          return false
        }
      } catch (error: any) {
        alert("Error Deactivate Profile: " + error.Message)
      }
    },

        async ActivateUser(userId: number, payload: []){
      try {
        const token = localStorage.getItem("token"); // should match what backend expects
        const response = await axios.patch(`${runtime.public.backend}/api/Profile/User-Deactivate-Profile/${userId}`, payload,{
          headers:{Authorization: `Bearer ${token}`,'Content-Type': 'application/json'}
        })

        if(response.status === 200){
          alert("Activate Profile Successfully")
          return true
        }else{
          alert("Activate Profile Failed")
          return false
        }
      } catch (error: any) {
        alert("Error Deactivate Profile: " + error.Message)
      }
    },



  }
})
