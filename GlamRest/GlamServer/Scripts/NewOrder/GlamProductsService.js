function GlamProductsService() {
    //this.self = this;
    var _loadedProducts;

    this.loadProducts = function (callback) {
        if (_loadedProducts) {
            callback(_loadedProducts);
            return;
        }

        $.getJSON('/api/products', function (items) {
            _loadedProducts = items;
            if (callback) {
                callback(items);
            }
        });
    };

    this.loadHolidays = function (callback) {
        $.getJSON('/api/holidays', function (holidays) {
            if (callback)
                callback(holidays);
        });
    }
}