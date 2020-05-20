<template>
  <v-app>
    <v-navigation-drawer app clipped expand-on-hover mini-variant>
      <v-list dense flat>
        <v-list-item-group color="teal" mandatory>
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
            <v-card>
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
          <v-col :cols="3">
            <v-card id="plus-card">
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
    </v-content>

    <v-content v-if="MenuSection === 1">
      <v-container fluid>
        <v-row dense v-for="(link, index) in links" :key="link.id">
          <v-col :cols="index !== links.length - 1 ? 11 : 12">
            <v-text-field dense solo hide-details
                          v-model="link.url" @input.once="addLink" placeholder="Введите адрес страницы здесь">
            </v-text-field>
          </v-col>
          <v-col :cols="1" v-if="index !== links.length - 1">
            <v-btn block @click="removeLink(index)">
              <v-icon color="red">mdi-close</v-icon>
            </v-btn>
          </v-col>
        </v-row>
        <v-row justify="center">
          <v-col cols="2">
            <v-btn @click="getScrapedData" :disabled="parsingRules.length === 0 || links.length === 1">
              Получить контент
            </v-btn>
          </v-col>
        </v-row>
        <v-row dense align="start">
          <v-col cols="12" v-if="scrapedValues.length > 0">
            <v-data-table :headers="parsingRules.map(a => ({text: a.title, value: a.title}))" :items="scrapedValues"></v-data-table>
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
      parsingRules: [
        {
          title: 'Title',
          description: '',
          prefix: '<title>',
          suffix: '</title>',
          details: false,
          isEditFormOpen: false
        }
      ],
      isCreateRuleFormOpen: false,
      newRule: {
        title: '',
        description: '',
        prefix: '',
        suffix: '',
        details: false,
        isEditFormOpen: false
      },
      pageResponse: '',
      scrapedValues: [],
      linksCount: 1,
      links: [
        {
          id: 1,
          url: ''
        }
      ]
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
      addLink() {
        this.linksCount += 1
        this.links.push({url:'', id: this.linksCount})
      },
      removeLink(index) {
        this.$delete(this.links, index)
      },
      getScrapedData() {
        let parsingRules = this.parsingRules
        let App = this
        this.links.forEach(function (linkObj) {
          let url = linkObj.url
          if (url !== '') {
            axios.post('https://localhost:5003/extractvalues', parsingRules,{
              headers: {'url-to-request': url}
            }).then(response => App.scrapedValues.push(response.data))
          }
        })
      }
    }
  }
</script>