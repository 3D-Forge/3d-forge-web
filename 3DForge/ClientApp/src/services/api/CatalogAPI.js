import { BaseAPI } from "./BaseAPI";
export class CatalogAPI {
    static async getCategories() {
        return await BaseAPI.get('catalog/categories');
    }

    static async getAuthors(query, pageNumber = 1, pageSize = 10) {
        const p = pageNumber ? `?page=${pageNumber}` : '?page=1';
        const ps = pageSize ? `&page_size=${pageSize}` : '&page_size=10';
        const q = query ? `&q=${query}` : '';
        return await BaseAPI.get(`catalog/authors${p}${ps}${q}`);
    }

    static async search(
        query,
        minPrice,
        maxPrice,
        sortParameter,
        sortDirection,
        categories,
        ratings,
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
        let rl = '';

        categories?.forEach(el => {
            cl += `&categories=${el}`;
        });
        ratings?.forEach(el => {
            rl += `&rating=${el}`;
        })

        return await BaseAPI.get(`catalog/search${p}${ps}${q}${minP}${maxP}${sp}${sd}${a}${cl}${rl}`);
    }

    static async getModel(id) {
        return await BaseAPI.get(`catalog/${id}`);
    }

    static async getModelPicture(id) {
        return await BaseAPI.get(`catalog/model/picture/${id}`);
    }
}