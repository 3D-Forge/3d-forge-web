import React from "react";
import cl from './.module.css';
import { UserAPI } from "../../services/api/UserAPI";
import LoadingAnimation from "../../components/LoadingAnimation/LoadingAnimation";

const UserEditPage = () => {
    const [isSavingChanges, setSavingChangesState] = React.useState(false);
    const [userInfo, setUserInfo] = React.useState(undefined);
    const [userAvatar, setUserAvatar] = React.useState({ url: undefined, file: undefined });
    const [currentDeliveryType, setDeliveryType] = React.useState(undefined);
    const [currentModalWindowType, setModalWindowType] = React.useState(undefined);
    const [hasOrderStateChangedNote, setOrderStateChangedNote] = React.useState(true);
    const [hasGetForumResponseNote, setGetForumResponseNote] = React.useState(true);
    const [hasModelRatedNote, setModelRatedNote] = React.useState(true);

    const avatarInputRef = React.useRef();

    const lastNameInputRef = React.useRef();
    const firstNameInputRef = React.useRef();
    const midNameInputRef = React.useRef();
    const loginInputRef = React.useRef();
    const phoneNumberInputRef = React.useRef();
    const streetInputRef = React.useRef();
    const regionInputRef = React.useRef();
    const cityInputRef = React.useRef();
    const houseInputRef = React.useRef();
    const apartmentInputRef = React.useRef();

    const citySelectorRef = React.useRef();
    const citySelectorListRef = React.useRef();
    const departmentSelectorRef = React.useRef();
    const departmentSelectorListRef = React.useRef();
    const streetSelectorRef = React.useRef();
    const streetSelectorListRef = React.useRef();

    const modalWindowCurrentPasswordInputRef = React.useRef();
    const modalWindowNewPasswordInputRef = React.useRef();
    const modalWindowConfirmNewPasswordInputRef = React.useRef();
    const modalWindowNewEmailInputRef = React.useRef();

    function ScrollToSection(sectionId) {
        let element = document.getElementById(sectionId);

        element.scrollIntoView({ behavior: 'smooth' });
        element.style.border = '2px solid #C99E22';

        setTimeout(() => {
            element.style.border = '2px solid rgba(0, 0, 0, 0)';
        }, 1000);
    }

    function LoadImageFromAvatarInput(event) {
        let file = event.target.files[0];

        if (!file) {
            return;
        }

        if (file.type !== 'image/png') {
            alert('Please select a PNG image file.');
            return;
        }

        const imgReader = new FileReader();

        imgReader.onload = (event) => {
            setUserAvatar(p => {
                let newP = { ...p };
                newP.url = event.target.result;
                newP.file = file;
                return newP;
            });
        };
        imgReader.readAsDataURL(file);
    }

    async function SaveChanges() {
        setSavingChangesState(true);

        let formData = new FormData();
        formData.append("userAvatarFile", userAvatar.file);

        let saveInfoResponse = await UserAPI.updateUserInfo({
            login: loginInputRef.current.value,
            phoneNumber: phoneNumberInputRef.current.value,
            firstname: firstNameInputRef.current.value,
            midname: midNameInputRef.current.value,
            lastname: lastNameInputRef.current.value,
            region: regionInputRef.current.value,
            city: cityInputRef.current.value,
            street: streetInputRef.current.value,
            house: houseInputRef.current.value,
            apartment: apartmentInputRef.current.value,
            departmentNumber: userInfo.departmentNumber,
            deliveryType: currentDeliveryType,
            orderStateChangedNotification: hasOrderStateChangedNote,
            getForumResponseNotification: hasGetForumResponseNote,
            modelRatedNotification: hasModelRatedNote
        });

        let saveAvatarResponse = userAvatar.file ? await UserAPI.updateUserAvatar(formData) : { status: 200 };

        if (saveInfoResponse.status === 200 && saveAvatarResponse.status === 200) {
            window.location.reload();
        }
        else if (saveInfoResponse.status !== 200 && saveAvatarResponse.status !== 200) {
            alert("Info and avatar couldn't save!");
            setSavingChangesState(false);
        }
        else if (saveInfoResponse.status !== 200) {
            alert("Info couldn't save!");
            setSavingChangesState(false);
        }
        else if (saveAvatarResponse.status !== 200) {
            alert("Avatar couldn't save!");
            setSavingChangesState(false);
        }
    }

    function ChangePasswordRequest() {
        UserAPI.resetPassword(
            userInfo.login,
            modalWindowNewPasswordInputRef.current.value,
            modalWindowConfirmNewPasswordInputRef.current.value,
            modalWindowCurrentPasswordInputRef.current.value,
            null
            ).then(res => {
                return res.json()
            }).then(data => {
                if (data.success === true) {
                    setModalWindowType(undefined);
                    alert("The password is changed.");
                }
                else if (data.success === false) {
                    alert(data.message);
                }
                else {
                    alert("Some fields are empty");
                }
            });
    }

    function ChangeEmailRequest() {
        UserAPI.changeEmail(
            modalWindowCurrentPasswordInputRef.current.value,
            modalWindowNewEmailInputRef.current.value
            ).then(res => {
                return res.json()
            }).then(data => {
                if (data.success === true) {
                    setModalWindowType(undefined);
                    alert("Email is sent.");
                }
                else if (data.success === false) {
                    alert(data.message);
                }
                else {
                    alert("Some fields are empty");
                }
            });
    }

    function RenderDeliveryMenu() {
        if (currentDeliveryType === "branch") {
            return (
                <>
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
                </>
            );
        }

        if (currentDeliveryType === "courier") {
            return (
                <>
                    <h3 className={`${cl.info_field_header} ${cl.info_filed_delivery_header}`}>Вулиця</h3>
                    <div className={`${cl.info_selector} ${cl.info_selector_street}`} ref={streetSelectorRef}>
                        <span className={cl.info_selector_street_value}>Медичка</span>
                        <img className={cl.info_selector_street_arrow_down} alt="arrow down" />
                        <div className={`${cl.info_selector_list} ${cl.info_selector_list_street}`} ref={streetSelectorListRef}>
                            <input className={`${cl.info_selector_list_search} ${cl.info_selector_list_search_street}`} type="text" placeholder="Введіть назву вулиці" />
                            <p className={`${cl.info_selector_list_option}`}>
                                Польова
                            </p>
                            <p className={`${cl.info_selector_list_option}`}>
                                Героїв Харкова
                            </p>
                            <p className={`${cl.info_selector_list_option}`}>
                                Тюрінська
                            </p>
                            <p className={`${cl.info_selector_list_option}`}>
                                Достоєвського
                            </p>
                            <p className={`${cl.info_selector_list_option}`}>
                                Кононенка
                            </p>
                            <p className={`${cl.info_selector_list_option}`}>
                                Свободи Людвіга
                            </p>
                            <p className={`${cl.info_selector_list_option}`}>
                                Гончарівська
                            </p>
                        </div>
                    </div>
                    <div className={`${cl.info_field} ${cl.info_field_delivery_house}`}>
                        <h3 className={`${cl.info_field_header} ${cl.info_field_header_delivery_house}`}>Будинок</h3>
                        <input className={`${cl.info_field_input} ${cl.info_field_input_delivery_house}`}
                            type='number' />
                    </div>
                    <div className={`${cl.info_field} ${cl.info_field_delivery_apartment}`}>
                        <h3 className={`${cl.info_field_header} ${cl.info_field_header_delivery_apartment}`}>Квартира</h3>
                        <input className={`${cl.info_field_input} ${cl.info_field_input_delivery_apartment}`}
                            type='number' />
                    </div>
                </>
            );
        }

        if (currentDeliveryType === "post-office") {
            return (
                <>
                    <h3 className={`${cl.info_field_header} ${cl.info_filed_delivery_header}`}>Поштомат</h3>
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
                </>
            );
        }
    }

    function RenderModalWindow() {
        if (currentModalWindowType === "change-password") {
            return (
                <div className={cl.modal_window_background}>
                    <div className={cl.modal_window}>
                        <div className={cl.modal_window_content}>
                            <h2 className={cl.modal_window_header}>Зміна паролю</h2>
                            <p className={cl.modal_window_description}>Уведіть Ваш поточний та новий пароль</p>
                            <div className={`${cl.modal_window_panel} ${cl.modal_window_current_password_panel}`}>
                                <h3 className={`${cl.modal_window_panel_header} ${cl.modal_window_current_password_panel_header}`}>
                                    Поточний пароль
                                </h3>
                                <input className={`${cl.modal_window_input} ${cl.modal_window_current_password_input}`}
                                    type="password"
                                    ref={modalWindowCurrentPasswordInputRef} />
                            </div>
                            <div className={`${cl.modal_window_panel} ${cl.modal_window_new_password_panel}`}>
                                <h3 className={`${cl.modal_window_panel_header} ${cl.modal_window_new_password_panel_header}`}>
                                    Новий пароль
                                </h3>
                                <input className={`${cl.modal_window_input} ${cl.modal_window_new_password_input}`}
                                    type="password"
                                    ref={modalWindowNewPasswordInputRef} />
                            </div>
                            <div className={`${cl.modal_window_panel} ${cl.modal_window_confirm_new_password_panel}`}>
                                <h3 className={`${cl.modal_window_panel_header} ${cl.modal_window_confirm_new_password_panel_header}`}>
                                    Підтвердження нового паролю
                                </h3>
                                <input className={`${cl.modal_window_input} ${cl.modal_window_confirm_new_password_input}`}
                                    type="password"
                                    ref={modalWindowConfirmNewPasswordInputRef} />
                            </div>
                        </div>
                        <div className={cl.modal_window_control}>
                            <div className={cl.modal_window_change_password_button} onClick={() => ChangePasswordRequest()}>
                                <span className={cl.modal_window_change_password_button_text}>Зберегти</span>
                            </div>
                            <div className={cl.modal_window_cancel_button} onClick={() => setModalWindowType(undefined)}>
                                <span className={cl.modal_window_cancel_button_text}>Скасувати</span>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }

        if (currentModalWindowType === "change-email") {
            return (
                <div className={cl.modal_window_background}>
                    <div className={cl.modal_window}>
                        <div className={cl.modal_window_content}>
                            <h2 className={cl.modal_window_header}>Зміна пошти</h2>
                            <p className={cl.modal_window_description}>Уведіть Ваш поточний пароль та нову пошту</p>
                            <div className={`${cl.modal_window_panel} ${cl.modal_window_current_password_panel}`}>
                                <h3 className={`${cl.modal_window_panel_header} ${cl.modal_window_current_password_panel_header}`}>
                                    Поточний пароль
                                </h3>
                                <input className={`${cl.modal_window_input} ${cl.modal_window_current_password_input}`}
                                    type="password"
                                    ref={modalWindowCurrentPasswordInputRef} />
                            </div>
                            <div className={`${cl.modal_window_panel} ${cl.modal_window_new_email_panel}`}>
                                <h3 className={`${cl.modal_window_panel_header} ${cl.modal_window_new_email_header}`}>
                                    Нова пошта
                                </h3>
                                <input className={`${cl.modal_window_input} ${cl.modal_window_new_email_input}`}
                                    type="email"
                                    ref={modalWindowNewEmailInputRef} />
                            </div>
                        </div>
                        <div className={cl.modal_window_control}>
                            <div className={cl.modal_window_change_password_button} onClick={() => ChangeEmailRequest()}>
                                <span className={cl.modal_window_change_password_button_text}>Зберегти</span>
                            </div>
                            <div className={cl.modal_window_cancel_button} onClick={() => setModalWindowType(undefined)}>
                                <span className={cl.modal_window_cancel_button_text}>Скасувати</span>
                            </div>
                        </div>
                    </div>
                </div>
            );
        }

        if (currentModalWindowType === "delete-account") {
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

        if (currentDeliveryType === "branch" || currentDeliveryType === "post-office") {
            if (departmentSelectorListRef.current.style.display === 'block' && !departmentSelectorRef.current.contains(event.target)) {
                departmentSelectorListRef.current.style.display = 'none';
            }

            if (departmentSelectorRef.current.contains(event.target) && !departmentSelectorListRef.current.contains(event.target)) {
                departmentSelectorListRef.current.style.display =
                    departmentSelectorListRef.current.style.display === 'block' ? 'none' : 'block';
            }
        }

        if (currentDeliveryType === "courier") {
            if (streetSelectorListRef.current.style.display === 'block' && !streetSelectorRef.current.contains(event.target)) {
                streetSelectorListRef.current.style.display = 'none';
            }

            if (streetSelectorRef.current.contains(event.target) && !streetSelectorListRef.current.contains(event.target)) {
                streetSelectorListRef.current.style.display =
                    streetSelectorListRef.current.style.display === 'block' ? 'none' : 'block';
            }
        }
    }

    React.useEffect(() => {
        if (userInfo === undefined) {
            UserAPI.getSelfInfo()
                .then(res => { return res.json() })
                .then(data => {
                    if (data.success) {
                        setUserInfo(data.data);
                        setDeliveryType(data.data.deliveryType !== null ? data.data.deliveryType : undefined);
                        setOrderStateChangedNote(data.data.orderStateChangedNotification);
                        setGetForumResponseNote(data.data.getForumResponseNotification);
                        setModelRatedNote(data.data.modelRatedNotification);
                    }
                });
        }

        if (userAvatar.url === undefined) {
            UserAPI.getSelfAvatar()
                .then(res => { return res.blob(); })
                .then(blob => {
                    setUserAvatar(p => {
                        let newP = { ...p };
                        newP.url = URL.createObjectURL(blob);
                        return newP;
                    });
                });
        }

        window.addEventListener("click", WindowClickEvent);

        return () => {
            window.removeEventListener("click", WindowClickEvent);
        };
    });

    return (
        <div className={cl.main}>
            {userInfo !== undefined ?
                <div className={cl.content}>
                    <div className={cl.top_panel}>
                        <div className={cl.avatar_cont}>
                            {userAvatar.url !== undefined ?
                                <img className={cl.avatar} alt="avatar" src={userAvatar.url} /> :
                                <div className={cl.avatar_unloaded}>
                                    <LoadingAnimation size="100px" loadingCurveWidth="20px" />
                                </div>
                            }
                        </div>
                        <div className={cl.avatar_editor}>
                            <div className={cl.avatar_editor_cont}>
                                <h2 className={cl.profile_text}>Профіль</h2>
                                <p className={cl.login}>{userInfo?.login}</p>
                                <p className={cl.description}>Оновіть своє фото профілю та особисту інформацію</p>
                                <div className={cl.change_avatar_button} onClick={() => avatarInputRef.current.click()}>
                                    <input
                                        type='file'
                                        accept=".png"
                                        style={{ display: 'none' }}
                                        onChange={LoadImageFromAvatarInput}
                                        ref={avatarInputRef} />
                                    <span className={cl.change_avatar_button_text}>Обрати Фото</span>
                                </div>
                            </div>
                        </div>
                        <div className={cl.info_control}>
                            <div className={cl.info_control_cont}>
                                <div className={cl.save_changes_button} onClick={() => SaveChanges()}>
                                    {
                                        isSavingChanges
                                            ? <LoadingAnimation size="30px" loadingCurveWidth="6px" />
                                            : <span className={cl.save_changes_button_text}>Зберегти</span>
                                    }
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
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_last_name}`}
                                        type='text'
                                        defaultValue={userInfo?.lastName}
                                        ref={lastNameInputRef} />
                                </div>
                                <div className={`${cl.info_field} ${cl.info_field_first_name}`}>
                                    <h3 className={`${cl.info_field_header} ${cl.info_field_header_first_name}`}>Ім’я</h3>
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_first_name}`}
                                        type='text'
                                        defaultValue={userInfo?.firstName}
                                        ref={firstNameInputRef} />
                                </div>
                                <div className={`${cl.info_field} ${cl.info_field_middle_name}`}>
                                    <h3 className={`${cl.info_field_header} ${cl.info_field_header_middle_name}`}>По-батькові</h3>
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_middle_name}`}
                                        type='text'
                                        defaultValue={userInfo?.midName}
                                        ref={midNameInputRef} />
                                </div>
                                <div className={`${cl.info_field} ${cl.info_field_login}`}>
                                    <h3 className={`${cl.info_field_header} ${cl.info_field_header_login}`}>Нікнейм</h3>
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_login}`}
                                        type='text'
                                        defaultValue={userInfo?.login}
                                        ref={loginInputRef} />
                                </div>
                                <div className={`${cl.info_field} ${cl.info_field_phone_number}`}>
                                    <h3 className={`${cl.info_field_header} ${cl.info_field_header_phone_number}`}>Номер телефону</h3>
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_phone_number}`}
                                        type='text'
                                        defaultValue={userInfo?.phoneNumber}
                                        ref={phoneNumberInputRef} />
                                </div>
                            </div>
                            <div className={`${cl.section_content} ${cl.section_address_content}`} id="address-content">
                                <h2 className={`${cl.section_header} ${cl.section_address_header}`}>Адреса</h2>
                                <div className={`${cl.info_field} ${cl.info_field_street}`}>
                                    <h3 className={`${cl.info_field_header} ${cl.info_field_header_street}`}>Вулиця</h3>
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_street}`}
                                        type='text'
                                        defaultValue={userInfo?.street}
                                        ref={streetInputRef} />
                                </div>
                                <div className={`${cl.info_field} ${cl.info_field_region}`}>
                                    <h3 className={`${cl.info_field_header} ${cl.info_field_header_region}`}>Область</h3>
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_region}`}
                                        type='text'
                                        defaultValue={userInfo?.region}
                                        ref={regionInputRef} />
                                </div>
                                <div className={`${cl.info_field} ${cl.info_field_city}`}>
                                    <h3 className={`${cl.info_field_header} ${cl.info_field_header_city}`}>Місто</h3>
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_city}`}
                                        type='text'
                                        defaultValue={userInfo?.city}
                                        ref={cityInputRef} />
                                </div>
                                <div className={`${cl.info_field} ${cl.info_field_house_number}`}>
                                    <h3 className={`${cl.info_field_header} ${cl.info_field_header_house_number}`}>Номер будинку</h3>
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_house_number}`}
                                        type='number'
                                        defaultValue={userInfo?.house}
                                        ref={houseInputRef} />
                                </div>
                                <div className={`${cl.info_field} ${cl.info_field_apartment}`}>
                                    <h3 className={`${cl.info_field_header} ${cl.info_field_header_apartment}`}>Квартира</h3>
                                    <input className={`${cl.info_field_input} ${cl.info_field_input_apartment}`}
                                        type='number'
                                        defaultValue={userInfo?.apartment}
                                        ref={apartmentInputRef} />
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
                                        <p className={cl.post_logo_text}>Нова Пошта</p>
                                        <p className={cl.post_price}>Від 50 ₴</p>
                                    </div>
                                    <div className={cl.delivery_type_list}>
                                        <div className={`${cl.delivery_type} ${cl.delivery_type_branch}`}
                                            onClick={() => { setDeliveryType('branch') }}>
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
                                    {RenderDeliveryMenu()}
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
                                        className={`${hasGetForumResponseNote ? cl.notification_checkbox_checked : cl.notification_checkbox_unchecked} 
                                ${cl.notification_checkbox_change_forum_response}`}
                                        onClick={() => setGetForumResponseNote(p => !p)}>
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
                                <div className={`${cl.security_button} ${cl.change_password_button}`} onClick={() => setModalWindowType("change-password")}>
                                    <span className={`${cl.security_button_text} ${cl.change_password_button_text}`}>Змінити пароль</span>
                                </div>
                                <div className={`${cl.security_button} ${cl.change_email_button}`} onClick={() => setModalWindowType("change-email")}>
                                    <span className={`${cl.security_button_text} ${cl.change_email_button_text}`}>Змінити пошту</span>
                                </div>
                                <div className={`${cl.security_button} ${cl.delete_account_button}`}>
                                    <span className={`${cl.security_button_text} ${cl.delete_account_button_text}`}>Видалити акаунт</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                : <LoadingAnimation size="100px" loadingCurveWidth="20px" />}
            {RenderModalWindow()}
        </div>
    );
}

export default UserEditPage;