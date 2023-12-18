import React from "react";
import cl from './.module.css';
import { CatalogAPI } from "../../services/api/CatalogAPI";
import LoadingAnimation from "../LoadingAnimation/LoadingAnimation";
import { ReviewModelWindowContext } from "../../ContextProvider";
import { AdministrateUsersAPI } from "../../services/api/AdministrateUsersAPI";
import { UserAPI } from "../../services/api/UserAPI";

const useRefDimensions = (ref) => {
    const [dimensions, setDimensions] = React.useState({ width: 1, height: 2 });

    function WindowResizeEvent() {
        if (ref.current) {
            const boundingRect = ref.current.getBoundingClientRect();
            setDimensions({ width: boundingRect.width, height: boundingRect.height });
        }
    }

    React.useEffect(() => {
        if (ref.current) {
            const boundingRect = ref.current.getBoundingClientRect();
            setDimensions({ width: boundingRect.width, height: boundingRect.height });
        }

        window.addEventListener('resize', WindowResizeEvent);

        return () => {
            window.removeEventListener('resize', WindowResizeEvent);
        };
    }, [ref]);

    return dimensions;
}

const ReviewModelWindow = ({ visible = false, reviewingModelId = null, onAccept = null, onDeny = null, onBlock = null, onClose = null }) => {
    const { reviewModelWindowInfo, setReviewModelWindowInfo } = React.useContext(ReviewModelWindowContext);

    const [isModelDataLoading, setModelDataLoading] = React.useState(false);
    const [isAccepting, setAccepting] = React.useState(false);
    const [isDenying, setDenying] = React.useState(false);
    const [isBlocking, setBlocking] = React.useState(false);

    const [modelInfo, setModelInfo] = React.useState(null);
    const [modelFiles, setModelFiles] = React.useState({ preview: null, print: null, image: null });

    const modalWindowRef = React.useRef();

    const modalWindowDimensions = useRefDimensions(modalWindowRef);

    function ConvertDateTime(str) {
        let [date, time] = str.split('.')[0].split('T');
        date = date.split('-').reverse().join('.');
        return `${date} ${time}`;
    }

    async function AcceptRequest() {
        setAccepting(true);

        let res = await CatalogAPI.acceptModel(reviewingModelId, true, `Model "${modelInfo.name}" is accepted!`);

        if (res.ok) {
            alert(`Model "${modelInfo.name}" is accepted!`);
            setReviewModelWindowInfo({ visible: false, modelId: null });
            onAccept();
        }

        setAccepting(false);
    }

    async function DenyRequest() {
        setDenying(true);

        let res = await CatalogAPI.acceptModel(reviewingModelId, false, `Model "${modelInfo.name}" is denied!`);

        if (res.ok) {
            alert(`Model "${modelInfo.name}" is denied!`);
            setReviewModelWindowInfo({ visible: false, modelId: null });
            onDeny();
        }

        setDenying(false);
    }

    async function BlockRequest() {
        /*setBlocking(true);

        let userId = (await (await UserAPI.getUserInfo(modelInfo.owner)).json()).data.id;
        let res1 = await AdministrateUsersAPI.blocked(userId);
        let res2 = await CatalogAPI.acceptModel(reviewingModelId, false, `Model "${modelInfo.name}" is denied!`);

        if (res1.ok && res2.ok) {
            alert('User is blocked');
            setReviewModelWindowInfo({ visible: false, modelId: null });
            onBlock();
        }

        setBlocking(false);*/
    }

    async function LoadModelData() {
        setModelDataLoading(true);

        let mi = await (await CatalogAPI.getModel(reviewingModelId)).json();
        let preview = await CatalogAPI.getModelPreview(reviewingModelId);
        let picture = URL.createObjectURL(await (await CatalogAPI.getModelPicture(mi.data.picturesIDs[0])).blob());

        setModelInfo(mi.data);
        setModelFiles({ preview: preview, print: null, image: picture });

        setModelDataLoading(false);
    }

    React.useEffect(() => {
        if (reviewingModelId !== null && reviewingModelId !== modelInfo?.id) {
            LoadModelData();
        }
    });

    if (!visible) {
        return;
    }

    if (isModelDataLoading || modelInfo === null) {
        return (
            <div className={cl.model_review_window_background}
                onScroll={(e) => e.stopPropagation()}>
                <div className={cl.model_review_window_loading}>
                    <LoadingAnimation size="100px" loadingCurveWidth="20px" />
                </div>
            </div>
        );
    }

    return (
        <div className={cl.model_review_window_background}>
            <div className={`${cl.model_review_window} ${modalWindowDimensions.height > window.innerHeight ? cl.model_review_window_fixed : ''}`} ref={modalWindowRef}>
                <img className={cl.model_review_window_cancel_sign}
                    alt="cancel"
                    onClick={() => {
                        setReviewModelWindowInfo({ visible: false, modelId: null });
                        onClose();
                    }} />
                <div className={cl.model_review_window_top}>
                    <h2 className={cl.model_review_window_header}>Затвердження моделі</h2>
                    <p className={cl.model_review_window_instruction}>
                        Затвердіть або відхіліть публікування моделі до каталогу, мяв
                    </p>
                </div>
                <div className={cl.model_review_window_model_info}>
                    <div className={cl.model_review_window_model_files}>
                        <img className={cl.model_review_window_model_image} alt="model" src={modelFiles.image} />
                        <div className={cl.model_review_window_open_preview_file_cont}>
                            <p className={cl.model_review_window_open_preview_file_label}>Файл для перегляду</p>
                            <div className={cl.model_review_window_open_file_button}>
                                <img className={cl.model_review_window_open_file_button_img} alt="open file" />
                                <span className={cl.model_review_window_open_file_button_text}>Відкрити</span>
                            </div>
                            <p className={cl.model_review_window_open_preview_file_label}>Файл для друку</p>
                            <div className={cl.model_review_window_open_file_button}>
                                <img className={cl.model_review_window_open_file_button_img} alt="open file" />
                                <span className={cl.model_review_window_open_file_button_text}>Відкрити</span>
                            </div>
                        </div>
                    </div>
                    <div className={cl.model_review_window_model_general_info}>
                        <div className={cl.model_review_window_model_info_field_set_1}>
                            <div className={`${cl.model_review_window_info_field_cont} ${cl.model_review_window_info_field_cont_login}`}>
                                <span className={cl.model_review_window_info_field_header}>Користувач</span>
                                <input className={cl.model_review_window_info_field}
                                    value={modelInfo.owner} readOnly />
                            </div>
                            <div className={`${cl.model_review_window_info_field_cont} ${cl.model_review_window_info_field_cont_price}`}>
                                <span className={cl.model_review_window_info_field_header}>Мінімальна ціна</span>
                                <input className={cl.model_review_window_info_field}
                                    value={`${modelInfo.minPrice} ₴`} readOnly />
                            </div>
                            <div className={`${cl.model_review_window_info_field_cont} ${cl.model_review_window_info_field_cont_uploaded}`}>
                                <span className={cl.model_review_window_info_field_header}>Дата</span>
                                <input className={cl.model_review_window_info_field}
                                    value={ConvertDateTime(modelInfo.uploaded)} readOnly />
                            </div>
                        </div>
                        <div className={`${cl.model_review_window_info_field_cont} ${cl.model_review_window_info_field_cont_name}`}>
                            <span className={cl.model_review_window_info_field_header}>Назва моделі</span>
                            <input className={cl.model_review_window_info_field}
                                value={modelInfo.name} readOnly />
                        </div>
                        <div className={cl.model_review_window_model_info_field_set_2}>
                            <div className={`${cl.model_review_window_info_field_cont} ${cl.model_review_window_info_field_cont_tags}`}>
                                <span className={cl.model_review_window_info_field_header}>Теги</span>
                                <div className={cl.model_review_window_info_field}>
                                    {modelInfo.keywords.join(', ')}
                                </div>
                            </div>
                            <div className={`${cl.model_review_window_info_field_cont} ${cl.model_review_window_info_field_cont_categories}`}>
                                <span className={cl.model_review_window_info_field_header}>Категорії</span>
                                <div className={cl.model_review_window_info_field}>
                                    {modelInfo.categories.map(p => p.name).join(', ')}
                                </div>
                            </div>
                        </div>
                        <div className={`${cl.model_review_window_info_field_cont} ${cl.model_review_window_info_field_cont_description}`}>
                            <span className={cl.model_review_window_info_field_header}>Опис моделі</span>
                            <textarea className={cl.model_review_window_info_field_description}
                                rows='7' value={modelInfo.description} readOnly />
                        </div>
                    </div>
                </div>
                <div className={cl.model_review_window_control}>
                    <div className={cl.model_review_window_accept_button} onClick={() => AcceptRequest()}>
                        <img className={cl.model_review_window_accept_button_img} alt="accept" />
                        <span className={cl.model_review_window_accept_button_text}>Затвердити</span>
                    </div>
                    <div className={cl.model_review_window_deny_button} onClick={() => DenyRequest()}>
                        <img className={cl.model_review_window_deny_button_img} alt="deny" />
                        <span className={cl.model_review_window_deny_button_text}>Відхилити</span>
                    </div>
                    <div className={cl.model_review_window_block_button} onClick={() => BlockRequest()}>
                        <img className={cl.model_review_window_block_button_img} alt="block" />
                        <span className={cl.model_review_window_block_button_text}>Заблокувати користувача</span>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ReviewModelWindow;