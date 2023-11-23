import { BaseAPI } from "./BaseAPI";
export class CatalogAPI {
    static async getCategories() {
        return await BaseAPI.get('catalog/categories');
    }

    static async getAuthors(pageNumber = 1, pageSize = 10) {
        const p = pageNumber ? `?page=${pageNumber}` : '?page=1';
        const ps = pageSize ? `&page_size=${pageSize}` : '&page_size=10';
        return await BaseAPI.get(`catalog/authors${p}${ps}`);
    }

    static async search(
        query,
        minPrice,
        maxPrice,
        sortParameter,
        sortDirection,
        categories,
        author = null,
        pageNumber = 1,
        pageSize = 15
    ) {
        const p = pageNumber ? `?page=${pageNumber}` : '?page=1';
        const ps = pageSize ? `&page_size=${pageSize}` : '&page_size=15';
        const q = query ? `&q=${query}` : '';
        const minP = minPrice ? `&min_price=${minPrice}` : '';
        const maxP = maxPrice ? `&max_price=${maxPrice}` : '';
        const sp = sortParameter ? `&sort_parameter=${sortParameter}` : '';
        const sd = sortDirection ? `&sort_direction=${sortDirection}` : '';
        const a = author ? `&author=${author}` : '';

        let cl = '';

        categories?.forEach(el => {
            cl += `&categories=${el}`;
        });

        return await BaseAPI.get(`catalog/search${p}${ps}${q}${minP}${maxP}${sp}${sd}${a}${cl}`);
    }

    static async getModel(id) {
        return await BaseAPI.get(`catalog/${id}`);
    }

    static async getModelPicture(id) {
        return await BaseAPI.get(`catalog/model/picture/${id}`);
    }
}