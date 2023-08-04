import React from "react";
import { Button as MuiButton } from '@mui/material';

export default class Button extends React.Component {
    render() {
        const { label, onClick, color, variant } = this.props;

        const handleClick = () => {
            try {
                onClick();
            } catch (error) {
                console.log(error);
                alert('An error occurred. Please try again later.');
            }
        };

        return (
            <MuiButton label={label} onClick={handleClick} color={color} variant={variant}>
                {label}
            </MuiButton>
        );
    }
}

