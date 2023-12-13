import React from "react";
import cl from './.module.css';
import uploadFileImg from './img/upload-file.png';
import fileUploadedImg from './img/file-uploaded.png';
import { CatalogAPI } from "../../services/api/CatalogAPI";
import LoadingAnimation from "../LoadingAnimation/LoadingAnimation";
import { UploadModelWindowContext } from "../../ContextProvider";

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

const UploadModelWindow = ({ visible = false, editingModelId = null, onUpload = null, onClose = null }) => {
    const { uploadModelWindowInfo, setUploadModelWindowInfo } = React.useContext(UploadModelWindowContext);

    const [isModelDataLoading, setModelDataLoading] = React.useState(false);
    const [isTagListLoading, setTagListLoading] = React.useState(false);
    const [isSaving, setSaving] = React.useState(false);

    const [currentModelId, setCurrentModelId] = React.useState(null);

    const [categoryList, setCategoryList] = React.useState(null);
    const [tagList, setTagList] = React.useState(null);

    const [modelPreviewFile, setModelPreviewFile] = React.useState({ file: null, loading: false });
    const [modelPrintFile, setModelPrintFile] = React.useState({ file: null, loading: false });
    const [modelImageFile, setModelImageFile] = React.useState({ file: null, loading: false });

    const [isModelPreviewFileChanged, setModelPreviewFileChanged] = React.useState(false);
    const [isModelPrintFileChanged, setModelPrintFileChanged] = React.useState(false);
    const [isModelImageFileChanged, setModelImageFileChanged] = React.useState(false);

    const [categoriesForOwnModel, setCategoriesForOwnModel] = React.useState([]);
    const [tagsForOwnModel, setTagsForOwnModel] = React.useState([]);

    const [modelNameInputValue, setModelNameInputValue] = React.useState("");
    const [modelDescriptionInputValue, setModelDescriptionInputValue] = React.useState("");
    const [modelDepthInputValue, setModelDepthInputValue] = React.useState("");
    const [tagNameInputValue, setTagNameInputValue] = React.useState("");

    const modalWindowRef = React.useRef();

    const modelNameInputRef = React.useRef();
    const modelDescriptionInputRef = React.useRef();
    const modelDepthInputRef = React.useRef();
    const tagNameInputRef = React.useRef();

    const modelPreviewFileInputRef = React.useRef();
    const modelPrintFileInputRef = React.useRef();
    const modelImageFileInputRef = React.useRef();

    const modalWindowDimensions = useRefDimensions(modalWindowRef);

    async function UploadModelRequest() {
        setSaving(true);

        let formData = new FormData();

        formData.append('Name', modelNameInputRef.current.value);
        formData.append('Description', modelDescriptionInputRef.current.value);
        formData.append('Depth', modelDepthInputRef.current.value);

        categoriesForOwnModel.map(el => el.id).forEach((e) => {
            formData.append('Categories', e);
        });

        tagsForOwnModel.forEach((e) => {
            formData.append('Keywords', e);
        });

        formData.append('files', modelPreviewFile.file);
        formData.append('files', modelPrintFile.file);
        formData.append('files', modelImageFile.file);

        let json = await (await CatalogAPI.addNewModel(formData)).json();

        if (json.success) {
            alert('Your model is uploaded!');
            setUploadModelWindowInfo({ visible: false, modelId: null });
            onUpload();
            return;
        }

        if (json.message) {
            alert(json.message);
        }
        else {
            alert("Invalid data!");
        }

        setSaving(false);
    }

    async function EditModelRequest() {
        setSaving(true);

        let formDataInfo = new FormData();

        formDataInfo.append('Name', modelNameInputRef.current.value);
        formDataInfo.append('Description', modelDescriptionInputRef.current.value);
        formDataInfo.append('Depth', modelDepthInputRef.current.value);

        categoriesForOwnModel.map(el => el.id).forEach((e) => {
            formDataInfo.append('Categories', e);
        });

        tagsForOwnModel.forEach((e) => {
            formDataInfo.append('Keywords', e);
        });

        let jsonInfo = await (await CatalogAPI.updateModelInfo(editingModelId, formDataInfo)).json();

        if (jsonInfo.success === false) {
            alert(jsonInfo.message);
            setSaving(false);
            return;
        }

        if (!jsonInfo.success) {
            alert("Info is not saved!");
            setSaving(false);
            return;
        }

        if (isModelPreviewFileChanged) {
            let formDataPreviewFile = new FormData();
            formDataPreviewFile.append('file', modelPreviewFile.file);

            let jsonFile = await (await CatalogAPI.updatePreviewFile(editingModelId, formDataPreviewFile)).json();

            if (jsonFile.success === false) {
                alert(jsonInfo.message);
                setSaving(false);
                return;
            }

            if (!jsonInfo.success) {
                alert("Preview is not saved!");
                setSaving(false);
                return;
            }
        }

        if (isModelPrintFileChanged) {
            let formDataPrintFile = new FormData();
            formDataPrintFile.append('file', modelPrintFile.file);

            let jsonFile = await (await CatalogAPI.updatePrintFile(editingModelId, formDataPrintFile)).json();

            if (jsonFile.success === false) {
                alert(jsonInfo.message);
                setSaving(false);
                return;
            }

            if (!jsonInfo.success) {
                alert("Print is not saved!");
                setSaving(false);
                return;
            }
        }

        if (isModelImageFileChanged) {
            let formDataImageFile = new FormData();
            formDataImageFile.append('files', modelImageFile.file);

            let jsonFile = await (await CatalogAPI.updatePictureFiles(editingModelId, formDataImageFile)).json();

            if (jsonFile.success === false) {
                alert(jsonInfo.message);
                setSaving(false);
                return;
            }

            if (!jsonInfo.success) {
                alert("Image is not saved!");
                setSaving(false);
                return;
            }
        }

        alert('Model data is updated!');
        setUploadModelWindowInfo({ visible: false, modelId: null });
        onUpload();

        setSaving(false);
    }

    async function LoadModelData() {
        setModelDataLoading(true);

        let json = await (await CatalogAPI.getModel(editingModelId)).json();
        let preview = await CatalogAPI.getModelPreview(json.data.id);
        let print = await CatalogAPI.getPrintFile(json.data.id);
        let picture = URL.createObjectURL(await (await CatalogAPI.getModelPicture(json.data.picturesIDs[0])).blob());

        setCurrentModelId(json.data.id);
        setModelNameInputValue(json.data.name);
        setModelPreviewFile({ file: preview, loading: false });
        setModelPrintFile({ file: print, loading: false });
        setModelImageFile({ file: picture, loading: false });
        setModelDescriptionInputValue(json.data.description);
        setCategoriesForOwnModel(json.data.categories);
        setTagsForOwnModel(json.data.keywords);
        setModelDepthInputValue(json.data.depth);

        setModelDataLoading(false);
    }

    async function LoadCategoryList() {
        setCategoryList((await (await CatalogAPI.getCategories()).json()).data);
    }

    async function LoadTagList() {
        setTagListLoading(true);
        setTagList((await (await CatalogAPI.searchKeywords(tagNameInputRef.current.value)).json()).data);
        setTagListLoading(false);
    }

    function RenderSelectedCategories() {
        let result = [];

        categoriesForOwnModel.forEach((el) => {
            result.push(
                <div className={cl.model_upload_window_category} key={el.id}>
                    <div className={cl.model_upload_window_category_name_cont}>
                        <span className={cl.model_upload_window_category_name}>{el.name}</span>
                    </div>
                    <div className={cl.model_upload_window_category_img_cont}>
                        <img className={cl.model_upload_window_category_img}
                            alt="add"
                            onClick={(e) => {
                                e.stopPropagation();
                                setCategoriesForOwnModel(p => p.filter(e => e.id !== el.id))
                            }} />
                    </div>
                </div>
            )
        });

        result.push(
            <div className={cl.model_upload_window_category_adder} key="adder"
                onClick={() => {
                    let categoryList = document.getElementsByClassName(cl.model_upload_window_model_all_categories)[0];
                    categoryList.style.display = categoryList.style.display === 'flex' ? 'none' : 'flex';
                }}>
                <img className={cl.model_upload_window_category_adder_img} alt="add" />
            </div>
        );

        return result;
    }

    function RenderAllCategories() {
        if (categoryList === null) {
            return (
                <div className={cl.model_upload_window_category_list_loading_cont}>
                    <div className={cl.model_upload_window_category_list_loading}>
                        <LoadingAnimation size="25px" loadingCurveWidth="5px" />
                    </div>
                </div>
            );
        }

        let result = [];

        categoryList.forEach((el) => {
            result.push(
                <div className={cl.model_upload_window_category} key={el.id}
                    onClick={() => {
                        if (!categoriesForOwnModel.map(el => el.id).includes(el.id) && categoriesForOwnModel.length < 3) {
                            setCategoriesForOwnModel(p => [...p, { id: el.id, name: el.name }]);
                        }
                    }}>
                    <div className={cl.model_upload_window_category_name_cont}>
                        <span className={cl.model_upload_window_category_name}>{el.name}</span>
                    </div>
                    {categoriesForOwnModel.map(el => el.id).includes(el.id) ?
                        <div className={cl.model_upload_window_category_img_cont}>
                            <img className={cl.model_upload_window_category_img} alt="remove"
                                onClick={(e) => {
                                    e.stopPropagation();
                                    setCategoriesForOwnModel(p => p.filter(e => e.id !== el.id))
                                }} />
                        </div>
                        : <></>
                    }
                </div>
            );
        });

        return result;
    }

    function RenderSelectedTags() {
        let result = [];

        tagsForOwnModel.forEach((el) => {
            result.push(
                <div className={cl.model_upload_window_tag} key={el}>
                    <div className={cl.model_upload_window_tag_name_cont}>
                        <span className={cl.model_upload_window_tag_name}>{el}</span>
                    </div>
                    <div className={cl.model_upload_window_tag_img_cont}>
                        <img className={cl.model_upload_window_tag_img}
                            alt="add"
                            onClick={(e) => {
                                e.stopPropagation();
                                setTagsForOwnModel(p => p.filter(e => e !== el))
                            }} />
                    </div>
                </div>
            )
        });

        result.push(
            <div className={cl.model_upload_window_tag_adder} key="adder"
                onClick={() => {
                    let tagList = document.getElementsByClassName(cl.model_upload_window_model_edit_tag_menu)[0];
                    tagList.style.display = tagList.style.display === 'block' ? 'none' : 'block';
                }}>
                <img className={cl.model_upload_window_tag_adder_img} alt="add" />
            </div>
        );

        return result;
    }

    function RenderSearchedTags() {
        if (isTagListLoading) {
            return (
                <div className={cl.model_upload_window_tag_list_loading_cont}>
                    <div className={cl.model_upload_window_tag_list_loading}>
                        <LoadingAnimation size="25px" loadingCurveWidth="5px" />
                    </div>
                </div>
            );
        }

        let result = [];

        tagList?.forEach((el) => {
            result.push(
                <div className={cl.model_upload_window_tag} key={el.name}
                    onClick={() => {
                        setTagNameInputValue(el.name);
                    }}>
                    <span className={cl.model_upload_window_tag_name}>{el.name}</span>
                </div>
            );
        });

        return result;
    }

    function WindowClickEvent(event) {
        let categoryContainer = document.getElementsByClassName(cl.model_upload_window_model_categories_cont)[0];
        let categoryList = document.getElementsByClassName(cl.model_upload_window_model_all_categories)[0];
        let tagContainer = document.getElementsByClassName(cl.model_upload_window_model_tags_cont)[0];
        let tagList = document.getElementsByClassName(cl.model_upload_window_model_edit_tag_menu)[0];

        if (categoryList?.style.display === 'flex' && !categoryContainer?.contains(event.target)) {
            categoryList.style.display = 'none';
        }

        if (tagList?.style.display === 'block' && !tagContainer?.contains(event.target)) {
            tagList.style.display = 'none';
        }
    }

    React.useEffect(() => {
        if (categoryList === null) {
            LoadCategoryList();
        }

        if (editingModelId === null && currentModelId !== null) {
            setCurrentModelId(null);
            setModelNameInputValue("");
            setModelPreviewFile({ file: null, loading: false });
            setModelPrintFile({ file: null, loading: false });
            setModelImageFile({ file: null, loading: false });
            setModelDescriptionInputValue("");
            setCategoriesForOwnModel([]);
            setTagsForOwnModel([]);
            setModelDepthInputValue("");
        }

        if (editingModelId !== null && editingModelId !== currentModelId) {
            LoadModelData();
        }

        window.addEventListener("click", WindowClickEvent);

        return () => {
            window.removeEventListener("click", WindowClickEvent);
        };
    });

    if (!visible) {
        return;
    }

    if (isModelDataLoading) {
        return (
            <div className={cl.model_upload_window_background}
                onScroll={(e) => e.stopPropagation()}>
                <div className={cl.model_upload_window_loading}>
                    <LoadingAnimation size="100px" loadingCurveWidth="20px" />
                </div>
            </div>
        );
    }

    return (
        <div className={cl.model_upload_window_background}
            onScroll={(e) => e.stopPropagation()}>
            <div className={`${cl.model_upload_window} ${modalWindowDimensions.height > window.innerHeight ? cl.model_upload_window_fixed : ''}`} ref={modalWindowRef}>
                <img className={cl.model_upload_window_cancel_sign}
                    alt="cancel"
                    onClick={() => {
                        setUploadModelWindowInfo({ visible: false, modelId: null });
                        onClose();
                    }} />
                <div className={cl.model_upload_window_top}>
                    <h2 className={cl.model_upload_window_header}>
                        {editingModelId === null ? "Додати модель" : "Зміна інформації моделі"}
                    </h2>
                    <p className={cl.model_upload_window_instruction}>
                        {editingModelId === null ?
                            "Додайте інформацію про модель, необхідні файли, категорії та теги (необов’язково)"
                            : "Змініть модель, ваш підпис, адресу, гроші, ще одного кота та пса, бхехе"
                        }
                    </p>
                </div>
                <div className={cl.model_upload_window_panel}>
                    <div className={cl.model_upload_window_main_info}>
                        <h3 className={cl.model_upload_window_model_name_header}>Назва моделі</h3>
                        <input className={cl.model_upload_window_model_name}
                            type="text"
                            value={modelNameInputValue}
                            ref={modelNameInputRef}
                            onChange={(e) => { setModelNameInputValue(e.target.value); }} />
                        <div className={`${cl.model_upload_window_files}`}>
                            <div className={`${cl.model_upload_window_load_file_cont}`}>
                                <div className={`${cl.model_upload_window_load_file_description}`}>
                                    <h3 className={`${cl.model_upload_window_load_file_header}`}>
                                        Файл моделі для перегляду
                                    </h3>
                                    <p className={`${cl.model_upload_window_load_file_extensions}`}>
                                        (формати: STL, OBJ)
                                    </p>
                                </div>
                                <div className={`${cl.model_upload_window_load_file_button}`}>
                                    <div className={`${cl.model_upload_window_load_file}`}
                                        onClick={() => {
                                            if (modelPreviewFile.file === null) {
                                                modelPreviewFileInputRef.current.click();
                                            }
                                        }}>
                                        <input className={cl.model_upload_window_load_preview_file_input}
                                            type="file"
                                            accept=".stl, .obj"
                                            style={{ display: 'none' }}
                                            ref={modelPreviewFileInputRef}
                                            onChange={() => {
                                                let file = modelPreviewFileInputRef.current.files[0];

                                                if (!file) {
                                                    return;
                                                }

                                                let fileSpl = file.name.split('.');
                                                const fileName = fileSpl[fileSpl.length - 1];

                                                if (fileName !== 'obj' && fileName !== 'stl') {
                                                    alert('Please, select an OBJ or STL file.');
                                                    return;
                                                }

                                                let formData = new FormData();
                                                let xml = new XMLHttpRequest();

                                                setModelPreviewFile({ file: file, loading: true });
                                                formData.append('model', file);

                                                xml.onreadystatechange = () => {
                                                    if (xml.readyState === XMLHttpRequest.DONE) {
                                                        setModelPreviewFile(p => {
                                                            let newP = { ...p };
                                                            newP.loading = false;
                                                            return newP;
                                                        });
                                                        xml.abort();


                                                        if (!isModelPreviewFileChanged) {
                                                            setModelPreviewFileChanged(true);
                                                        }
                                                    }
                                                };

                                                xml.upload.addEventListener("progress", (event) => {
                                                    let progressBar = document.getElementsByClassName(cl.model_upload_window_load_file_done_progress)[0];
                                                    progressBar.style.width = `${(event.loaded / event.total) * 100}%`;
                                                }, false);

                                                xml.open("POST", "fileupload.php");
                                                xml.send(formData);
                                            }} />
                                        <div className={cl.model_upload_window_delete_file_button}
                                            style={{ display: modelPreviewFile.file && !modelPreviewFile.loading ? 'flex' : 'none' }}
                                            title="Видалити"
                                            onClick={() => {
                                                setModelPreviewFile({ file: null, loading: false });
                                                modelPreviewFileInputRef.current.value = "";
                                            }}>
                                            <img className={cl.model_upload_window_delete_file_button_img} alt="delete file" />
                                        </div>
                                        <img className={cl.model_upload_window_load_file_img} alt="load file"
                                            src={modelPreviewFile.file && !modelPreviewFile.loading ? fileUploadedImg : uploadFileImg} />
                                        <span className={cl.model_upload_window_load_file_text}>
                                            {
                                                !modelPreviewFile.file && !modelPreviewFile.loading
                                                    ? "Перетягніть файл сюди"
                                                    : modelPreviewFile.file && !modelPreviewFile.loading
                                                        ? "Файл було успішно завантажено!"
                                                        : "Завантаження фалу..."
                                            }
                                        </span>
                                        <div className={`${cl.model_upload_window_load_file_progress}`}>
                                            <div className={`${cl.model_upload_window_load_file_done_progress}`}
                                                style={{
                                                    width:
                                                        !modelPreviewFile.file && !modelPreviewFile.loading ? '0%'
                                                            : modelPreviewFile.file && !modelPreviewFile.loading ? '100%'
                                                                : undefined
                                                }} />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className={`${cl.model_upload_window_load_file_cont}`}>
                                <div className={`${cl.model_upload_window_load_file_description}`}>
                                    <h3 className={`${cl.model_upload_window_load_file_header}`}>
                                        Файл моделі для друку
                                    </h3>
                                    <p className={`${cl.model_upload_window_load_file_extensions}`}>
                                        (формати: STL, OBJ)
                                    </p>
                                </div>
                                <div className={`${cl.model_upload_window_load_file_button}`}>
                                    <div className={`${cl.model_upload_window_load_file}`}
                                        onClick={() => {
                                            if (modelPrintFile.file === null) {
                                                modelPrintFileInputRef.current.click();
                                            }
                                        }}>
                                        <input className={cl.model_upload_window_load_real_file_input}
                                            type="file"
                                            accept=".stl, .obj"
                                            style={{ display: 'none' }}
                                            ref={modelPrintFileInputRef}
                                            onChange={() => {
                                                let file = modelPrintFileInputRef.current.files[0];

                                                if (!file) {
                                                    return;
                                                }

                                                let fileSpl = file.name.split('.');
                                                const fileName = fileSpl[fileSpl.length - 1];

                                                if (fileName !== 'obj' && fileName !== 'stl') {
                                                    alert('Please, select an OBJ or STL file.');
                                                    return;
                                                }

                                                let formData = new FormData();
                                                let xml = new XMLHttpRequest();

                                                setModelPrintFile({ file: file, loading: true });
                                                formData.append('model', file);

                                                xml.onreadystatechange = () => {
                                                    if (xml.readyState === XMLHttpRequest.DONE) {
                                                        setModelPrintFile(p => {
                                                            let newP = { ...p };
                                                            newP.loading = false;
                                                            return newP;
                                                        });
                                                        xml.abort();

                                                        if (!isModelPrintFileChanged) {
                                                            setModelPrintFileChanged(true);
                                                        }
                                                    }
                                                };

                                                xml.upload.addEventListener("progress", (event) => {
                                                    let progressBar = document.getElementsByClassName(cl.model_upload_window_load_file_done_progress)[1];
                                                    progressBar.style.width = `${(event.loaded / event.total) * 100}%`;
                                                }, false);

                                                xml.open("POST", "fileupload.php");
                                                xml.send(formData);
                                            }} />
                                        <div className={cl.model_upload_window_delete_file_button}
                                            style={{ display: modelPrintFile.file && !modelPrintFile.loading ? 'flex' : 'none' }}
                                            title="Видалити"
                                            onClick={() => {
                                                setModelPrintFile({ file: null, loading: false });
                                                modelPrintFileInputRef.current.value = "";
                                            }}>
                                            <img className={cl.model_upload_window_delete_file_button_img} alt="delete file" />
                                        </div>
                                        <img className={`${cl.model_upload_window_load_file_img}`} alt="load file"
                                            src={modelPrintFile.file && !modelPrintFile.loading ? fileUploadedImg : uploadFileImg} />
                                        <span className={`${cl.model_upload_window_load_file_text}`}>
                                            {
                                                !modelPrintFile.file && !modelPrintFile.loading
                                                    ? "Перетягніть файл сюди"
                                                    : modelPrintFile.file && !modelPrintFile.loading
                                                        ? "Файл було успішно завантажено!"
                                                        : "Завантаження фалу..."
                                            }
                                        </span>
                                        <div className={`${cl.model_upload_window_load_file_progress}`}>
                                            <div className={`${cl.model_upload_window_load_file_done_progress}`}
                                                style={{
                                                    width:
                                                        !modelPrintFile.file && !modelPrintFile.loading ? '0%'
                                                            : modelPrintFile.file && !modelPrintFile.loading ? '100%'
                                                                : undefined
                                                }} />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div className={`${cl.model_upload_window_load_file_cont}`}>
                                <div className={`${cl.model_upload_window_load_file_description}`}>
                                    <h3 className={`${cl.model_upload_window_load_file_header}`}>
                                        Фото моделі
                                    </h3>
                                    <p className={`${cl.model_upload_window_load_file_extensions}`}>
                                        (формати: PNG)
                                    </p>
                                </div>
                                <div className={`${cl.model_upload_window_load_file_button}`}>
                                    <div className={`${cl.model_upload_window_load_file}`}
                                        onClick={() => {
                                            if (modelImageFile.file === null) {
                                                modelImageFileInputRef.current.click();
                                            }
                                        }}>
                                        <input className={cl.model_upload_window_load_image_file_input}
                                            type="file"
                                            accept=".png"
                                            style={{ display: 'none' }}
                                            ref={modelImageFileInputRef}
                                            onChange={() => {
                                                let file = modelImageFileInputRef.current.files[0];

                                                if (!file) {
                                                    return;
                                                }

                                                let fileSpl = file.name.split('.');
                                                const fileName = fileSpl[fileSpl.length - 1];

                                                if (fileName !== 'png') {
                                                    alert('Please, select a PNG file.');
                                                    return;
                                                }

                                                let formData = new FormData();
                                                let xml = new XMLHttpRequest();

                                                setModelImageFile({ file: file, loading: true });
                                                formData.append('model', file);

                                                xml.onreadystatechange = () => {
                                                    if (xml.readyState === XMLHttpRequest.DONE) {
                                                        setModelImageFile(p => {
                                                            let newP = { ...p };
                                                            newP.loading = false;
                                                            return newP;
                                                        });
                                                        xml.abort();

                                                        if (!isModelImageFileChanged) {
                                                            setModelImageFileChanged(true);
                                                        }
                                                    }
                                                };

                                                xml.upload.addEventListener("progress", (event) => {
                                                    let progressBar = document.getElementsByClassName(cl.model_upload_window_load_file_done_progress)[2];
                                                    progressBar.style.width = `${(event.loaded / event.total) * 100}%`;
                                                }, false);

                                                xml.open("POST", "fileupload.php");
                                                xml.send(formData);
                                            }} />
                                        <div className={cl.model_upload_window_delete_file_button}
                                            style={{ display: modelImageFile.file && !modelImageFile.loading ? 'flex' : 'none' }}
                                            title="Видалити"
                                            onClick={() => {
                                                setModelImageFile({ file: null, loading: false });
                                                modelImageFileInputRef.current.value = "";
                                            }}>
                                            <img className={cl.model_upload_window_delete_file_button_img} alt="delete file" />
                                        </div>
                                        <img className={`${cl.model_upload_window_load_file_img}`} alt="load file"
                                            src={modelImageFile.file && !modelImageFile.loading ? fileUploadedImg : uploadFileImg} />
                                        <span className={`${cl.model_upload_window_load_file_text}`}>
                                            {
                                                !modelImageFile.file && !modelImageFile.loading
                                                    ? "Перетягніть файл сюди"
                                                    : modelImageFile.file && !modelImageFile.loading
                                                        ? "Файл було успішно завантажено!"
                                                        : "Завантаження фалу..."
                                            }
                                        </span>
                                        <div className={`${cl.model_upload_window_load_file_progress}`}>
                                            <div className={cl.model_upload_window_load_file_done_progress}
                                                style={{
                                                    width:
                                                        !modelImageFile.file && !modelImageFile.loading ? '0%'
                                                            : modelImageFile.file && !modelImageFile.loading ? '100%'
                                                                : undefined
                                                }} />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className={cl.model_upload_window_additional_info}>
                        <h3 className={cl.model_upload_window_model_description_header}>Опис моделі</h3>
                        <textarea className={cl.model_upload_window_model_description}
                            rows="8"
                            value={modelDescriptionInputValue}
                            ref={modelDescriptionInputRef}
                            onChange={(e) => { setModelDescriptionInputValue(e.target.value); }} />
                        <h3 className={cl.model_upload_window_model_categories_header}>Категорії</h3>
                        <div className={cl.model_upload_window_model_categories_cont}>
                            <div className={cl.model_upload_window_model_categories}>
                                {RenderSelectedCategories()}
                            </div>
                            <div className={cl.model_upload_window_model_all_categories}>
                                {RenderAllCategories()}
                            </div>
                        </div>
                        <h3 className={cl.model_upload_window_model_tags_header}>Теги</h3>
                        <div className={cl.model_upload_window_model_tags_cont}>
                            <div className={cl.model_upload_window_model_tags}>
                                {RenderSelectedTags()}
                            </div>
                            <div className={cl.model_upload_window_model_edit_tag_menu}>
                                <div className={cl.model_upload_window_model_edit_tag_menu_control}>
                                    <div className={cl.model_upload_window_model_edit_tag_menu_input_cont}>
                                        <input className={cl.model_upload_window_model_edit_tag_menu_input}
                                            type="text"
                                            placeholder="Ім'я тегу"
                                            maxLength="30"
                                            value={tagNameInputValue}
                                            ref={tagNameInputRef}
                                            onChange={(e) => {

                                                if (e.target.value && e.target.value.match("^[\\w\\d_]+$") === null) {
                                                    return;
                                                }

                                                setTagNameInputValue(e.target.value);

                                                if (e.target.value) {
                                                    LoadTagList();
                                                }
                                                else {
                                                    setTagList(null);
                                                }
                                            }} />
                                    </div>
                                    <div className={cl.model_upload_window_model_edit_tag_menu_apply_cont}>
                                        <img className={cl.model_upload_window_model_edit_tag_menu_apply} alt="apply"
                                            onClick={() => {
                                                if (tagNameInputRef.current.value
                                                    && tagsForOwnModel.length < 25
                                                    && !tagsForOwnModel.includes(tagNameInputRef.current.value)) {
                                                    setTagsForOwnModel(p => [...p, tagNameInputRef.current.value]);
                                                }
                                            }} />
                                    </div>
                                </div>
                                <div className={cl.model_upload_window_model_edit_tag_menu_list}>
                                    {RenderSearchedTags()}
                                </div>
                            </div>
                        </div>
                        <h3 className={cl.model_upload_window_model_depth_header}>Відсоток заповенення</h3>
                        <input className={cl.model_upload_window_model_depth}
                            type="number"
                            min={5}
                            max={100}
                            value={modelDepthInputValue}
                            ref={modelDepthInputRef}
                            onChange={(e) => { setModelDepthInputValue(e.target.value); }} />
                        <div className={cl.model_upload_window_control}>
                            <div className={cl.model_upload_window_accept_button} onClick={() => {
                                if (editingModelId === null) {
                                    UploadModelRequest();
                                }
                                else {
                                    EditModelRequest();
                                }
                            }}>
                                {!isSaving ?
                                    <>
                                        <img className={cl.model_upload_window_accept_button_img} alt="upload" />
                                        <span>{editingModelId ? 'Змінити модель' : 'Додати модель'}</span>
                                    </>
                                    : <LoadingAnimation size="25px" loadingCurveWidth="5px" />
                                }
                            </div>
                            <div className={cl.model_upload_window_cancel_button}
                                onClick={() => {
                                    setUploadModelWindowInfo({ visible: false, modelId: null });
                                    onClose();
                                }}>
                                <span>Скасувати</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default UploadModelWindow;