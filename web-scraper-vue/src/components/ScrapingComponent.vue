<template>
  <v-container fluid>
    <v-row dense v-for="(link, index) in links" :key="link.id">
      <v-col :cols="index !== links.length - 1 ? 11 : 12">
        <v-text-field dense filled hide-details
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
        <v-data-table :headers="parsingRules.map(a => ({text: a.title, value: a.title}))"
                      :items="scrapedValues"></v-data-table>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
    import axios from "axios";

    export default {
        name: "ScrapingComponent",
        props: {
            parsingRules: {
                type: Array,
                required: true
            }
        },
        data: () => ({
            scrapedValues: [],
            linksCount: 1,
            links: [
                {
                    id: 1,
                    url: ''
                }
            ]
        }),
        methods: {
            addLink() {
                this.linksCount += 1
                this.links.push({url: '', id: this.linksCount})
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
                        axios.post('https://localhost:5003/extractvalues', parsingRules, {
                            headers: {'url-to-request': url}
                        }).then(response => {
                            if (response.data !== '') App.scrapedValues.push(response.data)
                        })
                    }
                })
            }
        }
    }
</script>

<style scoped>

</style>