<template>
  <v-app>
    <div id="app" data-app>
      <!-- list of employees -->
      <v-card
      class="mx-auto"
      tile
      >
        <v-toolbar
          dark
        >
          <v-toolbar-title>Administer employee reviews</v-toolbar-title>
          <v-spacer></v-spacer>

          <!-- add employee -->
          <v-tooltip bottom>
            <template v-slot:activator="{ on }">
              <v-btn 
                icon 
                v-on="on"
                @click="openAddEmployeeDialog = true"
              >
                <v-icon>mdi-plus</v-icon>                              
              </v-btn>
            </template>
            <span>Add employee</span>
          </v-tooltip>

        </v-toolbar>

        <v-list>
          <v-list-group
            v-for="item in employees"
            :key="item.name"
            color="primary"
          >
            <!-- employee item -->
            <template v-slot:activator>
              <employee-item :item="item" v-on:remove="removeEmployee" v-on:addReview="addReview" v-on:inviteReview="inviteReview"></employee-item>            
            </template>

            <!-- reviews -->
            <v-list-item
              v-for="subItem in item.reviews"
              :key="subItem.title" 
              class="review-v-list-item"                  
            >
              <v-list-item-content>
                <review-item :item="subItem" :employeeName="item.name" v-on:updateReview="updateReview"></review-item>   
              </v-list-item-content>
            </v-list-item> 
          </v-list-group>
        </v-list>
      </v-card>

      <add-employee-dialog v-model="openAddEmployeeDialog" v-on:addEmployee="addEmployee"></add-employee-dialog>
      <add-review-dialog ref="createReviewDialog" v-on:addEmployee="addEmployee"></add-review-dialog>
      <invite-review-dialog ref="inviteReviewDialog" :employees="employees"></invite-review-dialog>
    </div>
  </v-app>
</template>

<script>
import AddEmployeeDialog from './components/AddEmployeeDialog.vue';
import AddReviewDialog from './components/AddReviewDialog.vue';
import InviteReviewDialog from './components/InviteReviewDialog.vue';
import EmployeeItem from './components/EmployeeItem.vue';
import ReviewItem from './components/ReviewItem.vue';

export default {
  name: 'App',
  components: {
    AddEmployeeDialog,
    AddReviewDialog,
    InviteReviewDialog,
    EmployeeItem,
    ReviewItem,
  },
  data()  {
    return {
      employees: [],
      openAddEmployeeDialog: false,
    }
  },
  methods: {
    /**
     * Add an employee to the list
     */
    addEmployee(employee) {
      this.$emit('add-employee', employee);
    },
    /**
     * Remove an employee from the list
     */
    removeEmployee(employee) {
      this.$emit('remove-employee', employee);
    },
    /**
     * Add performance review
     */
    addReview(employee) {
      this.$refs.createReviewDialog.ask().then((review) => {
          if(review.author && review.contents) {
            this.$emit('add-review', {employee, review});
          }
      });        
    },
    /**
     * Invite employee to review other employee
     */
    inviteReview(val) {
      if(val.item) {
        const employee = val.item;
        this.$refs.inviteReviewDialog.ask(employee).then((reviews) => {          
            if(reviews) {              
              reviews.forEach(review => {
                console.log(review);
                this.$emit('add-review', {employee, review});
              });              
            }
        });
      } 
    },
    /**
     * Update a review
     */
    updateReview(val) {
      const employee = this.employees.find(elem => elem.name === val.name);
      const review = val.review;
      if(employee && review) {
        this.$emit('update-review', {employee, review});
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
    color: #2c3e50;
}
.review-v-list-item {
  background: aliceblue;
  margin-left: 20px;
  margin-bottom: 5px;
}
</style>
