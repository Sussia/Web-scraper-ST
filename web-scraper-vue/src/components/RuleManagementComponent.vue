<template>
  <v-container fluid>
    <v-row>
      <v-col :cols="6">
        <FileUpload button-text="Загрузить правила" @loaded="importRules"></FileUpload>
      </v-col>
      <v-col :cols="6">
        <FileDownload button-text="Скачать правила" :download-data="parsingRules"
                      file-name="parsing_rules" file-type="json"></FileDownload>
      </v-col>
    </v-row>
    <v-row dense align="start">
      <v-col v-for="(rule, index) in parsingRules" :key="rule.title">
        <v-card min-width="200px">
          <div v-if="!rule.isEditFormOpen">
            <v-card-title class="teal--text text--accent-3">
              <span>{{rule.title}}</span>
              <v-spacer></v-spacer>
              <v-btn icon @click="rule.details = !rule.details" color="grey">
                <v-icon large v-if="!rule.details">mdi-chevron-down</v-icon>
                <v-icon large v-if="rule.details">mdi-chevron-up</v-icon>
              </v-btn>
              <v-btn icon @click="openEditForm(index, rule)">
                <v-icon color="orange" class="text--lighten-2">mdi-pencil</v-icon>
              </v-btn>
              <v-btn icon @click="deleteRule(index)">
                <v-icon color="red" class="text--lighten-1">mdi-close</v-icon>
              </v-btn>
            </v-card-title>
            <v-card-subtitle class="teal--text text--accent-2" v-text="rule.description"></v-card-subtitle>
            <v-card-text v-if="rule.details">
              <div>Prefix: {{rule.prefix}}</div>
              <div>Suffix: {{rule.suffix}}</div>
            </v-card-text>
          </div>
          <RuleForm :rule="rule" v-if="rule.isEditFormOpen"
                    @close-form="closeEditForm(index, rule)" @submit-form="editRule(index, $event)">
          </RuleForm>
        </v-card>
      </v-col>
      <v-col>
        <v-card id="plus-card" min-width="300px">
          <v-btn block v-if="!isCreateRuleFormOpen" @click="isCreateRuleFormOpen = true">
            <v-icon>mdi-plus</v-icon>
          </v-btn>
          <RuleForm :rule="newRule" v-if="isCreateRuleFormOpen"
                    @close-form="closeCreateForm" @submit-form="saveNewRule">
          </RuleForm>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
    import RuleForm from "./RuleForm";
    import FileDownload from "./FileDownload";
    import FileUpload from "./FileUpload";

    export default {
        name: "RuleManagementComponent",
        components: {
            RuleForm,
            FileDownload,
            FileUpload
        },
        props: {
            parsingRules: {
                type: Array,
                required: true
            }
        },
        data: () => ({
            isCreateRuleFormOpen: false,
            newRule: {
                title: '',
                description: '',
                prefix: '',
                suffix: '',
                details: false,
                isEditFormOpen: false
            },
        }),
        methods: {
            saveNewRule(rule) {
                this.parsingRules.push({
                    title: rule.title,
                    description: rule.description,
                    prefix: rule.prefix,
                    suffix: rule.suffix,
                    details: false
                })
                this.closeCreateForm()
            },
            closeCreateForm() {
                this.newRule = {
                    title: '',
                    description: '',
                    prefix: '',
                    suffix: '',
                    details: false,
                    isEditFormOpen: false
                }
                this.isCreateRuleFormOpen = false
            },
            openEditForm(index, rule) {
                rule.isEditFormOpen = true
                this.$set(this.parsingRules, index, rule)
            },
            editRule(index, rule) {
                this.closeEditForm(index, rule)
            },
            closeEditForm(index, rule) {
                rule.isEditFormOpen = false
                this.$set(this.parsingRules, index, rule)
            },
            deleteRule(index) {
                this.$delete(this.parsingRules, index)
            },
            importRules(rules) {
                this.parsingRules.push(...JSON.parse(rules))
            }
        }
    }
</script>

<style scoped>

</style>