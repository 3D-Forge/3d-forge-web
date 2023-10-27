import React, { Component } from 'react';
import styles from './styles.module.css'; // імпорт CSS модуля
class Registration extends Component {
    render() {
        return (
            <div className={styles.index}>
                <div className={styles.div}>
                    <div className={styles.view}>
                        <img
                            className={styles.image}
                            alt="Image"
                            src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65379b523ebb47c44c902675/img/----------.svg"
                        />
                        <div className={styles['text-wrapper']}>3D Forge</div>
                    </div>
                    <div className={styles.overlap}>
                        <div className={styles['text-wrapper-2']}>Already signed up?</div>
                        <a className={styles['text-wrapper-3']}>Log in</a>
                        <div className={styles.group}>
                            <div className={styles['overlap-group']}>
                                <div className={styles['text-wrapper-4']}>Sign Up</div>
                                <div className={styles.rectangle} />
                                <button className={styles['sign_up_button']}>Sign Up</button>
                              
                                <input type="text" className={styles['view-2']} />
                                <input type="password" className={styles['view-3']} />
                                <input type="email" className={styles['view-4']} />
                                <input type="password" className={styles['view-5']} />
                                <div className={styles['text-wrapper-5']}>Email</div>
                                <p className={styles.p}>
                                    <span className={styles.span}>Password</span>
                                    <span className={styles['text-wrapper-6']}>*</span>
                                </p>
                                <p className={styles['div-2']}>
                                    <span className={styles.span}>Login</span>
                                    <span className={styles['text-wrapper-6']}>*</span>
                                </p>
                                <p className={styles['div-3']}>
                                    <span className={styles.span}>Repeat password</span>
                                    <span className={styles['text-wrapper-6']}>*</span>
                                </p>
                              
                                <div className={styles['group-2']}>
                                    <input type="checkbox" name="myCheckbox" value="isChecked">
                                    </input>
                                    <label className={styles['text-wrapper-8']}>
                                        By creating an account you agree to sell us your body and soul (and cat)
                                    </label>

                                </div>
                            </div>
                        </div>
                    </div>
                    <p className={styles['text-wrapper-9']}>Let's help your ideas become reality. Lost in the enchanting world of 3D!</p>
                    <img
                        className={styles['image-2']}
                        alt="Image"
                        src="https://cdn.animaapp.com/projects/6537996634ad3d584d8c9f1f/releases/65379b523ebb47c44c902675/img/image-4.png"
                    />
                </div>
            </div>
        );
    }
}

export default Registration;












