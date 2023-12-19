import React from 'react';
import cl from "./.module.css";
import { CatalogAPI } from '../../services/api/CatalogAPI';
import { useParams } from "react-router-dom";
import LoadingAnimation from '../../components/LoadingAnimation/LoadingAnimation';
import { UserAPI } from '../../services/api/UserAPI';
import { CatalogModelFeedbackAPI } from '../../services/api/CatalogModelFeedbackAPI';
import { OrderAPI } from '../../services/api/OrderAPI';


import StarRating from './StarRating';
import { CartAPI } from '../../services/api/CartAPI';

const ModelPage = () => {
    const { id } = useParams();
    const [modelInfo, setModelInfo] = React.useState(undefined);
    const [modelPicture, setModelPicture] = React.useState(undefined);
    const [userInfo, setUserInfo] = React.useState(undefined);
    const [feedbacks, setFeedbacks] = React.useState(undefined);
    const [imageClass, setImageClass] = cl.edit_image;
    const [orders, setOrders] = React.useState(undefined);
    const [isBought, setIsBought] = React.useState(undefined);
    const [orderedModelId, setOrderedModelId] = React.useState(undefined);

    const [feedbackText, setFeedbackText] = React.useState(undefined);

    const [feedbackPros, setFeedbackPros] = React.useState(undefined);

    const [feedbackCons, setFeedbackCons] = React.useState(undefined);

    const [productRating, setProductRating] = React.useState(0);
    function getStyleClass(rate) {
        if (rate >= 0 && rate < 1) {
            return cl.group_0_5_star;
        }
        else if (rate >= 1 && rate < 2) {
            return cl.group_1_star;
        } else if (rate >= 2 && rate < 3) {
            return cl.group_2_star;
        } else if (rate >= 3 && rate < 4) {
            return cl.group_3_star;
        } else if (rate >= 4 && rate < 5) {
            return cl.group_4_star;
        } else if (rate >= 55) {
            return cl.group_5_star;
        } else {
            return cl.group_12;
        }
    }
    function getImageClass() {
        if (userInfo != undefined) {
            //console.log(userInfo);
            if (userInfo.login == modelInfo.owner) {
                return cl.edit_image;
            }
        }

        return cl.edit_image_invisible;
    }
    function addItemToCart() {
        CartAPI.addItem(1, 1, 5, 1, 1, "FDM", `ABS`);
       // alert('FEWf')
    }
    const handleRatingChange = (newRating) => {
        // Оновлюємо стан productRating при зміні рейтингу
        setProductRating(newRating);
    };
    React.useEffect(() => {
        let isMounted = true;
        const fetchData = async () => {
            if (modelInfo === undefined && isMounted) {
                try {
                    const response = await CatalogAPI.getModel(id);

                    console.log(response.status);
                    if (response.ok) {
                        const resModel = await response.json();
                        if (isMounted) {
                            setModelInfo(resModel.data);
                        }
                        setModelPicture(URL.createObjectURL(await (await CatalogAPI.getModelPicture(resModel.data.picturesIDs[0])).blob()));
                    }
                    else {
                        console.error('Помилка отримання моделі:', response.statusText);
                    }
                } catch (error) {
                    console.error('Помилка отримання моделі:', error);
                }
            }
            if (feedbacks === undefined) {
                try {
                    const response = await CatalogModelFeedbackAPI.getFeedback(id);

                    console.log(response.status);
                    if (response.ok) {
                        const resModel = await response.json();
                        for (let i = 0; i < resModel.data.items.length; i++) {
                            const response = await UserAPI.getUserAvatar(resModel.data.items[i].userLogin);
                            const blob = await response.blob();
                            resModel.data.items[i].avatar = URL.createObjectURL(blob);
                        };
                        //console.log(resModel.data.items);
                        if (isMounted) {
                            setFeedbacks(resModel.data.items);
                        }
                        //setModelPicture(URL.createObjectURL(await (await CatalogAPI.getModelPicture(resModel.data.picturesIDs[0])).blob()));
                    }
                    else {
                        console.error('Помилка отримання моделі:', response.statusText);
                    }
                } catch (error) {
                    console.error('Помилка отримання моделі:', error);
                }
            }
            if (userInfo === undefined) {
                try {
                    const response = await UserAPI.getSelfInfo();
                    if (response.ok) {
                        const resModel = await response.json();
                        //console.log(resModel.data);
                        setUserInfo(resModel.data);
                        console.log(userInfo);
                        if (userInfo.data.login == modelInfo.owner) {
                            console.error("Your model");
                            setImageClass(cl.edit_image);
                        }
                    }
                }
                catch (error) {
                    console.error('Помилка отримання інформації про користувача:', error);
                }
            }
            if (orders === undefined) {
                try {
                    const response = await OrderAPI.getHistory();
                    if (response.ok) {
                        const resModel = await response.json();
                        console.log(resModel.data);
                        resModel.data.forEach((item, index) => {
                            // Check if the current item has a 'models' property
                            if (item.models) {
                                const modelsArray = item.models;
                                modelsArray.forEach(model => {
                                    console.log(`id: ${model.id}, catalogModelId: ${model.catalogModelId}`);
                                    if (model.id == id) {
                                        console.log(" model was bought");
                                        setIsBought(true);
                                        console.log(model.catalogModelId);
                                        setOrderedModelId(model.catalogModelId);
                                    }
                                });

                                console.log("OrderedModelId = " + orderedModelId);
                            } else {
                                console.log(`Element ${index + 1} does not have a 'models' property.`);
                            }
                        });
                        setOrders(resModel.data);
                        console.log(orders);

                    }
                }
                catch (error) {
                    console.error('Помилка отримання інформації про замовлення:', error);
                }
            }
        };

        fetchData();

        return () => {
            isMounted = false;
        };
    }, [modelInfo, id, orderedModelId]);
    function PutFeedback() {
        //alert(productRating);
        //alert(feedbackText);
        //alert(feedbackPros+" pros");
        //alert(feedbackCons);
        CatalogModelFeedbackAPI.putFeedback(id, orderedModelId,
            productRating, feedbackText, feedbackPros, feedbackCons);
        //console.log("OrderedModelId = " + orderedModelId);
        window.location.reload(); // або window.location.href = window.location.href;
    }

    const handleTextChange = (event) => {
        setFeedbackText(event.target.value);
    };
    const handleProsChange = (event) => {
        setFeedbackPros(event.target.value);
    };
    const handleConsChange = (event) => {
        setFeedbackCons(event.target.value);
    };
    function renderFeedbackList() {
        if (feedbacks != undefined) {

            return (
                <div>
                    {feedbacks.map((feedback) => (
                        <div key={feedback.id} className={cl.group_18}>
                            <div className={cl.group_19}>
                                <img
                                    className={cl.avatar_svgrepo_com_3}
                                    alt="Avatar svgrepo com"
                                    src={`/api/user/${feedback.userLogin}/avatar`}
                                />
                                <img
                                    className={getStyleClass(feedback.rate)}
                                    alt="Stars"
                                    style={{ top: '18px', left: '35%' }}
                                />
                                <p className={cl.text_wrapper_47}>{feedback.userLogin}</p>
                                <div className={cl.text_wrapper_49}>{feedback.createdAt.substring(0, feedback.createdAt.indexOf('T'))}</div>
                            </div>
                            <div className={cl.group_20}>
                                <img
                                    className={cl.img_3}
                                    alt="Plus svgrepo com"
                                    src={feedback.plusIcon}
                                />

                                <p className={cl.text_wrapper_51}>{feedback.pros}</p>
                            </div>
                            <div className={cl.group_20}>
                                <img
                                    className={cl.img_minus}
                                    alt="Plus svgrepo com"
                                />

                                <p className={cl.text_wrapper_50}>{feedback.cons}</p>

                            </div>
                            <p className={cl.text_wrapper_48}>{feedback.text}</p>
                        </div>
                    ))}
                </div>
            );
        }

    }
    if (modelInfo === undefined) {
        return (
            <div className={cl.unloaded_page}>
                <LoadingAnimation size="100px" loadingCurveWidth="20px" />
            </div>
        );
    }
    function RenderFeedbackZona() {
        return (
            <div className={cl.overlap_7} >
                <img
                    className={cl.avatar_svgrepo_com_2}
                    alt="Avatar svgrepo com"
                    src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65477cb487304b74da313e8b/img/avatar-svgrepo-com-2.svg"
                />
                <div className={cl.view_16}>
                    <div className={cl.overlap_8} id='feedback'>
                        <div id="feedbacks" className={cl.text_wrapper_31}>Відгуки</div>
                        {renderFeedbackList()};
                        <div className={cl.text_wrapper_54}>Залишити відгук</div>

                        <div className={cl.group_28} style={{ display: isBought ? 'block' : 'none' }}>
                            <input
                                className={cl.overlap_12}
                                type="text"
                                name="feedbackText"
                                placeholder="Відгук"
                                value={feedbackText}
                                onChange={handleTextChange}
                            />
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
                            <input
                                className={cl.group_wrapper}
                                type="text"
                                name="feedbackText" // Додайте обов'язковий атрибут name
                                placeholder="Мінуси"
                                value={feedbackCons}
                                onChange={handleConsChange}
                            />
                            <div className={cl.group_31}>
                                <div className={cl.text_wrapper_56}>Плюси</div>
                                <img
                                    className={cl.img_4}
                                    alt="Plus svgrepo com"
                                />
                            </div>
                            <div className={cl.group_32}>
                                <div className={cl.overlap_13}>
                                    <div className={cl.text_wrapper_57}>Ім’я</div>
                                    <div className={cl.text_wrapper_58}>{userInfo?.login}</div>
                                </div>
                            </div>
                            <input
                                className={cl.group_35}
                                type="text"
                                name="feedbackPros" // Додайте обов'язковий атрибут name
                                placeholder="Плюси"
                                value={feedbackPros}
                                onChange={handleProsChange}
                            // Інші необхідні атрибути
                            />
                            <div className={cl.group_36}>
                                <img
                                    className={cl.img_4_minus}
                                    alt="Minus svgrepo com"
                                />
                                <div className={cl.text_wrapper_56}>Мінуси</div>
                            </div>
                            <div className={cl.group_37}>
                                <div className={cl.text_wrapper_62}>Загальне враження про товар</div>
                                <StarRating rating={productRating} onRatingChange={handleRatingChange} />
                            </div>
                            <button className={cl.frame_wrapper} onClick={PutFeedback}>
                                <div className={cl.frame_2}>
                                    <div className={cl.text_wrapper_16}>Залишити відгук</div>
                                </div>
                            </button>
                        </div>
                    </div>
                </div>
            </div>);
    }
    function RenderFreeFeedbackZona() {
        return (
            <div className={cl.custom_height} >
                <div id="feedbacks" className={cl.text_wrapper_31}>Відгуки</div></div>
        )
    }
    return (
        <div className={cl.index}>
            <div className={cl.div_2}>
                <div className={cl.view_5}>
                    <div className={cl.navbar}>
                        <span className={cl.text_wrapper_12}
                            onClick={() => {
                                document.getElementById('general-model-info').scrollIntoView({ behavior: "smooth" });
                            }}>
                            ВСЕ ПРО ТОВАР
                        </span>
                        <span className={cl.text_wrapper_12}
                            onClick={() => {
                                document.getElementById('model-description').scrollIntoView({ behavior: "smooth" });
                            }}>
                            ОПИС</span>
                        <span className={cl.text_wrapper_12}
                            onClick={() => {
                                document.getElementById('feedback').scrollIntoView({ behavior: "smooth" });
                            }}>
                            ВІДГУКИ</span>
                    </div>
                </div>
                <div className={cl.overlap_3}>
                    <div className={cl.view_6} id='general-model-info'>
                        {modelPicture !== undefined ?
                            <img
                                className={cl.image_removebg}
                                alt="Image removebg"
                                src={modelPicture}
                            />
                            :
                            <div className={cl.unloaded_model_picture}>
                                <LoadingAnimation size="100px" loadingCurveWidth="20px" />
                            </div>
                        }
                        <div id="All" className={cl.text_wrapper_13}>{modelInfo?.name}</div>
                        <div className={cl.text_wrapper_14}>Код товару: {modelInfo?.id}</div>

                        <div className={cl.group_3}>
                            <div className={cl.overlap_group_3}>
                                <div className={cl.text_wrapper_15}>{modelInfo?.depth} ₴</div>
                                <button className={cl.frame} onClick={addItemToCart}>
                                    <img
                                        className={cl.img_bag}
                                        alt="Cart large"
                                    />
                                    <div className={cl.text_wrapper_16}>Додати модель до кошика</div>
                                </button>
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
                            {modelInfo?.keywords?.map((keyword, index) => (
                                <button className={cl.keywords_buttons} key={index}>{keyword}</button>
                            ))}
                        </div>
                        <div className={cl.categories_group}>
                            {modelInfo?.categories?.map((category, index) => (
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

                        <img
                            className={getStyleClass(modelInfo?.rating)}
                            alt="Stars"
                        />
                        <div className={cl.group_13}>

                            <img className={cl.box_image}></img>
                            <img className={getImageClass()}></img>

                        </div>
                        <div className={cl.text_wrapper_28}>{feedbacks?.length} відгуків</div>
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
                </div>
                <div className={cl.view_14}>
                    <div className={cl.overlap_5} id='model-description'>
                        <div id="description" className={cl.text_wrapper_31}>Опис</div>
                        <p className={cl.text_wrapper_32}>
                            {modelInfo?.description}
                        </p>
                    </div>
                </div>
                <div>
                    {feedbacks === undefined && !isBought ? RenderFreeFeedbackZona() : RenderFeedbackZona()}
                </div>
            </div>
        </div >
    );
}
export default ModelPage;
