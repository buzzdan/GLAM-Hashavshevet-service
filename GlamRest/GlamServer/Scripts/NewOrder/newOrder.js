/// <reference path="productsLoader.js" />
/// <reference path="../BrowseOrders/BrowseOrders.js" />

var newOrderPage = (function () {
    // private variables and functions
    var _productRowsFactory;
    var _products;
    var glamProductsService;
    var _clientsSelector;
    var _discountTextbox;
    var _totalLabel;
    var _allItems = [];

    // constructor
    var newOrderPage = function () {
        glamProductsService = new GlamProductsService();
    };

    var addItem = function () {
        var newItem = new ProductRow(_products);
        var rowID = newItem.getProductRowId();
        _allItems[rowID] = newItem;
        newItem.onTotalPriceChanged = calculateTotalAmount;

        $("#itemsContainer").append(newItem.getContainer());
        $(newItem.getSelector()).select2();
    };

    var calculateTotalAmount = function () {
        if (_allItems.length == 0) {
            return;
        }

        var discount = _discountTextbox.value;
        var totalFullPrice = 0;
        for (var i in _allItems) {
            if (_allItems[i].getOrderItemViewModel()) {
                totalFullPrice += _allItems[i].getOrderItemViewModel().TotalPrice;
            }
        }

        if (discount == 0) {
            _totalLabel.innerText = totalFullPrice;
        }
        else {
            _totalLabel.innerText = totalFullPrice * (100 - discount) / 100;
        }
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
        _clientsSelector = createClientsSelector(function () {
            container.innerText = "";
            container.appendChild(_clientsSelector);
            $("#clientsPicker").select2();
        });
    };
    var initDiscount = function () {
        _discountTextbox = document.getElementById("discount");
        _discountTextbox.onkeyup = calculateTotalAmount;
    };

    var initTotalPrice = function () {
        _totalLabel = document.getElementById("TotalPrice");
    };

    var initSubmitOrder = function () {
        var submitButton = document.getElementById("SubmitOrder");
        submitButton.onclick = submitOrder;
    };

    var submitOrder = function () {
        var order = collectOrder();

        $.post('api/order', order, function (data, x, c) {
            $("#orderContainer").hide('fast');
            setTimeout(function () {
                $("#orderContainer").html("ההזמנה מספר " + data.OrderID + " התקבלה וממתינה להזנה לחשבשבת").addClass('noItems');
                $("#orderContainer").show('slow');
                var div = document.createElement("div");
                div.setAttribute("id", "orderBrowserContainer");
                $("#orderContainer").append(div);
                var orderpage = new OrdersBrowserPage();
                orderpage.init();
                orderpage.HideSelector();
                $("#orderContainer").removeClass('noItems');
                orderpage.loadOrdersForClient(order.ClientID);
            }, 1000);
            //alert('ההזמנה התקבלה וממתינה להזנה לחשבשבת');
        }).fail(function (e) {
            if (e.status == 401) {
                alert("הנך מנותק, בבקשה התחבר מחדש ונסה שוב");
                //window.open("http://www.yashvekaria.com", "_blank");
            }
            else {
                alert("אירעה שגיאה");
            }
        });
    };

    var collectOrder = function () {
        return {
            ClientID: _clientsSelector.value,
            DiscountPercentage: Number(_discountTextbox.value),
            Comment: "",
            Items: getOrderItems()
        };
    };

    var getOrderItems = function () {
        var orderItems = [];
        for (var i in _allItems) {
            var orderItemViewModel = _allItems[i].getOrderItemViewModel();
            if (orderItemViewModel.IsValidItem()) {
                orderItems.push({ ItemID: orderItemViewModel.ProductID, Quantity: orderItemViewModel.Quantity, DiscountInShekels: 0 });
            }
        }
        return orderItems;
    };

    var initItemsContainer = function () {
        glamProductsService.loadProducts(function (products) {
            _products = products;

            //first item
            addItem();
        });

        $("#AddItemButton").click(function () {
            addItem();
        });
    };

    var init = function () {
        initSubmitOrder();

        initTotalPrice();

        initDiscount();

        initClients();

        initItemsContainer();
    };

    // prototype
    newOrderPage.prototype = {
        constructor: newOrderPage,
        init: init
    };

    // return productsLoader
    return newOrderPage;
})();