<template>
  <div class="results container">
    <el-row>
      <el-col :span="24">
        <h4>Showing {{ count }} journals matching your paper!</h4>
        <ResultItem v-for="(r, i) in results" :key="i" :result="r"/>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import ResultItem from "@/components/Results/ResultItem.vue";
import useSearch from "@/uses/useSearch";

export default defineComponent({
  components: {
    ResultItem,
  },
  setup() {
    const count = "XX";
    const { journals } = useSearch();
    return {
      count,
      results: journals,
    };
  },
});
</script>