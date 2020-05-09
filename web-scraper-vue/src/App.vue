<template>
  <v-app>
    <v-navigation-drawer app clipped expand-on-hover mini-variant>
      <v-list dense>
        <v-list-item link @click="isRuleManagementSection = true">
          <v-list-item-action>
            <v-icon>mdi-view-dashboard</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Управление правилами</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item link @click="isRuleManagementSection = false">
          <v-list-item-action>
            <v-icon>mdi-magnify</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>Скрейпинг</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>

    </v-navigation-drawer>

    <v-app-bar app clipped-left>
      <v-toolbar-title>Scraper</v-toolbar-title>
    </v-app-bar>

    <v-content v-if="isRuleManagementSection">
      <v-container fluid>
        <v-row dense align="start">
          <v-col v-for="rule in parsingRules" :key="rule.title" :cols="3">
            <v-card @click="rule.details = !rule.details">
              <v-card-title class="teal--text text--accent-3" v-text="rule.title"></v-card-title>
              <v-card-subtitle class="teal--text text--accent-2" v-text="rule.description"></v-card-subtitle>
              <v-card-text v-if="rule.details">
                <div>Prefix: {{rule.prefix}}</div>
                <div>Suffix: {{rule.suffix}}</div>
              </v-card-text>
            </v-card>
          </v-col>
          <v-col :cols="3">
            <v-card id="plus-card">
                <v-btn block v-if="!isCreateRuleFormOpen" @click="isCreateRuleFormOpen = true">
                  <v-icon>mdi-plus</v-icon>
                </v-btn>
              <v-form v-if="isCreateRuleFormOpen" v-model="valid">
                <v-container>
                  <v-row no-gutters>
                    <v-col cols="12">
                      <v-text-field dense v-model="newRule.title" label="Название"
                                    required outlined :rules="textFieldRules"></v-text-field>
                    </v-col>
                    <v-col cols="12">
                      <v-text-field dense v-model="newRule.description" label="Описание" outlined></v-text-field>
                    </v-col>
                    <v-col cols="12">
                      <v-text-field dense v-model="newRule.suffix" label="Префикс"
                                    required outlined :rules="textFieldRules"></v-text-field>
                    </v-col>
                    <v-col cols="12">
                      <v-text-field dense v-model="newRule.prefix" label="Суффикс"
                                    required outlined :rules="textFieldRules"></v-text-field>
                    </v-col>
                    <v-col cols="6">
                      <v-btn block @click="saveNewRule" :disabled="!valid" color="success">
                        Сохранить
                      </v-btn>
                    </v-col>
                    <v-col cols="6">
                      <v-btn block @click="closeCreateForm">
                        Отмена
                      </v-btn>
                    </v-col>
                  </v-row>
                </v-container>
              </v-form>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-content>

    <v-content v-if="!isRuleManagementSection">
      <v-container fluid>
        <v-row>
          <v-col>
            <p>Under construction</p>
          </v-col>
        </v-row>
      </v-container>
    </v-content>
    <v-footer app>
      <span>Aleksandr Artamonov &copy; 2020</span>
    </v-footer>
  </v-app>
</template>

<script>
  export default {
    name: 'App',
    data: () => ({
      isRuleManagementSection: true,
      parsingRules: [],
      isCreateRuleFormOpen: false,
      valid: false,
      textFieldRules: [v => !!v || 'Поле обязательно'],
      newRule: {
        title: '',
        description: '',
        prefix: '',
        suffix: '',
        details: false
      }
    }),
    created () {
      this.$vuetify.theme.dark = true
    },
    methods: {
      saveNewRule() {
        this.parsingRules.push({
          title: this.newRule.title,
          description: this.newRule.description,
          prefix: this.newRule.prefix,
          suffix: this.newRule.suffix,
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
          details: false
        }
        this.isCreateRuleFormOpen = false
      }
    }
  }
</script>