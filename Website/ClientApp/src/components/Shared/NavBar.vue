<template>
  <nav class="navbar navbar-expand-lg navbar-light fixed-top">
    <div class="container-fluid">
      <div class="navbar-header">
        <a class="navbar-brand" @click="onLogoClick" tabindex="1" href="/">
            <h4>HelpMePublish! - Journal Finder</h4>
        </a>
      </div>
      <form class="form-inline">
        <el-dropdown>
          <el-avatar>
            <img src="https://avatars0.githubusercontent.com/u/17098981?s=460&v=4" />
          </el-avatar>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item class="avatar-link">
                <router-link to="/profile">Profile</router-link>
              </el-dropdown-item>
              <el-dropdown-item class="avatar-link">
                <a tabindex="1" @click="onLogout">Logout</a>
              </el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>          
      </form>
    </div>
  </nav>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import useAuthentication from '@/uses/useAuthentication';
import useSearch from '@/uses/useSearch';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: "NavBar",
  setup() {    
    const { logout } = useAuthentication();
    const { resetForm } = useSearch();
    const router = useRouter();

    const onLogoClick = async () =>  {
      await resetForm();
    }

    const onLogout = async () =>  {
      logout();
      await router.push({ name: 'Login' });
    };

    const goHome = async () => await router.push({ name: 'Home' });
    
    return {
      onLogout,
      goHome, 
      onLogoClick
    }
  }
});
</script>

<style scoped>

.navbar {
    height: 100%;
    position: relative;
    background-color: rgb(25, 72, 84);
}

.navbar h4 {
    margin: 0;
    padding: 0;
    font-size: 1.5em;
    color: white;
    font-weight: bold;
}
.avatar-link a {
  text-decoration: none;
  color: black;
}
.avatar-link a:not([href]) {
  text-decoration: none;
  color: black;
}
</style>