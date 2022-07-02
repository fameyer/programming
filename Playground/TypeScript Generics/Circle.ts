import Shape from './Shape';
import type {CircleGeometry} from './types/Geometry';

class Circle extends Shape<CircleGeometry> {
        constructor(x: number, y: number, radius: number) {
            super({
                type: 'circle', 
                center: {
                    x, 
                    y
                },
                radius,
            });
        }

        calculateArea(): number {
            return Math.PI*(this.geometry.radius**2);
        }
}

export default Circle;