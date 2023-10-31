import React from 'react';
import cl from "./.module.css";
import { useNavigate } from 'react-router-dom';
import { UserAPI } from '../../services/api/UserAPI';
import LoadingAnimation from '../../components/LoadingAnimation/LoadingAnimation';

const AuthorizationPage = () => {

    const navigate = useNavigate();

    const loginOrEmailRef = React.useRef();
    const passwordRef = React.useRef();

    const [isAuthorizing, setAuthorizingStatus] = React.useState(false);

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
                    <a className={cl.forgot_password_link} href='/register'>Забули свій пароль?</a>
                </form>
            </div>
        </div>
    );
}

export default AuthorizationPage;
