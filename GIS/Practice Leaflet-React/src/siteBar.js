import React from 'react';

// mui imports
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import Button from '@mui/material/Button';
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';
import TextField from '@mui/material/TextField';
import Typography from '@mui/material/Typography';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

import { DataGrid } from '@mui/x-data-grid';

class SiteBar extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            numberRobots: this.props.defaultNumberRobots,
            numberProducts: this.props.defaultNumberProducts,
            validation: {
                inputRobotsError: false,
                inputProductsError: false,
            }
        };

        // static columns for datatable
        this.columns = [
            { field: 'name', headerName: 'Fieldname', flex: 0.5 },
            { field: 'value', headerName: 'Value', flex: 1 }
        ];
    }

    /**
     * Handle text input
     * @param {} change 
     */
    handleProductsInput(change) {
        // check if valid number
        if(/^-?\d+$/.test(change.currentTarget?.value)) {
        this.setState({ numberProducts: parseInt(change.currentTarget?.value, 10) }, () => {
            this.setState({ validation: {inputProductsError: 0 > this.state.numberProducts || this.state.numberProducts > this.props.maxProducts}});
        });
        } 
        else {
        this.setState({ validation: {inputProductsError: true}});
        }
    }

    /**
     * Handle text input
     * @param {} change 
     */
    handleRobotsInput(change) {
        // check if valid number
        if(/^-?\d+$/.test(change.currentTarget?.value)) {
            this.setState({ numberRobots: parseInt(change.currentTarget?.value, 10) }, () => {
            this.setState({ validation: {inputRobotsError: 0 > this.state.numberRobots || this.state.numberRobots > this.props.maxRobots}});
            });
        } 
        else {
            this.setState({ validation: {inputRobotsError: true}});
        }
    }

    render() { 
        return (
            <Container 
                className="sidebar" 
                maxWidth="sm"
            >
                <Accordion>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                >
                <Typography variant="h6">Set simulation parameters</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <Grid container direction={'column'} spacing={5}>
                    <Grid item>
                        <TextField 
                        required 
                        error={this.state.validation.inputProductsError}
                        helperText={`Up to ${this.props.maxProducts} products are allowed`}
                        label="Number of Products"
                        variant="outlined"
                        defaultValue={this.state.numberProducts}
                        onChange={(change => this.handleProductsInput(change))}
                        >
                        </TextField> 
                    </Grid>
                    <Grid item>
                        <TextField 
                        required 
                        error={this.state.validation.inputRobotsError}
                        helperText={`Up to ${this.props.maxRobots} robots are allowed`}
                        label="Number of Robots" 
                        variant="outlined"
                        defaultValue={this.state.numberRobots}
                        onChange={(change => this.handleRobotsInput(change))}>
                        </TextField> 
                    </Grid>
                    <Grid item>
                        <Button 
                        variant="contained"
                        onClick={() => {
                            this.props.onSimulate(this.state.numberRobots, this.state.numberProducts);
                        }}
                        >Simulate</Button>
                    </Grid>
                    </Grid>
                </AccordionDetails>
                </Accordion>
                <Accordion expanded={true}>
                <AccordionSummary
                    aria-controls="panel2a-content"
                    id="panel2a-header"
                >
                    <Typography variant="h6">Feature Info</Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <Typography variant="subtitle1" color="green ">Select a robot or product for more info</Typography>
                    <DataGrid className="featureInfo" 
                    rows={this.props.tableRows}
                    columns={this.columns}
                    />
                </AccordionDetails>
                </Accordion>
            </Container> 
        )
    }
}

export default SiteBar;