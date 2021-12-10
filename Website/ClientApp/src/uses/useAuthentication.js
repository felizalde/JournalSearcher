import { computed } from 'vue';
import { useStore } from 'vuex';
export default function useAuthentication() {
    const store = useStore();
    const accessToken = computed(() => store.getters['Authentication/getAccessToken']);
    const username = computed(() => store.getters['Authentication/getUsername']);
    async function authenticate(payload) {
        try {
            const success = await store.dispatch('Authentication/authenticate', payload);
            return success;
        }
        catch (error) {
            console.error(error);
            return false;
        }
    }
    function logout() {
        try {
            store.dispatch('Authentication/logout');
        }
        catch (error) {
            console.error(error);
        }
    }
    return {
        accessToken,
        username,
        authenticate,
        logout
    };
}
//# sourceMappingURL=useAuthentication.js.map