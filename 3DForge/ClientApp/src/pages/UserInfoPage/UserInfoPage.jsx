import React from "react";
import cl from "./.module.css";
import { UserAPI } from "../../services/api/UserAPI";

const UserInfoPage = () => {

    const [userInfo, setUserInfo] = React.useState(undefined);
    const [isInfoLoading, setInfoLoading] = React.useState(false);
    const [isAvatarLoading, setAvatarLoading] = React.useState(false);

    async function UpdateUserInfo() {
        setInfoLoading(true);
        setAvatarLoading(true);

        await UserAPI.getSelfInfo()
            .then(res => { return res.json() })
            .then(data => {
                setUserInfo(data.data);
            });

        setInfoLoading(false);
        setAvatarLoading(false);
    }

    React.useEffect(() => {
        document.body.style.background = "linear-gradient(270deg, #7C26BF 0.13%, #BA67D3 99.84%, #7030B5 99.85%)";
        UpdateUserInfo();
    });

    return (
        <div className={cl.main}>
            <div className={cl.content}>
                <div className={cl.base_info}>
                    <img className={cl.avatar} alt="user avatar" />
                    <div className={cl.login_email_country_city}>
                        <span className={cl.login}>ergrsig</span>
                        <span className={cl.email}>1111@gmail.com</span>
                        <span className={cl.country_and_city}>Country, City</span>
                    </div>
                    <div className={cl.log_out_button}>
                        <span className={cl.log_out_button_text}>Вийти</span>
                    </div>
                </div>
                <div />
                <div className={cl.advanced_info}>
                    <h2 className={cl.general_info_header}>Загальна інформація</h2>
                    <div className={cl.general_info}>
                        <div className={`${cl.info_field} ${cl.info_field_last_name}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_last_name}`}>Прізвище</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_last_name}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_last_name}`}>Прізвище</span>
                            </div>
                        </div>
                        <div className={`${cl.info_field} ${cl.info_field_first_name}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_first_name}`}>Ім’я</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_first_name}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_first_name}`}>Ім’я</span>
                            </div>
                        </div>
                        <div className={`${cl.info_field} ${cl.info_field_middle_name}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_middle_name}`}>По-батькові</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_middle_name}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_middle_name}`}>По-батькові</span>
                            </div>
                        </div>
                        <div className={`${cl.info_field} ${cl.info_field_email}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_email}`}>Пошта</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_email}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_email}`}>Пошта</span>
                            </div>
                        </div>
                        <div className={`${cl.info_field} ${cl.info_field_phone_number}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_phone_number}`}>Номер телефону</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_phone_number}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_phone_number}`}>Номер телефону</span>
                            </div>
                        </div>
                    </div>
                    <h2 className={cl.address_header}>Адреса</h2>
                    <div className={cl.address}>
                        <div className={`${cl.info_field} ${cl.info_field_street}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_street}`}>Вулиця</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_street}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_street}`}>Вулиця</span>
                            </div>
                        </div>
                        <div className={`${cl.info_field} ${cl.info_field_house_number}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_house_number}`}>Номер будинку</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_house_number}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_house_number}`}>Номер будинку</span>
                            </div>
                        </div>
                        <div className={`${cl.info_field} ${cl.info_field_country}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_country}`}>Країна</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_country}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_country}`}>Країна</span>
                            </div>
                        </div>
                        <div className={`${cl.info_field} ${cl.info_field_city}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_city}`}>Місто</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_city}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_city}`}>Місто</span>
                            </div>
                        </div>
                        <div className={`${cl.info_field} ${cl.info_field_zip_code}`}>
                            <h3 className={`${cl.info_field_header} ${cl.info_field_header_zip_code}`}>Поштовий індекс</h3>
                            <div className={`${cl.info_field_cell} ${cl.info_field_cell_zip_code}`}>
                                <span className={`${cl.info_field_value} ${cl.info_field_value_zip_code}`}>Поштовий індекс</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default UserInfoPage;