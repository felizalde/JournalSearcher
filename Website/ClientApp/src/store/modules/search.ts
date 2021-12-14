import { ActionContext } from 'vuex';
import { AxiosResponse } from 'axios';


import $http from '../../plugins/axios';

import { IJournal, ISearchState } from '@/interfaces/Search';
import { IRootState } from '@/interfaces/RootState';

export default {
  namespaced: true,

  state: {
    journals: [],
    selectedJournal: null,
    isLoading: false,
    error: '',
  },

  getters: {
    getJournals: (state: ISearchState): IJournal[] => state.journals,
    getSelectedJournal: (state: ISearchState): IJournal => state.selectedJournal,
    getIsLoading: (state: ISearchState): boolean => state.isLoading,
  },

  mutations: {
    SET_JOURNALS(state: ISearchState, payload: IJournal[]): void {
      state.journals = payload;
    },
    SET_SELECTED_JOURNAL(state: ISearchState, payload: IJournal): void {
      state.selectedJournal = payload;
    },
    SET_IS_LOADING(state: ISearchState, payload: boolean): void {
      state.isLoading = payload;
    }
  },

  actions: {
    /**
     * Search journals.
     *
     * @param {ActionContext<ISearchState, IRootState>} { commit }
     * @return {*}  {Promise<void>}
     */
    async search(
      { commit }: ActionContext<ISearchState, IRootState>,
      payload: { title: string, abstract: string, keywords: string[], impactFactor: { min: number, max: number } }
    ): Promise<void> {
      try {
        const response = await $http.Api({
          method: 'POST',
          url: `/search`,
          data: payload
        }) as AxiosResponse<{ total: number, page: number, size: number, items: IJournal[] }, any>;

        //TODO: Handle pagination
        const journals: IJournal[] = response.data?.items;
        commit('SET_JOURNALS', journals);

      } catch (error: any) {
        throw error.response;
      }
    },

   
  },
};