<template>
    <v-dialog
      v-model="show"
      width="500"
    >
        <v-form onSubmit="return false;" class="invite-review-dialog">
            <v-card>
                <v-card-title
                class="headline dark lighten-2"
                primary-title
                >
                Invite employee to a performance review
                </v-card-title>

                <v-divider></v-divider>

                <v-select
                    :items="employees.filter(employee => employee !== selectedEmployee).map(employee => employee.name)"                    
                    v-model="selectedItems"
                    chips
                    multiple
                    label="Select employee"
                ></v-select>

                <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                    color="primary"
                    text
                    @click="$emit('invite')"
                    :disabled="selectedItems.length === 0"
                >
                    Invite
                </v-btn>
                    <v-btn
                    color="secondary"
                    text
                    @click="$emit('abort')"
                >
                    Cancel
                </v-btn>
                </v-card-actions>
            </v-card>
        </v-form>
    </v-dialog>
</template>

<script>
import review from './../source/review';

export default {
  name: 'InviteReviewDialog',
  props: {
      employees: Array,
  },
  data()  {
    return {                
        reviewText: '',
        show: false,
        selectedItems: [],
        selectedEmployee: {},
    }
  },
  methods: {
    /**
     * Set invited employees
     */
    ask(employee) {
        this.show = true;
        this.selectedEmployee = employee;
        return new Promise((resolve, reject) => {
            this.$on('invite', () => {
                this.$off('invite');
                this.show = false;
                
                const reviews = [];
                this.selectedItems.forEach(name => {
                    reviews.push(new review(name));
                });

                // clean
                this.selectedItems = [];

                console.log(reviews);                
                resolve(reviews);
            });

            this.$on('abort', () => {
                this.$off('abort');
                this.show = false;

                // clean
                this.selectedItems = [];
                
                reject('abort');
            });
        });
    },
  },
}
</script>

<style>
.invite-review-dialog .v-select {
    margin: 10px;
}
</style>
