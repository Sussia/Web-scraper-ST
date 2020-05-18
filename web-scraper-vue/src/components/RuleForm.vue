<template>
    <v-form v-model="valid">
        <v-container>
            <v-row no-gutters>
                <v-col cols="12">
                    <v-text-field dense v-model="rule.title" label="Название"
                                  required outlined :rules="textFieldRules"></v-text-field>
                </v-col>
                <v-col cols="12">
                    <v-text-field dense v-model="rule.description" label="Описание" outlined></v-text-field>
                </v-col>
                <v-col cols="12">
                    <v-text-field dense v-model="rule.prefix" label="Префикс"
                                  required outlined :rules="textFieldRules"></v-text-field>
                </v-col>
                <v-col cols="12">
                    <v-text-field dense v-model="rule.suffix" label="Суффикс"
                                  required outlined :rules="textFieldRules"></v-text-field>
                </v-col>
                <v-col cols="6">
                    <v-btn block @click="$emit('submit-form', rule)" :disabled="!valid" color="success">
                        Сохранить
                    </v-btn>
                </v-col>
                <v-col cols="6">
                    <v-btn block @click="$emit('close-form')">
                        Отмена
                    </v-btn>
                </v-col>
            </v-row>
        </v-container>
    </v-form>
</template>

<script>
    export default {
        name: "RuleForm",
        props: [
            'rule'
        ],
        data: function() {
            return {
                valid: false,
                textFieldRules: [v => !!v || 'Поле обязательно']
            }
        }
    }
</script>

<style scoped>

</style>