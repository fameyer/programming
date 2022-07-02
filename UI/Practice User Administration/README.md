# Example for user administration app

Falk Meyer, 17.05.2020

## Description

This application represents a simple example of a web application for adding or removing users/employees in an appropriate administration user-interface. It thereby uses a Node.js server mocking a corresponding back-end API server and database.

## Requirements

* Node.js and npm

## How to run

Run the API server:
* open terminal
* change to ./server-side-api/
* run *'npm install'* (installs dependencies)
* run *'npm start'* to start the server
* Server runs on port 9090 by default

Open admin view app:
* open ./administer-employee-reviews/dist/index.html with your preferred browser
* Alternative (not recommended): run *'npm install'* in ./administer-employee-reviews and start development view by *'npm run serve'*

## Browser

Tested on:
* Chrome Version 81

## Technologies used

* Web app: Vue.js web application (with vuetify as the component framework)
* Server-side API: very simple Node.js express app with five possible API calls
    * getEmployees
    * addEmployee
    * removeEmployee
    * addReview
    * updateReview

## Design

The admin web app is designed following a basic MVVM-pattern: the Vue component *App.vue* as the view/viewModel and the class *model.js* as the model. Bindings between these components are done via event handling. The calls to the API are triggered by the model, while the view displays the data as a well designed list and enables user interaction.