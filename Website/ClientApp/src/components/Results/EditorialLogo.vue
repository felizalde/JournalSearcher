<template>
  <img
    v-if="editorial === 'Sage'"
    class="editorial-logo"
    src="../../assets/sage-logo.png"
    height="35"
    width="90"
    alt="sage-logo"
  />
  <img
    v-if="editorial === 'Springer'"
    class="editorial-logo"
    src="../../assets/springer-logo.png"
    height="45"
    width="100"
    alt="springer-logo"
  />
  <img
    v-if="editorial === 'Elsevier'"
    class="editorial-logo"
    src="../../assets/elsevier-logo.png"
    height="45"
    width="100"
    alt="elsevier-logo"
  />
  <img
    v-if="editorial === 'Wiley'"
    class="editorial-logo"
    src="../../assets/wiley-logo.png"
    height="45"
    width="100"
    alt="wiley-logo"
  />
</template>
<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    props: {
        editorial: {
            type: String,
            required: true,
        },
    },
})
</script>
