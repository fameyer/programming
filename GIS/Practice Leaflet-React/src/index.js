import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import StoreMap from './storeMap';
import SiteBar from './siteBar';

// mirage import
import { makeServer } from "./server"

const maxRobots = 50;
const maxProducts = 100000;

const defaultNumberRobots = 10;
const defaultNumberProducts = 2500;

/**
 * Main class
 */
class RobotMap extends React.Component {
  constructor(props) {
      super(props);

      this.state = {
        // tablerows are filled in the map component and then delegated to the sitebar
        tableRows: [],
      };

      this.storeMapRef = React.createRef();  

      // create API mock server
      this.apiServer = {};
      if (process.env.NODE_ENV === "development") {
        this.apiServer = makeServer(defaultNumberRobots, defaultNumberProducts, { environment: "development" });
      }
  }

  /**
   * Reset simulation parameters by changing server
   */
  setSimulationParameter(numberRobots, numberProducts) {
      // create API mock server
      if (process.env.NODE_ENV === "development") {
        this.apiServer.shutdown();
        this.apiServer = makeServer(numberRobots, numberProducts, { environment: "development" });

        // rerender products on the map, robots are automatically updated
        this.storeMapRef?.current.renderProducts();
      }
  }

  /**
   * Set table rows and delegate to site bar
   * @param {} rows 
   */
  setTableRows(rows) {
    this.setState({tableRows: rows});
  }

  /**
   * Render function
   * @returns 
   */
  render() {    
    return (
        <div className="main">
          <StoreMap 
            ref={this.storeMapRef}
            onFeatureInfoClicked={(rows) => this.setTableRows(rows)}
          ></StoreMap>
          <SiteBar
            maxRobots={maxRobots}
            maxProducts={maxProducts}
            defaultNumberRobots={defaultNumberRobots}
            defaultNumberProducts={defaultNumberProducts}
            tableRows={this.state.tableRows}
            onSimulate={(numberRobots, numberProducts) => this.setSimulationParameter(numberRobots, numberProducts)}
          ></SiteBar>
        </div>
    );
  }
}

// ========================================

const root = ReactDOM.createRoot(document.getElementById("root"));

root.render(<RobotMap />);
  