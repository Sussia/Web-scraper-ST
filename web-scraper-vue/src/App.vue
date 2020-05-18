<template>
  <v-app>
    <v-navigation-drawer app clipped expand-on-hover mini-variant>
      <v-list dense flat>
        <v-list-item-group color="teal" v-model="MenuSection">
          <v-list-item @click="MenuSection = 0">
            <v-list-item-action>
              <v-icon>mdi-view-dashboard</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>Управление правилами</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item @click="MenuSection = 1">
            <v-list-item-action>
              <v-icon>mdi-magnify</v-icon>
            </v-list-item-action>
            <v-list-item-content>
              <v-list-item-title>Скрейпинг</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list-item-group>
      </v-list>

    </v-navigation-drawer>

    <v-app-bar app clipped-left>
      <v-toolbar-title>Scraper</v-toolbar-title>
    </v-app-bar>

    <v-content v-if="MenuSection === 0">
      <v-container fluid>
        <v-row dense align="start">
          <v-col v-for="(rule, index) in parsingRules" :key="rule.title" :cols="3">
            <v-card @click="rule.details = !rule.details">
              <v-card-title class="teal--text text--accent-3">
                <span>{{rule.title}}</span>
                <v-spacer></v-spacer>
                <v-btn icon @click="deleteRule(index)">
                  <v-icon color="red" class="text--lighten-1">mdi-close</v-icon>
                </v-btn>
              </v-card-title>

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
              <RuleForm :rule="newRule" v-if="isCreateRuleFormOpen"
                        v-on:close-form="closeCreateForm" v-on:submit-form="saveNewRule">
              </RuleForm>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-content>

    <v-content v-if="MenuSection === 1">
      <v-container fluid>
        <v-row dense align="start">
          <v-col cols="12">
            <v-data-table :headers="parsingRules.map(a => a.title)"></v-data-table>
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="2">
            <v-btn @click="getScrapedData">
              Получить контент
            </v-btn>
          </v-col>
          <v-col cols="10">
            <p>{{ pageResponse }}</p>
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
  import axios from 'axios'
  import RuleForm from "./components/RuleForm";

  export default {
    name: 'App',
    components: {
      RuleForm
    },
    data: () => ({
      MenuSection: 0,
      parsingRules: [],
      isCreateRuleFormOpen: false,
      newRule: {
        title: '',
        description: '',
        prefix: '',
        suffix: '',
        details: false
      },
      pageResponse: ''
    }),
    created () {
      this.$vuetify.theme.dark = true
    },
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
          details: false
        }
        this.isCreateRuleFormOpen = false
      },
      deleteRule(index) {
        this.$delete(this.parsingRules, index)
      },
      getScrapedData() {
        let site = "https://example.com"
        axios.post('https://localhost:5003/extractvalues', this.parsingRules,{
          headers: {'url-to-request': site}
        },
        )
        .then(response => this.pageResponse = response.data)
      }
    }
  }
</script>