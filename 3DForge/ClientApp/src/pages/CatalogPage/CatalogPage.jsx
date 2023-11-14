import React from "react";
import cl from './.module.css';
import ratingImg from './img/sort-by-rating.png';
import noRatingImg from './img/no-rating.png';

const CatalogPage = () => {
    return (
        <div className={cl.main}>
            <div className={cl.content}>
                <div className={cl.filter_section}>
                    <div className={`${cl.filter} ${cl.price_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.price_filter_header}`}>Ціна</h2>
                        <div className={cl.price_filter_inputs}>
                            <input className={cl.price_filter_min} type="number" defaultValue={0} />
                            <input className={cl.price_filter_max} type="number" defaultValue={10000} />
                        </div>
                    </div>
                    <div className={`${cl.filter} ${cl.category_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.category_filter_header}`}>Категорії</h2>
                        <img className={`${cl.filter_list_hider} ${cl.category_filter_list_hider}`} alt="hide" />
                    </div>
                    <div className={`${cl.filter} ${cl.publisher_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.publisher_filter_header}`}>Публікатор</h2>
                        <img className={`${cl.filter_list_hider} ${cl.publisher_filter_list_hider}`} alt="hide" />
                    </div>
                    <div className={`${cl.filter} ${cl.size_filter}`}>
                        <h2 className={`${cl.filter_header} ${cl.size_filter_header}`}>Розмір</h2>
                        <img className={`${cl.filter_list_hider} ${cl.size_filter_list_hider}`} alt="hide" />
                    </div>
                </div>
                <div className={cl.model_section}>
                    <h1 className={cl.catalog_header}>КАТАЛОГ</h1>
                    <input className={cl.search} type="text" placeholder="Пошук моделі за назвою" />
                    <div className={cl.sort_and_add}>
                        <div className={cl.sort}>
                            <div className={`${cl.sort_mode} ${cl.sort_by_cheapest}`}>
                                <img className={`${cl.sort_mode_img} ${cl.sort_by_cheapest_img}`} alt="cheapest" />
                                <span className={`${cl.sort_mode_text} ${cl.sort_by_cheapest_text}`}>СПОЧАТКУ ДЕШЕВІ</span>
                            </div>
                            <div className={`${cl.sort_mode} ${cl.sort_by_most_expensive}`}>
                                <img className={`${cl.sort_mode_img} ${cl.sort_by_most_expensive_img}`} alt="most expensive" />
                                <span className={`${cl.sort_mode_text} ${cl.sort_by_most_expensive_text}`}>СПОЧАТКУ ДОРОГІ</span>
                            </div>
                            <div className={`${cl.sort_mode} ${cl.sort_by_rating}`}>
                                <img className={`${cl.sort_mode_img} ${cl.sort_by_rating_img}`} alt="most rated" />
                                <span className={`${cl.sort_mode_text} ${cl.sort_by_rating_text}`}>ЗА ОЦІНКОЮ</span>
                            </div>
                        </div>
                        <div className={cl.add_model_button}>
                            <img className={cl.add_model_button_img} alt="add model" />
                            <span className={cl.add_model_button_text}>Додати модель</span>
                        </div>
                    </div>
                    <div className={cl.model_list}>
                        <div className={cl.model}>
                            <span className={cl.model_size}>L</span>
                            <div className={cl.model_img_cont}>
                                <img className={cl.model_img} alt="model" />
                            </div>
                            <p className={cl.model_description}>Cупер крута машина маквін</p>
                            <div className={cl.model_footer}>
                                <div className={cl.model_other_info}>
                                    <div className={cl.model_price}>
                                        <span className={cl.model_price_number}>120</span>
                                        <span className={cl.model_money_character}>₴</span>
                                    </div>
                                    <div className={cl.model_rating}>
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                    </div>
                                </div>
                                <div className={cl.buy_model_button}>
                                    <img className={cl.buy_model_button_img} alt="buy model" />
                                    <span className={cl.buy_model_button_text}>Придбати</span>
                                </div>
                            </div>
                        </div>
                        <div className={cl.model}>
                            <span className={cl.model_size}>L</span>
                            <div className={cl.model_img_cont}>
                                <img className={cl.model_img} alt="model" />
                            </div>
                            <p className={cl.model_description}>Cупер крута машина маквін</p>
                            <div className={cl.model_footer}>
                                <div className={cl.model_other_info}>
                                    <div className={cl.model_price}>
                                        <span className={cl.model_price_number}>120</span>
                                        <span className={cl.model_money_character}>₴</span>
                                    </div>
                                    <div className={cl.model_rating}>
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                    </div>
                                </div>
                                <div className={cl.buy_model_button}>
                                    <img className={cl.buy_model_button_img} alt="buy model" />
                                    <span className={cl.buy_model_button_text}>Придбати</span>
                                </div>
                            </div>
                        </div>
                        <div className={cl.model}>
                            <span className={cl.model_size}>L</span>
                            <div className={cl.model_img_cont}>
                                <img className={cl.model_img} alt="model" />
                            </div>
                            <p className={cl.model_description}>Cупер крута машина маквін</p>
                            <div className={cl.model_footer}>
                                <div className={cl.model_other_info}>
                                    <div className={cl.model_price}>
                                        <span className={cl.model_price_number}>120</span>
                                        <span className={cl.model_money_character}>₴</span>
                                    </div>
                                    <div className={cl.model_rating}>
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                    </div>
                                </div>
                                <div className={cl.buy_model_button}>
                                    <img className={cl.buy_model_button_img} alt="buy model" />
                                    <span className={cl.buy_model_button_text}>Придбати</span>
                                </div>
                            </div>
                        </div>
                        <div className={cl.model}>
                            <span className={cl.model_size}>L</span>
                            <div className={cl.model_img_cont}>
                                <img className={cl.model_img} alt="model" />
                            </div>
                            <p className={cl.model_description}>Cупер крута машина маквін</p>
                            <div className={cl.model_footer}>
                                <div className={cl.model_other_info}>
                                    <div className={cl.model_price}>
                                        <span className={cl.model_price_number}>120</span>
                                        <span className={cl.model_money_character}>₴</span>
                                    </div>
                                    <div className={cl.model_rating}>
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                        <img className={cl.model_rating_star} alt="rating star" src={ratingImg} />
                                    </div>
                                </div>
                                <div className={cl.buy_model_button}>
                                    <img className={cl.buy_model_button_img} alt="buy model" />
                                    <span className={cl.buy_model_button_text}>Придбати</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default CatalogPage;