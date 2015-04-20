//JS module pattern explained here: http://briancray.com/posts/javascript-module-pattern

function GlamProductsService() {
    //this.self = this;
    var _loadedProducts;

    this.loadProducts = function (callback) {
        if (_loadedProducts) {
            callback(_loadedProducts);
            return;
        }

        $.getJSON('api/products', function (items) {
            _loadedProducts = items;
            if (callback) {
                callback(items);
            }
        });
    };

    //return this;
}

var ProductRow = (function () {
    var _self = null;
    var _products = null;
    var _orderItemViewModel = null;
    var _container = null;
    var _selector = null;
    var _priceLabel = null;
    var _quantityTextbox = null;
    var _totalPriceLabel = null;

    var ProductRow = function (products/*, selector, priceLabel, quantityTextbox, totalPriceLabel*/) {
        //init
        /*       _self = null;
               _products = null;
               _orderItemViewModel = null;
               _container = null;
               _selector = null;
               _priceLabel = null;
               _quantityTextbox = null;
               _totalPriceLabel = null;*/

        //static counter
        if (ProductRow.count == undefined) {
            ProductRow.count = 1;
        }
        else {
            ProductRow.count++;
        }

        _self = this;
        _products = createProductsHashById(products);
        createItemsSelector();
        /*setSelector(selector);
        _priceLabel = priceLabel;
        setQuantityTextbox(quantityTextbox);
        _totalPriceLabel = totalPriceLabel;*/
    };

    var createItemsSelector = function () {
        //rowsCounter++;
        //rows[rowsCounter] = {};
        //var itemContainer = document.createElement("div");
        //itemContainer.setAttribute("id", "itemContainer-" + rowsCounter);
        //itemContainer.setAttribute("class", "itemContainer");

        var selectorWidth = "300px";
        _selector = document.createElement("select");
        _selector.setAttribute("name", "itemSelector-" + ProductRow.count);
        _selector.setAttribute("id", "itemSelector-" + ProductRow.count);
        _selector.style.width = selectorWidth;
        loadProductsToSelector(_selector);
        //selector.setAttribute("disabled", "disabled");
        //selector.add(new Option("Loading...", -1));

        //$(selector).change(onSelectionChanged);

        _priceLabel = document.createElement("input");
        _priceLabel.setAttribute("id", "price-" + ProductRow.count);
        _priceLabel.setAttribute("class", "price");
        _priceLabel.setAttribute("type", "text");
        _priceLabel.setAttribute("disabled", "disabled");

        _quantityTextbox = document.createElement("input");
        _quantityTextbox.setAttribute("type", "text");
        _quantityTextbox.setAttribute("class", "quantity");
        _quantityTextbox.setAttribute("id", "quantity-" + ProductRow.count);
        _quantityTextbox.setAttribute("disabled", "disabled");
        $(_quantityTextbox).ForceNumericOnly();
        $(_quantityTextbox).keyup(onQuantityChange);

        _totalPriceLabel = document.createElement("input");
        _totalPriceLabel.setAttribute("id", "totalPrice-" + ProductRow.count);
        _totalPriceLabel.setAttribute("type", "text");
        _totalPriceLabel.setAttribute("disabled", "disabled");

        _container = document.createElement("div");
        _container.setAttribute("id", "itemContainer-" + ProductRow.count);
        _container.setAttribute("class", "itemContainer");
        _container.appendChild(_selector);
        _container.appendChild(_priceLabel);
        _container.appendChild(_quantityTextbox);
        _container.appendChild(_totalPriceLabel);

        //if (products) {
        //    loadProductsToSelector(selector);
        //} else {
        //    loadProducts(function (items) {
        //        products = createProductsHashById(items);
        //        loadProductsToSelector(selector);
        //    });
        //}

        //itemContainer.appendChild(selector);
        //itemContainer.appendChild(priceLabel);
        //itemContainer.appendChild(quantity);
        //itemContainer.appendChild(totalPriceLabel);

        /* var productRow = new ProductRow(products, selector, priceLabel, quantity, totalPriceLabel);
         productRow["id"] = rowsCounter;
         return productRow;*/
    };

    var registerSelectorToChangeEvent = function () {
        $("#" + _selector.id).change(function () {
            var chosenProductId = $(_selector).val();
            _orderItemViewModel = _products[chosenProductId];
            refreshFields();
        });
    };

    var createProductsHashById = function (items) {
        var hashedProducts = [];
        for (var i in items) {
            hashedProducts[items[i].ProductID] = new orderItemViewModel(items[i]);
        }
        return hashedProducts;
    };

    /*var setQuantityTextbox = function (quantityTextbox) {
        _quantityTextbox = quantityTextbox;
        $(quantityTextbox).keyup(onQuantityChange);
    };*/

    var onQuantityChange = function () {
        _orderItemViewModel.Quantity = $(this).val();
        var totalPrice = _orderItemViewModel.Quantity * _orderItemViewModel.Price;
        $(_totalPriceLabel).val(totalPrice);
    };

    var onSelectionChanged = function () {
        var chosenProductId = $(_selector).val();
        _orderItemViewModel = _products[chosenProductId];
        refreshFields();
    };

    var loadProductsToSelector = function (selector) {
        for (var i in _products) {
            selector.add(new Option(_products[i].ProductID + " - " + _products[i].ProductName, _products[i].ProductID));
        }
    };

    var getContainer = function () {
        return _container;
    };

    var refreshFields = function () {
        $(_quantityTextbox).removeAttr('disabled');
        $(_quantityTextbox).val("");
        $(_totalPriceLabel).val("");
        $(_priceLabel).val(_orderItemViewModel.Price);
    };

    ProductRow.prototype = {
        constructor: ProductRow,
        getContainer: getContainer,
        selector: function () { return _selector },
        priceLabel: function () { _priceLabel },
        quantityTextbox: function () { _quantityTextbox },
        totalPriceLabel: function () { _totalPriceLabel },
        registerSelectorToChangeEvent: registerSelectorToChangeEvent
    };

    // return ProductRow
    return ProductRow;
})();
/*
var productRowsFactory = (function () {
    // private variables and functions
    var rowsCounter = 0;
    var selectorWidth = "300px";
    var products;
    var rows = [];
    // constructor
    var productRowsFactory = function (theProducts) {
        products = theProducts;
    };

    //var loadProducts = function (callback) {
    //    $.getJSON('api/products', function (items) {
    //        if (callback) {
    //            callback(items);
    //        }
    //    });
    //};
    //var onSelectionChanged = function () {
    //    var chosenProductId = $(this).val();
    //    var rowId = $(this).attr('id').split('-')[1];
    //    rows[rowId] = products[chosenProductId];
    //    $(this).parent().find(".quantity").first().removeAttr('disabled');
    //    $(this).parent().find(".quantity").val("");
    //    $(this).parent().find("#totalPrice-" + rowId).val("");
    //    $(this).parent().find("#price-" + rowId).first().val(products[chosenProductId].Price);
    //}

    //var onQuantityChange = function () {
    //    var typedAmount = $(this).val();
    //    var rowId = $(this).attr('id').split('-')[1];
    //    var price = rows[rowId].Price;

    //    $(this).parent().find("#totalPrice-" + rowId).first().val(typedAmount * price);
    //}

    var createItemsSelector = function () {
        rowsCounter++;
        rows[rowsCounter] = {};
        //var itemContainer = document.createElement("div");
        //itemContainer.setAttribute("id", "itemContainer-" + rowsCounter);
        //itemContainer.setAttribute("class", "itemContainer");

        var selector = document.createElement("select");
        selector.setAttribute("name", "itemSelector-" + rowsCounter);
        selector.setAttribute("id", "itemSelector-" + rowsCounter);
        //selector.setAttribute("disabled", "disabled");
        //selector.add(new Option("Loading...", -1));

        selector.style.width = selectorWidth;

        //$(selector).change(onSelectionChanged);

        var priceLabel = document.createElement("input");
        priceLabel.setAttribute("id", "price-" + rowsCounter);
        priceLabel.setAttribute("class", "price");
        priceLabel.setAttribute("type", "text");
        priceLabel.setAttribute("disabled", "disabled");

        var quantity = document.createElement("input");
        quantity.setAttribute("type", "text");
        quantity.setAttribute("class", "quantity");
        quantity.setAttribute("id", "quantity-" + rowsCounter);
        quantity.setAttribute("disabled", "disabled");
        $(quantity).ForceNumericOnly();
        //$(quantity).keyup(onQuantityChange);

        var totalPriceLabel = document.createElement("input");
        totalPriceLabel.setAttribute("id", "totalPrice-" + rowsCounter);
        priceLabel.setAttribute("class", "price");
        totalPriceLabel.setAttribute("type", "text");
        totalPriceLabel.setAttribute("disabled", "disabled");

        //if (products) {
        //    loadProductsToSelector(selector);
        //} else {
        //    loadProducts(function (items) {
        //        products = createProductsHashById(items);
        //        loadProductsToSelector(selector);
        //    });
        //}

        //itemContainer.appendChild(selector);
        //itemContainer.appendChild(priceLabel);
        //itemContainer.appendChild(quantity);
        //itemContainer.appendChild(totalPriceLabel);

        var productRow = new ProductRow(products, selector, priceLabel, quantity, totalPriceLabel);
        productRow["id"] = rowsCounter;
        return productRow;
    };

    //var createProductsHashById = function (items) {
    //    var hashedProducts = [];
    //    for (var i in items) {
    //        hashedProducts[items[i].ProductID] = items[i];
    //    }
    //    return hashedProducts;
    //};

    //var loadProductsToSelector = function (selector) {
    //    for (var i in products) {
    //        selector.add(new Option(products[i].ProductID + " - " + products[i].ProductName, products[i].ProductID));
    //    }
    //};

    // prototype
    productRowsFactory.prototype = {
        constructor: productRowsFactory,
        createItemsSelector: createItemsSelector
    };

    // return productRowsFactory
    return productRowsFactory;
})();*/