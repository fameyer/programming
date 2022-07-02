import type {Geometry} from './types/Geometry';

abstract class Shape<T extends Geometry> {
    protected geometry: T;

    constructor(geometry: T) {
        this.geometry = geometry;
    }

    abstract calculateArea(): number;
}

export default Shape;