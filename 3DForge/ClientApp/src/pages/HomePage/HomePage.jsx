import React from "react";
import cl from "./.module.css";

const HomePage = () => {

    React.useEffect(() => {
        document.body.style.background = "linear-gradient(270deg, #7C26BF 0.13%, #BA67D3 99.84%, #7030B5 99.85%)";
    });

    return (
        <img className={cl.logo} />
    );
}

export default HomePage;