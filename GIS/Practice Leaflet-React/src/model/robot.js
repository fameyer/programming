import { norm } from 'mathjs';
import generateName from '../helper/generateName';

/**
 * Create a robot object
 */
class Robot {
    constructor(id, x, y, speed, dest_x, dest_y) {
        this.name = generateName();
        this.id = id;
        this.x = x;
        this.y = y;
        
        this.speed = speed;
        this.dest_x = dest_x;
        this.dest_y = dest_y;   
        this.orientation = this.determineOrientation();

        // additional properties to determine routing
        this.start_x = x;
        this.start_y = y;
        this.direct_x = 0;
        this.direct_y = 0;   
        
        this.calculationDirection();
    }

    /**
     * Serialize the robot object
     * @returns JSON
     */
    toJson() {
        return {
            id: this.id,
            name: this.name,
            x: this.x,
            y: this.y,
            orientation: this.orientation,
            speed: this.speed,
            dest_x: this.dest_x,
            dest_y: this.dest_y
        };
    }

    /**
     * Calculate the direction vector form start to destination
     */
    calculationDirection() {
        const dist = norm([this.dest_x - this.start_x, this.dest_y - this.start_y]);

        if(dist > 0) {
            this.direct_x = (this.dest_x - this.start_x)/dist;
            this.direct_y = (this.dest_y - this.start_y)/dist;
        }
    }

    /**
     * Calculation the sign of the one-dimensional orientation - either positive, negative or zero
     * @param {*} dest 
     * @param {*} orig 
     * @returns 
     */
    calculateOrientationSign(dest, orig) {
        return dest - orig > 0 ? 1 : (dest - orig < 0 ? - 1 : 0);
    }

    /**
     * Determine the current orientation
     * @returns 
     */
    determineOrientation() {
        const orientation_x = this.calculateOrientationSign(this.dest_x, this.x);
        const orientation_y = this.calculateOrientationSign(this.dest_y, this.y);
        return orientation_x >= 0 ? (orientation_y >= 0 ? 'East' : 'West') : (orientation_y >= 0 ? 'North' : 'South');
    }

    /**
     * Move the robot by speed times the direction vector
     */
    move() {
        const prev_orientation_x = this.calculateOrientationSign(this.dest_x, this.x);
        const prev_orientation_y = this.calculateOrientationSign(this.dest_y, this.y);

        // linearly change coordinates for speed
        this.x += this.speed * this.direct_x;
        this.y += this.speed * this.direct_y;

        // check if orientation changed, then either destination is reached or overpassed
        const curr_orientation_x = this.calculateOrientationSign(this.dest_x, this.x);
        const curr_orientation_y = this.calculateOrientationSign(this.dest_y, this.y);

        if(curr_orientation_x !== prev_orientation_x || curr_orientation_y !== prev_orientation_y) {
            // switch start and destination for simplicity
            const x = this.start_x;
            const y = this.start_y;

            this.start_x = this.dest_x;
            this.start_y = this.dest_y;

            this.dest_x = x;
            this.dest_y = y;

            this.calculationDirection();
        }
    }
}

export default Robot;