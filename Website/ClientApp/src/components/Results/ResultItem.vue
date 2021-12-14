<template>
  <el-card class="result-item m-3">

    <div class="result-item-content">
        <div class="result-item-content-image">
            <img :src="result.imgUrl" alt="journal image" />
        </div>
        <div class="result-item-content-text">
            <div class="result-item-header">
                <div class="result-item-header-title">
                    <h5 class="result-item-header-title-text">
                        <a :href="result.url" target="_blank">{{ result.title }}</a>
                    </h5>

                    
                    <img v-if="result.editorial === 'Sage'" class="editorial-logo" src="../../assets/sage-logo.png" height="35" width="90" alt="sage-logo" />
                    <img v-if="result.editorial === 'Springer'" class="editorial-logo" src="../../assets/springer-logo.png" height="45" width="100" alt="springer-logo" />
                    <img v-if="result.editorial === 'Elsevier'" class="editorial-logo" src="../../assets/elsevier-logo.png" height="45" width="100" alt="elsevier-logo" />
                    <img v-if="result.editorial === 'Wiley'" class="editorial-logo" src="../../assets/wiley-logo.png" height="45" width="100" alt="wiley-logo" />
                    <!--<a :href="result.url" target="_blank">Journal website</a>-->
                </div>
            </div>
            <div class="result-item-metrics">
            </div>
        </div>
    </div>
    
  </el-card>
</template>
<script lang="ts">
import { defineComponent, PropType } from "vue";
import { IJournal } from "@/interfaces/Search";

export default defineComponent({
  props: {
    result: {
      type: Object as PropType<IJournal>,
      required: true
    }
  }
});
</script>

<style scoped>

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
    float:right;
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
