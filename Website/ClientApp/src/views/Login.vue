<template>
  <div class="container-fluid login">
    <el-row class="w-100">
      <el-col :span=8 :offset=8>
        <el-card>
          <h3 class="text-center">Login</h3>
          <div class="contact-form px-4 mx-3 pb-3">
            <el-form
              class="login-form"
              :model="model"
              :rules="rules"
              ref="form"
              @submit.prevent="login"
            >
              <el-form-item prop="username">
                <el-input
                  v-model="model.username"
                  placeholder="Username"
                  prefix-icon="fas fa-user"
                ></el-input>
              </el-form-item>
              <el-form-item prop="password">
                <el-input
                  v-model="model.password"
                  placeholder="Password"
                  type="password"
                  prefix-icon="fas fa-lock"
                ></el-input>
              </el-form-item>
              <el-form-item>
                <el-button
                  :loading="loading"
                  class="login-button"
                  type="primary"
                  native-type="submit"
                  block
                  >Login</el-button
                >
              </el-form-item>
            </el-form>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive, ref } from "vue";
import { ElLoading } from "element-plus";
import useAuthentication from "@/uses/useAuthentication";
import { useRouter } from "vue-router";

export default defineComponent({
  name: "Login",
  setup() {
    const { authenticate } = useAuthentication();
    const router = useRouter();
    const loading = ref(false);
    const form = ref(null);
    const model = reactive({ username: "", password: "" });
    const rules = {
      username: [
        {
          required: true,
          message: "Username is required",
          trigger: "blur",
        },
        {
          min: 4,
          message: "Username length should be at least 5 characters",
          trigger: "blur",
        },
      ],
      password: [
        { required: true, message: "Password is required", trigger: "blur" },
        {
          min: 5,
          message: "Password length should be at least 5 characters",
          trigger: "blur",
        },
      ],
    };

    const login = async () => {
      const valid = ((form.value as unknown) as { validate: () => boolean }).validate();
      if (!valid) return;

      const loadingInstance = ElLoading.service({ fullscreen: true });
      try {
        const success = await authenticate(model);
        if (success) {
          router.push({ name: "Home" });
        }
      } catch (error) {
        console.log(error);
      } finally {
        loadingInstance.close();
      }
    };

    return {
      form,
      model,
      rules,
      loading,
      login,
    };
  },
});
</script>
