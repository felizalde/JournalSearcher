import axios, { AxiosRequestConfig } from 'axios'
import { ElNotification } from 'element-plus';

import router from '@/router';

const VUE_APP_API_BASE_URL = '/api/';
const $http = {
    Auth: axios.create({
        baseURL: VUE_APP_API_BASE_URL,
        headers: {
            'Access-Control-Allow-Origin': '*',
        },
    }),
    Api: axios.create({
        baseURL: VUE_APP_API_BASE_URL,
        headers: {
            'Access-Control-Allow-Origin': '*',
        },
    }),
};

const accessToken = () => localStorage.getItem('access_token');

const authenticationInterceptor = (config: AxiosRequestConfig) => {
    if (config?.headers)
        config.headers.Authorization = `Bearer ${accessToken()}`;
    return config;
};

$http.Api.interceptors.request.use(authenticationInterceptor);
$http.Api.interceptors.response.use((response) => response, (error) => {
  if (error.response.status === 401) {
    router.push('/login');
  } else if (error.response.status === 403) {
    router.push('/forbidden');
  } else if (error.response.status === 500) {
    ElNotification({
      title: 'Error',
      message: error.response?.data?.error,
      type: 'error',
    });
    router.push('/error');
  }
  return Promise.reject(error);
});

export default $http;