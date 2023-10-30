import React from "react";
import cl from "./.module.css";

const HomePage = () => {

    React.useEffect(() => {
        document.body.style.background = "linear-gradient(270deg, #7C26BF 0.13%, #BA67D3 99.84%, #7030B5 99.85%)";
    });

    return (
        <div>
            <div className={cl.index}>
                <div className={cl.div_2}>
                  
                   
                    <div className={cl.frame_2}>
                        <div className={cl.rectangle_2} />
                        <div className={cl.rectangle_3} />
                        <div className={cl.rectangle_4} />

                        <img
                            className={cl.image_3D_modelling}
                            alt="Computer svgrepo"
                        />
                        <div className={cl.element}>3d Моделювання</div>
                        <p className={cl.p}>
                            Cтворення 3D моделі у спеціальному програмному середовищі, такому як Blender, AutoCAD, чи інший програмний
                            пакет для 3D моделювання.
                        </p>
                        <p className={cl.element_d_d_d}>
                            Після створення 3D моделі, вона відправляється на друк на 3D-принтері.&nbsp;&nbsp;ми встановлюємо матеріал і
                            параметри друку відповідно до вашого вибору. Після чого 3D-принтер надрукує фізичний об&#39;єкт.
                        </p>
                        <p className={cl.text_wrapper_11}>
                            На завершення, ми отримуємо готову 3D модель, яка готова для використання або відправки клієнту.
                        </p>
                        <div className={cl.element_d}>3d Друк</div>
                        <div className={cl.text_wrapper_12}>Вилучення моделі зі станка</div>
                        <img
                            className={cl.image_3D_print}
                            alt="Extrude svgrepo com"
                           
                        />
                        <img
                            className={cl.image_3D_import}
                            alt="Hand holding svgrepo"
                           
                        />
                    </div>
                    <p className={cl.div_5}>
                        <span className={cl.span}>Обмеження - твоя </span>
                        <span className={cl.text_wrapper_13}>фантазія</span>
                        <span className={cl.span}>!</span>
                    </p>
                    <p className={cl.text_wrapper_14}>
                        Допоможемо твоїм ідеям стати реальністю. Поглибся у чарівний світ 3D та насолоджуйся найкращими візуальними
                        рішеннями!
                    </p>
                    <img
                        className={cl.image_house}
                        alt="Image"
                    />
                    <div className={cl.frame_3}>
                        <div className={cl.group}>
                            <img
                                className={cl.element_printer_variant}
                                alt="Element printer variant"
                            />
                            <div className={cl.text_wrapper_15}>Друк</div>
                        </div>
                        <div className={cl.overlap_group_wrapper}>
                            <div className={cl.overlap_group_2}>
                                <div className={cl.text_wrapper_16}>Шлях</div>
                                <img
                                    className={cl.delivery_svgrepo_com}
                                    alt="Delivery svgrepo com"
                                />
                            </div>
                        </div>
                        <div className={cl.group_2}>
                            <img
                                className={cl.receive_package}
                                alt="Receive package"
                            />
                            <div className={cl.text_wrapper_17}>Отримання</div>
                        </div>
                    </div>
                    <div className={cl.element_2}>ЕТАПИ СТВОРЕННЯ 3D МОДЕЛІ</div>
                    <div className={cl.overlap_wrapper}>
                        <div className={cl.overlap}>
                            <img
                                className={cl.double_arrow_up}
                                alt="Double arrow up"
                            />
                            <div className={cl.text_wrapper_18}>Вгору</div>
                        </div>
                    </div>
                    <img
                        className={cl.down_arrow_backup}
                        alt="Down arrow backup"
                    />
                   
                </div>
            </div>
        <div className={cl.footer}>
            <img className={cl.line} alt="Logo"/>
            <p className={cl.text_wrapper}>
                Ми надаємо можливість створювати, ділитися та замовляти унікальні 3D-моделі. Наша мета - зробити 3D-друк
                доступним для кожного. Долучайтеся до нашої спільноти та створюйте разом з нами!
            </p>
            <p className={cl.div}>+380 999 999 99 99</p>
            <div className={cl.text_wrapper_2}>3d.forgehub@gmail.com</div>
            <div className={cl.text_wrapper_3}>Про Нас</div>
            <div className={cl.text_wrapper_4}>Зв&#39;яжіться з нами</div>
            <div className={cl.text_wrapper_5}>Підпишіться на розсилку</div>
            <img className={cl.instagram} alt="Instagram"/>
            < img className={cl.facebook_svgrepo_com} alt="facebookSvgrepoCom" />

            <img className={cl.google_svgrepo} alt="googleSvgrepo" />
            <div className={cl.overlap_group}>
                <img className={cl.letter_svgrepo_com} alt="Letter svgrepo com"/>
                <div className={cl.text_wrapper_6}>Введіть Ваш Email</div>
            </div>
            <div className={cl.text_wrapper_7}>Підписатися</div>
            <img
                className={cl.img}
                alt="Line"/>
            <img
                className={cl.line_2}
                alt="Line"
            />
            </div>
        </div>
    );
}

export default HomePage;