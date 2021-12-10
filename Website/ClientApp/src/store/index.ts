import { createStore } from 'vuex'

import { IRootState } from '@/interfaces/RootState';


import Authentication from './modules/authentication';
import Search from './modules/search';

export default createStore<IRootState>({
  modules: {
    Authentication,
    Search
  },
  strict: process.env.NODE_ENV !== 'production',
});