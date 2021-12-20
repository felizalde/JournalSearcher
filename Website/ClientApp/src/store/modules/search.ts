import { ActionContext } from 'vuex';
import { AxiosResponse } from 'axios';


import $http from '../../plugins/axios';

import { IResult, ISearchForm, ISearchState } from '@/interfaces/Search';
import { IRootState } from '@/interfaces/RootState';
import { RefineInitialValues } from '@/utils/RefineSettings';

export default {
  namespaced: true,

  state: {
    journals: [],
    isLoading: false,
    error: '',
    form: {
      title: '',
      abstract: '',
      keywords: [],
      setting: RefineInitialValues
    }
  },

  getters: {
    getJournals: (state: ISearchState): IResult[] => state.journals,
    getFormValues: (state: ISearchState): ISearchForm => state.form,
    getIsLoading: (state: ISearchState): boolean => state.isLoading,
  },

  mutations: {
    SET_JOURNALS(state: ISearchState, payload: IResult[]): void {
      state.journals = payload;
    },
    SET_FORM_VALUES(state: ISearchState, payload: ISearchForm): void {
      state.form = payload;
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
      payload: ISearchForm
    ): Promise<void> {
      try {
        commit('SET_IS_LOADING', true);
        const response = await $http.Api({
          method: 'POST',
          url: `/search`,
          data: payload
        }) as AxiosResponse<{ total: number, page: number, size: number, items: IResult[] }, any>;

        //TODO: Make Pagination in front-end
        const journals: IResult[] = response.data?.items;
        commit('SET_JOURNALS', journals);
        commit('SET_FORM_VALUES', payload);
        commit('SET_IS_LOADING', false);

      } catch (error: any) {
        throw error.response;
      }
    },

    /**
     * Reset search form.
     * @param {ActionContext<ISearchState, IRootState>} { commit }
     * @return {*}  {Promise<void>}
     */
    async resetForm({ commit }: ActionContext<ISearchState, IRootState>): Promise<void> {
      return new Promise<void>(resolve =>  {
        commit('SET_FORM_VALUES', {
          title: '',
          abstract: '',
          keywords: [],
          setting: RefineInitialValues
        });
        resolve();
      });
    }


   
  },
};