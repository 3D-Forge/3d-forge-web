import { BaseAPI } from "./BaseAPI";
export class OrderAPI {
    static async getHistory() {
        return await BaseAPI.get('orders/history');
    }
}
