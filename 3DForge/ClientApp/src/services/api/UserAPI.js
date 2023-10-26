import { BaseAPI } from "./BaseAPI";

export class UserAPI {
    static async register(login, email, password) {
        return await BaseAPI.post("user/register", { login, email, password });
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
}