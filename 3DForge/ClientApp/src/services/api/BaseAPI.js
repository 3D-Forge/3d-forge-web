export class BaseAPI {
    static async get(url, options = {}) {
        let response = fetch(`/api/${url}`, options).then((response) => BaseAPI.addAuthorizationHeader(response, options));
        response.catch((error) => BaseAPI.errorHandling(error));
        return response;
    }

    static async post(url, data, contentType = undefined, options = {}) {
        let headers = {};
        
        if (contentType) {
            headers["Content-Type"] = contentType;
        }

        let response = fetch(`/api/${url}`, {
            method: "POST",
            headers: headers,
            body: contentType === "application/json" ? JSON.stringify(data) : data,
            ...options,
        }).then((response) => BaseAPI.addAuthorizationHeader(response, options));
        response.catch((error) => BaseAPI.errorHandling(error));
        return response;
    }

    static async put(url, data, contentType = undefined, options = {}) {
        let headers = {};
        
        if (contentType) {
            headers["Content-Type"] = contentType;
        }

        let response = fetch(`/api/${url}`, {
            method: "PUT",
            headers: headers,
            body: contentType === "application/json" ? JSON.stringify(data) : data,
            ...options,
        }).then((response) => BaseAPI.addAuthorizationHeader(response, options));
        response.catch((error) => BaseAPI.errorHandling(error));
        return response;
    }

    static async delete(url, options = {}) {
        let response = fetch(`/api/${url}`, {
            method: "DELETE",
            ...options,
        }).then((response) => BaseAPI.addAuthorizationHeader(response, options));
        response.catch((error) => BaseAPI.errorHandling(error));
        return response;
    }

    static addAuthorizationHeader(response, options) {
        if(response.status === 401 && window.location.pathname !== "/" && options?.ignoreAuth !== true) {
            console.log("Unauthorized");
        }
        return response;
    }

    static async errorHandling(error) {
        console.log(error);
    }

    static async getTextOrJson(response) {
        let contentType = response.headers.get("content-type");
        if (contentType && contentType.indexOf("application/json") !== -1) {
            return await response.json();
        } else {
            return await response.text();
        }
    }
}