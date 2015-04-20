var newOrderPage = (function () {
    // private variables and functions
    var _productRowsFactory;
    var _products;
    var glamProductsService;

    // constructor
    var newOrderPage = function () {
        glamProductsService = new GlamProductsService();
    };

    var addItem = function () {
        var newItem = new ProductRow(_products);
        $("#itemsContainer").append(newItem.getContainer());
        $(newItem.selector()).select2();
        newItem.registerSelectorToChangeEvent();
    };

    var loadClients = function (selectElement, callback) {
        $.getJSON('api/clients', function (items) {
            for (var i in items) {
                selectElement.add(new Option(items[i].ClientID + " - " + items[i].ClientName, items[i].ClientID));
            }

            if (callback)
                callback();
        });
    };

    var createClientsSelector = function (callback) {
        var selector = document.createElement("select");
        selector.setAttribute("name", "clientsPicker");
        selector.setAttribute("id", "clientsPicker");
        selector.style.width = "300px";

        loadClients(selector, callback);

        return selector;
    };

    var initClients = function () {
        var container = document.getElementById("clientsPickerContainer");
        container.innerText = "Loading...";
        var clientsSelector = createClientsSelector(function () {
            container.innerText = "";
            document.getElementById("clientsPickerContainer").appendChild(clientsSelector);
            $("#clientsPicker").select2();
        });
    };

    var init = function () {
        initClients();

        glamProductsService.loadProducts(function (products) {
            //_productRowsFactory = new productRowsFactory(products);
            _products = products;
            //first item
            addItem();
        });

        $("#AddItemButton").click(function () {
            addItem();
        });
    };

    // prototype
    newOrderPage.prototype = {
        constructor: newOrderPage,
        init: init
    };

    // return productsLoader
    return newOrderPage;
})();

//var newOrderPage = (function () {
//    // private variables and functions
//    var itemsLoader;

//    // constructor
//    var newOrderPage = function () {
//        itemsLoader = new productsLoader();
//    };

//    var init = function () { alert("hi"); };
//    // prototype
//    newOrderPage.prototype = {
//        constructor: newOrderPage,
//        init: init
//    };

//    // return module
//    return newOrderPage;
//})();