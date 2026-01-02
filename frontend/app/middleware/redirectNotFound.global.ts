// middleware/redirectNotFound.global.ts
export default defineNuxtRouteMiddleware((to, from) => {
  // Access router to get list of all valid routes
  const routeMatched = to.matched.length > 0

  if (!routeMatched) {
    return navigateTo('/dashboard')
  }

  // If it's a valid route, do nothing (continue as normal)
})