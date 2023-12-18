import React from "react";
import cl from "./.module.css";
import { useLocation, useParams } from "react-router-dom";
import { UserAPI } from "../../services/api/UserAPI";

const ResetPasswordPage = () => {

    const searchParams = new URLSearchParams(useLocation().search);

    const [isModalWindowVisible, setModalWindowVisibility] = React.useState(false);

    const newPasswordRef = React.useRef();
    const confirmNewPasswordRef = React.useRef();

    function ResetPasswordRequest() {
        UserAPI.resetPassword(
            searchParams.get('login'),
            newPasswordRef.current.value,
            confirmNewPasswordRef.current.value,
            null,
            searchParams.get('token')
        ).then(res => { return res.json() })
        .then(data => {
            if (data.success === undefined) {
                alert("Some fields are empty!");
                return;
            }
            if (!data.success) {
                alert(data.message);
                return;
            }
            setModalWindowVisibility(true);
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
                        <h2 className={cl.modal_window_header}>Зміна паролю пройшла успішно!</h2>
                    </div>
                    <div className={cl.modal_window_control}>
                        <div className={cl.modal_window_send_email_button} onClick={() => { window.location.pathname = '/auth' }}>
                            <span className={cl.modal_window_send_email_button_text}>ОК</span>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    React.useEffect(() => {
        document.body.style.background = "white";
        document.body.style.overflow = "auto";
    });

    return (
        <div className={cl.main}>
            <form className={cl.reset_password_panel}>
                <h2 className={cl.reset_password_header}>Змінити пароль</h2>
                <div className={cl.new_password}>
                    <div className={`${cl.input_header} ${cl.new_password_header}`}>
                        <span className={`${cl.input_header_text} ${cl.new_password_header_text}`}>Новий пароль</span>
                        <span className={`${cl.necessary_star} ${cl.new_password_header_star}`}>*</span>
                    </div>
                    <input className={`${cl.password_input} ${cl.new_password_input}`} type="password" placeholder="6+ символів" ref={newPasswordRef} />
                </div>
                <div className={cl.confirm_password}>
                    <div className={`${cl.input_header} ${cl.confirm_password_header}`}>
                        <span className={`${cl.input_header_text} ${cl.confirm_password_header_text}`}>Підтвердження нового паролю</span>
                        <span className={`${cl.necessary_star} ${cl.confirm_password_header_star}`}>*</span>
                    </div>
                    <input className={`${cl.password_input} ${cl.confirm_password_input}`} type="password" ref={confirmNewPasswordRef} />
                </div>
                <div className={cl.submit_button} onClick={() => ResetPasswordRequest()}>
                    <span className={cl.submit_button_text}>Змінити пароль</span>
                </div>
                <a className={cl.go_to_main_page_link} href='/'>Перейти до головної сторінки</a>
            </form>
            {RenderModalWindow()}
        </div>
    );
}

export default ResetPasswordPage;