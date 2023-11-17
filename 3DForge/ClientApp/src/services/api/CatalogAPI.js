import { BaseAPI } from "./BaseAPI";
export class CatalogAPI {
    static async getCategories() {
        return await BaseAPI.get('catalog/categories');
    }

    static async search(query, sortParameter, sortDirection) {
        const q = query ? `q=${query}` : '';
        const sp = sortParameter ? `&sort_parameter=${sortParameter}` : '';
        const sd = sortDirection ? `&sort_direction=${sortDirection}` : '';
        return await BaseAPI.get(`catalog/search?${q}${sp}${sd}`);
    }

    static async getModel(id) {
        return await BaseAPI.get(`catalog/${id}`);
    }

    static async getModelPicture(id) {
        return await BaseAPI.get(`catalog/model/picture/${id}`);
    }
}