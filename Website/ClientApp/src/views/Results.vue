<template>
  <div class="results container">
    <el-row>
      <el-col :span="24">
        <div class="result-header">
          <h4>Showing {{ count }} journals matching your paper!</h4>
          <div class="result-header-buttons">
            <el-button type="info" plain size="small" @click="onEditClick"><em class="fas fa-edit"></em> Edit Search</el-button>
            <el-button type="info" plain size="small" @click="onNewClick"><em class="fas fa-search"></em> New Search</el-button>
          </div>
        </div>

        <ResultItem v-for="(r, i) in results" :key="i" :result="r"/>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import ResultItem from "@/components/Results/ResultItem.vue";
import useSearch from "@/uses/useSearch";
import { useRouter } from "vue-router";

export default defineComponent({
  components: {
    ResultItem,
  },
  setup() {
    const { journals, resetForm } = useSearch();
    const router = useRouter();
    const count = journals.value.length;

    const onEditClick = async () => await router.push({ name: "Home" });

    const onNewClick = async () => {
      resetForm();
      await router.push({ name: "Home" });
    };

    return {
      count,
      results: journals,
      onEditClick,
      onNewClick,
    };
  },
});
</script>

<style scoped>

.result-header {
  padding: 5px 20px;
  display: flex;
  border-bottom: 1px solid #e8e8e8;
}

.result-header-buttons {
  display: flex;
  justify-content: flex-end;
  margin-left: auto;
}

</style>