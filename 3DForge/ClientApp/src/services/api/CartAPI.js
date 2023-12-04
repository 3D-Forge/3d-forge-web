import { BaseAPI } from "./BaseAPI";
export class CartAPI {
    static async getItems() {
        return await BaseAPI.get('cart/getItems');
    }
}
