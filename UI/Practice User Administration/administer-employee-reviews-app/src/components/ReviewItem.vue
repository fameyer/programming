<template>
    <div class='review-list-item'>
        <v-list-item-content>
            <div>
                <b>Author</b>
                <v-list-item-title v-text="item.author"></v-list-item-title>
                <br>
                <b>Review</b>
                <div v-if="!updateItem">
                    {{item.contents}}
                </div>
                <!-- rename textfield -->
                <v-textarea   
                    v-else             
                    v-model="newContents"
                    v-on:keyup.enter="updateReview()"
                    autofocus
                    class="rename-text-field"
                    @blur="updateReview()"
                    :rules="updateRules"
                    :required="true"
                ></v-textarea>
            </div>


        </v-list-item-content>
        <!-- update review -->
        <v-tooltip bottom>
            <template v-slot:activator="{ on }">
                <v-btn 
                class="review-tooltip"
                icon 
                v-on="on"
                @click="update()"
                >
                <v-icon>mdi-pencil</v-icon>                              
                </v-btn>
            </template>
            <span>Update review</span>
        </v-tooltip>   
    </div> 
</template>

<script>

export default {
  name: 'ReviewItem',
  props: {
      item: Object,
      employeeName: String,
  },
  data()  {
    return { 
        newContents: '', 
        updateItem: false,   
        updateRules: [
            // name should not be empty
            text => (text ? text.trim().length > 0 : false) || 'Text can not be empty',
        ],    
    }
  },
  methods: {
    /**
     * Update Review
     */
    updateReview() {
        this.updateItem = false;
        if(this.newContents !== '') {
            this.item.contents = this.newContents;
            // emit change
            this.$emit('updateReview', {name: this.employeeName, review: this.item});
        }          
    },
    /**
     * Update item
     */
    update() {
        this.newContents = this.item.contents;
        this.updateItem = true;
    }
  },
}
</script>

<style>
.review-list-item {
    display: flex;
    width: 100%;
}

.review-tooltip {
    margin-top: auto;
    margin-bottom: auto;
}
</style>
