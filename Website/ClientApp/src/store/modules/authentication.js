import $http from '../../plugins/axios';
export default {
    namespaced: true,
    state: {
        accessToken: localStorage.getItem('access_token') || '',
        username: localStorage.getItem('username') || '',
        firstname: localStorage.getItem('firstname') || '',
        lastname: localStorage.getItem('lastname') || ''
    },
    getters: {
        getAccessToken: (state) => state.accessToken,
        getUsername: (state) => state.username,
        getFullName: (state) => state.firstname + ' ' + state.lastname,
    },
    mutations: {
        SET_ACCESS_TOKEN(state, payload) {
            localStorage.setItem('access_token', payload);
            state.accessToken = payload;
        },
        SET_USERNAME(state, payload) {
            localStorage.setItem('username', payload);
            state.username = payload;
        },
        SET_FIRSTNAME(state, payload) {
            localStorage.setItem('firstname', payload);
            state.firstname = payload;
        },
        SET_LASTNAME(state, payload) {
            localStorage.setItem('lastname', payload);
            state.lastname = payload;
        },
    },
    actions: {
        /**
         * Authenticate an existing user.
         *
         * @param {ActionContext<IAuthenticationState, IRootState>} { commit }
         * @return {*}  {Promise<void>}
         */
        async authenticate({ commit }, payload) {
            try {
                const response = await $http.Auth({
                    method: 'POST',
                    url: `/auth`,
                    data: payload
                });
                const accessToken = response.data?.token || '';
                const username = response.data?.email || '';
                const firstname = response.data?.firstname || '';
                const lastname = response.data?.lastname || '';
                commit('SET_ACCESS_TOKEN', accessToken);
                commit('SET_USERNAME', username);
                commit('SET_FIRSTNAME', firstname);
                commit('SET_LASTNAME', lastname);
                return accessToken !== '';
            }
            catch (error) {
                throw error.response;
            }
        },
        /**
         * Authenticate an existing user.
         *
         * @param {ActionContext<IAuthenticationState, IRootState>} { commit }
         * @return {*}  {Promise<void>}
         */
        logout({ commit }) {
            try {
                commit('SET_ACCESS_TOKEN', '');
                commit('SET_USERNAME', '');
                commit('SET_FIRSTNAME', '');
                commit('SET_LASTNAME', '');
            }
            catch (error) {
                throw error.response;
            }
        },
    },
};
//# sourceMappingURL=authentication.js.map