import React from "react";
import cl from './.module.css';

const UserEditPage = () => {

    const [modalWindowType, setModalWindowType] = React.useState(undefined);

    function RenderModalWindow() {
        switch (modalWindowType) {
            case "change-email":
                return (
                    <div className={cl.modal_widow_background}>
                        <div className={cl.modal_widow_panel}>
                            <div className={cl.modal_widow_content}>

                            </div>
                            <div className={cl.modal_widow_control}>

                            </div>
                        </div>
                    </div>
                );
            case "change-password":
                return;
            case "delete-account":
                return;
            default:
                return;
        }
    }

    React.useEffect(() => {
        document.body.style.background = "linear-gradient(270deg, #7C26BF 0.13%, #BA67D3 99.84%, #7030B5 99.85%)";
    });

    return (
        <div className={cl.main}>
            <div className={cl.main_panel}>
                <div className={cl.avatar_menu}>
                    <div className={cl.avatar_cont}>
                        <img className={cl.avatar} alt="avatar" />
                    </div>
                    <div className={cl.avatar_control}>
                        <p className={cl.profile_text}>Профіль</p>
                        <p className={cl.login}>KotykV</p>
                        <p className={cl.description}>Оновіть своє фото профілю та особисту інформацію</p>
                        <div className={cl.change_avatar_button}>
                            <span className={cl.change_avatar_button_text}>Обрати Фото</span>
                        </div>
                    </div>
                </div>
                <div className={cl.info_control}>
                    <div className={cl.save_changes_button}>
                        <span className={cl.save_changes_button_text}>Зберегти</span>
                    </div>
                    <div className={cl.cancel_changes_button}>
                        <span className={cl.cancel_changes_button_text}>Скасувати</span>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default UserEditPage;