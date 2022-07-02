/**
 * Class representing an employee entity in the given context
 */
class employee {
    constructor(name) {
        this.name = name ? name : 'Unset';
        this.reviews =  [];
    }
}

export default employee;