// create most simple restful server
const express = require("express");
const app = express();

// use cors
const cors = require('cors')
app.use(cors())

// properties
const employees = [ {name: 'Schmidt', reviews: [{author: 'admin', contents: 'Good performance', pending: false}]}, 
                    {name: 'Yamada', reviews: [{author: 'Schmidt', contents: 'Excellent performance', pending: false}]} ];

// get all employees
app.get("/getEmployees", (req, res, next) => {
    res.json(employees);
});

// add employee
app.get("/addEmployee", (req, res, next) => {
    if(req.query.name) {
        employees.push({name: req.query.name, reviews: []});
        res.json({msg: 'Success'});
        return;
    }
    res.json({msg: 'Input invalid'});
});

// remove employee
app.get("/removeEmployee", (req, res, next) => {
    if(req.query.name) {
        const name = req.query.name
        const employee = employees.find(item => item.name === name);
        if(employee) {
            const index = employees.indexOf(employee);
            if (index > -1) {         
                employees.splice(index, 1);
                res.json({msg: 'Success'});
                return;
            }
        }
        res.json({msg: 'Employee not found'});     
    }
    else {
        res.json({msg: 'Input invalid'});
    }    
});

// add review
app.get("/addReview", (req, res, next) => {
    if(req.query.name && req.query.author && req.query.contents && req.query.pending) {
        const employee = employees.find(item => item.name === req.query.name);
        if(employee) {
            employee.reviews.push({author: req.query.author, contents: req.query.contents, pending: req.query.pending})
            res.json({msg: 'Success'});
            return;
        }
        res.json({msg: 'Employee not found'});  
    }
    res.json({msg: 'Input invalid'});
});

// update review
app.get("/updateReview", (req, res, next) => {
    if(req.query.name && req.query.author && req.query.contents) {
        const employee = employees.find(item => item.name === req.query.name);
        if(employee) {
            const review = employee.reviews.find(item => item.author === req.query.author);
            if(review) {
                review.contents = req.query.contents;
                review.pending = false;
                res.json({msg: 'Success'});
                return;
            }
            res.json({msg: 'Review not found'});              
        }
        else {
            res.json({msg: 'Employee not found'});  
        }
    }
    res.json({msg: 'Input invalid'});
});

// start api app
app.listen(9090, () => console.log('Employee node server started'));
