<template>
  <div id="app">
    <img alt="Vue logo" src="./assets/logo.png">
    <div>
      <ol>
        <li v-for="rule in rules" :key="rule.id">
          <p>{{ rule.title }}: {{ rule.prefix }} - {{ rule.suffix }}</p>
        </li>
      </ol>
    </div>
    <input type="text" placeholder="Введите текст" v-model="textToParse">
    <button @click="parseText">Извлечь значения</button>
    <p v-if="showExtractedValues">{{extractedValue.title}}: {{extractedValue.value}}</p>
  </div>
</template>

<script>

export default {
  name: 'App',
  data: function () {
    return {
      showExtractedValues: false,
      textToParse: '',
      rules: [
        {
          title: 'MyTestTitle', suffix: '</title>', prefix: '<title>', id: 1
        },
        {
          title: 'MyTestTitle2', suffix: '</title>', prefix: '<title>', id: 2
        }
      ],
      extractedValue: {
        title: '',
        value: ''
      }
    }
  },
  methods: {
    parseText: function() {
      function extractValue(text, prefix, suffix) {
        let matchResult = text.match(new RegExp(prefix + '(.*)' + suffix))
        if (matchResult != null) {
          return matchResult[1]
        } else {
          return null
        }
      }
      let rule = this.rules[0]
      let value = extractValue(this.textToParse, rule.prefix, rule.suffix)
      if (value != null) {
        this.extractedValue =  {
          title: rule.title,
          value: extractValue(this.textToParse, rule.prefix, rule.suffix)
        }
        this.showExtractedValues = true
      } else {
        this.showExtractedValues = false
      }
    }
  }
}

</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
