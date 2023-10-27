import React from "react";
import cl from "./.module.css";
import { Outlet } from "react-router-dom";
import InfoFooter from "../InfoFooter/InfoFooter";

const NavigationHeader = () => {
    return (
        <>
            <header className={cl.nav_header}>
                <div className={cl.logo}>
                    <img className={cl.logo_img} />
                    <h1 className={cl.logo_text}>3D Forge</h1>
                </div>
            </header>
            <Outlet />
            <InfoFooter />
        </>
    );
}

export default NavigationHeader;