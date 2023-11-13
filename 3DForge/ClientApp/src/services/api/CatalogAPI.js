import { BaseAPI } from "./BaseAPI";
export class CatalogAPI {
    static async getModel(id) {
        return await BaseAPI.get(`catalog/${id}`);
    }
}