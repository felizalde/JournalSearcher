import { createStore } from 'vuex';
import Authentication from './modules/authentication';
import Search from './modules/search';
export default createStore({
    modules: {
        Authentication,
        Search
    },
    strict: process.env.NODE_ENV !== 'production',
});
//# sourceMappingURL=index.js.map