import { BaseAPI } from "./BaseAPI";
export class CatalogModelFeedbackAPI {
    static async getFeedback(id) {
        return await BaseAPI.get(`catalog/${id}/feedback`);
    }
}