export class ThemeService {
    static defaultTheme = 'light';
    static data = null;
    
    static get(key) {
        if (CookieService.get('theme') === null) {
            CookieService.set('theme', this.defaultTheme);
        }

        if (this.data === null) {
            this.data = require(`../storage/colors/${CookieService.get('theme')}.json`)
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

    static setTheme(lang) {
        try {
            this.data = require(`../storage/colors/${lang}.json`);
        } catch (err) {
            return false;
        }

        CookieService.set('theme', lang);
        return true;
    }

    static currentTheme() {
        return CookieService.get('theme');
    }
}