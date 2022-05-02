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
            <div class="score-container-tex text-centert">
              <h6 class="score-container-text-text">Score</h6>
              <span class="score-container-text-score">{{ result.score }}</span>
            </div>
            <el-divider direction="vertical"></el-divider>
            <div
              class="journal-metrics-container"
              v-if="filteredMetrics"
            >
              <div
                class="journal-metrics-container-text"
                v-for="m in filteredMetrics"
                :key="m.id"
              >
                <h6 class="journal-metrics-container-text-text">{{
                  capitalize(m.name)
                }}</h6>
                <span class="journal-metrics-container-text-score">{{
                  format(m.name, m.value)
                }}</span>
              </div>
            </div>
          </div>
        </div>
        <div class="result-item-position">
          <div>#{{ position }}</div>
        </div>
      </div>
    </div>
  </el-card>
</template>
<script lang="ts">
import { defineComponent, PropType } from "vue";
import { IResult } from "@/interfaces/Search";
import EditorialLogo from "./EditorialLogo.vue";
import { FormatMetricValue, 
    FilterMetricsToShow, 
    CapitalizeFirstLetterOfEachWord } from "@/utils/MetricsHelper";

export default defineComponent({
  components: {
    EditorialLogo
  },
  props: {
    result: {
      type: Object as PropType<IResult>,
      required: true,
    },
    position: {
      type: Number,
      required: true
    }
  },
  setup(props) {
    const format = (metric: string, value: number) => FormatMetricValue(metric, value);
    const capitalize = (str: string) => CapitalizeFirstLetterOfEachWord(str);
    return {
      filteredMetrics: FilterMetricsToShow(props.result.document.metrics),
      format,
      capitalize
    }
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
  height: 75%;
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
  min-width: 115px;
  max-width: 115px;
}

.result-item-content-text {
  font-size: 0.7rem;
  margin: 5px;
  width: 100%;
  display: flex;
  flex-direction: column;
  align-content: space-between;
}

.score-container {
  display: flex;
  flex-direction: row;
  column-gap: 15px;
  height: 80%;
  padding: 0 2rem;
}

.score-container-text {
  display: flex;
  flex-direction: column;
}

.score-container .el-divider {
  height: 100%;
  margin: 0;
}

.score-container h6 {
  font-size: 0.9rem;
  margin: 0;
}

.score-container span {
  font-size: 1rem;
  font-weight: bold;
}

.journal-metrics-container-text {
  width: 300px;
  text-align: center;
}

.journal-metrics-container {
    display: flex;
    align-items: flex-start;
    flex-direction: column;
    flex-wrap: wrap;
    align-content: space-between;
}

.result-item-position {
  font-size: 0.9rem;
  font-weight: bold;
  font-style: italic;
  width: 100%;
}

.result-item-position > div {
  text-align: right;
}
</style>
