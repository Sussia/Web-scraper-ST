<template>
  <v-form v-model="valid">
    <v-container>
      <v-row no-gutters>
        <v-col cols="12">
          <v-text-field dense v-model="baseRule.title" label="Название"
                        required outlined :rules="textFieldRules" class="title-field"></v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field dense v-model="baseRule.description" label="Описание" outlined class="description-field"></v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field dense v-model="baseRule.prefix" label="Префикс" class="prefix-field"
                        required outlined :rules="textFieldRules"></v-text-field>
        </v-col>
        <v-col cols="12">
          <v-text-field dense v-model="baseRule.suffix" label="Суффикс" class="suffix-field"
                        required outlined :rules="textFieldRules"></v-text-field>
        </v-col>
        <v-col cols="6">
          <v-btn block @click="$emit('submit-form', baseRule)" :disabled="!valid" color="success" class="save-button">
            Сохранить
          </v-btn>
        </v-col>
        <v-col cols="6">
          <v-btn block @click="$emit('close-form')" class="cancel-button">
            Отмена
          </v-btn>
        </v-col>
      </v-row>
    </v-container>
  </v-form>
</template>

<script>
    export default {
        name: "RuleForm",
        props: {
            rule: {
                type: Object,
                required: true
            }
        },
        data: function () {
            return {
                valid: false,
                textFieldRules: [v => !!v || 'Поле обязательно'],
                baseRule: {
                    title: this.rule.title,
                    description: this.rule.description,
                    prefix: this.rule.prefix,
                    suffix: this.rule.suffix,
                    details: false,
                    isEditFormOpen: true
                }
            }
        }
    }
</script>

<style scoped>

</style>