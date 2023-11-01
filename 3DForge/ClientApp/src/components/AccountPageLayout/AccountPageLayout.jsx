import React from "react";
import cl from "./.module.css";
import { Outlet, useNavigate } from "react-router-dom";
import { UserAPI } from "../../services/api/UserAPI";
import noAvatarImg from './img/no-avatar.png';
import LoadingAnimation from "../LoadingAnimation/LoadingAnimation";

const AccountPageLayout = () => {

    const navigate = useNavigate();

    const [isAuthorized, setAuthorizationState] = React.useState(false);
    const [isCheckingAuthState, setCheckingAuthState] = React.useState(true);
    const [isDropMenuVisible, setDropMenuVisibility] = React.useState(false);
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
                    <img className={cl.avatar} alt="avatar" src={userAvatar} onClick={() => { navigate("/user/info") }} />
                    <div className={cl.drop_menu} ref={dropMenuRef}>
                        <img className={cl.drop_menu_img} onClick={() => setDropMenuVisibility(p => !p)} alt="drop menu" />
                        <div className={cl.drop_list} style={{ display: isDropMenuVisible ? 'block' : 'none' }}>
                            <div class={cl.drop_menu_triangle} />
                            <p className={`${cl.drop_list_element} ${cl.drop_list_element_orders}`}>Мої замовлення</p>
                            <p className={`${cl.drop_list_element} ${cl.drop_list_element_settings}`}
                                onClick={() => navigate('/user/edit')}>Налаштування</p>
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
        UserAPI.check().then(res => {
            if (res.ok) {
                setAuthorizationState(true);
            }
            setCheckingAuthState(false);
        });

        if (userAvatar === undefined) {
            UserAPI.getSelfAvatar()
                .then(res => {
                    if (res.status === 404) {
                        throw new Error();
                    }
                    return res.blob();
                })
                .then(blob => {
                    setUserAvatar(URL.createObjectURL(blob));
                })
                .catch(() => {
                    setUserAvatar(noAvatarImg);
                });
        }

        window.addEventListener("click", WindowClickEvent);

        return () => {
            window.removeEventListener("click", WindowClickEvent);
        };
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
                    <div className={cl.contact_icon_list}>
                        <img className={`${cl.contact_icon} ${cl.contact_icon_instagram}`} alt="instagram" />
                        <img className={`${cl.contact_icon} ${cl.contact_icon_facebook}`} alt="facebook" />
                        <img className={`${cl.contact_icon} ${cl.contact_icon_google_plus}`} alt="google plus" />
                    </div>
                </div>
                <div className={cl.line_separator} />
                <div className={`${cl.info_footer_container} ${cl.subscribe}`}>
                    <h3 className={`${cl.info_footer_header} ${cl.subscribe_header}`}>Підпишіться на розсилку</h3>
                    <div className={cl.subscribe_button}>
                        <img className={cl.subscribe_button_img} alt="email" />
                        <span className={cl.subscribe_button_text}>Введіть Ваш Email</span>
                    </div>
                    <p className={`${cl.info_footer_text} ${cl.subscribe_text}`}>Підписатися</p>
                </div>
            </footer>
        </>
    );
}

export default AccountPageLayout;