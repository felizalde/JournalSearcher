<template>
  <el-card class="result-item m-3">
    <div class="result-item-content">
      <div class="result-item-content-image">
        <img :src="result.document.imgUrl" alt="journal image" />
      </div>
      <div class="result-item-content-text">
        <div class="result-item-header">
          <div class="result-item-header-title">
            <h5 class="result-item-header-title-text">
              <a :href="result.document.url" target="_blank">{{
                result.document.title
              }}</a>
            </h5>
            <EditorialLogo :editorial="result.document.editorial" />
          </div>
        </div>
        <div class="result-item-metrics">
          <div class="score-container">
            <div class="score-container-text">
              <span class="score-container-text-text">Score: </span>
              <span class="score-container-text-score">{{ result.score }}</span>
            </div>
            <el-divider direction="vertical"></el-divider>
            <div
              class="journal-metrics-container"
              v-if="result.document.metrics"
            >
              <div
                class="journal-metrics-container-text"
                v-for="m in result.document.metrics"
                :key="m.id"
              >
                <span class="journal-metrics-container-text-text">{{
                  m.name
                }}: </span>
                <span class="journal-metrics-container-text-score">{{
                  m.value
                }}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </el-card>
</template>
<script lang="ts">
import { defineComponent, PropType } from "vue";
import { IResult } from "@/interfaces/Search";
import EditorialLogo from "./EditorialLogo.vue";

export default defineComponent({
  components: {
    EditorialLogo
  },
  props: {
    result: {
      type: Object as PropType<IResult>,
      required: true,
    },
  },
});
</script>

<style>
.result-item {
  width: 100%;
  max-height: 300px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.result-item-header {
  height: 70px;
}

.result-item-header-title {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
}

.result-item-header-title-text {
  font-size: 1.2rem;
  font-weight: bold;
}

.result-item-header-title a {
  margin-left: auto;
  color: var(--el-text-color-primary);
  font-weight: bold;
}

.editorial-logo {
  float: right;
}

.result-item-metrics {
  height: 100%;
}

.result-item-content {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  max-height: 180px;
}

.result-item-content-image {
  background-color: #f5f5f5;
  margin: 5px;
  border: 1px solid #ececec;
}

.result-item-content-image img {
  width: auto;
  height: auto;
  max-height: 150px;
}

.result-item-content-text {
  font-size: 0.7rem;
  margin: 5px;
  width: 100%;
  display: flex;
  flex-direction: column;
  align-content: space-between;
}
</style>
