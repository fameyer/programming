<template>
    <v-dialog
      v-model="dialog"
      width="500"      
    >
        <v-form ref="form" onSubmit="return false;" class="add-employee-dialog">
            <v-card>
                <v-card-title
                class="headline dark lighten-2"
                primary-title
                >
                Add employee
                </v-card-title>

                <v-divider></v-divider>

                <v-text-field
                v-model="inputName"          
                label="Name"
                v-on:keyup.enter="add()"
                ></v-text-field>

                <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                    color="primary"
                    text
                    @click="add"
                    :disabled="inputName === ''"
                >
                    Add
                </v-btn>
                    <v-btn
                    color="secondary"
                    text
                    @click="dialog = false"
                >
                    Cancel
                </v-btn>
                </v-card-actions>
            </v-card>
        </v-form>
    </v-dialog>
</template>

<script>
import employee from './../source/employee';

export default {
  name: 'AddEmployeeDialog',
  props: {
      value: Boolean,
  },
  data()  {
    return {                
        inputName: '',
    }
  },
  methods: {
    add() {
      this.$emit('addEmployee', new employee(this.inputName));
      this.dialog = false;
    },
  },
  computed: {
    dialog: {
        get() {
            return this.value;
        },
        set(value) {
            this.$emit('input', value);            
            // reset form
            this.$refs.form.reset();
        },
    },
  }
}
</script>

<style>
.add-employee-dialog .v-text-field {
    margin: 10px;
}
</style>
