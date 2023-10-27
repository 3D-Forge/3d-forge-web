import React from "react";

const Space = ({ height = "0px", width = "0px", backgroundColor = "" }) => {

    return (
        <div style={{
            height: height,
            width: width,
            backgroundColor: backgroundColor
        }} />
    );
}

export default Space;