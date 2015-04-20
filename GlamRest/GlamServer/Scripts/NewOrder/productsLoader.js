//JS module pattern explained here: http://briancray.com/posts/javascript-module-pattern

function ProductRow(products) {
    var _self = this;
    var _products = null;
    var _orderItemViewModel = null;
    var _container = null;
    var _selector = null;
    var _priceLabel = null;
    var _quantityTextbox = null;
    var _totalPriceLabel = null;
    var _productRowId = 0;
    this.onTotalPriceChanged;

    var ctor = function (products) {
        //static counter
        if (ProductRow.count == undefined) {
            ProductRow.count = 1;
        }
        else {
            ProductRow.count++;
        }
        _productRowId = ProductRow.count;
        _products = createProductsHashById(products);
        createItemsSelector();
    };

    this.getProductRowId = function () { return _productRowId; };

    var createItemsSelector = function () {
        var selectorWidth = "300px";
        _selector = document.createElement("select");
        _selector.setAttribute("name", "itemSelector-" + _productRowId);
        _selector.setAttribute("id", "itemSelector-" + _productRowId);
        _selector.style.width = selectorWidth;
        loadProductsToSelector(_selector);
        _selector.onchange = onSelectionChanged;

        _priceLabel = document.createElement("input");
        _priceLabel.setAttribute("id", "price-" + _productRowId);
        _priceLabel.setAttribute("class", "price");
        _priceLabel.setAttribute("type", "text");
        _priceLabel.setAttribute("disabled", "disabled");

        _quantityTextbox = document.createElement("input");
        _quantityTextbox.setAttribute("type", "text");
        _quantityTextbox.setAttribute("class", "quantity");
        _quantityTextbox.setAttribute("id", "quantity-" + _productRowId);
        _quantityTextbox.setAttribute("disabled", "disabled");
        $(_quantityTextbox).ForceNumericOnly();
        $(_quantityTextbox).keyup(onQuantityChange);

        _totalPriceLabel = document.createElement("input");
        _totalPriceLabel.setAttribute("id", "totalPrice-" + _productRowId);
        _totalPriceLabel.setAttribute("type", "text");
        _totalPriceLabel.setAttribute("disabled", "disabled");

        _container = document.createElement("div");
        _container.setAttribute("id", "itemContainer-" + _productRowId);
        _container.setAttribute("class", "itemContainer");

        _container.appendChild(_selector);
        _container.appendChild(_priceLabel);
        _container.appendChild(_quantityTextbox);
        _container.appendChild(_totalPriceLabel);
    };
    var fireTotalPriceChangedEvent = function () {
        _orderItemViewModel.TotalPrice = Number(_totalPriceLabel.value);
        if (_self.onTotalPriceChanged) {
            _self.onTotalPriceChanged();
        }
    };
    var createProductsHashById = function (items) {
        var hashedProducts = [];
        for (var i in items) {
            hashedProducts[items[i].ProductID] = new orderItemViewModel(items[i]);
        }
        return hashedProducts;
    };

    var onQuantityChange = function () {
        var quantity = 0;
        if ($(this).val().toString().length > 0) {
            quantity = $(this).val();
        }
        _orderItemViewModel.Quantity = quantity;
        var totalPrice = _orderItemViewModel.Quantity * _orderItemViewModel.Price;
        $(_totalPriceLabel).val(totalPrice);
        fireTotalPriceChangedEvent();
    };

    var onSelectionChanged = function () {
        var chosenProductId = $(_selector).val();
        _orderItemViewModel = _products[chosenProductId];
        refreshFields();
        fireTotalPriceChangedEvent();
    };

    var loadProductsToSelector = function (selector) {
        for (var i in _products) {
            selector.add(new Option(_products[i].ProductID + " - " + _products[i].ProductName, _products[i].ProductID));
        }
    };

    this.getContainer = function () {
        return _container;
    };

    this.getOrderItemViewModel = function () { return _orderItemViewModel; }

    this.getSelector = function () { return _selector; };
    var refreshFields = function () {
        $(_quantityTextbox).removeAttr('disabled');
        $(_quantityTextbox).val("");
        $(_totalPriceLabel).val("");
        $(_priceLabel).val(_orderItemViewModel.Price);
    };

    ctor(products);
};