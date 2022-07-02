<template>
    <v-dialog
      v-model="show"
      width="500"    
    >
        <v-form ref="form" onSubmit="return false;" class="add-review-dialog">
            <v-card>
                <v-card-title
                class="headline dark lighten-2"
                primary-title
                >
                Add performance review
                </v-card-title>

                <v-divider></v-divider>

                <v-textarea
                v-model="reviewText"          
                label="Review"
                ></v-textarea>

                <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn
                    color="primary"
                    text
                    @click="$emit('add')"
                    :disabled="reviewText === ''"
                >
                    Add
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
  name: 'AddReviewDialog',
  data()  {
    return {                
        reviewText: '',
        show: false,
    }
  },
  methods: {
    ask() {
        this.show = true;
        return new Promise((resolve) => {
            this.$on('add', () => {
                this.show = false;
                const reviewItem = new review('admin',this.reviewText);
                resolve(reviewItem);
                // reset form
                this.$refs.form.reset();
            });
            this.$on('abort', () => {
                this.show = false;                
            });
        });
    },
  },
}
</script>

<style>
.add-review-dialog .v-textarea {
    margin: 10px;
}
</style>
