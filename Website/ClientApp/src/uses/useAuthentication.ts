import { computed, ComputedRef } from 'vue';
import { useStore } from 'vuex';

export default function useAuthentication(): {
    accessToken: ComputedRef<string>;
    username: ComputedRef<string>;
    authenticate: (payload: { username: string, password: string }) => Promise<boolean>;
    logout: () => void;
} {
    const store = useStore();

    const accessToken = computed((): string => store.getters['Authentication/getAccessToken']);

    const username = computed((): string => store.getters['Authentication/getUsername']);

    async function authenticate(payload: { username: string, password: string }): Promise<boolean> {
        try {
            const success = await store.dispatch('Authentication/authenticate', payload);
            return success;
        } catch (error) {
            console.error(error);
            return false;
        }
    }

    function logout(): void {
        try {
            store.dispatch('Authentication/logout');
        } catch (error) {
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