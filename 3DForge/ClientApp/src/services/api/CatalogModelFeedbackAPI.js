import { BaseAPI } from "./BaseAPI";
export class CatalogModelFeedbackAPI {
    static async getFeedback(id) {
        return await BaseAPI.get(`catalog/${id}/feedback`);
    }
    static async putFeedback(id, OrderedModelId, Rate, Text, Pros, Cons) {
        return  BaseAPI.post(`catalog/${id}/feedback`, {
            OrderedModelId, Rate, Text, Pros, Cons
        }, "application/json");
    }
}