import React from "react";
import cl from './.module.css';
import { CatalogAPI } from '../../services/api/CatalogAPI';
import LoadingAnimation from "../../components/LoadingAnimation/LoadingAnimation";
import sortAcsImg from './img/sort-by-asc.png';
import sortDecsImg from './img/sort-by-desc.png';

const CatalogPage = () => {

    const [categoryList, setCategoryList] = React.useState(undefined);
    const [modelList, setModelList] = React.useState(undefined);
    const [isCategoryListLoading, setCategoryListLoading] = React.useState(false);
    const [isModelListLoading, setModelListLoading] = React.useState(false);
    const [isCategoryListVisible, setCategoryListVisibility] = React.useState(true);
    const [isPublisherListVisible, setPublisherListVisibility] = React.useState(true);
    const [categorySearch, setCategorySearch] = React.useState('');
    const [sortMode, setSortMode] = React.useState({ value: 'name', asc: true });

    const modelSearchInputRef = React.useRef();

    async function LoadModelList(sortParameter, sortDirection) {
        setModelListLoading(true);
        setModelList((await (await CatalogAPI.search(
            modelSearchInputRef.current.value,
            sortParameter,
            sortDirection ? 'asc' : 'desc'
        )).json()).data.items);
        setModelListLoading(false);
    }

    async function LoadCategoryList() {
        setCategoryListLoading(true);
        setCategoryList((await (await CatalogAPI.getCategories()).json()).data);
        setCategoryListLoading(false);
    }

    function RenderModelList() {
        let result = [];

        modelList?.forEach(el => {
            result.push(
                <div className={cl.model} key={el.id} onClick={() => { window.location.pathname = `catalog/${el.id}` }}>
                    <div className={cl.model_img_cont}>
                        <img className={cl.model_img} alt="model" src={`/api/catalog/model/picture/${el.picturesIDs[0]}`} />
                    </div>
                    <h2 className={cl.model_name}>{el.name}</h2>
                    <p className={cl.model_description}>{el.description}</p>
                    <div className={cl.model_footer}>
                        <div className={cl.model_other_info}>
                            <div className={cl.model_price}>
                                <span className={cl.model_price_number}>{Number((el.minPrice).toFixed(2))}</span>
                                <span className={cl.model_money_character}>₴</span>
                            </div>
                            <div className={cl.model_rating}>
                                <img className={cl.model_rating_star} alt="rating star"
                                    style={{ filter: el.rating >= 1 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                                <img className={cl.model_rating_star} alt="rating star"
                                    style={{ filter: el.rating >= 2 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                                <img className={cl.model_rating_star} alt="rating star"
                                    style={{ filter: el.rating >= 3 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                                <img className={cl.model_rating_star} alt="rating star"
                                    style={{ filter: el.rating >= 4 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                                <img className={cl.model_rating_star} alt="rating star"
                                    style={{ filter: el.rating >= 5 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                            </div>
                        </div>
                        <div className={cl.buy_model_button}>
                            <img className={cl.buy_model_button_img} alt="buy model" />
                            <span className={cl.buy_model_button_text}>Придбати</span>
                        </div>
                    </div>
                </div>
            );
        });

        return result;
    }

    function RenderCategoryList() {
        let result = [];

        categoryList?.forEach(el => {
            if (!el.name.toLowerCase().includes(categorySearch.toLowerCase())) {
                return;
            }

            result.push(
                <div className={cl.category} key={el.id}>
                    <input className={cl.category_checkbox} type="checkbox" />
                    <span className={cl.category_name}>{el.name}</span>
                    <span className={cl.items_with_category_count}>{el.count}</span>
                </div>
            );
        });

        return result;
    }

    React.useEffect(() => {
        if (modelList === undefined) {
            LoadModelList(sortMode.value, true);
        }

        if (categoryList === undefined) {
            LoadCategoryList();
        }
    });

    return (
        <div className={cl.main}>
            <div className={cl.content}>
                <div className={cl.filter_section}>
                    <div className={cl.reload_filters_button}>
                        <img className={cl.reload_filters_button_img} />
                        <span className={cl.reload_filters_button_text}>Скинути фільтри</span>
                    </div>
                    <div className={`${cl.filter} ${cl.price_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.price_filter_header}`}>Ціна</h2>
                        <div className={cl.price_filter_inputs}>
                            <input className={cl.price_filter_min} type="number" defaultValue={0} />
                            <input className={cl.price_filter_max} type="number" defaultValue={10000} />
                        </div>
                    </div>
                    <div className={`${cl.filter} ${cl.category_filter} ${isCategoryListVisible ? '' : cl.closed_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.category_filter_header}`}>Категорії</h2>
                        <img
                            className={`
                            ${isCategoryListVisible ? cl.filter_list_hider_opened : cl.filter_list_hider_closed} 
                            ${cl.category_filter_list_hider}`} alt="hide"
                            onClick={() => setCategoryListVisibility(!isCategoryListVisible)} />
                        <input className={`${cl.filter_input} ${cl.category_filter_input}`}
                            type="text"
                            placeholder="Пошук категорії"
                            onChange={(e) => setCategorySearch(e.target.value)} />
                        <div className={`${cl.filter_list} ${cl.category_list}`}>
                            {isCategoryListLoading ?
                                <div className={cl.category_list_is_unloaded}>
                                    <LoadingAnimation size="50px" loadingCurveWidth="10px" />
                                </div>
                                :
                                RenderCategoryList()
                            }
                        </div>
                    </div>
                    <div className={`${cl.filter} ${cl.publisher_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.publisher_filter_header}`}>Публікатори</h2>
                        <img
                            className={`
                            ${isPublisherListVisible ? cl.filter_list_hider_opened : cl.filter_list_hider_closed} 
                            ${cl.category_filter_list_hider}`} alt="hide"
                            onClick={() => setPublisherListVisibility(!isPublisherListVisible)} />
                        <input className={`${cl.filter_input} ${cl.publisher_filter_input}`} type="text" placeholder="Пошук публікатора" />
                    </div>
                </div>
                <div className={cl.model_section}>
                    <h1 className={cl.catalog_header}>КАТАЛОГ</h1>
                    <input className={cl.search} type="text" placeholder="Пошук моделі" ref={modelSearchInputRef}
                        onChange={() => LoadModelList()} />
                    <div className={cl.sort_and_add}>
                        <div className={cl.sort}>
                            <div className={`${cl.sort_mode_cont} ${cl.sort_by_name_cont}`}>
                                <div className={`${cl.sort_mode} ${cl.sort_by_name} 
                                ${sortMode.value === 'name' ? cl.sort_mode_selected : ''}`}
                                    onClick={() => {
                                        const parameter = 'name';
                                        const direction = sortMode.value === 'name' ? !sortMode.asc : true;
                                        setSortMode({ value: parameter, asc: direction });
                                        LoadModelList(parameter, direction);
                                    }}>
                                    <img className={`${cl.sort_mode_img} ${cl.sort_by_name_img}`} alt="sort"
                                        src={sortMode.value === 'name' && sortMode.asc === false ? sortDecsImg : sortAcsImg} />
                                    <span className={`${cl.sort_mode_text} ${cl.sort_by_name_text}`}>НАЗВА</span>
                                </div>
                            </div>
                            <div className={`${cl.sort_mode_cont} ${cl.sort_by_price_cont}`}>
                                <div className={`${cl.sort_mode} ${cl.sort_by_price} 
                                ${sortMode.value === 'price' ? cl.sort_mode_selected : ''}`}
                                    onClick={() => {
                                        const parameter = 'price';
                                        const direction = sortMode.value === 'price' ? !sortMode.asc : true;
                                        setSortMode({ value: parameter, asc: direction });
                                        LoadModelList(parameter, direction);
                                    }}>
                                    <img className={`${cl.sort_mode_img} ${cl.sort_by_price_img}`} alt="sort"
                                        src={sortMode.value === 'price' && sortMode.asc === false ? sortDecsImg : sortAcsImg} />
                                    <span className={`${cl.sort_mode_text} ${cl.sort_by_price_text}`}>ЦІНА</span>
                                </div>
                            </div>
                            <div className={`${cl.sort_mode_cont} ${cl.sort_by_rating_cont}`}>
                                <div className={`${cl.sort_mode} ${cl.sort_by_rating} 
                                ${sortMode.value === 'rating' ? cl.sort_mode_selected : ''}`}
                                    onClick={() => {
                                        const parameter = 'rating';
                                        const direction = sortMode.value === 'rating' ? !sortMode.asc : true;
                                        setSortMode({ value: parameter, asc: direction });
                                        LoadModelList(parameter, direction);
                                    }}>
                                    <img className={`${cl.sort_mode_img} ${cl.sort_by_rating_img}`} alt="sort"
                                        src={sortMode.value === 'rating' && sortMode.asc === false ? sortDecsImg : sortAcsImg} />
                                    <span className={`${cl.sort_mode_text} ${cl.sort_by_rating_text}`}>ОЦІНКА</span>
                                </div>
                            </div>
                        </div>
                        <div className={cl.add_model_button}>
                            <img className={cl.add_model_button_img} alt="add model" />
                            <span className={cl.add_model_button_text}>Додати модель</span>
                        </div>
                    </div>
                    {isModelListLoading ?
                        <div className={cl.model_list_is_unloaded}>
                            <LoadingAnimation size="100px" loadingCurveWidth="20px" />
                        </div>
                        :
                        <div className={cl.model_list}>
                            {RenderModelList()}
                        </div>
                    }
                </div>
            </div>
        </div>
    );
}

export default CatalogPage;