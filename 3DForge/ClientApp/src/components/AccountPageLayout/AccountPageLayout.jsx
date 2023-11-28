import React from "react";
import cl from "./.module.css";
import { Outlet, useNavigate } from "react-router-dom";
import { UserAPI } from "../../services/api/UserAPI";
import LoadingAnimation from "../LoadingAnimation/LoadingAnimation";

const AccountPageLayout = () => {

    const navigate = useNavigate();

    const [isAuthorized, setAuthorizationState] = React.useState(false);
    const [isCheckingAuthState, setCheckingAuthState] = React.useState(true);
    const [isDropMenuVisible, setDropMenuVisibility] = React.useState(false);
    const [canAdministrateSystem, setAdministrateSystemRight] = React.useState(false);
    const [userAvatar, setUserAvatar] = React.useState(undefined);

    const dropMenuRef = React.useRef();

    function LogoutRequest() {
        UserAPI.logout().then(() => {
            if (window.location.pathname !== '/') {
                window.location.pathname = '/';
                return;
            }
            window.location.reload();
        });
    }

    function RenderUserOptions() {
        if (isCheckingAuthState) {
            return (
                <div className={cl.loading}>
                    <LoadingAnimation size="50px" loadingCurveWidth="8px" />
                </div>
            );
        }

        if (isAuthorized) {
            return (
                <>
                    <img className={cl.shop} alt="shop" />
                    {!userAvatar ?
                        <div className={cl.loading_avatar}>
                            <LoadingAnimation size="30px" loadingCurveWidth="6px" />
                        </div>
                        : <img className={cl.avatar} alt="avatar" src={userAvatar} onClick={() => { navigate("/user/info") }} />
                    }
                    <div className={cl.drop_menu} ref={dropMenuRef}>
                        <img className={cl.drop_menu_img} onClick={() => setDropMenuVisibility(p => !p)} alt="drop menu" />
                        <div className={cl.drop_list} style={{ display: isDropMenuVisible ? 'block' : 'none' }}>
                            <div className={cl.drop_menu_triangle} />
                            <p className={`${cl.drop_list_element} ${cl.drop_list_element_orders}`}>Мої замовлення</p>
                            <p className={`${cl.drop_list_element} ${cl.drop_list_element_models}`}>Мої публікації</p>
                            <p className={`${cl.drop_list_element} ${cl.drop_list_element_settings}`}
                                onClick={() => {
                                    window.location.pathname = 'user/edit';
                                    setDropMenuVisibility(false);
                                }}>Налаштування</p>
                            {canAdministrateSystem ?
                                <p className={`${cl.drop_list_element} ${cl.drop_list_element_admin}`}
                                    onClick={() => {
                                        window.location.pathname = 'admin';
                                    }}>Адмінстрування</p>
                                : <></>
                            }
                            <p className={`${cl.drop_list_element} ${cl.drop_list_element_logout}`}
                                onClick={() => LogoutRequest()}>Вихід</p>
                        </div>
                    </div>
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

    function WindowClickEvent(event) {
        if (isDropMenuVisible && !dropMenuRef.current.contains(event.target)) {
            setDropMenuVisibility(false);
            return;
        }
    }

    React.useEffect(() => {
        UserAPI.check().then(res => res.json()).then(json => {
            if (json.success) {
                setAuthorizationState(true);
                setAdministrateSystemRight(json.message === "administrator" ? true : false);
            }

            const isCurrentPageAllowed =
                window.location.pathname === '/'
                || window.location.pathname.includes('/catalog');

            if (!isCurrentPageAllowed && !json.success) {
                window.history.back();
            }

            if (json.message !== "administrator" && window.location.pathname === '/admin') {
                window.history.back();
            }

            setCheckingAuthState(false);
        });

        if (userAvatar === undefined) {
            UserAPI.getSelfAvatar()
                .then(res => { return res.blob(); })
                .then(blob => { setUserAvatar(URL.createObjectURL(blob)); });
        }

        window.addEventListener("click", WindowClickEvent);

        return () => {
            window.removeEventListener("click", WindowClickEvent);
        };
    });

    return (
        <>
            <nav className={cl.nav_header}>
                <div className={cl.logo} onClick={() => { window.location.pathname = '/' }}>
                    <img className={cl.logo_img} alt='logo' />
                    <h1 className={cl.logo_text}>3D Forge</h1>
                </div>
                <div className={cl.page_list}>
                    <div className={`${cl.page} ${cl.forum_page}`}>
                        <span className={`${cl.page_text} ${cl.forum_page_text}`}>Форум</span>
                    </div>
                    <div className={`${cl.page} ${cl.catalog_page}`} onClick={() => { window.location.pathname = '/catalog' }}>
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
            <footer className={cl.info_footer}>
                <div className={`${cl.info_footer_container} ${cl.about_us}`}>
                    <h3 className={`${cl.info_footer_header} ${cl.about_us_header}`}>Про Нас</h3>
                    <p className={`${cl.info_footer_text} ${cl.about_us_text}`}>
                        Ми надаємо можливість створювати, ділитися та замовляти унікальні 3D-моделі.
                        Наша мета - зробити 3D-друк доступним для кожного. Долучайтеся до нашої
                        спільноти та створюйте разом з нами!
                    </p>
                </div>
                <div className={cl.line_separator} />
                <div className={`${cl.info_footer_container} ${cl.contacts}`}>
                    <h3 className={`${cl.info_footer_header} ${cl.contacts_header}`}>Зв'яжіться з нами</h3>
                    <p className={`${cl.info_footer_text} ${cl.contacts_phone_numbers}`}>
                        +380 999 999 99 99
                    </p>
                    <p className={`${cl.info_footer_text} ${cl.contacts_emails}`}>
                        3d.forgehub@gmail.com
                    </p>
                </div>
                <div className={cl.line_separator} />
                <div className={`${cl.info_footer_container} ${cl.find_us}`}>
                    <h3 className={`${cl.info_footer_header} ${cl.find_us_header}`}>Знайдіть нас</h3>
                    <p className={`${cl.info_footer_text} ${cl.find_us_text}`}>
                        Проспект Науки, 14, Харків, Харківська область, Україна, 61166
                    </p>
                </div>
            </footer>
        </>
    );
}

export default AccountPageLayout;