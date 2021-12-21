<template>
  <el-col :span="8" class="refine-item">
    <div class="refine-item-container">
      <label class="el-form-item__label refine-item-title text-center pb-0">{{title}}</label>
      <div class="refine-item-header p-1">
        <div class="d-inline el-form-item__label mr-auto"><strong>Journal Fields</strong></div>
        <div class="d-inline el-form-item__label mr-auto"><strong>Boost</strong></div>
      </div>
      <el-form-item v-for="field in model.fields" :key="field.name" class="mb-0">
        <el-checkbox v-model="field.active" 
        :label="field.name" 
        size="small"
        >
        </el-checkbox>
        <el-input-number v-model="field.boost"  :step="0.1" size="mini" />
      </el-form-item>
    </div>
  </el-col>
</template>

<script lang="ts">
import { defineComponent, PropType, reactive } from 'vue'
import { IRefineField } from '@/interfaces/Search'

export default defineComponent({
    props: {
        title: {
            type: String,
            required: true
        },
        fields: {
            type: Object as PropType<IRefineField[]>,
            required: true
        }
    },
    setup(props) {
        const model = reactive({
            fields: props.fields
        });

        return {
            model,
        }
    },
})
</script>

<style>
.el-checkbox__input.is-checked .el-checkbox__inner {
    background-color: #606266;
    border-color: #606266;
}

.el-checkbox__input.is-checked+.el-checkbox__label {
    color: #606266;
}

.el-form-item__label {
    padding-bottom: 0px;
}

label.el-checkbox.el-checkbox--small  {
    max-width: 145px;
    width: 100%;
}

.refine-item-title {
    font-weight: bold;
    border-bottom: 1px solid #d1d1d1;
    margin-bottom: 0;
    padding-bottom: 0;
    text-align: center;
}


.refine-item-header {
    display: flex;
    justify-content: space-around;
}

.refine-item { 
    padding: 5px;
}

.refine-item-container {
    display: flex;
    flex-direction: column;    
    border: 1px solid #e3e3e3;
    border-radius: 4px;
    padding: 5px;
}

</style>