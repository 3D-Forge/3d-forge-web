import { CookieService } from "./CookieService";

export class LanguageService {
    static defaultLang = 'en';
    static data = null;

    static get(key) {
        if (CookieService.get('language') === null) {
            CookieService.set('language', this.defaultLang);
        }

        if (this.data === null) {
            this.data = require(`../storage/strings/${CookieService.get('language')}.json`)
        }

        var value = this.data;

        key.split('.').forEach(keyPer => {
            if (value[keyPer]) {
                value = value[keyPer];
            } else {
                return key;
            }
        });

        return typeof value === 'string' ? value : key;
    }

    static setLang(lang) {
        try {
            this.data = require(`../storage/strings/${lang}.json`);
        } catch (err) {
            return false;
        }

        CookieService.set('language', lang);
        return true;
    }

    static currentLang() {
        return CookieService.get('language');
    }
}