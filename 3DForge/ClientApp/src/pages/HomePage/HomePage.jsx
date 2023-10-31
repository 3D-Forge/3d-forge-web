import React from "react";
import cl from "./.module.css";

const HomePage = () => {

    React.useEffect(() => {
        document.body.style.background = "linear-gradient(270deg, #7C26BF 0.13%, #BA67D3 99.84%, #7030B5 99.85%)";
    });

    return (
        <div>
            <div className={cl.main}>
                <div className={cl.intro}>
                    <div className={cl.intro_text}>
                        <h2 className={cl.intro_header}> Обмеження - твоя <i>фантазія</i>!</h2>
                        <p className={cl.intro_description}>
                            Допоможемо твоїм ідеям стати реальністю. Поглибся у
                            чарівний світ 3D та насолоджуйся найкращими візуальними
                            рішеннями!
                        </p>
                        <div className={cl.opportunity_list}>
                            <div className={`${cl.opportunity} ${cl.opportunity_print}`}>
                                <img className={`${cl.opportunity_img} ${cl.opportunity_img_print}`} alt="element printer variant" />
                                <p className={`${cl.opportunity_text} ${cl.opportunity_text_print}`}>Друк</p>
                            </div>
                            <div />
                            <div className={`${cl.opportunity} ${cl.opportunity_route}`}>
                                <img className={`${cl.opportunity_img} ${cl.opportunity_img_route}`} alt="delivery svgrepo com" />
                                <p className={`${cl.opportunity_text} ${cl.opportunity_text_route}`}>Шлях</p>
                            </div>
                            <div />
                            <div className={`${cl.opportunity} ${cl.opportunity_delivery}`}>
                                <img className={`${cl.opportunity_img} ${cl.opportunity_img_delivery}`} alt="receive package" />
                                <p className={`${cl.opportunity_text} ${cl.opportunity_text_delivery}`}>Отримання</p>
                            </div>
                        </div>
                    </div>
                    <img className={cl.go_down_img} alt="move down" />
                    <img className={cl.intro_img} alt="intro image" />
                </div>
                <h1 className={cl.creating_3D_model_header}>ЕТАПИ СТВОРЕННЯ 3D МОДЕЛІ</h1>
                <div className={cl.creating_3D_model_stage_list}>
                    <div className={`${cl.creating_3D_model_stage} ${cl.creating_3D_model_stage_modelling}`}>
                        <img className={`${cl.creating_3D_model_stage_img}  ${cl.image_3D_modelling}`} alt="Computer svgrepo" />
                        <h2 className={`${cl.creating_3D_model_stage_header}  ${cl.creating_3D_model_stage_header_modelling}`}>3D Моделювання</h2>
                        <p className={`${cl.creating_3D_model_stage_text}  ${cl.image_3D_import_text_modelling}`}>
                            Cтворення 3D моделі у спеціальному програмному середовищі, такому як Blender, AutoCAD, чи інший програмний
                            пакет для 3D моделювання.
                        </p>
                    </div>
                    <div />
                    <div className={`${cl.creating_3D_model_stage} ${cl.creating_3D_model_stage_print}`}>
                        <img className={`${cl.creating_3D_model_stage_img}  ${cl.image_3D_print}`} alt="Extrude svgrepo com" />
                        <h2 className={`${cl.creating_3D_model_stage_header}  ${cl.creating_3D_model_stage_header_print}`}>3D Друк</h2>
                        <p className={`${cl.creating_3D_model_stage_text}  ${cl.image_3D_import_text_print}`}>
                            Після створення 3D моделі, вона відправляється на друк на 3D-принтері.&nbsp;&nbsp;ми встановлюємо матеріал і
                            параметри друку відповідно до вашого вибору. Після чого 3D-принтер надрукує фізичний об&#39;єкт.
                        </p>
                    </div>
                    <div />
                    <div className={`${cl.creating_3D_model_stage} ${cl.creating_3D_model_stage_import}`}>
                        <img className={`${cl.creating_3D_model_stage_img}  ${cl.image_3D_import}`} alt="Hand holding svgrepo" />
                        <h2 className={`${cl.creating_3D_model_stage_header}  ${cl.creating_3D_model_stage_header_import}`}>Вилучення моделі зі станка</h2>
                        <p className={`${cl.creating_3D_model_stage_text}  ${cl.image_3D_import_text_import}`}>
                            На завершення, ми отримуємо готову 3D модель, яка готова для використання або відправки клієнту.
                        </p>
                    </div>
                </div>
                <div className={cl.go_up_button_cont}>
                    <div className={cl.go_up_button} onClick={() => window.scrollTo(0, 0)}>
                        <img className={cl.go_up_img} alt="move up" />
                        <p className={cl.go_up_text}>Вгору</p>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default HomePage;