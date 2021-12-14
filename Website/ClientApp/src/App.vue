<template>
  <el-container>
    <el-header><NavBar ref="navBar" v-if="isLoggedIn"/></el-header>
    <el-main><router-view /></el-main>
  </el-container>
</template>

<script lang="ts">
import { defineComponent, computed } from 'vue';

import NavBar from '@/components/Shared/NavBar.vue';
import useAuthentication from '@/uses/useAuthentication';

export default defineComponent({
  name: 'App',

  components: {
    NavBar
  },
  setup() {
    const { accessToken } = useAuthentication();

    const isLoggedIn = computed(() => accessToken.value !== '');

    return {
      isLoggedIn
    }
  }
});
</script>

<style>
  * {
    box-sizing: border-box;    
    font-family: 'Courier New', Courier, monospace;
  }

  #app {
    height: 100%;
  }

  html, body {
    height: 100%;
  }
  .el-header {
    padding: 0;
  }

  .el-container {
    padding: 0;
    background-color: whitesmoke;
    height: 100%;
    width: 100%;
  }
</style>