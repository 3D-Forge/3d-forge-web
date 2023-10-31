import React from 'react';
import cl from "./.module.css";
import { UserAPI } from '../../services/api/UserAPI';
import LoadingAnimation from '../../components/LoadingAnimation/LoadingAnimation';

const RegistrationPage = () => {

    const loginRef = React.useRef();
    const emailRef = React.useRef();
    const passwordRef = React.useRef();
    const confirmPasswordRef = React.useRef();

    const [isRegistering, setRegisteringStatus] = React.useState(false);

    async function RegisterRequest(login, email, password, confirmPassword) {
        setRegisteringStatus(true);
        await UserAPI.register(login, email, password, confirmPassword)
        .then(res => { return res.json() })
        .then(data => {
            if (data.success === undefined) {
                alert("Some fields are empty!");
            }
            else {
                alert(data.message);
            }
        });
        setRegisteringStatus(false);
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
                <img className={cl.intro_img} alt='registration' />
            </aside>
            <div className={cl.reg}>
                <div className={cl.auth_request}>
                    <span className={cl.auth_request_question}>Вже зареєстровані?</span>
                    <a className={cl.auth_request_link} href="/auth">Увійти</a>
                </div>
                <form className={cl.reg_panel}>
                    <h2 className={cl.reg_header}>Зареєструватись</h2>
                    <div className={cl.login_name}>
                        <div className={`${cl.reg_input_header} ${cl.login_name_header}`}>
                            <span className={`${cl.reg_input_header_text} ${cl.login_name_header_text}`}>Логін</span>
                            <span className={`${cl.necessary_star} ${cl.login_name_header_star}`}>*</span>
                        </div>
                        <input className={`${cl.reg_input} ${cl.login_name_input}`} type="text" ref={loginRef} />
                    </div>
                    <div className={cl.email}>
                        <div className={`${cl.reg_input_header} ${cl.email_header}`}>
                            <span className={`${cl.reg_input_header_text} ${cl.email_header_text}`}>Електрона пошта</span>
                            <span className={`${cl.necessary_star} ${cl.email_header_star}`}>*</span>
                        </div>
                        <input className={`${cl.reg_input} ${cl.email_input}`} type="email" ref={emailRef} />
                    </div>
                    <div className={cl.password}>
                        <div className={`${cl.reg_input_header} ${cl.password_header}`}>
                            <span className={`${cl.reg_input_header_text} ${cl.password_header_text}`}>Пароль</span>
                            <span className={`${cl.necessary_star} ${cl.password_header_star}`}>*</span>
                        </div>
                        <input className={`${cl.reg_input} ${cl.password_input}`} type="password" placeholder="6+ символів" ref={passwordRef} />
                    </div>
                    <div className={cl.password_confirm}>
                        <div className={`${cl.reg_input_header} ${cl.password_confirm_header}`}>
                            <span className={`${cl.reg_input_header_text} ${cl.password_confirm_header_text}`}>Повтор паролю</span>
                            <span className={`${cl.necessary_star} ${cl.password_confirm_header_star}`}>*</span>
                        </div>
                        <input className={`${cl.reg_input} ${cl.password_confirm_input}`} type="password" ref={confirmPasswordRef} />
                    </div>
                    <div className={cl.agreement}>
                        <div className={cl.agreement_checkbox_cont}>
                            <div className={cl.agreement_checkbox}>
                                <img className={cl.agreement_checkbox_check} alt='check' />
                            </div>
                        </div>
                        <span className={cl.agreement_text}>Створюючи акаунт ви погоджуєтесь продати нам своє тіло та душу (і кота)</span>
                    </div>
                    <div className={cl.reg_button} onClick={() => {
                        RegisterRequest(
                            loginRef.current.value,
                            emailRef.current.value,
                            passwordRef.current.value,
                            confirmPasswordRef.current.value
                        );
                    }}>
                        {
                            isRegistering
                                ? <LoadingAnimation size="30px" loadingCurveWidth="6px" />
                                : <p className={cl.log_in_button_text}>Створити акаунт</p>
                        }
                    </div>
                </form>
            </div>
        </div>
    );
}

export default RegistrationPage;
