# Leaflet-React test

This documentation describes all the requirements and steps necessary to run this program.

## Author
Falk Meyer, 12.05.2022

## Requirements
The following software has to be installed on the machine:

* Node.js of recent Version (v18.1.0)
* npm

## Browser

Tested for Microsoft Edge and Chrome of recent versions (101.0.1210.39)

## Used third party packages

* [React](https://reactjs.org/): Frontend framework
* [MUI](https://mui.com/): React UI tools
* [Mirage JS](https://miragejs.com/): API mocking library 
* [Leaflet](https://leafletjs.com/): Interactive map library 
* [react-leaflet](https://react-leaflet.js.org/): React components for Leaflet maps
* [PixiJS](https://pixijs.com/): 2D WebGL renderer
* [leaflet-pixi-overlay](https://github.com/manubb/Leaflet.PixiOverlay): Pixi overlay class for Leaflet

All these packages are provided with open-source licenses. 

## Description

This React app shows a map of a store with moving robots and fixed product locations. It depicts an overview of warehouse-robot activities as a simple showcase.

## Instructions

* Run "npm install" in the project folder
* Run "npm start" to start the project in development mode, it should directly open up your browser on "localhost:3000", otherwise open the browser and navigate to "localhost:3000"
* The map is shown on the left, centered on the IKEA store in Sendai (Miyagi prefecture), products are visualized as red rectangles and robots as blue circles
* On the right of the screen is a site bar, the upper part can be used to change the number of products and robots in the simulation, the lower part shows attribute info on a selected map object

## Assumptions

* There is not interaction between robots and products, it is simply a showcase of the drawn features on the map.
* Robots just move on simple lines from one point to the another at a random speed, there is no possibility of robots colliding, since the lines are distinct.
* Robots movement is updated each time the REST-API endpoint is called, i.e. triggered by the front-end components. This of course does not make sense, but is incorporated here for simplicity reasons.