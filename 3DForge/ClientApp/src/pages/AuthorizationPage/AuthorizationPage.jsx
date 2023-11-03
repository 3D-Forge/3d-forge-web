import React from 'react';
import cl from "./.module.css";
import { useNavigate } from 'react-router-dom';
import { UserAPI } from '../../services/api/UserAPI';
import LoadingAnimation from '../../components/LoadingAnimation/LoadingAnimation';

const AuthorizationPage = () => {

    const navigate = useNavigate();

    const loginOrEmailRef = React.useRef();
    const passwordRef = React.useRef();
    const modalWindowLoginRef = React.useRef();

    const [isAuthorizing, setAuthorizingStatus] = React.useState(false);
    const [isModalWindowVisible, setModalWindowVisibility] = React.useState(false);

    async function AuthorizeRequest(loginOrEmail, password) {
        setAuthorizingStatus(true);
        await UserAPI.login(loginOrEmail, password)
            .then(res => { return res.json(); })
            .then(data => {
                if (data.success) {
                    navigate('/', { replace: true });
                    return;
                }

                if (data.success === undefined) {
                    alert("Some fields are empty!");
                }
                else {
                    alert(data.message);
                }
            });
        setAuthorizingStatus(false);
    }

    function GetResetPasswordPermission() {
        UserAPI.sendResetPasswordPermission(modalWindowLoginRef.current.value)
            .then(res => { return res.json(); })
            .then(data => {
                alert(data.message);
                if (data.success) {
                    modalWindowLoginRef.current.value = "";
                    setModalWindowVisibility(false);
                }
            });
    }

    function RenderModalWindow() {
        if (!isModalWindowVisible) {
            return;
        }

        return (
            <div className={cl.modal_window_background}>
                <div className={cl.modal_window}>
                    <div className={cl.modal_window_content}>
                        <h2 className={cl.modal_window_header}>Відправка посилання на пошту</h2>
                        <p className={cl.modal_window_description}>
                            Напишіть логін вашого акаунту для відправки письма з
                            посиланням на на сторінку зміни паролю.
                        </p>
                        <div className={cl.modal_window_login_panel}>
                            <h3 className={cl.modal_window_login_panel_header}>Логін</h3>
                            <input className={cl.modal_window_login_input} type="text" ref={modalWindowLoginRef} />
                        </div>
                    </div>
                    <div className={cl.modal_window_control}>
                        <div className={cl.modal_window_send_email_button} onClick={() => GetResetPasswordPermission()}>
                            <span className={cl.modal_window_send_email_button_text}>Відправити</span>
                        </div>
                        <div className={cl.modal_window_cancel_button} onClick={() => setModalWindowVisibility(false)}>
                            <span className={cl.modal_window_cancel_button_text}>Скасувати</span>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    React.useEffect(() => {
        document.body.style.overflow = "auto";
    });

    return (
        <div className={cl.main}>
            <aside className={cl.intro}>
                <div className={cl.intro_main_part}>
                    <div className={cl.logo}>
                        <img className={cl.logo_img} alt='logo' />
                        <h1 className={cl.logo_text}>3D Forge</h1>
                    </div>
                    <p className={cl.description}>Допоможемо твоїм ідеям стати реальністю. Поглибся у чарівний світ 3D!</p>
                </div>
                <img className={cl.intro_img} alt='authorization' />
            </aside>
            <div className={cl.auth}>
                <div className={cl.reg_request}>
                    <span className={cl.reg_request_question}>Немає акаунту?</span>
                    <a className={cl.reg_request_link} href="/register">Зареєструватися</a>
                </div>
                <form className={cl.auth_panel}>
                    <h2 className={cl.auth_header}>Увійти</h2>
                    <div className={cl.login_or_email}>
                        <div className={`${cl.auth_input_header} ${cl.login_or_email_header}`}>
                            <span className={`${cl.auth_input_header_text} ${cl.login_or_email_header_text}`}>Логін/Пошта</span>
                            <span className={`${cl.necessary_star} ${cl.login_or_email_header_star}`}>*</span>
                        </div>
                        <input className={`${cl.auth_input} ${cl.login_or_email_input}`} type="text" ref={loginOrEmailRef} />
                    </div>
                    <div className={cl.password}>
                        <div className={`${cl.auth_input_header} ${cl.password_header}`}>
                            <span className={`${cl.auth_input_header_text} ${cl.password_header_text}`}>Пароль</span>
                            <span className={`${cl.necessary_star} ${cl.password_header_star}`}>*</span>
                        </div>
                        <input className={`${cl.auth_input} ${cl.password_input}`} type="password" ref={passwordRef} />
                    </div>
                    <div className={cl.remember}>
                        <div className={cl.remember_checkbox_cont}>
                            <div className={cl.remember_checkbox}>
                                <img className={cl.remember_checkbox_check} alt='check' />
                            </div>
                        </div>
                        <span className={cl.remember_text}>Запам'ятати мене</span>
                    </div>
                    <div className={cl.auth_button} onClick={() => {
                        AuthorizeRequest(
                            loginOrEmailRef.current.value,
                            passwordRef.current.value
                        );
                    }}>
                        {
                            isAuthorizing
                                ? <LoadingAnimation size="30px" loadingCurveWidth="6px" />
                                : <p className={cl.log_in_button_text}>Увійти</p>
                        }
                    </div>
                    <p className={cl.forgot_password} onClick={() => setModalWindowVisibility(true)}>Забули свій пароль?</p>
                </form>
            </div>
            {RenderModalWindow()}
        </div>
    );
}

export default AuthorizationPage;
