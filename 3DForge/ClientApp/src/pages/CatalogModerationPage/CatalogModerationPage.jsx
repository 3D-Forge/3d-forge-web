import React from "react";
import cl from './.module.css';
import LoadingAnimation from "../../components/LoadingAnimation/LoadingAnimation";
import { CatalogAPI } from "../../services/api/CatalogAPI";
import sortAscImg from './img/sort-by-asc.png';
import sortDescImg from './img/sort-by-desc.png';
import { ReviewModelWindowContext } from "../../ContextProvider";

const CatalogModerationPage = () => {
    const {reviewModelWindowInfo, setReviewModelWindowInfo} = React.useContext(ReviewModelWindowContext);

    const [uploadedModelList, setUploadedModelList] = React.useState(null);
    const [sortMode, setSortMode] = React.useState({ value: 'login', asc: true });

    const [isUploadedModelListLoading, setUploadedModelListLoading] = React.useState(false);

    async function LoadUploadedModelList(sortParam = sortMode.value, sortDir = sortMode.asc) {
        setUploadedModelListLoading(true);
        setUploadedModelList((await (await CatalogAPI.getUnacceptedModels(sortParam, sortDir ? 'asc' : 'desc')).json()).data);
        setUploadedModelListLoading(false);
    }

    function RenderUploadedModelList() {
        let result = [];

        if (isUploadedModelListLoading) {
            return (
                <div className={cl.section_content_loading}>
                    <LoadingAnimation size="50px" loadingCurveWidth="10px" />
                </div>
            );
        }

        uploadedModelList?.forEach((el) => {
            let [date, time] = el.uploaded.split('.')[0].split('T');
            date = date.split('-').reverse().join('.');

            result.push(
                <div className={cl.catalog_section__uploaded_model_table__list_element} key={el.id}>
                    <div className={cl.catalog_section__uploaded_model_table__list_element_column}>
                        <div className={cl.catalog_section__uploaded_model_table__list_element_column_cont}>
                            <p className={cl.catalog_section__uploaded_model_table__list_element_column_login_text}
                                title={el.owner}>
                                {el.owner}
                            </p>
                        </div>
                    </div>
                    <div className={cl.catalog_section__uploaded_model_table__list_element_column}>
                        <div className={cl.catalog_section__uploaded_model_table__list_element_column_cont}>
                            <p className={cl.catalog_section__uploaded_model_table__list_element_column_name_text}
                                title={el.name}>
                                {el.name}
                            </p>
                        </div>
                    </div>
                    <div className={cl.catalog_section__uploaded_model_table__list_element_column}>
                        <div className={cl.catalog_section__uploaded_model_table__list_element_column_cont}>
                            <p className={cl.catalog_section__uploaded_model_table__list_element_column_price_text}>
                                {el.minPrice} ₴
                            </p>
                        </div>
                    </div>
                    <div className={cl.catalog_section__uploaded_model_table__list_element_column}>
                        <div className={cl.catalog_section__uploaded_model_table__list_element_column_cont}>
                            <p className={cl.catalog_section__uploaded_model_table__list_element_column_date_text}>{date}</p>
                            <p className={cl.catalog_section__uploaded_model_table__list_element_column_date_text}>{time}</p>
                        </div>
                    </div>
                    <div className={cl.catalog_section__uploaded_model_table__list_element_column}>
                        <div className={cl.catalog_section__uploaded_model_table__list_element_column_open_cont}
                            onClick={() => { setReviewModelWindowInfo({ visible: true, modelId: el.id }); }}>
                            <img className={cl.catalog_section__uploaded_model_table__list_element_column_open_img} alt="open" />
                        </div>
                    </div>
                </div>
            );
        });

        return result;
    }

    React.useEffect(() => {
        if (uploadedModelList === null) {
            LoadUploadedModelList();
        }
    });

    if (uploadedModelList?.length === 0) {
        return (
            <h4 className={cl.catalog_section__uploaded_model_table__list_is_empty}>
                На даний момент завантажених моделей не має.
            </h4>
        )
    }

    return (
        <>
            <div className={cl.catalog_section__uploaded_model_table__header}>
                <div className={cl.catalog_section__uploaded_model_table__header__column_header}>
                    <div className={cl.catalog_section__uploaded_model_table__header__column_header_name}
                        onClick={() => {
                            const parameter = 'login';
                            const direction = sortMode.value === 'login' ? !sortMode.asc : true;
                            setSortMode({ value: parameter, asc: direction });
                            LoadUploadedModelList(parameter, direction);
                        }}>
                        <img className={cl.catalog_section__uploaded_model_table__header__column_header_img} alt="sort"
                            style={{ display: sortMode.value === 'login' ? 'block' : 'none' }}
                            src={sortMode.asc ? sortAscImg : sortDescImg} />
                        <span className={cl.catalog_section__uploaded_model_table__header__column_header_text}>Користувач</span>
                    </div>
                </div>
                <div className={cl.catalog_section__uploaded_model_table__header__column_header}>
                    <div className={cl.catalog_section__uploaded_model_table__header__column_header_name}
                        onClick={() => {
                            const parameter = 'name';
                            const direction = sortMode.value === 'name' ? !sortMode.asc : true;
                            setSortMode({ value: parameter, asc: direction });
                            LoadUploadedModelList(parameter, direction);
                        }}>
                        <img className={cl.catalog_section__uploaded_model_table__header__column_header_img} alt="sort"
                            style={{ display: sortMode.value === 'name' ? 'block' : 'none' }}
                            src={sortMode.asc ? sortAscImg : sortDescImg} />
                        <span className={cl.catalog_section__uploaded_model_table__header__column_header_text}>Назва моделі</span>
                    </div>
                </div>
                <div className={cl.catalog_section__uploaded_model_table__header__column_header}>
                    <div className={cl.catalog_section__uploaded_model_table__header__column_header_name}
                        onClick={() => {
                            const parameter = 'price';
                            const direction = sortMode.value === 'price' ? !sortMode.asc : true;
                            setSortMode({ value: parameter, asc: direction });
                            LoadUploadedModelList(parameter, direction);
                        }}>
                        <img className={cl.catalog_section__uploaded_model_table__header__column_header_img} alt="sort"
                            style={{ display: sortMode.value === 'price' ? 'block' : 'none' }}
                            src={sortMode.asc ? sortAscImg : sortDescImg} />
                        <span className={cl.catalog_section__uploaded_model_table__header__column_header_text}>Ціна</span>
                    </div>
                </div>
                <div className={cl.catalog_section__uploaded_model_table__header__column_header}>
                    <div className={cl.catalog_section__uploaded_model_table__header__column_header_name}
                        onClick={() => {
                            const parameter = 'date';
                            const direction = sortMode.value === 'date' ? !sortMode.asc : true;
                            setSortMode({ value: parameter, asc: direction });
                            LoadUploadedModelList(parameter, direction);
                        }}>
                        <img className={cl.catalog_section__uploaded_model_table__header__column_header_img} alt="sort"
                            style={{ display: sortMode.value === 'date' ? 'block' : 'none' }}
                            src={sortMode.asc ? sortAscImg : sortDescImg} />
                        <span className={cl.catalog_section__uploaded_model_table__header__column_header_text}>Дата</span>
                    </div>
                </div>
            </div>
            <div className={cl.catalog_section__uploaded_model_table__list}>
                {RenderUploadedModelList()}
            </div>
        </>
    );
}

export default CatalogModerationPage;