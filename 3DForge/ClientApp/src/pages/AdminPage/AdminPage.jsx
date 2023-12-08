import React from "react";
import cl from './.module.css';
import { Outlet, useNavigate } from "react-router-dom";
import { UserAPI } from "../../services/api/UserAPI";
import LoadingAnimation from "../../components/LoadingAnimation/LoadingAnimation";

const AdminPage = () => {
    const navigate = useNavigate();

    const [userRights, setUserRights] = React.useState(null);

    React.useEffect(() => {
        if (userRights === null) {
            UserAPI.check()
                .then(res => res.json())
                .then(json => { setUserRights(json.data); });
        }
    });

    if (userRights === null) {
        return (
            <div className={cl.page_loading}>
                <LoadingAnimation size="100px" loadingCurveWidth="20px" />
            </div>
        )
    }

    return (
        <div className={cl.main}>
            <div className={cl.content}>
                <div className={cl.navigation}>
                    <div className={`${cl.section_list} ${cl.section_list_system_tables}`}>
                        <div className={`${cl.section_list_header} ${cl.section_list_header_system_tables}`}>
                            <h2 className={`${cl.section_list_header_name} ${cl.section_list_header_name_system_tables}`}>
                                Системні таблиці
                            </h2>
                        </div>
                        <div className={`${cl.section} ${cl.section_something}`}>
                            <span className={`${cl.section_name} ${cl.section_name_something}`}>
                                Щось тут
                            </span>
                        </div>
                    </div>
                    <div className={`${cl.section_list} ${cl.section_list_accounts}`}>
                        <div className={`${cl.section_list_header} ${cl.section_list_header_accounts}`}>
                            <h2 className={`${cl.section_list_header_name} ${cl.section_list_header_name_accounts}`}>
                                Облікові записи
                            </h2>
                        </div>
                        <div className={`${cl.section} ${cl.section_general_enum}`}>
                            <span className={`${cl.section_name} ${cl.section_name_general_enum}`}>
                                Загальний перелік
                            </span>
                        </div>
                    </div>

                    <div className={`${cl.section_list} ${cl.section_list_moderation}`}>
                        <div className={`${cl.section_list_header} ${cl.section_list_header_moderation}`}>
                            <h2 className={`${cl.section_list_header_name} ${cl.section_list_header_name_moderation}`}>
                                Модерація
                            </h2>
                        </div>
                        <div className={`
                        ${cl.section} 
                        ${cl.section_catalog} 
                        ${window.location.pathname.includes('/admin/catalog') ? cl.selected_section : ''}
                        ${!userRights.canModerateCatalog ? cl.disabled_section : ''}`}
                            style={{ pointerEvents: !userRights.canModerateCatalog ? 'none' : 'all' }}
                            onClick={() => {
                                if (userRights.canModerateCatalog) {
                                    navigate('catalog');
                                }
                            }}>
                            <span className={`
                            ${cl.section_name} ${cl.section_name_catalog}`}>
                                Каталог
                            </span>
                        </div>
                        <div className={`${cl.section} ${cl.section_forum}`}>
                            <span className={`${cl.section_name} ${cl.section_name_forum}`}>
                                Форум
                            </span>
                        </div>
                    </div>
                </div>
                <div className={cl.separating_line} />
                <div className={cl.section_content}>
                    <Outlet />
                </div>
            </div>
        </div>
    );
}

export default AdminPage;