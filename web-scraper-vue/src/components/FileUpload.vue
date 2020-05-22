<template>
  <div>
    <input type="file" accept=".json" @change="loadTextFromFile" ref="file" hidden>
    <v-btn block @click="$refs.file.click()" min-width="200px">
      {{ buttonText }}
    </v-btn>
  </div>
</template>

<script>
    export default {
        name: "FileUpload",
        props: {
            buttonText: {
                type: String,
                required: true
            }
        },
        methods: {
            loadTextFromFile(ev) {
                const file = ev.target.files[0];
                const reader = new FileReader();

                reader.onload = e => this.$emit("loaded", e.target.result);
                reader.readAsText(file);
            }
        }
    }
</script>

<style scoped>

</style>