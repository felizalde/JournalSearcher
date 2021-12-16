import { IResult } from '@/interfaces/Search';
import { computed, ComputedRef } from 'vue';
import { useStore } from 'vuex';

export default function useSearch(): {
    journals: ComputedRef<IResult[]>;
    search: (payload: {title: string, abstract: string, keywords: string[], impactFactor: { min: number, max: number }}) => Promise<void>;
} {
    const store = useStore();

    const journals = computed((): IResult[] => store.getters['Search/getJournals']);

    async function search(payload: {title: string, abstract: string, keywords: string[], impactFactor: { min: number, max: number }}): Promise<void> {
        try {
            await store.dispatch('Search/search', payload);
        } catch (error) {
            console.error(error);
        }
    }

    return {
        journals,
        search
    };
}