import React from 'react';

// leaflet and pixi imports
import { MapContainer, TileLayer } from 'react-leaflet';
import 'leaflet-pixi-overlay';
import * as PIXI from 'pixi.js'
import L from "leaflet";

import 'leaflet/dist/leaflet.css';

const robotUpdateInterval = 100;

// constants
const cornerX = 38.22509852244465;
const cornerY = 140.8854051947837;
const factorX = 0.00077330364606 * 0.01;
const factorY =  0.0015125658824 * 0.01;

// helper function
function transformToMap(xCoord, yCoord) {
  // formula is: cornerX + x * factor under assumption that x is between 0 and 100
  // other corner is reached for x or y equal to 100
  return [
    cornerX + xCoord * factorX,
    cornerY + yCoord * factorY,
  ];
}

class StoreMap extends React.Component {
    constructor(props) {
        super(props);
        this.mapRef = React.createRef();

        // the projection method later referenced through pixi
        this.project = {};

        // pixi container
        this.productContainer = new PIXI.Container();
        this.robotContainer = new PIXI.Container();
        this.selectionContainer = new PIXI.Container();
    }

      /**
   * Set the table feature info on the last selected object
   * @param {} obj 
   */
  setFeatureInfo(obj) {
    // create appropriate object
    const info = [];
    Object.entries(obj).forEach(([key, value], index) => {
      const dict = {id: index, name: key, value: value};
      info.push(dict);
    });
    // update table in sitebar
    this.props.onFeatureInfoClicked(info);
  } 

  /**
   * Render products on the overlay
   */
  async renderProducts() {
    if(!this.project || !this.productContainer) {
      return;
    }

    let products = [];
    this.productContainer.removeChildren();

    // Simple GET requesting the API
    await fetch('/api/products')
      .then(response => response.json())
      .then(data => {
          products = data.products ?? [];
    });

    // filter products for same coordinates
    const displayProducts = [];
    products.forEach(product => {
      const productStack = displayProducts.find(elem => elem.x === product.x && elem.y === product.y);
      if(productStack) {
        productStack.count += 1;
        productStack.ids.push(product.id);
      }
      else {
        displayProducts.push({
          ids: [product.id],
          count: 1,
          x: product.x,
          y: product.y
        });
      }
    });

    // draw products as rectangles due to performance
    displayProducts.forEach(productStack => {
      const graphic = new PIXI.Graphics();
      const size = 20;
      graphic.beginFill(0xFF0000);
      graphic.drawRect(0, 0, size, size);
      var projectedCenter = this.project(transformToMap(productStack.x, productStack.y));                   
      graphic.x = projectedCenter.x;
      graphic.y = projectedCenter.y;
      graphic.endFill();
      graphic.interactive = true;
      graphic.click = () => {
        // on click show feature info
        this.setFeatureInfo(productStack);

        // show selection graphic
        this.selectionContainer.removeChildren();
        var selectionGraphic = new PIXI.Graphics();
        selectionGraphic.lineStyle(3, 0x00FF00);
        selectionGraphic.x = graphic.x;
        selectionGraphic.y = graphic.y;
        selectionGraphic.drawRect(0, 0, size, size);

        this.selectionContainer.addChild(selectionGraphic);
      };

      const textInput = String(productStack.count);
      const fontSize = textInput.length === 1 ? 15 : (
        textInput.length === 2 ? 15 : 10
      ); 
      
      // label according to length of label
      const text = new PIXI.Text(textInput,{fontFamily : 'Arial', fontSize: fontSize, fill : 0xffffff, align : 'center'});
      text.x = text.text.length === 1 ? projectedCenter.x + 5 : (
        text.text.length === 2 ? projectedCenter.x + 1.5 : projectedCenter.x + 1
      ); 
      text.y = text.text.length === 1 ? projectedCenter.y + 2 : (
        text.text.length === 2 ? projectedCenter.y + 2 : projectedCenter.y + 3
      ); 

      this.productContainer.addChild(graphic);
      this.productContainer.addChild(text);
    });
  }

  /**
   * Render robots on the overlay
   */
  async renderRobotsOnInterval() {
    let robots = [];

    setInterval(async () => {
      await fetch('/api/robots')
      .then(response => response.json())
      .then(data => {
          robots = data.robots ?? [];
          this.robotContainer.removeChildren();
          robots.forEach(robot => {
            // draw robots as points
            var circle = new PIXI.Graphics();
            const circleCenter = transformToMap(robot.x, robot.y);
            var projectedCenter = this.project(circleCenter);
            var circleRadius = 10;
            circle.clear();
            
            circle.beginFill(0x0000FF);
            circle.x = projectedCenter.x;
            circle.y = projectedCenter.y;
            circle.drawCircle(0, 0, circleRadius);
            circle.endFill();
            circle.interactive = true;
            
            circle.click = () => {
              // on click show feature info
              this.setFeatureInfo(robot);

              // show selection graphic
              this.selectionContainer.removeChildren();
              var selectionGraphic = new PIXI.Graphics();
              var circleRadius = 10;
              selectionGraphic.lineStyle(3 , 0x00FF00);
              selectionGraphic.x = circle.x;
              selectionGraphic.y = circle.y;
              selectionGraphic.drawCircle(0, 0, circleRadius);

              this.selectionContainer.addChild(selectionGraphic);
            };

            this.robotContainer.addChild(circle);
          });
      });
    }, robotUpdateInterval);
  }

  /**
   * On mount create and add the pixi overlay for the features
   */
  async componentDidMount() {
    let pixiOverlay;
    let loader = new PIXI.Loader();
    let ticker = PIXI.Ticker.shared;

    // load pixie overlay
    loader.load((loader, resources) => {           
        var pixiContainer = new PIXI.Container();
        
        pixiContainer.addChild(this.productContainer);
        pixiContainer.addChild(this.robotContainer);
        pixiContainer.addChild(this.selectionContainer);

        var firstDraw = true;
        var prevZoom;
    
        pixiOverlay = L.pixiOverlay((utils) => {
            var zoom = utils.getMap().getZoom();
            var container = utils.getContainer();
            var renderer = utils.getRenderer();
            this.project = utils.latLngToLayerPoint;
    
            if (firstDraw) {
              // render products
              this.renderProducts();
                
              // render robots
              this.renderRobotsOnInterval();

              // use ticker to rerender
              ticker.add(function () {
                renderer.render(container);
              });
            }
    
            if (firstDraw || prevZoom !== zoom) {

            }
    
            firstDraw = false;
            prevZoom = zoom;
            renderer.render(container);
        }, pixiContainer);

        setTimeout(() => {
          // add overlay
          const map = this.mapRef.current;
          pixiOverlay.addTo(map);
        }, 0);
      });
  }



  render() {
    return (
      <MapContainer
        center={[38.22546, 140.88620]} 
        zoom={19}
        minZoom={18}
        maxBounds={[
          [38.23546, 140.89620],
          [38.21546, 140.87620]
        ]}
        ref={this.mapRef}
      >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          attribution='&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
          maxNativeZoom={19}
          maxZoom={25}
        />        
      </MapContainer>
    );
    }
  }

  export default StoreMap;