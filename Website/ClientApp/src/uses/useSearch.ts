import { IResult, ISearchForm} from '@/interfaces/Search';
import { computed, ComputedRef } from 'vue';
import { useStore } from 'vuex';

export default function useSearch(): {
    journals: ComputedRef<IResult[]>;
    formValues: ComputedRef<ISearchForm>;
    search: (payload: ISearchForm) => Promise<void>;
    resetForm: () => Promise<void>;
} {
    const store = useStore();

    const journals = computed((): IResult[] => store.getters['Search/getJournals']);

    const formValues = computed((): ISearchForm => store.getters['Search/getFormValues']);

    async function search(payload: ISearchForm): Promise<void> {
        try {
            await store.dispatch('Search/search', payload);
        } catch (error) {
            console.error(error);
        }
    }

    async function resetForm(): Promise<void> {
        try {
            await store.dispatch('Search/resetForm');
        } catch (error) {
            console.error(error);
        }
    }

    return {
        journals,
        formValues,
        search,
        resetForm
    };
}