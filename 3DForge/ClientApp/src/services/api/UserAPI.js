import { BaseAPI } from "./BaseAPI";

export class UserAPI {
    static async register(login, email, password, confirmPassword) {
        return await BaseAPI.post("user/register", { login, email, password, confirmPassword });
    }

    static async confirmEmail(email, token) {
        return await BaseAPI.get(`user/confirm-registration-email/${encodeURIComponent(email)}?token=${token}`)
    }

    static async login(loginOrEmail, password) {
        return await BaseAPI.post("user/login", { loginOrEmail, password });
    }

    static async logout() {
        return await BaseAPI.get('user/logout');
    }

    static async check() {
        return await BaseAPI.get('user/check');
    }

    static async sendResetPasswordPermission(login) {
        return await BaseAPI.post(`user/${login}/send-reset-password-permission`);
    }

    static async resetPassword(login, newPassword, confirmNewPassword, oldPassword = null, token = null) {
        return await BaseAPI.put(`user/${login}/reset-password`,
            {
                newPassword,
                confirmNewPassword,
                oldPassword,
                token
            });
    }

    static async getSelfInfo() {
        return await BaseAPI.get('user/self/info');
    }

    static async getUserInfo(login) {
        return await BaseAPI.get(`user/${login}/info`);
    }

    static async getSelfAvatar() {
        return await BaseAPI.get(`user/self/avatar`);
    }

    static async getUserAvatar(login) {
        return await BaseAPI.get(`user/${login}/avatar`);
    }
}