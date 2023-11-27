import React from "react";
import cl from './.module.css';

const AdminPage = () => {

    const [currentSection, setCurrentSection] = React.useState(undefined);

    function RenderCatalogSection() {
        return (
            <div className={cl.uploaded_model_panel}>
                <div className={cl.uploaded_model_list_header}>
                    <div className={`${cl.uploaded_model_list_header_column} ${cl.uploaded_model_list_header_column_login}`}>
                        <div className={`${cl.uploaded_model_list_header_column_content} ${cl.uploaded_model_list_header_column_content_login}`}>
                            <img className={`${cl.uploaded_model_list_header_column_sort} ${cl.uploaded_model_list_header_column_sort_login}`} />
                            <span className={`${cl.uploaded_model_list_header_column_text} ${cl.uploaded_model_list_header_column_text_login}`}>
                                Користувач
                            </span>
                        </div>
                    </div>
                </div>
                <div className={cl.uploaded_model_list}>

                </div>
            </div>
        );
    }

    function RenderSections() {
        switch (currentSection) {
            case "catalog":
                return RenderCatalogSection();
            default:
                return;
        }
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
                        <div className={`${cl.section} ${cl.section_catalog}`}>
                            <span className={`${cl.section_name} ${cl.section_name_catalog}`}
                                onClick={() => setCurrentSection("catalog")}>
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
                    {RenderSections()}
                </div>
            </div>
        </div>
    );
}

export default AdminPage;