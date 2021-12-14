import { createRouter, createWebHistory } from 'vue-router';
import Home from '../views/Home.vue';
import Login from '../views/Login.vue';
import Results from '../views/Results.vue';
const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home,
        meta: { requiresAuth: true }
    },
    {
        path: '/results',
        name: 'Results',
        component: Results,
        meta: { requiresAuth: true }
    },
    {
        path: '/login',
        name: 'Login',
        component: Login
    },
    {
        path: '/:pathMatch(.*)*',
        redirect: '/'
    }
];
const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
});
router.beforeEach((to, from, next) => {
    const isAuthenticated = localStorage.getItem('access_token');
    if (to.matched.some((record) => record.meta.requiresAuth) &&
        !isAuthenticated)
        next({ name: 'Login' });
    else
        next();
});
export default router;
//# sourceMappingURL=index.js.map