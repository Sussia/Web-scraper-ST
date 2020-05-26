<template>
  <v-app>
    <v-app-bar app>
      <v-tabs color="teal" centered>
        <v-tab @click="MenuSection = 0" id="rule-management-tab">
          <v-icon>mdi-view-dashboard</v-icon>
          Управление правилами
        </v-tab>
        <v-tab @click="MenuSection = 1" id="scraping-tab">
          <v-icon>mdi-magnify</v-icon>
          Скрейпинг
        </v-tab>
      </v-tabs>
    </v-app-bar>

    <v-content v-if="MenuSection === 0">
      <RuleManagementComponent :parsing-rules="parsingRules"></RuleManagementComponent>
    </v-content>

    <v-content v-if="MenuSection === 1">
      <ScrapingComponent :parsing-rules="parsingRules"></ScrapingComponent>
    </v-content>

    <v-footer app>
      <span>Aleksandr Artamonov &copy; 2020</span>
    </v-footer>
  </v-app>
</template>

<script>
    import RuleManagementComponent from "./components/RuleManagementComponent";
    import ScrapingComponent from "./components/ScrapingComponent";

    export default {
        name: 'App',
        components: {
            RuleManagementComponent,
            ScrapingComponent
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
            ]
        }),
        created() {
            this.$vuetify.theme.dark = true
        }
    }
</script>