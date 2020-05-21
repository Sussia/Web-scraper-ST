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