// For authoring Nightwatch tests, see
// https://nightwatchjs.org/guide

module.exports = {
    'default e2e tests': browser => {
        browser
            .url('http://localhost:8080')
            .waitForElementVisible('#app')
            .assert.titleContains('Scraper')
            .end()
    },

    'example e2e test using a custom command': browser => {
        browser
            .openHomepage()
            .assert.elementPresent('.hello')
            .end()
    }
}
