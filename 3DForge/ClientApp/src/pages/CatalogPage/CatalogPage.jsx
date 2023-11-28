import React from "react";
import cl from './.module.css';
import { CatalogAPI } from '../../services/api/CatalogAPI';
import LoadingAnimation from "../../components/LoadingAnimation/LoadingAnimation";
import sortAcsImg from './img/sort-by-asc.png';
import sortDecsImg from './img/sort-by-desc.png';
import { useNavigate } from "react-router-dom";

const CatalogPage = () => {

    const navigate = useNavigate();

    const [modelList, setModelList] = React.useState(undefined);
    const [categoryList, setCategoryList] = React.useState(undefined);
    const [authorList, setAuthorList] = React.useState({ list: undefined, loadedPageCount: 0, generalPageCount: 0 });

    const [isModelListLoading, setModelListLoading] = React.useState(false);
    const [isCategoryListLoading, setCategoryListLoading] = React.useState(false);
    const [isAuthorListLoading, setAuthorListLoading] = React.useState(false);

    const [isCategoryListVisible, setCategoryListVisibility] = React.useState(true);
    const [isAuthorListVisible, setAuthorListVisibility] = React.useState(true);
    const [isRatingListVisible, setRatingListVisibility] = React.useState(true);

    const [categorySearch, setCategorySearch] = React.useState('');
    const [sortMode, setSortMode] = React.useState({ value: 'name', asc: true });

    const [categoriesForFilter, setCategoriesForFilter] = React.useState([]);
    const [authorForFilter, setAuthorForFilter] = React.useState(undefined);

    const [modelListPageInfo, setModelListPageInfo] = React.useState({ count: undefined, current: 1 });

    const [isUploadModelMenuVisible, setUploadModelMenuVisibility] = React.useState(false);

    const minPriceInputRef = React.useRef();
    const maxPriceInputRef = React.useRef();
    const modelSearchInputRef = React.useRef();
    const authorSearchInputRef = React.useRef();
    const allAuthorsRadioRef = React.useRef();

    async function LoadModelList(
        sortParameter = sortMode.value,
        sortDirection = sortMode.asc,
        categoryIDs = categoriesForFilter,
        author = authorForFilter,
        pageNumber = 1
    ) {
        setModelListLoading(true);

        let ratings = Array.from(document.getElementsByClassName(cl.rating_value_checkbox))
            .filter(el => el.checked)
            .map(el => el.value);

        let json = await (await CatalogAPI.search(
            modelSearchInputRef.current.value,
            minPriceInputRef.current.value,
            maxPriceInputRef.current.value,
            sortParameter,
            sortDirection ? 'asc' : 'desc',
            categoryIDs,
            ratings,
            author,
            pageNumber
        )).json();

        setModelList(json.data.items);
        setModelListPageInfo({ count: json.data.pageCount, current: json.data.pageIndex });

        setModelListLoading(false);
    }

    async function LoadCategoryList() {
        setCategoryListLoading(true);
        setCategoryList((await (await CatalogAPI.getCategories()).json()).data);
        setCategoryListLoading(false);
    }

    async function LoadAuthorList(addPage) {
        setAuthorListLoading(true);

        let json = await (await CatalogAPI.getAuthors(
            authorSearchInputRef.current.value,
            addPage ? authorList.loadedPageCount + 1 : 1
        )).json();

        setAuthorList(p => {
            let newP = { ...p };

            if (addPage) {
                newP.list.push(...json.data.items);
                newP.loadedPageCount += json.data.pageCount > p.loadedPageCount ? 1 : 0;
            }
            else {
                newP.list = json.data.items;
                newP.loadedPageCount = 1;
            }

            newP.generalPageCount = json.data.pageCount;
            return newP;
        });

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

        authorList.list?.forEach(el => {
            result.push(
                <div className={cl.author} key={el.login}>
                    <input
                        className={cl.author_checkbox}
                        type="radio"
                        name="author"
                        value={el.login}
                        defaultChecked={authorForFilter === el.login}
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

    function RenderUploadModelMenu() {
        if (!isUploadModelMenuVisible) {
            return;
        }

        return (
            <div className={cl.model_upload_window_background}>
                <div className={cl.model_upload_window}>

                </div>
            </div>
        );
    }

    function RenderPageNavigator() {
        if (modelListPageInfo.count <= 1) {
            return;
        }

        const loadPage = (idx) => {
            if (idx !== modelListPageInfo.current) {
                window.scroll({ top: 0 });
                LoadModelList(undefined, undefined, undefined, undefined, idx);
            }
        };

        let result = [];
        let left = [];
        let right = [];
        let start, end;

        if (modelListPageInfo.current > 1) {
            left.push(
                <span className={cl.page_navigator_obj} key="arrow-left"
                    onClick={() => loadPage(modelListPageInfo.current - 1)}>&#60;</span>
            );

            if ((modelListPageInfo.current - 4) > 1) {
                left.push(
                    <span className={`${cl.page_navigator_obj}`} key="1" onClick={() => loadPage(1)}>1</span>
                );

                left.push(
                    <span className={`${cl.page_navigator_obj}`} key="dots-left"
                        onClick={() => {
                            const idxLink = (modelListPageInfo.current - 10) > 1
                                ? modelListPageInfo.current - 10
                                : 2;

                            loadPage(idxLink);
                        }}>...</span>
                );

                start = modelListPageInfo.current - 2;
            }
            else {
                start = 1;
            }
        }
        else {
            start = 1;
        }

        if (modelListPageInfo.current < modelListPageInfo.count) {
            right.unshift(
                <span className={cl.page_navigator_obj} key="arrow-right"
                    onClick={() => loadPage(modelListPageInfo.current + 1)}>&#62;</span>
            );

            if ((modelListPageInfo.current + 4) < modelListPageInfo.count) {
                right.unshift(
                    <span className={`${cl.page_navigator_obj}`} key={modelListPageInfo.count}
                        onClick={() => loadPage(modelListPageInfo.count)}>{modelListPageInfo.count}</span>
                );

                right.unshift(
                    <span className={`${cl.page_navigator_obj}`} key="dots-right"
                        onClick={() => {
                            const idxLink = (modelListPageInfo.current + 10) < modelListPageInfo.count
                                ? modelListPageInfo.current + 10
                                : modelListPageInfo.count - 1;

                            loadPage(idxLink);
                        }}>...</span>
                );

                end = modelListPageInfo.current + 2;
            }
            else {
                end = modelListPageInfo.count;
            }
        }
        else {
            end = modelListPageInfo.count;
        }

        if ((end - start) < 4) {
            while ((end - start) < 4 && end < modelListPageInfo.count) {
                end++;
            }
        }

        if ((end - start) < 4) {
            while ((end - start) < 4 && start > 1) {
                start--;
            }
        }

        for (let i = start; i <= end; i++) {
            result.push(
                <span
                    className={`${cl.page_navigator_obj} ${modelListPageInfo.current === i ? cl.current_page_navigator_obj : ''}`}
                    key={i}
                    onClick={() => loadPage(i)}>{i}</span>
            );
        }

        result.unshift(left);
        result.push(right);

        return result;
    }

    function WindowKeyPressEvent(event) {
        if (event.key === "Enter" && (document.activeElement === modelSearchInputRef.current)) {
            LoadModelList();
        }
    }

    React.useEffect(() => {
        if (modelList === undefined) {
            LoadModelList(sortMode.value, true);
        }

        if (categoryList === undefined) {
            LoadCategoryList();
        }

        if (authorList.list === undefined) {
            LoadAuthorList(false);
        }

        window.addEventListener("keypress", WindowKeyPressEvent);

        return () => {
            window.removeEventListener("keypress", WindowKeyPressEvent);
        };
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

                        LoadModelList('name', true, [], null, 1);
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
                        <h2 className={`${cl.filter_header} ${cl.author_filter_header}`}>Автори</h2>
                        <img
                            className={`
                            ${cl.filter_list_hider}
                            ${isAuthorListVisible ? cl.filter_list_hider_opened : cl.filter_list_hider_closed} 
                            ${cl.author_filter_list_hider}`} alt="hide"
                            onClick={() => setAuthorListVisibility(!isAuthorListVisible)} />
                        <input className={`${cl.filter_input} ${cl.author_filter_input}`}
                            type="text"
                            placeholder="Пошук авторів"
                            ref={authorSearchInputRef}
                            onChange={() => LoadAuthorList(false)} />
                        <div className={`${cl.filter_list} ${cl.author_list}`}>
                            {RenderAuthorList()}
                            {isAuthorListLoading ?
                                <div className={cl.author_list_is_unloaded}>
                                    <LoadingAnimation size="50px" loadingCurveWidth="10px" />
                                </div>
                                : <></>
                            }
                            {authorList.loadedPageCount < authorList.generalPageCount && !isAuthorListLoading ?
                                <div className={cl.show_more_authors} onClick={() => LoadAuthorList(true)}>
                                    <span className={cl.show_more_authors_text}>Показати ще</span>
                                </div>
                                : <></>
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
                    <div className={cl.search_model_cont}>
                        <div className={cl.search_model_img_cont} onClick={() => LoadModelList()}>
                            <img className={cl.search_model_img} alt="search" />
                        </div>
                        <input className={cl.search_model} type="text" placeholder="Пошук моделі" ref={modelSearchInputRef} />
                    </div>
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
                        <div className={cl.add_model_button} onClick={() => setUploadModelMenuVisibility(true)}>
                            <img className={cl.add_model_button_img} alt="add model" />
                            <span className={cl.add_model_button_text}>Додати модель</span>
                        </div>
                    </div>
                    {isModelListLoading ?
                        <div className={cl.model_list_is_unloaded}>
                            <LoadingAnimation size="100px" loadingCurveWidth="20px" />
                        </div>
                        :
                        <>
                            <div className={cl.model_list}>
                                {RenderModelList()}
                            </div>
                            <div className={cl.page_navigator_cont}>
                                <div className={cl.page_navigator}>
                                    {RenderPageNavigator()}
                                </div>
                            </div>
                        </>
                    }
                </div>
            </div>
            {RenderUploadModelMenu()}
        </div>
    );
}

export default CatalogPage;