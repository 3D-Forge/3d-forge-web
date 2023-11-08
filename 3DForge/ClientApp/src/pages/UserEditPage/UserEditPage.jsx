import React from "react";
import cl from './.module.css';
import { UserAPI } from "../../services/api/UserAPI";

const UserEditPage = () => {
    const [userInfo, setUserInfo] = React.useState(undefined);
    const [userAvatar, setUserAvatar] = React.useState(undefined);
    const [currentDeliveryType, setDeliveryType] = React.useState("branch");
    const [currentModalWindowType, setModalWindowType] = React.useState(undefined);
    const [hasOrderStateChangedNote, setOrderStateChangedNote] = React.useState(true);
    const [hasGotForumResponseNote, setGotForumResponseNote] = React.useState(true);
    const [hasModelRatedNote, setModelRatedNote] = React.useState(true);

    const citySelectorRef = React.useRef();
    const citySelectorListRef = React.useRef();
    const departmentSelectorRef = React.useRef();
    const departmentSelectorListRef = React.useRef();

    function ScrollToSection(sectionId) {
        let element = document.getElementById(sectionId)

        element.scrollIntoView({ behavior: 'smooth' });
        element.style.border = '2px solid white';

        setTimeout(() => {
            element.style.border = '2px solid rgba(0, 0, 0, 0)';
        }, 1000);
    }

    function RenderModalWindow() {
        switch (currentModalWindowType) {
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
        }
    }

    function WindowClickEvent(event) {
        if (citySelectorListRef.current.style.display === 'block' && !citySelectorRef.current.contains(event.target)) {
            citySelectorListRef.current.style.display = 'none';
        }

        if (citySelectorRef.current.contains(event.target) && !citySelectorListRef.current.contains(event.target)) {
            citySelectorListRef.current.style.display =
                citySelectorListRef.current.style.display === 'block' ? 'none' : 'block';
        }

        if (departmentSelectorListRef.current.style.display === 'block' && !departmentSelectorRef.current.contains(event.target)) {
            departmentSelectorListRef.current.style.display = 'none';
        }

        if (departmentSelectorRef.current.contains(event.target) && !departmentSelectorListRef.current.contains(event.target)) {
            departmentSelectorListRef.current.style.display =
                departmentSelectorListRef.current.style.display === 'block' ? 'none' : 'block';
        }
    }

    React.useEffect(() => {
        document.body.style.background = "linear-gradient(270deg, #7C26BF 0.13%, #BA67D3 99.84%, #7030B5 99.85%)";

        if (userInfo === undefined) {
            UserAPI.getSelfInfo()
                .then(res => { return res.json() })
                .then(data => {
                    if (data.success) {
                        setUserInfo(data.data);
                    }
                });
        }

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
        <div className={cl.main}>
            <div className={cl.content}>
                <div className={cl.top_panel}>
                    <div className={cl.avatar_cont}>
                        <img className={cl.avatar} alt="avatar" src={userAvatar} />
                    </div>
                    <div className={cl.avatar_editor}>
                        <div className={cl.avatar_editor_cont}>
                            <h2 className={cl.profile_text}>Профіль</h2>
                            <p className={cl.login}>{userInfo?.login}</p>
                            <p className={cl.description}>Оновіть своє фото профілю та особисту інформацію</p>
                            <div className={cl.change_avatar_button}>
                                <span className={cl.change_avatar_button_text}>Обрати Фото</span>
                            </div>
                        </div>
                    </div>
                    <div className={cl.info_control}>
                        <div className={cl.info_control_cont}>
                            <div className={cl.save_changes_button}>
                                <span className={cl.save_changes_button_text}>Зберегти</span>
                            </div>
                            <div className={cl.cancel_changes_button} onClick={() => window.history.back()}>
                                <span className={cl.cancel_changes_button_text}>Скасувати</span>
                            </div>
                        </div>
                    </div>
                </div>
                <div className={cl.other_info_panel}>
                    <div className={cl.section_navigation}>
                        <div className={`${cl.section} ${cl.section_general_info}`}
                            onClick={() => ScrollToSection("general-info-content")}>
                            <img className={`${cl.section_img} ${cl.section_general_info_img}`} alt='general info' />
                            <span className={`${cl.section_text} ${cl.section_general_info_text}`}>Загальна інформація</span>
                        </div>
                        <div className={`${cl.section} ${cl.section_address}`}
                            onClick={() => ScrollToSection("address-content")}>
                            <img className={`${cl.section_img} ${cl.section_address_img}`} alt='address' />
                            <span className={`${cl.section_text} ${cl.section_address_text}`}>Адреса</span>
                        </div>
                        <div className={`${cl.section} ${cl.section_delivery}`}
                            onClick={() => ScrollToSection("delivery-content")}>
                            <img className={`${cl.section_img} ${cl.section_delivery_img}`} alt='delivery' />
                            <span className={`${cl.section_text} ${cl.section_delivery_text}`}>Доставка</span>
                        </div>
                        <div className={`${cl.section} ${cl.section_notifications}`}
                            onClick={() => ScrollToSection("notifications-content")}>
                            <img className={`${cl.section_img} ${cl.section_notifications_img}`} alt='notifications' />
                            <span className={`${cl.section_text} ${cl.section_notifications_text}`}>Повідомлення</span>
                        </div>
                        <div className={`${cl.section} ${cl.section_security}`}
                            onClick={() => ScrollToSection("security-content")}>
                            <img className={`${cl.section_img} ${cl.section_security_img}`} alt='security' />
                            <span className={`${cl.section_text} ${cl.section_security_text}`}>Безпека</span>
                        </div>
                    </div>
                    <div className={cl.other_info_editor}>
                        <div className={`${cl.section_content} ${cl.section_general_info_content}`} id="general-info-content">
                            <h2 className={`${cl.section_header} ${cl.section_general_info_header}`}>Загальна інформація</h2>
                            <div className={`${cl.info_field} ${cl.info_field_last_name}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_last_name}`}>Прізвище</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_last_name}`} type='text' defaultValue={userInfo?.lastName} />
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_first_name}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_first_name}`}>Ім’я</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_first_name}`} type='text' defaultValue={userInfo?.firstName} />
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_middle_name}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_middle_name}`}>По-батькові</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_middle_name}`} type='text' defaultValue={userInfo?.midName} />
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_login}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_login}`}>Нікнейм</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_login}`} type='text' defaultValue={userInfo?.login} />
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_phone_number}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_phone_number}`}>Номер телефону</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_phone_number}`} type='text' defaultValue={userInfo?.phoneNumber} />
                            </div>
                        </div>
                        <div className={`${cl.section_content} ${cl.section_address_content}`} id="address-content">
                            <h2 className={`${cl.section_header} ${cl.section_address_header}`}>Адреса</h2>
                            <div className={`${cl.info_field} ${cl.info_field_street}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_street}`}>Вулиця</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_street}`} type='text' defaultValue={userInfo?.street} />
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_region}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_region}`}>Область</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_region}`} type='text' defaultValue={userInfo?.region} />
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_city}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_city}`}>Місто</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_city}`} type='text' defaultValue={userInfo?.city} />
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_city_region}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_city_region}`}>Район</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_city_region}`} type='text' defaultValue={userInfo?.city_region} />
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_house_number}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_house_number}`}>Номер будинку</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_house_number}`} type='number' defaultValue={userInfo?.house} />
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_apartment}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_apartment}`}>Квартира</h3>
                                <input className={`${cl.info_field_input} ${cl.info_field_input_apartment}`} type='number' defaultValue={userInfo?.apartment} />
                            </div>
                        </div>
                        <div className={`${cl.section_content} ${cl.section_delivery_content}`} id="delivery-content">
                            <h2 className={`${cl.section_header} ${cl.section_delivery_header}`}>Доставка</h2>
                            <h3 className={`${cl.info_field_header} ${cl.info_selector_header_city}`}>Ваше місто</h3>
                            <div className={`${cl.info_selector} ${cl.info_selector_city}`} ref={citySelectorRef}>
                                <span className={cl.info_selector_city_value}>Харків</span>
                                <img className={cl.info_selector_city_arrow_down} alt="arrow down" />
                                <div className={`${cl.info_selector_list} ${cl.info_selector_list_city}`} ref={citySelectorListRef}>
                                    <input className={`${cl.info_selector_list_search} ${cl.info_selector_list_search_city}`} type="text" placeholder="Введіть назву міста" />
                                    <p className={`${cl.info_selector_list_option}`}>
                                        Дніпро,
                                        <span className={`${cl.info_selector_list_option_extra}`}>Дніпровський р-н, Дніпропетровська область</span>
                                    </p>
                                    <p className={`${cl.info_selector_list_option}`}>
                                        Зпоріжжя,
                                        <span className={`${cl.info_selector_list_option_extra}`}>Запоріщький р-н, Запорізька область</span>
                                    </p>
                                    <p className={`${cl.info_selector_list_option}`}>
                                        Київ,
                                        <span className={`${cl.info_selector_list_option_extra}`}>Київська область</span>
                                    </p>
                                    <p className={`${cl.info_selector_list_option}`}>
                                        Кривий Ріг,
                                        <span className={`${cl.info_selector_list_option_extra}`}>Криворізький р-н, Дніпропетровська область</span>
                                    </p>
                                </div>
                            </div>
                            <h3 className={`${cl.info_field_header} ${cl.info_selector_header_delivery}`}>Метод Доставки</h3>
                            <div className={cl.info_container_delivery}>
                                <div className={cl.post_logo}>
                                    <img className={cl.post_logo_img} alt="nova poshta" />
                                    <span className={cl.post_logo_text}>Нова Пошта</span>
                                    <span className={cl.post_price}>Від 50 ₴</span>
                                </div>
                                <div className={cl.delivery_type_list}>
                                    <div className={`${cl.delivery_type} ${cl.delivery_type_branch}`}
                                        onClick={() => setDeliveryType('branch')}>
                                        <div className={`${cl.delivery_type_checkbox} ${cl.delivery_type_checkbox_branch} 
                                        ${currentDeliveryType === 'branch' ? cl.delivery_type_checkbox_activated : ''}`}>
                                            <div className={`${cl.delivery_type_check_dot} ${cl.delivery_type_check_dot_branch}`} />
                                        </div>
                                        <span className={`${cl.delivery_type_text} ${cl.delivery_type_text_branch}`}>У відділення</span>
                                    </div>
                                    <div className={`${cl.delivery_type} ${cl.delivery_type_courier}`}
                                        onClick={() => setDeliveryType('courier')}>
                                        <div className={`${cl.delivery_type_checkbox} ${cl.delivery_type_checkbox_courier} 
                                        ${currentDeliveryType === 'courier' ? cl.delivery_type_checkbox_activated : ''}`}>
                                            <div className={`${cl.delivery_type_check_dot} ${cl.delivery_type_check_dot_courier}`} />
                                        </div>
                                        <span className={`${cl.delivery_type_text} ${cl.delivery_type_text_courier}`}>Кур’єром</span>
                                    </div>
                                    <div className={`${cl.delivery_type} ${cl.delivery_type_post_office}`}
                                        onClick={() => setDeliveryType('post-office')}>
                                        <div className={`${cl.delivery_type_checkbox} ${cl.delivery_type_checkbox_post_office} 
                                        ${currentDeliveryType === 'post-office' ? cl.delivery_type_checkbox_activated : ''}`}>
                                            <div className={`${cl.delivery_type_check_dot} ${cl.delivery_type_check_dot_post_office}`} />
                                        </div>
                                        <span className={`${cl.delivery_type_text} ${cl.delivery_type_text_post_office}`}>У поштомат</span>
                                    </div>
                                </div>
                                <h3 className={`${cl.info_field_header} ${cl.info_filed_delivery_header}`}>Номер відділення</h3>
                                <div className={`${cl.info_selector} ${cl.info_selector_department}`} ref={departmentSelectorRef}>
                                    <span className={cl.info_selector_department_value}>Відд 1, Харків, вул. Польова, 67</span>
                                    <img className={cl.info_selector_department_arrow_down} alt="arrow down" />
                                    <div className={`${cl.info_selector_list} ${cl.info_selector_list_department}`} ref={departmentSelectorListRef}>
                                        <input className={`${cl.info_selector_list_search} ${cl.info_selector_list_search_department}`} type="text" placeholder="Введіть номер або адресу" />
                                        <p className={`${cl.info_selector_list_option}`}>
                                            Відд 1,
                                            <span className={`${cl.info_selector_list_option_extra}`}>Харків, вул. Польова, 67</span>
                                        </p>
                                        <p className={`${cl.info_selector_list_option}`}>
                                            Відд 2,
                                            <span className={`${cl.info_selector_list_option_extra}`}>Харків, просп.Героїв Харкова, 54а</span>
                                        </p>
                                        <p className={`${cl.info_selector_list_option}`}>
                                            Відд 3,
                                            <span className={`${cl.info_selector_list_option_extra}`}>Харків, вул. Тюрінська, 124</span>
                                        </p>
                                        <p className={`${cl.info_selector_list_option}`}>
                                            Відд 4,
                                            <span className={`${cl.info_selector_list_option_extra}`}>Харків, вул. Достоєвського, 5</span>
                                        </p>
                                        <p className={`${cl.info_selector_list_option}`}>
                                            Відд 5,
                                            <span className={`${cl.info_selector_list_option_extra}`}>Харків, пл. Ю. Кононенка, 1а</span>
                                        </p>
                                        <p className={`${cl.info_selector_list_option}`}>
                                            Відд 7,
                                            <span className={`${cl.info_selector_list_option_extra}`}>Харків, просп. Свободи Людвіга, 35</span>
                                        </p>
                                        <p className={`${cl.info_selector_list_option}`}>
                                            Відд 8,
                                            <span className={`${cl.info_selector_list_option_extra}`}>Харків, вул. М. Гончарівська, 28/30</span>
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className={`${cl.section_content} ${cl.section_notifications_content}`} id="notifications-content">
                            <h2 className={`${cl.section_header} ${cl.section_notifications_header}`}>Повідомлення</h2>
                            <div className={`${cl.notification} ${cl.notification_change_order_state}`}>
                                <div
                                    className={`${hasOrderStateChangedNote ? cl.notification_checkbox_checked : cl.notification_checkbox_unchecked} 
                                ${cl.notification_checkbox_change_order_state}`}
                                    onClick={() => setOrderStateChangedNote(p => !p)}>
                                    <div className={`${cl.notification_checkbox_no} ${cl.notification_checkbox_no_change_order_state}`}>
                                        <img className={`${cl.notification_checkbox_no_img} ${cl.notification_checkbox_no_img_change_order_state}`} alt="no" />
                                    </div>
                                    <div className={`${cl.notification_checkbox_yes} ${cl.notification_checkbox_yes_change_order_state}`}>
                                        <img className={`${cl.notification_checkbox_yes_img} ${cl.notification_checkbox_yes_img_change_order_state}`} alt="yes" />
                                    </div>
                                </div>
                                <span className={`${cl.notification_text} ${cl.notification_text_change_order_state}`}>
                                    Зміна стану Вашого замовлення
                                </span>
                            </div>
                            <div className={`${cl.notification} ${cl.notification_change_forum_response}`}>
                                <div
                                    className={`${hasGotForumResponseNote ? cl.notification_checkbox_checked : cl.notification_checkbox_unchecked} 
                                ${cl.notification_checkbox_change_forum_response}`}
                                    onClick={() => setGotForumResponseNote(p => !p)}>
                                    <div className={`${cl.notification_checkbox_no} ${cl.notification_checkbox_no_change_forum_response}`}>
                                        <img className={`${cl.notification_checkbox_no_img} ${cl.notification_checkbox_no_img_change_forum_response}`} alt="no" />
                                    </div>
                                    <div className={`${cl.notification_checkbox_yes} ${cl.notification_checkbox_yes_change_forum_response}`}>
                                        <img className={`${cl.notification_checkbox_yes_img} ${cl.notification_checkbox_yes_img_change_forum_response}`} alt="yes" />
                                    </div>
                                </div>
                                <span className={`${cl.notification_text} ${cl.notification_text_change_forum_response}`}>
                                    Відповідь на Вашу тему у форумі
                                </span>
                            </div>
                            <div className={`${cl.notification} ${cl.notification_change_model_rated}`}>
                                <div
                                    className={`${hasModelRatedNote ? cl.notification_checkbox_checked : cl.notification_checkbox_unchecked} 
                                ${cl.notification_checkbox_change_model_rated}`}
                                    onClick={() => setModelRatedNote(p => !p)}>
                                    <div className={`${cl.notification_checkbox_no} ${cl.notification_checkbox_no_change_model_rated}`}>
                                        <img className={`${cl.notification_checkbox_no_img} ${cl.notification_checkbox_no_img_change_model_rated}`} alt="no" />
                                    </div>
                                    <div className={`${cl.notification_checkbox_yes} ${cl.notification_checkbox_yes_change_model_rated}`}>
                                        <img className={`${cl.notification_checkbox_yes_img} ${cl.notification_checkbox_yes_img_change_model_rated}`} alt="yes" />
                                    </div>
                                </div>
                                <span className={`${cl.notification_text} ${cl.notification_text_change_model_rated}`}>
                                    Новий відгук до Вашої моделі
                                </span>
                            </div>
                        </div>
                        <div className={`${cl.section_content} ${cl.section_security_content}`} id="security-content">
                            <h2 className={`${cl.section_header} ${cl.section_security_header}`}>Безпека</h2>
                            <div className={`${cl.security_button} ${cl.change_password_button}`}>
                                <span className={`${cl.security_button_text} ${cl.change_password_button_text}`}>Змінити пароль</span>
                            </div>
                            <div className={`${cl.security_button} ${cl.change_email_button}`}>
                                <span className={`${cl.security_button_text} ${cl.change_email_button_text}`}>Змінити пошту</span>
                            </div>
                            <div className={`${cl.security_button} ${cl.delete_account_button}`}>
                                <span className={`${cl.security_button_text} ${cl.delete_account_button_text}`}>Видалити акаунт</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            {RenderModalWindow()}
        </div>
    );
}

export default UserEditPage;