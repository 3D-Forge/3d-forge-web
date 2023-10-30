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
                        <div className={cl.element}>3d �����������</div>
                        <p className={cl.p}>
                            C�������� 3D ����� � ������������ ����������� ����������, ������ �� Blender, AutoCAD, �� ����� ����������
                            ����� ��� 3D �����������.
                        </p>
                        <p className={cl.element_d_d_d}>
                            ϳ��� ��������� 3D �����, ���� ������������� �� ���� �� 3D-�������.&nbsp;&nbsp;�� ������������ ������� �
                            ��������� ����� �������� �� ������ ������. ϳ��� ���� 3D-������� ������� �������� ��&#39;���.
                        </p>
                        <p className={cl.text_wrapper_11}>
                            �� ����������, �� �������� ������ 3D ������, ��� ������ ��� ������������ ��� �������� �볺���.
                        </p>
                        <div className={cl.element_d}>3d ����</div>
                        <div className={cl.text_wrapper_12}>��������� ����� � ������</div>
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
                        <span className={cl.span}>��������� - ���� </span>
                        <span className={cl.text_wrapper_13}>�������</span>
                        <span className={cl.span}>!</span>
                    </p>
                    <p className={cl.text_wrapper_14}>
                        ���������� ���� ����� ����� ���������. �������� � ������� ��� 3D �� ������������ ���������� ����������
                        ��������!
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
                            <div className={cl.text_wrapper_15}>����</div>
                        </div>
                        <div className={cl.overlap_group_wrapper}>
                            <div className={cl.overlap_group_2}>
                                <div className={cl.text_wrapper_16}>����</div>
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
                            <div className={cl.text_wrapper_17}>���������</div>
                        </div>
                    </div>
                    <div className={cl.element_2}>����� ��������� 3D ����˲</div>
                    <div className={cl.overlap_wrapper}>
                        <div className={cl.overlap}>
                            <img
                                className={cl.double_arrow_up}
                                alt="Double arrow up"
                            />
                            <div className={cl.text_wrapper_18}>�����</div>
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
                �� ������ ��������� ����������, ������� �� ��������� ������� 3D-�����. ���� ���� - ������� 3D-����
                ��������� ��� �������. ����������� �� ���� �������� �� ��������� ����� � ����!
            </p>
            <p className={cl.div}>+380 999 999 99 99</p>
            <div className={cl.text_wrapper_2}>3d.forgehub@gmail.com</div>
            <div className={cl.text_wrapper_3}>��� ���</div>
            <div className={cl.text_wrapper_4}>��&#39;������ � ����</div>
            <div className={cl.text_wrapper_5}>ϳ��������� �� ��������</div>
            <img className={cl.instagram} alt="Instagram"/>
            < img className={cl.facebook_svgrepo_com} alt="facebookSvgrepoCom" />

            <img className={cl.google_svgrepo} alt="googleSvgrepo" />
            <div className={cl.overlap_group}>
                <img className={cl.letter_svgrepo_com} alt="Letter svgrepo com"/>
                <div className={cl.text_wrapper_6}>������ ��� Email</div>
            </div>
            <div className={cl.text_wrapper_7}>ϳ���������</div>
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