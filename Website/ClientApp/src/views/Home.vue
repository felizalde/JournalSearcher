<template>
  <div class="home container">
    <SearchForm />
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted } from 'vue';
import SearchForm from '@/components/Home/SearchForm.vue';
import { ElLoading } from 'element-plus';


export default defineComponent({
  name: 'Home',
  components: {
    SearchForm,
  },

  setup() {

    onMounted(async() => {
      const loadingInstance = ElLoading.service({ fullscreen: true });
      try {        
        //await fetchConfigurators();
      } catch (error) {
        console.error(error);
      } finally {
        loadingInstance.close();
      }
    });

    return {
    }
  }
});
</script>

<style scoped>
.home {
  padding: 20px;
}
</style>
