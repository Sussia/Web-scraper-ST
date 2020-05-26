<template>
  <v-container fluid id="scraping-component">
    <v-row dense v-for="(link, index) in links" :key="link.id">
      <v-col :cols="index !== links.length - 1 ? 11 : 12">
        <v-text-field dense filled hide-details class="url-field"
                      v-model="link.url" @input.once="addLink" placeholder="Введите адрес страницы здесь">
        </v-text-field>
      </v-col>
      <v-col cols="1" v-if="index !== links.length - 1">
        <v-btn @click="removeLink(index)" block class="remove-url-button">
          <v-icon color="red">mdi-close</v-icon>
        </v-btn>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <FileUpload id="upload-urls-button" button-text="Загрузить адреса" @loaded="importLinks"></FileUpload>
      </v-col>
      <v-col>
        <v-btn block @click="getScrapedData" :disabled="parsingRules.length === 0 || links.length === 1" min-width="200px" id="scrape-button">
          Получить контент
        </v-btn>
      </v-col>
      <v-col>
        <FileDownload id="download-table-button" :download-data="scrapedValues" file-name="scraped_values"
                      file-type="csv" button-text="Скачать таблицу"></FileDownload>
      </v-col>
      <v-col>
        <v-btn block :disabled="scrapedValues.length === 0" @click="clearTable" min-width="200px" id="clear-table-button">
          Отчистить таблицу
        </v-btn>
      </v-col>
    </v-row>
    <v-row dense align="start">
      <v-col cols="12" v-if="scrapedValues.length > 0">
        <v-data-table :headers="parsingRules.map(a => ({text: a.title, value: a.title}))"
                      :items="scrapedValues" id="data-table"></v-data-table>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
    import axios from "axios";
    import FileDownload from "./FileDownload";
    import FileUpload from "./FileUpload";

    export default {
        name: "ScrapingComponent",
        components: {
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
            clearTable() {
                this.scrapedValues = []
            },
            importLinks(text) {
                let links = JSON.parse(text)
                let counter = 0;
                let l = links.map(link => {
                    counter += 1
                    return {
                        url: link,
                        id: counter
                    }
                })
                this.links = []
                this.links.push(...l)
                this.linksCount = counter
                this.addLink()
            },
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