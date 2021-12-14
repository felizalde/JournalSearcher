import { ActionContext } from 'vuex';
import {AxiosResponse } from 'axios';


import $http from '../../plugins/axios';

import { IAuthenticationState } from '@/interfaces/Authentication';
import { IRootState } from '@/interfaces/RootState';

export default {
  namespaced: true,

  state: {
    accessToken: localStorage.getItem('access_token') || '',

    username: localStorage.getItem('username') || '',

    firstname: localStorage.getItem('firstname') || '',

    lastname: localStorage.getItem('lastname') || ''
  },

  getters: {
    getAccessToken: (state: IAuthenticationState): string => state.accessToken,

    getUsername: (state: IAuthenticationState): string => state.username,

    getFullName: (state: IAuthenticationState): string => state.firstname + ' ' + state.lastname,
  },

  mutations: {
    SET_ACCESS_TOKEN(state: IAuthenticationState, payload: string): void {
      localStorage.setItem('access_token', payload);
      state.accessToken = payload;
    },

    SET_USERNAME(state: IAuthenticationState, payload: string): void {
      localStorage.setItem('username', payload);
      state.username = payload;
    },

    SET_FIRSTNAME(state: IAuthenticationState, payload: string): void {
        localStorage.setItem('firstname', payload);
        state.firstname = payload;
    },

    SET_LASTNAME(state: IAuthenticationState, payload: string): void {
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
    async authenticate(
      { commit }: ActionContext<IAuthenticationState, IRootState>,
      payload: {username: string, password: string}
    ): Promise<boolean> {
      try {
        const response = await $http.Auth({
            method: 'POST',
            url: `/auth`,
            data: payload
          }) as AxiosResponse<{ token: string, email: string, firstname: string, lastname: string}, any>;

        const accessToken: string = response.data?.token || '';
        const username: string = response.data?.email || '';
        const firstname: string = response.data?.firstname || '';
        const lastname: string = response.data?.lastname || '';
        commit('SET_ACCESS_TOKEN', accessToken);
        commit('SET_USERNAME', username);
        commit('SET_FIRSTNAME', firstname);
        commit('SET_LASTNAME', lastname);
        return accessToken !== '';
      } catch (error: any) {
        throw error.response;
      }
    },

    /**
     * Authenticate an existing user.
     *
     * @param {ActionContext<IAuthenticationState, IRootState>} { commit }
     * @return {*}  {Promise<void>}
     */
     logout(
       { commit }: ActionContext<IAuthenticationState, IRootState>
      ): void {
      try {
        commit('SET_ACCESS_TOKEN', '');
        commit('SET_USERNAME', '');
        commit('SET_FIRSTNAME', '');
        commit('SET_LASTNAME', '');
      } catch (error: any) {
        throw error.response;
      }
    },
  },
};