import { BaseAPI } from "./BaseAPI";
export class CartAPI {
    static async getItems() {
        return await BaseAPI.get('cart/getItems');
    }
    static async addItem(id, Pieces, Depth, Scale, ColorId, PrintTypeName, PrintMaterialName) {

        return await BaseAPI.put("cart/addItem",
            {
                id, Pieces, Depth, Scale, ColorId, PrintTypeName, PrintMaterialName
        }, "application/json")
    }
    }

