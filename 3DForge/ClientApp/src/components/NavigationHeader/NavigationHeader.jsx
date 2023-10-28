import React from "react";
import cl from "./.module.css";
import { Outlet, useNavigate } from "react-router-dom";
import InfoFooter from "../InfoFooter/InfoFooter";
import { UserAPI } from "../../services/api/UserAPI";

const NavigationHeader = () => {

    const navigate = useNavigate();

    const [isAuthorized, setAuthorizationState] = React.useState(false);

    function RenderUserOptions() {
        if (isAuthorized) {
            return (
                <>
                    <img className={cl.shop} alt="shop" />
                    <img className={cl.avatar} alt="avatar" onClick={() => { navigate("/user/info") }} />
                    <img className={cl.drop_menu} alt="drop menu" />
                </>
            );
        }

        return (
            <>
                <div className={cl.login} onClick={() => navigate('/auth')}>
                    <p className={cl.login_text}>Увійти</p>
                </div>
                <div className={cl.register} onClick={() => navigate('/register')}>
                    <p className={cl.register_text}>Реєстрація</p>
                </div>
            </>
        );
    }

    React.useEffect(() => {
        UserAPI.check().then(res => {
            if (res.ok) {
                setAuthorizationState(true);
            }
        });
    });

    return (
        <>
            <nav className={cl.nav_header}>
                <div className={cl.logo}>
                    <img className={cl.logo_img} alt='logo' />
                    <h1 className={cl.logo_text}>3D Forge</h1>
                </div>
                <div className={cl.page_list}>
                    <div className={`${cl.page} ${cl.forum_page}`}>
                        <span className={`${cl.page_text} ${cl.forum_page_text}`}>Форум</span>
                    </div>
                    <div className={`${cl.page} ${cl.catalog_page}`}>
                        <span className={`${cl.page_text} ${cl.forum_page_text}`}>Каталог</span>
                    </div>
                </div>
                <div className={cl.account_options}>
                    <div className={cl.theme_editor}>
                        <img className={cl.light_theme} alt="light theme" />
                        <label className={cl.switch}>
                            <input className={cl.switch_checkbox} type="checkbox" />
                            <span className={cl.switch_slider}></span>
                        </label>
                        <img className={cl.dark_theme} alt="dark theme" />
                    </div>
                    {RenderUserOptions()}
                </div>
            </nav>
            <Outlet />
            <InfoFooter />
        </>
    );
}

export default NavigationHeader;