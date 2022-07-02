import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify';
import Model from './model';

Vue.config.productionTip = false

// set model with url to server API
const model = new Model('http://localhost:9090');

// set vue component
const vm = new Vue({
  vuetify,
  render: h => h(App)
}).$mount('#app');

// bind viewmodel to model
const appVue = vm['$children'][0];

model.getEmployeesAndReviews().then((employees) => {appVue.employees = employees});

// add employee and update list
appVue.$on('add-employee', (employee) =>{
  model.addEmployee(employee).then(() => model.getEmployeesAndReviews().then((employees) => {appVue.employees = employees}));  
});

// remove employee and update list
appVue.$on('remove-employee', (employee) =>{
  model.removeEmployee(employee).then(() => model.getEmployeesAndReviews().then((employees) => {appVue.employees = employees}));  
});

// add review and update list
appVue.$on('add-review', (item) =>{
  const employee = item.employee;
  const review = item.review;
  model.addReview(employee, review).then(() => model.getEmployeesAndReviews().then((employees) => {appVue.employees = employees})); 
});

// update review and update list
appVue.$on('update-review', (item) =>{
  const employee = item.employee;
  const review = item.review;
  model.updateReview(employee, review).then(() => model.getEmployeesAndReviews().then((employees) => {appVue.employees = employees})); 
});
