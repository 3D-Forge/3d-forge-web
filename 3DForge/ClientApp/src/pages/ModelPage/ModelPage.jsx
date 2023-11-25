import React from 'react';
import cl from "./.module.css";
import { CatalogAPI } from '../../services/api/CatalogAPI';
import { useParams } from "react-router-dom";





const ModelPage = () => {
    const { id } = useParams();
    const [modelInfo, setModelInfo] = React.useState(undefined);
    const [styleType, setStyleType] = React.useState('default');


    function getStyleClass(rate) {
        if (rate >= 0 && rate < 0.5) {
            //console.log("cond1")
            return cl.group_0_5_star;
        } else if (rate >= 0.5 && rate < 1) {
            return cl.group_1_star;
        } else if (rate >= 1 && rate < 1.5) {
            return cl.group_1_5_star;
        } else if (rate >= 1.5 && rate < 2) {
            return cl.group_2_star;
        } else if (rate >= 2 && rate < 2.5) {
            return cl.group_2_5_star;
        } else if (rate >= 2.5 && rate < 3) {
            return cl.group_3_star;
        } else if (rate >= 3 && rate < 3.5) {
            return cl.group_3_5_star;
        } else if (rate >= 3.5 && rate < 4) {
            return cl.group_4_star;
        } else if (rate >= 4 && rate < 4.5) {
            return cl.group_4_5_star;
        } else if (rate >= 4.5 && rate <= 5) {
            return cl.group_5_star;
        } else {
           // console.log("default " + rate)
            return cl.group_12;
        }
    }
    React.useEffect(() => {
        let isMounted = true;
        const fetchData = async () => {
            if (modelInfo === undefined && isMounted) {
                try {
                    const response = await CatalogAPI.getModel(id);
                    console.log(response.status);
                    console.log(response.status); 
                    
                    if (response.ok) {
                        const resModel = await response.json();
                        console.log(resModel);
                        if (isMounted) {
                            setModelInfo(resModel.data);
                            setStyleType(getStyleClass(resModel.data.rating));
                        }
                    }
                    else {
                        console.error('Помилка отримання моделі:', response.statusText);
                    }
                } catch (error) {
                    console.error('Помилка отримання моделі:', error);
                }
            }
        };

        fetchData();

        return () => {
            isMounted = false;
        };
    }, [modelInfo, id]);

    return (
        <div className={cl.index}>
            <div className={cl.div_2}>
                <div className={cl.view_5}>
                    <div className={cl.navbar}>
                        <a href="#All" className={cl.text_wrapper_12}>ВСЕ ПРО ТОВАР</a>
                        <a href="#description" class={cl.text_wrapper_12}>ОПИС</a>
                        <a href="#characteristics"
                            className={cl.text_wrapper_12}>ХАРАКТЕРИСТИКИ</a>
                        <a href="#feedbacks" className={cl.text_wrapper_12}>ВІДГУКИ</a>
                    </div>
                    <div className={cl.rectangle_2} />
                </div>
                <div className={cl.overlap_3}>
                    <div className={cl.view_6}>
                        <img
                            className={cl.image_removebg}
                            alt="Image removebg"
                            src={`/api/catalog/model/picture/${modelInfo?.picturesIDs[0]}`}
                        />
                        <div className={cl.group_2}>
                            <div id="All"className={cl.text_wrapper_13}>{modelInfo?.name}</div>
                            <div className={cl.text_wrapper_14}>Код товару: {modelInfo?.id}</div>
                        </div>
                        <div className={cl.group_3}>
                            <div className={cl.overlap_group_3}>
                                <div className={cl.text_wrapper_15}>{modelInfo?.depth} ₴</div>
                                <div className={cl.frame}>
                                    <img
                                        className={cl.img_bag}
                                        alt="Cart large"
                                    />
                                    <div className={cl.text_wrapper_16}>Додати модель до кошика</div>
                                </div>
                            </div>
                        </div>
                        <div className={cl.group_7}>
                            <p className={cl.div_5}>
                                <span className={cl.text_wrapper_18}>Способи оплати:</span>
                                <span className={cl.text_wrapper_19}>&nbsp;</span>
                            </p>
                            <p className={cl.text_wrapper_20}>
                                Готівкою при отриманні товару, Накладений платіж, Visa та MasterCard, Переказ на рахунок ФОП та ТОВ
                            </p>
                        </div>
                        <div className={cl.keywords_group} style={{ maxWidth: '550px' }}>
                            {modelInfo?.keywords.map((keyword, index) => (
                                <button class={cl.keywords_buttons} 
                                    key={index}>{keyword}</button>
                            ))}
                        </div>
                        <div className={cl.categories_group}>
                            {modelInfo?.categoryes.map((category, index) => (
                                <button className={cl.categories_buttons} key={index}>
                                    {category.name}
                                </button>
                            ))}
                        </div>
                        <img
                            className={cl.line_3}
                            alt="Line"
                            src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/line-43.svg"
                        />
                        <img
                            className={cl.line_4}
                            alt="Line"
                            src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/line-44.svg"
                        />
                        <img
                            className={cl.line_5}
                            alt="Line"
                            src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/line-44.svg"
                        />
                        <div className={cl.group_8}>
                            <div className={cl.group_9}>
                                <img
                                    className={cl.download_removebg}
                                    alt="Download removebg"

                                />
                                <div className={cl.text_wrapper_21}>Відділення Нова Пошта</div>
                                <div className={cl.text_wrapper_22}>1-3 дні</div>
                                <div className={cl.text_wrapper_23}>за тарифами перевізника</div>
                            </div>
                            <div className={cl.group_10}>
                                <img
                                    className={cl.download_removebg}
                                    alt="Download removebg"
                                />
                                <div className={cl.text_wrapper_21}>Кур’єром Нової Пошти</div>
                                <div className={cl.text_wrapper_22}>1-3 дні</div>
                                <div className={cl.text_wrapper_23}>за тарифами перевізника</div>
                            </div>
                            <div className={cl.group_11}>
                                <p className={cl.div_6}>
                                    <span className={cl.text_wrapper_24}>Доставка:</span>
                                    <span className={cl.text_wrapper_25}>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    <span className={cl.text_wrapper_26}>Харків</span>
                                </p>
                                <img
                                    className={cl.down_arrow_backup_2}
                                    alt="Down arrow backup"
                                    src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/down-arrow-backup-2-svgrepo-com-3.svg"
                                />
                            </div>

                        </div>
                        <img
                            className={getStyleClass(modelInfo?.rating)}
                            alt="Stars"
                        />
                        <div className={cl.group_13}>

                            <img className={cl.box_image}></img>
                            <img className={cl.edit_image}></img>
                            <div className={cl.overlap_4}>
                                <div className={cl.text_wrapper_27}>У наявності</div>
                                <div className={cl.view_13} />
                            </div>
                        </div>
                        <div className={cl.group_14}>
                            <div className={cl.text_wrapper_28}>5 відгуків</div>
                        </div>
                        <div className={cl.group_15}>
                            <div className={cl.group_16}>
                                <img
                                    className={cl.img_2}
                                    alt="Return box shipping"

                                />
                                <div className={cl.text_wrapper_29}>Повернення 14 днів</div>
                            </div>
                            <div className={cl.group_17}>
                                <div className={cl.text_wrapper_30}>Гарантія рівно 1 секунда</div>
                                <img
                                    className={cl.img_2}
                                    alt="Shield svgrepo com"
                                />
                            </div>
                        </div>
                    </div>
                    <img
                        className="cube-viewport"
                        alt="Cube viewport"
                        src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/cube-viewport-svgrepo-com-1.svg"
                    />
                </div>
                <div className={cl.view_14}>
                    <div className={cl.overlap_5}>
                        <div id="description"  className={cl.text_wrapper_31}>Опис</div>
                        <p className={cl.text_wrapper_32}>
                            {modelInfo?.description}
                        </p>
                    </div>
                </div>
                <div className={cl.view_15}>
                    <div className={cl.overlap_6}>
                        <div className={cl.text_wrapper_33}>Колір</div>
                        <div className={cl.text_wrapper_34}>Вага</div>
                        <div className={cl.text_wrapper_35}>Матеріал</div>
                        <div className={cl.text_wrapper_36}>Чорний</div>
                        <div className={cl.text_wrapper_37}>59г</div>
                        <div className={cl.text_wrapper_38}>Пластик</div>
                        <div className={cl.text_wrapper_39}>Ширина</div>
                        <div className={cl.text_wrapper_40}>{modelInfo?.xSize}см</div>
                        <div className={cl.text_wrapper_41}>Довжина</div>
                        <div className={cl.text_wrapper_42}>{modelInfo?.ySize}см</div>
                        <div className={cl.text_wrapper_43}>Товщина</div>
                        <div className={cl.text_wrapper_44}>{modelInfo?.zSize}см</div>
                        <div id="characteristics"className={cl.text_wrapper_31}>Характеристики</div>
                        <div className={cl.text_wrapper_45}>ФІЗИЧНІ ПАРАМЕТРИ</div>
                        <div className={cl.text_wrapper_46}>РОЗМІРИ</div>
                    </div>
                </div>
                <div className={cl.overlap_7}>
                    <img
                        className={cl.avatar_svgrepo_com_2}
                        alt="Avatar svgrepo com"
                        src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/avatar-svgrepo-com-2.svg"
                    />
                    <div className={cl.view_16}>
                        <div className={cl.overlap_8}>
                            <div id="feedbacks" className={cl.text_wrapper_31}>Відгуки</div>
                            <div className={cl.group_18}>
                                <div className={cl.overlap_9}>
                                    <div className={cl.overlap_9}>
                                        <div className={cl.group_19}>
                                            <div className={cl.div_7}>
                                                <div className={cl.ellipse_3} />
                                                <div className={cl.div_7}>
                                                    <div className={cl.overlap_group_5}>
                                                        <div className={cl.ellipse_4} />
                                                        <img
                                                            className={cl.avatar_svgrepo_com_3}
                                                            alt="Avatar svgrepo com"
                                                            src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/avatar-svgrepo-com-1-1.svg"
                                                        />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className={cl.text_wrapper_47}>KotykV</div>
                                        </div>
                                        <div className={cl.group_20}>
                                            <p className={cl.text_wrapper_48}>
                                                Досить добрий кубик маквін КЧАУ, мене влаштовує. Гарна якість матеріалів.
                                            </p>
                                            <img
                                                className={cl.img_3}
                                                alt="Plus svgrepo com"
                                                src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/plus-svgrepo-com--2--1.svg"
                                            />
                                        </div>
                                    </div>
                                    <div className={cl.group_22}>
                                        <img
                                            className={cl.image_star}
                                            alt="Group1"
                                        />
                                        <div className={cl.text_wrapper_49}>14.12.1476</div>
                                    </div>
                                </div>
                                <p className={cl.text_wrapper_50}>
                                    Дуже все файно, мама кубик маквін подарувала, я з ним сплю та миюсь. Не уявляю своє життя без нього.
                                    Татко пропив квартиру, а я залишилась без нагляду батьків. Кчау.
                                </p>
                                <div className={cl.group_23}>
                                    <p className={cl.text_wrapper_51}>Немає (мого кота тримають у заручниках, допоможіть)</p>
                                    <img
                                        className={cl.img_minus}
                                        alt="Minus svgrepo com"
                                        src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/minus-svgrepo-com--2--1.svg"
                                    />
                                </div>
                            </div>
                            <div className={cl.text_wrapper_54}>Залишити відгук</div>
                            <div className={cl.group_28}>
                                <div className={cl.group_29}>
                                    <div className={cl.overlap_12}>
                                        <div className={cl.text_wrapper_55}>Відгук</div>
                                        <div className={cl.rectangle_3} />
                                        <div className={cl.group_30}>
                                            <div className={cl.overlap_group_7}>
                                                <img
                                                    className={cl.line_6}
                                                    alt="Line"
                                                    src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/line-48.svg"
                                                />
                                                <img
                                                    className={cl.line_7}
                                                    alt="Line"
                                                    src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/line-49.svg"
                                                />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className={cl.group_wrapper}>
                                    <div className={cl.group_31}>
                                        <div className={cl.text_wrapper_56}>Плюси</div>
                                        <img
                                            className={cl.img_4}
                                            alt="Plus svgrepo com"
                                        />
                                    </div>
                                </div>
                                <div className={cl.group_32}>
                                    <div className={cl.overlap_13}>
                                        <div className={cl.text_wrapper_57}>Ім’я</div>
                                        <div className={cl.text_wrapper_58}>KotykV</div>
                                    </div>
                                </div>
                                <div className={cl.group_33}>
                                    <div className={cl.overlap_14}>
                                        <div className={cl.text_wrapper_59}>Телефон</div>
                                        <div className={cl.rectangle_4} />
                                    </div>
                                </div>
                                <div className={cl.group_34}>
                                    <div className={cl.overlap_14}>
                                        <div className={cl.text_wrapper_60}>valeria_ivanivna@valeria.com</div>
                                        <div className={cl.text_wrapper_61}>E-Mail</div>
                                        <div className={cl.rectangle_4} />
                                    </div>
                                </div>
                                <div className={cl.group_35}>
                                    <div className={cl.group_36}>
                                        <img
                                            className={cl.img_4_minus}
                                            alt="Minus svgrepo com"
                                        />
                                        <div className={cl.text_wrapper_56}>Мінуси</div>
                                    </div>
                                </div>
                                <div className={cl.group_37}>
                                    <div className={cl.text_wrapper_62}>Загальне враження про товар</div>
                                    <img
                                        className={cl.group_38}
                                        alt="StarConcl"
                                    />
                                </div>
                                <button className={cl.frame_wrapper}>
                                    <div className={cl.frame_2}>
                                        <div className={cl.text_wrapper_16}>Залишити відгук</div>

                                    </div>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div >
    );
}
export default ModelPage;

