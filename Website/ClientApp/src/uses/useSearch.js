import { computed } from 'vue';
import { useStore } from 'vuex';
export default function useSearch() {
    const store = useStore();
    const journals = computed(() => store.getters['Search/getJournals']);
    async function search(payload) {
        try {
            await store.dispatch('Search/search', payload);
        }
        catch (error) {
            console.error(error);
        }
    }
    return {
        journals,
        search
    };
}
//# sourceMappingURL=useSearch.js.map