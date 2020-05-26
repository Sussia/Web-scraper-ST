<template>
  <v-btn block :disabled="downloadData.length === 0"
     :href="convertData"
     :download="`${fileName}.${fileType}`" min-width="200px" class="download-button">
    {{ ButtonText }}
  </v-btn>
</template>

<script>
    export default {
        name: "FileDownload",
        props: {
            fileName: {
                type: String,
                required: true
            },
            downloadData: {
                type: Array,
                required: true
            },
            fileType: {
                type: String,
                required: true
            },
            dataTitles: {
                type: Array
            },
            ButtonText: {
                type: String,
                required: true
            }
        },
        computed: {
            _dataTitles() {
                if(this.dataTitles === undefined) {
                    if (this.downloadData.length === 0) {
                        return []
                    } else {
                        return Object.keys(this.downloadData[0])
                    }
                } else {
                    return this.dataTitles
                }
            },
            convertData () {
                let contentType = ''
                let dData = ''
                let blob
                let url
                let titles
                let titlesString
                switch (this.fileType) {
                    case 'json':
                        contentType = 'application/json'
                        dData = JSON.stringify(this.downloadData, null, 2)
                        blob = new Blob([dData], { type: contentType })
                        url = window.URL.createObjectURL(blob)
                        break

                    case 'csv':
                        contentType = "application/csvcharset=utf-8"
                        titles = this._dataTitles
                        titlesString = titles.join(';')
                        dData += titlesString + '\r\n'
                        this.downloadData.map(item => {
                            const keys = titles
                            keys.forEach((key, index) => {
                                dData += item[key]
                                if (keys.length > index) {
                                    dData += ';'
                                }
                            })
                            dData += '\r\n'
                        })
                        blob = new Blob([dData], { type: contentType })
                        url = window.URL.createObjectURL(blob)
                        break
                    default:
                        break
                }
                return url
            }
        }
    }
</script>

<style scoped>

</style>