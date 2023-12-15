import React from "react";
import cl from './.module.css';
import LoadingAnimation from "../../components/LoadingAnimation/LoadingAnimation";
import { CatalogAPI } from "../../services/api/CatalogAPI";
import { UploadModelWindowContext } from "../../ContextProvider";

const MyPublicationsPage = () => {
    const { setUploadModelWindowInfo, setUploadModelWindowEvents } = React.useContext(UploadModelWindowContext);

    const [areModalWindowEventsLoaded, setAreModalWindowEventsLoaded] = React.useState(false);

    const [modelList, setModelList] = React.useState({ list: null, currentPage: 1, pageCount: 0 });
    const [isModelListLoading, setModelListLoading] = React.useState(false);

    const [deleteModelWindowInfo, setDeleteModelWindowInfo] = React.useState({ visible: false, modelId: null });
    const [isModelDeleting, setModelDeleting] = React.useState(false);

    async function DeleteModelRequest() {
        setModelDeleting(true);

        await CatalogAPI.deleteModel(deleteModelWindowInfo.modelId);
        window.location.reload();

        setModelDeleting(false);
    }

    async function LoadModelList(pageNumber = 1) {
        setModelListLoading(true);
        let json = await (await CatalogAPI.getOwnModels(pageNumber)).json();
        setModelList({ list: json.data.items, currentPage: json.data.pageIndex, pageCount: json.data.pageCount });
        setModelListLoading(false);
    }

    function RenderModelList() {
        let result = [];

        if (modelList.currentPage === 1) {
            result.push(
                <div className={cl.model_uploader}
                    key="uploader"
                    onClick={() => setUploadModelWindowInfo({ visible: true, modelId: null })}>
                    <img className={cl.model_uploader_img} alt="upload" />
                </div>
            );
        }

        modelList.list?.forEach((el) => {
            result.push(
                <div className={cl.model} key={el.id}>
                    <div className={cl.model_options}>
                        <div className={cl.delete_model_button}
                            onClick={() => setDeleteModelWindowInfo({ visible: true, modelId: el.id })}>
                            <img className={cl.delete_model_button_img} alt="delete" />
                        </div>
                    </div>
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
                        <div className={cl.edit_model_button}
                            onClick={(e) => {
                                e.stopPropagation();
                                setUploadModelWindowInfo({ visible: true, modelId: el.id });
                            }}>
                            <img className={cl.edit_model_button_img} alt="edit model" />
                            <span className={cl.edit_model_button_text}>Змінити</span>
                        </div>
                    </div>
                </div>
            );
        });

        return result;
    }

    function RenderPageNavigator() {
        if (modelList.pageCount <= 1) {
            return;
        }

        const loadPage = (idx) => {
            if (idx !== modelList.currentPage) {
                window.scroll({ top: 0 });
                LoadModelList(idx);
            }
        };

        let result = [];
        let left = [];
        let right = [];
        let start, end;

        if (modelList.currentPage > 1) {
            left.push(
                <span className={cl.page_navigator_obj} key="arrow-left"
                    onClick={() => loadPage(modelList.currentPage - 1)}>&#60;</span>
            );

            if ((modelList.currentPage - 4) > 1) {
                left.push(
                    <span className={`${cl.page_navigator_obj}`} key="1" onClick={() => loadPage(1)}>1</span>
                );

                left.push(
                    <span className={`${cl.page_navigator_obj}`} key="dots-left"
                        onClick={() => {
                            const idxLink = (modelList.currentPage - 10) > 1
                                ? modelList.currentPage - 10
                                : 2;

                            loadPage(idxLink);
                        }}>...</span>
                );

                start = modelList.currentPage - 2;
            }
            else {
                start = 1;
            }
        }
        else {
            start = 1;
        }

        if (modelList.currentPage < modelList.pageCount) {
            right.unshift(
                <span className={cl.page_navigator_obj} key="arrow-right"
                    onClick={() => loadPage(modelList.currentPage + 1)}>&#62;</span>
            );

            if ((modelList.currentPage + 4) < modelList.pageCount) {
                right.unshift(
                    <span className={`${cl.page_navigator_obj}`} key={modelList.pageCount}
                        onClick={() => loadPage(modelList.pageCount)}>{modelList.pageCount}</span>
                );

                right.unshift(
                    <span className={`${cl.page_navigator_obj}`} key="dots-right"
                        onClick={() => {
                            const idxLink = (modelList.currentPage + 10) < modelList.pageCount
                                ? modelList.currentPage + 10
                                : modelList.pageCount - 1;

                            loadPage(idxLink);
                        }}>...</span>
                );

                end = modelList.currentPage + 2;
            }
            else {
                end = modelList.pageCount;
            }
        }
        else {
            end = modelList.pageCount;
        }

        if ((end - start) < 4) {
            while ((end - start) < 4 && end < modelList.pageCount) {
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
                    className={`${cl.page_navigator_obj} ${modelList.currentPage === i ? cl.current_page_navigator_obj : ''}`}
                    key={i}
                    onClick={() => loadPage(i)}>{i}</span>
            );
        }

        result.unshift(left);
        result.push(right);

        return result;
    }

    React.useEffect(() => {
        if (modelList.list === null) {
            LoadModelList();
        }

        if (!areModalWindowEventsLoaded) {
            setUploadModelWindowEvents({ onUpload: () => { window.location.reload(); }, onClose: () => { } });
            setAreModalWindowEventsLoaded(true);
        }
    });

    return (
        <div className={cl.main}>
            <div className={cl.content}>
                <h1 className={cl.page_header}>Мої публікації</h1>
                {isModelListLoading ?
                    <div className={cl.model_list_loading}>
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
            <div className={cl.delete_model_window_background}
                style={{ display: deleteModelWindowInfo.visible ? 'block' : 'none' }}>
                <div className={cl.delete_model_window}>
                    <p className={cl.delete_model_window_description}>
                        Ви впевнені, що хочете видалити Вашу модель з каталогу?
                    </p>
                    <div className={cl.delete_model_window_control}>
                        <div className={cl.delete_model_window_cancel_button}
                            onClick={() => setDeleteModelWindowInfo({ visible: false, modelId: null })}>
                            <span className={cl.delete_model_window_cancel_button_text}>Скасувати</span>
                        </div>
                        <div className={cl.delete_model_window_delete_button}
                            onClick={() => DeleteModelRequest()}>
                            {isModelDeleting
                                ? <LoadingAnimation size="30px" loadingCurveWidth="6px" />
                                : <span className={cl.delete_model_window_delete_button_text}>Видалити</span>
                            }
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default MyPublicationsPage;