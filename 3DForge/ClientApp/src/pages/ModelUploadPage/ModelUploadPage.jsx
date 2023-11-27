import React from 'react';
import { CatalogAPI } from '../../services/api/CatalogAPI';
import { useParams } from "react-router-dom";
import cl from "./.module.css";




const ModelUploadPage = () => {

    return (
        <div className={cl.page}>
            <div className={cl.header}>Додати модель</div>
            <div className={cl.header_below}>Додайте модель, ваш підпис адресу, гроші, ще одного пса та кота, бхехе</div>
            <div className={cl.left}>
                <div className={cl.input_name_group}>
                    <div className={cl.input_name_text}>Назва моделі</div>
                    <input type="text" className={cl.input_name}></input>
                </div>
                <div className={cl.file_model_view}>
                    <div className={cl.file_model_view_text}>
                        Файл моделі для перегляду</div>
                    <img className={cl.uploading} alt="uploading"></img>
                </div>
                <div className={cl.file_model_print}>
                    <div className={cl.file_model_print_text}>
                        Файл моделі для друку</div>
                </div>
                <div className={cl.file_model_image}>
                    <div className={cl.file_model_image_text}>
                        Фото моделі</div>
                </div>
            </div>
            <div className={cl.right}>
                <div className={cl.input_desc_group}>
                    <div className={cl.input_desc_text}>Опис моделі</div>
                    <input type="text" className={cl.input_desc}></input>
                </div>
                <div className={cl.input_categories_group}>
                    <div className={cl.input_categories_text}>Категорії</div>
                </div>
                <div className={cl.input_tegs_group}>
                    <div className={cl.input_tegs_text}>Теги</div>
                </div>
                <button className={cl.undo_button}>Скасувати</button>
                <button className={cl.add_button}>Додати модель</button>
            </div>
        </div>
    );
}

export default ModelUploadPage;

