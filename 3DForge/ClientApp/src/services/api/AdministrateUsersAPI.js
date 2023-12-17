import { BaseAPI } from "./BaseAPI";

export class AdministrateUsersAPI {
    static async blocked(id) {
        return await BaseAPI.put(`administration/users/${id}/blocked`);
    }
}
