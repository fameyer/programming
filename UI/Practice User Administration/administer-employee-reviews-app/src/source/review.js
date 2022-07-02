/**
 * Class representing a review entity in the given context
 */
class review {
    constructor(author, contents) {
        this.author = author ? author : 'Unknown';
        this.contents = contents ? contents : 'Pending';
        this.pending = contents === undefined;
    }
}

export default review;