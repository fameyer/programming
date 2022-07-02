class Model {
    constructor(url) {
        // url to api server
        this.url = url;
    }
    
    /**
     * Send get request (return a promise)   
     */
    async sendGetRequest(apiCommand, query) {
        return new Promise((resolve, reject) => {
            const http = new XMLHttpRequest()
            
            // set request url with optional query parameters
            let url = this.url + '/' + apiCommand;
            url += query ? '?' + query : '';

            http.open("GET", url);
            http.send();    
            http.onload = () => { 
                if(http.responseText) {
                    const jsonText = JSON.parse(http.responseText);
                    resolve(jsonText);
                }
                else {
                    reject();
                }      
            };
        })
    }

    /**
     * Get employee data (name and reviews)
     */
    getEmployeesAndReviews() {
        return this.sendGetRequest('getEmployees');
    }

    /**
     * Add employee of the given name
     */
    addEmployee(employee) {
        const query = 'name='+ employee.name;
        return this.sendGetRequest('addEmployee', query);                
    }

    /**
     * Remove employee of the given name     
     */
    removeEmployee(employee) {
        const query = 'name='+ employee.name;
        return this.sendGetRequest('removeEmployee', query);
    } 

    /**
     * Add review to an employee of the given name     
     */
    addReview(employee, review) {
        // add query parameter
        let query = 'name='+ employee.name;
        query+= '&author=' + review.author;
        query+= '&contents=' + review.contents;
        query+= '&pending=' + review.pending;

        return this.sendGetRequest('addReview', query);
    }

    /**
     * Add review to an employee of the given name     
     */
    updateReview(employee, review) {
        // add query parameter
        let query = 'name='+ employee.name;
        query+= '&author=' + review.author;
        query+= '&contents=' + review.contents;
        
        return this.sendGetRequest('updateReview', query);
    }
}

export default Model;
