import { createServer, Model } from "miragejs"
import Robot from "./model/robot";

// helper
const computeYPosition = (x, i, factor) =>  x * 0.15 + i * factor;

// constants

// lanes for product shelfs
const productLanes = [0, 10, 20, 30, 40, 50, 60, 70, 80, 90];

// Always do 100 places by shelf
const numberByShelf = 100;

// variables
let robots = [];

/**
 * This function creates rows for the products
 * storage room is 100 x 100 discret spaces with 10 shelfs
 */
function createProductShelfs(server, size) {

  let placedProducts = 0;

  // place products until y equals 80 not 100 to have necessary space
  const factor = 90 / numberByShelf;

  // create product lines
  while(placedProducts < size) {
    productLanes.forEach(xCoord => {  
      for(let i = 0; i < numberByShelf; i++) {
        // break if number of products reached
        if(placedProducts >= size) {
          break;
        }
        server.create("product", { id: placedProducts, x: xCoord, y: computeYPosition(xCoord, i, factor)});
        placedProducts++;
      }
    })
  };
}

/**
 * Maintain robot activity
 */
function createRobots(numberOfRobots) {
  robots = [];

  // possible lanes
  const factor = Math.floor(100 / numberOfRobots);

  for(let i = 0; i < numberOfRobots; i++) {
    // set robotlane
    let x = 2 + i * factor;
    // avoid shelfs
    x = x % 10 < 2 ? x + factor * 0.3: x;

    // set start and destination random with fixed x
    const yStart = computeYPosition(x, 0, 0) + Math.floor(Math.random() * 90);
    const yDest = computeYPosition(x, 0, 0) + Math.floor(Math.random() * 90);

    // set speed random
    const speed = Math.random().toFixed(2) * 0.1;

    // add robot
    robots.push(new Robot(i,x, yStart, speed, x, yDest));
  }
}

export function makeServer(numberRobots, numberProducts, { environment = "test" } = {}) {
  let server = createServer({
    environment,

    models: {
      product: Model,
    },

    seeds(server) {
      createProductShelfs(server, numberProducts);
      createRobots(numberRobots);
    },

    routes() {
      this.logging = false;
      this.namespace = "api";

      this.get("/products", (schema) => {
        return schema.products.all();
      });

      this.get("/robots", () => {
        // move robots at each call
        robots.forEach(robot => robot.move());
        return {robots: robots.map(robot => robot.toJson())};
      })
    },
  });

  return server;
}