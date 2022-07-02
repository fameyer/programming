<template>
    <div class='employee-list-item'>
        <v-list-item-content>
            <!-- name -->
            <v-list-item-title v-text="item.name"></v-list-item-title>                
        </v-list-item-content>        
        <v-list-item-action class="employee-item-action">
        <!-- add review -->
            <v-tooltip bottom>
                <template v-slot:activator="{ on }">
                    <v-btn 
                    icon 
                    v-on="on"
                    @click="$emit('addReview', item)"
                    >
                    <v-icon>mdi-comment-plus</v-icon>                              
                    </v-btn>
                </template>
                <span>Add performance review</span>
            </v-tooltip>

            <!-- add employee review -->
            <v-tooltip bottom>
                <template v-slot:activator="{ on }">
                    <v-btn 
                    icon 
                    v-on="on"
                    @click="$emit('inviteReview', {item})"
                    >
                    <v-icon>mdi-account-multiple-plus</v-icon>                              
                    </v-btn>
                </template>
                <span>Invite employee to performance review</span>
            </v-tooltip>

            <!-- remove employee -->
            <v-tooltip bottom>
                <template v-slot:activator="{ on }">
                    <v-btn 
                    icon 
                    v-on="on"
                    @click="$emit('remove', item)"
                    >
                    <v-icon>mdi-delete</v-icon>                              
                    </v-btn>
                </template>
                <span>Remove employee</span>
            </v-tooltip>    
        </v-list-item-action>  
    </div>        
</template>

<script>

export default {
  name: 'EmployeeItem',
  props: {
      item: Object,
  },
  data()  {
    return {          
    }
  },
  methods: {
  },
}
</script>

<style>
.employee-list-item {
    display: flex;
    width: 100%;
}
.employee-item-action {
    flex-direction: row;
}
</style>
