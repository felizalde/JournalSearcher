<template>
    <div class="el-input-tag input-tag-wrapper">
         <el-tag 
            v-for="tag in model.keywords" 
            :key="tag"
            closable
            type="info"
            :disable-transitions="false"
            @close="handleClose(tag)"
            > {{ tag }} </el-tag>
          <el-input
            v-model="model.keyword"
            class="tag-input"
            placeholder="Enter relevant keywords for your paper"
            clearable
            @change="handleChange"
          ></el-input>
    </div>
</template>

<script lang="ts">
import { defineComponent, reactive } from 'vue'

export default defineComponent({
    props: {
        keywords: {
            type: Array,
            default: () => []
        }
    },
    emits: ['update:keywords'],
    setup(props, { emit }) {
        const model = reactive({
            keywords: props.keywords,
            keyword: ''
        });

        const handleClose = (tag: string) => {
            model.keywords = model.keywords.filter(t => t !== tag);
            emitUpdate();
        };

        const handleChange = (keyword: string) => {
            if (keyword.length > 0) {                
                model.keywords.push(...keyword.split(',').map(k => k.trim()));
                model.keyword = "";
                emitUpdate();
            }
        };

        const emitUpdate = () => {
            emit('update:keywords', model.keywords);
        };

        return {
            model,
            handleClose,
            handleChange
        }
    },
})
</script>

<style>
.input-tag-wrapper {
    display: flex;
    flex-wrap: wrap;
}

.el-tag {
    margin-right: 5px;
    margin-bottom: 5px;
    background-color: #909399;
    color: white;
}

.el-tag i.el-tag__close {
    color: white;
}

.el-tag i.el-tag__close:hover {
    color: #909399;
    background-color: white;
}


</style>