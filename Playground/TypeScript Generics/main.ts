import Rectangle from "./Rectangle";
import Circle from "./Circle";

const rect = new Rectangle(0, 0, 10, 10);
const circ = new Circle(0, 0, 2);

console.log(rect.calculateArea());
console.log(circ.calculateArea());