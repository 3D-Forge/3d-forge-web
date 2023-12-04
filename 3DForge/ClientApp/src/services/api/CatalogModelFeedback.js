import { BaseAPI } from "./BaseAPI";
export class CatalogModelFeedbackAPI {
    static async getFeedback() {
        return await BaseAPI.get('catalog/categories');
    }
}