import Shape from './Shape';
import type {RectangleGeometry} from './types/Geometry';

class Rectangle extends Shape<RectangleGeometry> {
        constructor(x: number, y: number, width:number, height: number) {
            super({
                type: 'rectangle', 
                point: {
                    x, 
                    y
                },
                width,
                height
            });
        }

        calculateArea(): number {
            return this.geometry.width * this.geometry.height;
        }
}

export default Rectangle;