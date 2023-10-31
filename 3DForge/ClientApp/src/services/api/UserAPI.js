import { BaseAPI } from "./BaseAPI";

export class UserAPI {
    static async register(login, email, password, confirmPassword) {
        return await BaseAPI.post("user/register", { login, email, password, confirmPassword });
    }

    static async confirmEmail(email, token) {
        return await BaseAPI.get(`user/confirm-email/${encodeURIComponent(email)}?token=${token}`)
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

    static async getSelfInfo() {
        return await BaseAPI.get('user/self/info');
    }

    static async getUserInfo(id) {
        return await BaseAPI.get(`user/${id}/info`);
    }

    static async getSelfAvatar() {
        return await BaseAPI.get(`user/self/avatar`);
    }

    static async getUserAvatar(id) {
        return await BaseAPI.get(`user/${id}/avatar`);
    }
}