<template>
  <el-row class="refine-container">
    <RefineSearchItem v-for="item in items" :key="item.title"
        :title="item.title" :fields="item.fields" />
  </el-row>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue'
import RefineSearchItem from './RefineSearchItem.vue'
import { IRefineItem } from '@/interfaces/Search'


export default defineComponent({
    components: {
        RefineSearchItem,
    },
    props: {
        items: Object as PropType<IRefineItem[]>
    }
})
</script>

