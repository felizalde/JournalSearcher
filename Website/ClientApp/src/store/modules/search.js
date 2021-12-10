import $http from '../../plugins/axios';
export default {
    namespaced: true,
    state: {
        journals: [],
        selectedJournal: null,
        isLoading: false,
        error: '',
    },
    getters: {
        getJournals: (state) => state.journals,
        getSelectedJournal: (state) => state.selectedJournal,
        getIsLoading: (state) => state.isLoading,
    },
    mutations: {
        SET_JOURNALS(state, payload) {
            state.journals = payload;
        },
        SET_SELECTED_JOURNAL(state, payload) {
            state.selectedJournal = payload;
        },
        SET_IS_LOADING(state, payload) {
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
        async search({ commit }, payload) {
            try {
                const response = await $http.Api({
                    method: 'POST',
                    url: `/search`,
                    data: payload
                });
                //TODO: Handle pagination
                const journals = response.data?.items;
                commit('SET_JOURNALS', journals);
            }
            catch (error) {
                throw error.response;
            }
        },
    },
};
//# sourceMappingURL=search.js.map