import React from "react";
import cl from './.module.css';
import { CatalogAPI } from '../../services/api/CatalogAPI';
import LoadingAnimation from "../../components/LoadingAnimation/LoadingAnimation";
import sortAcsImg from './img/sort-by-asc.png';
import sortDecsImg from './img/sort-by-desc.png';

const CatalogPage = () => {

    const [modelList, setModelList] = React.useState(undefined);
    const [categoryList, setCategoryList] = React.useState(undefined);
    const [authorList, setAuthorList] = React.useState(undefined);

    const [isModelListLoading, setModelListLoading] = React.useState(false);
    const [isCategoryListLoading, setCategoryListLoading] = React.useState(false);
    const [isAuthorListLoading, setAuthorListLoading] = React.useState(false);

    const [isCategoryListVisible, setCategoryListVisibility] = React.useState(true);
    const [isAuthorListVisible, setAuthorListVisibility] = React.useState(true);
    const [isRatingListVisible, setRatingListVisibility] = React.useState(true);

    const [categorySearch, setCategorySearch] = React.useState('');
    const [authorSearch, setAuthorSearch] = React.useState('');
    const [sortMode, setSortMode] = React.useState({ value: 'name', asc: true });

    const [categoriesForFilter, setCategoriesForFilter] = React.useState([]);
    const [authorForFilter, setAuthorForFilter] = React.useState(undefined);

    const minPriceInputRef = React.useRef();
    const maxPriceInputRef = React.useRef();
    const modelSearchInputRef = React.useRef();
    const allAuthorsRadioRef = React.useRef();

    async function LoadModelList(
        sortParameter = sortMode.value,
        sortDirection = sortMode.asc,
        categoryIDs = categoriesForFilter,
        author = authorForFilter
    ) {
        setModelListLoading(true);

        let ratings = Array.from(document.getElementsByClassName(cl.rating_value_checkbox))
            .filter(el => el.checked)
            .map(el => el.value);

        setModelList((await (await CatalogAPI.search(
            modelSearchInputRef.current.value,
            minPriceInputRef.current.value,
            maxPriceInputRef.current.value,
            sortParameter,
            sortDirection ? 'asc' : 'desc',
            categoryIDs,
            ratings,
            author
        )).json()).data.items);

        setModelListLoading(false);
    }

    async function LoadCategoryList() {
        setCategoryListLoading(true);
        setCategoryList((await (await CatalogAPI.getCategories()).json()).data);
        setCategoryListLoading(false);
    }

    async function LoadAuthorList() {
        setAuthorListLoading(true);
        setAuthorList((await (await CatalogAPI.getAuthors()).json()).data.items);
        setAuthorListLoading(false);
    }

    function RenderModelList() {
        let result = [];

        if (modelList?.length === 0) {
            return (
                <div className={cl.model_list_is_empty}>
                    <h3 className={cl.model_list_is_empty_text}>Моделі за даними фільтрами та пошуком не були найдені</h3>
                </div>
            );
        }

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
            const print = el.name.toLowerCase().includes(categorySearch.toLowerCase());
            result.push(
                <div className={cl.category} style={{ display: print ? 'flex' : 'none' }} key={el.id}>
                    <div className={cl.category_checkbox_cont}>
                        <input
                            className={cl.category_checkbox}
                            value={el.id}
                            type="checkbox"
                            onChange={(e) => {
                                let categoryIDs = e.target.checked
                                    ? [...categoriesForFilter, e.target.value]
                                    : categoriesForFilter.filter(el => { return e.target.value !== el });

                                setCategoriesForFilter(categoryIDs);
                                LoadModelList(sortMode.value, sortMode.asc, categoryIDs);
                            }} />
                    </div>
                    <span className={cl.category_name}>{el.name}</span>
                    <span className={cl.items_with_category_count}>{el.count}</span>
                </div>
            );
        });

        return result;
    }

    function RenderAuthorList() {
        let result = [];

        result.push(
            <div className={cl.author} key="all-authors">
                <input
                    className={cl.author_checkbox}
                    type="radio"
                    name="author"
                    value={undefined}
                    defaultChecked={!authorForFilter}
                    ref={allAuthorsRadioRef}
                    onChange={() => {
                        setAuthorForFilter(undefined);
                        LoadModelList(sortMode.value, sortMode.asc, categoriesForFilter, null);
                    }} />
                <span className={cl.author_name}>Всі користувачі</span>
            </div>
        );

        authorList?.forEach(el => {
            const print = el.login.toLowerCase().includes(authorSearch.toLowerCase());
            result.push(
                <div className={cl.author} style={{ display: print ? 'flex' : 'none' }} key={el.login}>
                    <input
                        className={cl.author_checkbox}
                        type="radio"
                        name="author"
                        value={el.login}
                        onChange={(e) => {
                            setAuthorForFilter(e.target.value);
                            LoadModelList(sortMode.value, sortMode.asc, categoriesForFilter, e.target.value);
                        }} />
                    <span className={cl.author_name}>{el.login}</span>
                    <span className={cl.items_with_author_count}>{el.count}</span>
                </div>
            );
        });

        return result;
    }

    function RenderRatingList() {
        let result = [];

        for (let i = 5; i >= 1; i--) {
            result.push(
                <div className={cl.rating_value} key={i}>
                    <div className={cl.rating_value_checkbox_cont}>
                        <input
                            className={cl.rating_value_checkbox}
                            value={i}
                            type="checkbox"
                            onChange={() => LoadModelList()} />
                    </div>
                    <div className={cl.model_rating_value}>
                        <img className={cl.model_rating_value_star} alt="rating star"
                            style={{ filter: i >= 1 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                        <img className={cl.model_rating_value_star} alt="rating star"
                            style={{ filter: i >= 2 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                        <img className={cl.model_rating_value_star} alt="rating star"
                            style={{ filter: i >= 3 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                        <img className={cl.model_rating_value_star} alt="rating star"
                            style={{ filter: i >= 4 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                        <img className={cl.model_rating_value_star} alt="rating star"
                            style={{ filter: i >= 5 ? 'none' : 'grayscale(100%) brightness(250%)' }} />
                    </div>
                </div>
            );
        }

        return result;
    }

    React.useEffect(() => {
        if (modelList === undefined) {
            LoadModelList(sortMode.value, true);
        }

        if (categoryList === undefined) {
            LoadCategoryList();
        }

        if (authorList === undefined) {
            LoadAuthorList();
        }
    });

    return (
        <div className={cl.main}>
            <div className={cl.content}>
                <div className={cl.filter_section}>
                    <div className={cl.reload_filters_button} onClick={() => {
                        setCategoriesForFilter([]);
                        setAuthorForFilter(null);

                        modelSearchInputRef.current.value = '';
                        setSortMode({ value: 'name', asc: true });
                        minPriceInputRef.current.value = 0;
                        maxPriceInputRef.current.value = 10000;
                        allAuthorsRadioRef.current.checked = true;

                        let categories = document.getElementsByClassName(cl.category_checkbox);
                        let ratings = document.getElementsByClassName(cl.rating_value_checkbox);

                        for (let i = 0; i < categories.length; i++) {
                            categories[i].checked = false;
                        }
                        for (let i = 0; i < ratings.length; i++) {
                            ratings[i].checked = false;
                        }

                        LoadModelList('name', true, [], null);
                    }}>
                        <img className={cl.reload_filters_button_img} alt="reload filters" />
                        <span className={cl.reload_filters_button_text}>Скинути фільтри</span>
                    </div>
                    <div className={`${cl.filter} ${cl.price_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.price_filter_header}`}>Ціна</h2>
                        <div className={cl.price_filter_inputs}>
                            <input
                                className={cl.price_filter_min}
                                type="number"
                                defaultValue={0}
                                ref={minPriceInputRef}
                                onChange={() => LoadModelList()} />
                            <input className={cl.price_filter_max} type="number" defaultValue={10000} ref={maxPriceInputRef} />
                        </div>
                    </div>
                    <div className={`${cl.filter} ${cl.category_filter} ${isCategoryListVisible ? '' : cl.closed_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.category_filter_header}`}>Категорії</h2>
                        <img
                            className={`
                            ${cl.filter_list_hider}
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
                    <div className={`${cl.filter} ${cl.author_filter} ${isAuthorListVisible ? '' : cl.closed_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.author_filter_header}`}>Публікатори</h2>
                        <img
                            className={`
                            ${cl.filter_list_hider}
                            ${isAuthorListVisible ? cl.filter_list_hider_opened : cl.filter_list_hider_closed} 
                            ${cl.author_filter_list_hider}`} alt="hide"
                            onClick={() => setAuthorListVisibility(!isAuthorListVisible)} />
                        <input className={`${cl.filter_input} ${cl.author_filter_input}`}
                            type="text"
                            placeholder="Пошук публікатора"
                            onChange={(e) => setAuthorSearch(e.target.value)} />
                        <div className={`${cl.filter_list} ${cl.author_list}`}>
                            {isAuthorListLoading ?
                                <div className={cl.author_list_is_unloaded}>
                                    <LoadingAnimation size="50px" loadingCurveWidth="10px" />
                                </div>
                                :
                                RenderAuthorList()
                            }
                        </div>
                    </div>
                    <div className={`${cl.filter} ${cl.rating_value_filter} ${isRatingListVisible ? '' : cl.closed_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.rating_value_filter_header}`}>Рейтинг</h2>
                        <img
                            className={`
                            ${cl.filter_list_hider}
                            ${isRatingListVisible ? cl.filter_list_hider_opened : cl.filter_list_hider_closed} 
                            ${cl.rating_value_filter_list_hider}`} alt="hide"
                            onClick={() => setRatingListVisibility(!isRatingListVisible)} />
                        <div className={`${cl.filter_list} ${cl.rating_value_list}`}>
                            {RenderRatingList()}
                        </div>
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