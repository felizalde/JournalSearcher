<template>
  <el-row>
    <el-col :span="20" :offset="2">
      <h3>Find journals</h3>
      <el-form
        label-position="top"
        :model="model"
        :rules="rules"
        ref="form"
        status-icon
        hide-required-asterisk
        @submit.prevent="onSubmit"
      >
        <el-form-item label="Paper Title:" class="form-item" prop="title">
          <el-input
            v-model="model.title"
            placeholder="Enter your paper title"
            clearable
          ></el-input>
        </el-form-item>
        <el-form-item label="Paper Abstract:" class="form-item" prop="abstract">
          <el-input
            v-model="model.abstract"
            placeholder="Enter your paper abstract"
            clearable
            :rows="8"
            type="textarea"
          ></el-input>
        </el-form-item>
        <el-form-item label="Paper Keywords:" class="form-item" prop="keywords">          
          <InputTag v-model:keywords="model.keywords"/>
        </el-form-item>
        <el-form-item class="mb-0">
          <el-button type="text" @click="toggleRefine">
            <em :class="refineIcon"></em>
            Refine your search
          </el-button>
        </el-form-item>
        <RefineSearch v-show="refine" :items="model.setting" />
        <el-form-item>
          <el-button class="float-right mt-4" type="info" native-type="submit"><em class="fas fa-search"></em> Search Journals</el-button>
        </el-form-item>
      </el-form>
    </el-col>
  </el-row>
</template>
<script lang="ts">
import { defineComponent, reactive, ref, computed} from "vue";
import { ElLoading } from "element-plus";
import InputTag  from '../Shared/InputTag.vue';
import useSearch from "@/uses/useSearch";
import { useRouter } from "vue-router";
import RefineSearch from './RefineSearch.vue';
import { IRefineItem } from '@/interfaces/Search';
import clone from 'just-clone';

export default defineComponent({
  components: {
    InputTag,
    RefineSearch
  },
  setup() {    
    const { search, formValues } = useSearch();    
    const router = useRouter();
    const refine = ref(false);

    const toggleRefine = () => refine.value = !refine.value;

    const refineIcon = computed(() => refine.value ? 'fas fa-minus pr-2' : 'fas fa-plus pr-2');

    const form = ref(null);
    
    const model = reactive(clone(formValues.value));

    const rules = {
      title: [
        {
          required: true,
          message: "Please enter your paper title",
          trigger: "blur",
        },
      ],
      abstract: [
        {
          required: true,
          message: "Please enter your paper abstract",
        },
      ],
      keywords: [
        {
          // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-explicit-any 
          validator: (rule : any, value: any, callback: any) => {
            if (model.keywords.length === 0) {
              callback(new Error("Please enter at least one keyword"));
            } else {
              callback();
            }
          },
          trigger: "blur",
        },
      ],
    };

    const onSubmit = async () => {
      const valid = ((form.value as unknown) as { validate: () => boolean }).validate();
      if (!valid) return;

      const loadingInstance = ElLoading.service({ fullscreen: true });
      try {
        await search(model);
        router.push({ name: "Results" });
      } catch (error) {
        console.log(error);//TODO
      } finally {
        loadingInstance.close();
      }
    };

    const onKeywordChanged = (newTags: string[]) => {
      model.keywords = newTags;
    };

    return {
      model,
      rules,
      onSubmit,
      onKeywordChanged,
      form,
      refineIcon,
      refine,
      toggleRefine
    };
  },
});
</script>


<style>
.el-form-item {
  margin-bottom: 1rem;
}

.el-form-item .el-input {
  width: 100%;
}

.form-item > label.el-form-item__label {
  padding: 0;
  margin: 0;
}

.el-form-item__content {
  margin-left: 0;
}

.el-button--text span {
  font-size: var(--el-form-label-font-size);
  color: var(--el-text-color-regular);
}

.el-button--text span:hover {
  text-decoration: underline;
}
</style>