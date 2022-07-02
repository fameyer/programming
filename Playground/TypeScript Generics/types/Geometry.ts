type Geometry = {
    type: string;
}

type RectangleGeometry = Geometry & {
    point: {x: number, y:number};
    width: number;
    height: number;
}

type CircleGeometry = Geometry & {
    center: {x: number, y:number};
    radius: number;
}

export {Geometry, RectangleGeometry, CircleGeometry};