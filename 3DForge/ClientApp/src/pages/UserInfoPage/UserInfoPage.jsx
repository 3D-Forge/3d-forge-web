import React from "react";
import cl from "./.module.css";
import { UserAPI } from "../../services/api/UserAPI";
import LoadingAnimation from "../../components/LoadingAnimation/LoadingAnimation";

const UserInfoPage = () => {
    const [userAvatar, setUserAvatar] = React.useState(undefined);
    const [userInfo, setUserInfo] = React.useState(undefined);

    function GetCountryAndCity() {
        if (userInfo?.country && userInfo?.city) {
            return `${userInfo.country}, ${userInfo.city}`;
        }
        if (userInfo?.country) {
            return 'Країна: ' + userInfo.country;
        }
        if (userInfo?.city) {
            return 'Місто: ' + userInfo.city;
        }
        return undefined;
    }

    function LogoutRequest() {
        UserAPI.logout().then(() => {
            window.location.pathname = '/';
        });
    }

    React.useEffect(() => {
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
    });

    return (
        <div className={cl.main}>
            {userInfo !== undefined ?
                <div className={cl.content}>
                    <div className={cl.base_info}>
                        {userAvatar !== undefined ?
                            <img className={cl.avatar} alt="user avatar" src={userAvatar} /> :
                            <div className={cl.avatar_unloaded}>
                                <LoadingAnimation size="100px" loadingCurveWidth="20px" />
                            </div>
                        }
                        <div className={cl.login_email_country_city}>
                            <p className={cl.login}>{userInfo?.login ?? '-'}</p>
                            <p className={cl.email}>{userInfo?.email ?? '-'}</p>
                            <p className={cl.country_and_city}>{GetCountryAndCity()}</p>
                        </div>
                        <div className={cl.log_out_button} onClick={() => LogoutRequest()}>
                            <span className={cl.log_out_button_text}>Вийти</span>
                        </div>
                    </div>
                    <div />
                    <div className={cl.advanced_info}>
                        <h2 className={`${cl.section_header} ${cl.general_info_header}`}>Загальна інформація</h2>
                        <div className={`${cl.section} ${cl.general_info}`}>
                            <div className={`${cl.info_field} ${cl.info_field_last_name}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_last_name}`}>Прізвище</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_last_name}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_last_name}`}>{userInfo?.lastName}</span>
                                </div>
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_first_name}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_first_name}`}>Ім’я</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_first_name}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_first_name}`}>{userInfo?.firstName}</span>
                                </div>
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_middle_name}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_middle_name}`}>По-батькові</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_middle_name}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_middle_name}`}>{userInfo?.midName}</span>
                                </div>
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_email}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_email}`}>Пошта</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_email}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_email}`}>{userInfo?.email}</span>
                                </div>
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_phone_number}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_phone_number}`}>Номер телефону</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_phone_number}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_phone_number}`}>{userInfo?.phoneNumber}</span>
                                </div>
                            </div>
                        </div>
                        <h2 className={`${cl.section_header} ${cl.address_header}`}>Адреса</h2>
                        <div className={`${cl.section} ${cl.address}`}>
                            <div className={`${cl.info_field} ${cl.info_field_street}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_street}`}>Вулиця</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_street}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_street}`}>{userInfo?.street}</span>
                                </div>
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_region}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_region}`}>Область</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_region}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_region}`}>{userInfo?.region}</span>
                                </div>
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_city}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_city}`}>Місто</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_city}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_city}`}>{userInfo?.city}</span>
                                </div>
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_house_number}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_house_number}`}>Номер будинку</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_house_number}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_house_number}`}>{userInfo?.house}</span>
                                </div>
                            </div>
                            <div className={`${cl.info_field} ${cl.info_field_apartment}`}>
                                <h3 className={`${cl.info_field_header} ${cl.info_field_header_apartment}`}>Квартира</h3>
                                <div className={`${cl.info_field_cell} ${cl.info_field_cell_apartment}`}>
                                    <span className={`${cl.info_field_value} ${cl.info_field_value_apartment}`}>{userInfo?.apartment}</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                : <LoadingAnimation size="100px" loadingCurveWidth="20px" />
            }
        </div>
    );
}

export default UserInfoPage;