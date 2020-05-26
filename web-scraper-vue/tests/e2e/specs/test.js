// For authoring Nightwatch tests, see
// https://nightwatchjs.org/guide

module.exports = {
    beforeEach: (browser) => browser.init(),



    'Rule creation test': browser => {

        const cardsList = '#app > div > main > div > div > div.row.row--dense.align-start'

        browser
            .waitForElementVisible('#app');

        browser.expect.elements(cardsList + ' > div').count.to.equal(2);

        browser
            .click('#plus-card > button')
            .assert.visible('#plus-card > form')
            .setValue('#plus-card > form > div > div > div:nth-child(1) > div > div > div.v-input__slot > div > input', 'Test title')
            .setValue('#plus-card > form > div > div > div:nth-child(2) > div > div > div.v-input__slot > div > input', 'Some description')
            .setValue('#plus-card > form > div > div > div:nth-child(3) > div > div > div.v-input__slot > div > input', 'prefix')
            .setValue('#plus-card > form > div > div > div:nth-child(4) > div > div > div.v-input__slot > div > input', 'suffix')

        browser.expect.element('#plus-card > form > div > div > div:nth-child(5) > button').to.be.enabled;

        browser
            .click('#plus-card > form > div > div > div:nth-child(5) > button')
            .pause(5000)
            .assert.visible('#plus-card > button');

        browser.expect.elements(cardsList + ' > div').count.to.equal(3);
        browser.expect.element(cardsList + ' > div:nth-child(2) > div > div > div.v-card__title > span').text.to.contain('Test title');

        browser
            .end()
    }
}
