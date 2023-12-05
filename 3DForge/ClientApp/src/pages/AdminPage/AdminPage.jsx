import React from "react";
import cl from './.module.css';
import { CatalogAPI } from "../../services/api/CatalogAPI";
const AdminPage = () => {

    const [currentSection, setCurrentSection] = React.useState(undefined);
    const [modelsInfo, setModelsInfo] = React.useState(undefined);
    React.useEffect(() => {
        
        let isMounted = true;
        const fetchData = async () => {
            try {
                const response = await CatalogAPI.GetUnacceptedModels();
                console.log(response.status);
                if (response.ok) {
                    const resModel = await response.json();
                    console.log(resModel);
                    setModelsInfo(resModel.data);
                }
                else {
                    console.error('Помилка отримання моделі:', response.statusText);
                }
            } catch (error) {
                console.error('Помилка отримання моделі:', error);
            }
        };
        if (modelsInfo === undefined) {
            fetchData();
        }

        return () => {
            isMounted = false;
        };
    });
    function RenderCatalogSection() {
        if (Array.isArray(modelsInfo)) {
            return (
                <div className={cl.models_group}>

                    <p className={cl.Owner}> Автор </p>
                    <p className={cl.Name}> Модель </p>
                    <p className={cl.Depth}>Ціна</p>
                    <p className={cl.Uploaded}>Дата завантаження</p>
                    {modelsInfo.map((model) => (
                        <div key={model.id} className={cl.model_item}>
                            {/* Render your content for each model */}
                            <p className={cl.model_Owner}> {model.owner}</p>
                            <p className={cl.model_Name}> {model.name}</p>
                            <p className={cl.model_Depth}> {model.depth}₴</p>
                            <p className={cl.model_Uploaded}> {model.uploaded.replace(/T.*/, "T")}</p>
                        </div>
                    ))}
                </div>
            );
        } else {
            // Handle the case where modelsInfo is not an array
            return <p>modelsInfo is not an array.</p>;
        }
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