function OrderRow(order) {
    var self = this;
    this.order = order;
    var row;
    var orderIdCell;
    var orderStatusCell;
    var orderDiscountCell;
    this.onRowClicked;

    var ctor = function (order) {
        row = document.createElement('tr');
        row.onclick = function () {
            if (self.onRowClicked) {
                self.onRowClicked(self.order);
            }
        };

        orderIdCell = createTableCellWithValue(order.OrderID);
        orderStatusCell = createTableCellWithValue(convertStatus(order.Status));
        orderDiscountCell = createTableCellWithValue(order.DiscountPercentage + "%");
        var orderDate = new Date(order.OrderDate);
        var momentWrapper = moment(orderDate);
        orderDateCell = createTableCellWithValue(momentWrapper.format("DD-MM-YYYY"));

        row.appendChild(orderIdCell);
        row.appendChild(orderStatusCell);
        row.appendChild(orderDiscountCell);
        row.appendChild(orderDateCell);
    };

    this.getRow = function () {
        return row;
    };

    var convertStatus = function (statusID) {
        switch (statusID) {
            case 1:
                return "חדש";
            case 2:
                return "בטיפול";
            case 3:
                return "הוכנס בהצלחה";
            case 4:
                return "נכשל";
            default:
        }
    };

    var createTableCellWithValue = function (value) {
        var cell = document.createElement('td');
        cell.innerText = value;
        return cell;
    };

    ctor(order);
}

function OrderItemRow(item) {
    var self = this;
    this.item = item;
    var row;
    var itemIdCell;
    var quntityCell;
    this.totalPrice;

    var ctor = function (item) {
        row = document.createElement('tr');

        itemIdCell = createTableCellWithValue(item.ItemID);
        itemNameCell = createTableCellWithValue(item.ProductDetails.ProductName);
        itemPriceCell = createTableCellWithValue(item.ProductDetails.Price);
        quntityCell = createTableCellWithValue(item.Quantity);

        self.totalPrice = parseInt(item.Quantity) * parseInt(item.ProductDetails.Price);
        totalPriceCell = createTableCellWithValue(self.totalPrice);

        row.appendChild(itemIdCell);
        row.appendChild(itemNameCell);
        row.appendChild(itemPriceCell);
        row.appendChild(quntityCell);
        row.appendChild(totalPriceCell);
    };

    this.getRow = function () {
        return row;
    };

    var createTableCellWithValue = function (value) {
        var cell = document.createElement('td');
        cell.innerText = value;
        return cell;
    };

    ctor(item);
}

function OrdersBrowserPage() {
    var self = this;
    var mainContainer;
    var _clientsSelector;
    var clientsContainer;
    var ordersGrid;
    var noItemsDiv;
    var orderRows = [];
    var itemRows = [];
    var orderItemsGrid;
    var orderDetailsContainer;
    var currentOrder;
    var gridWrapper;
    var currentClientID;

    var ctor = function () {
        mainContainer = document.getElementById("orderBrowserContainer");
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

    this.loadOrdersForClient = function (clientID) {
        currentClientID = clientID;
        setItemsGridVisibility(false);

        //api/clients/10001/orders
        $.getJSON('/api/clients/' + clientID + '/orders', function (orders, status, xhr) {
            var clientHasOrders = !arrayIsNullOrEmpty(orders);
            if (clientHasOrders) {
                resetGrid();
                fillOrdersGrid(orders);
            }
            setOrdersGridVisibility(clientHasOrders);
        });
    };

    var setOrdersGridVisibility = function (show) {
        if (show) {
            $(gridWrapper).show('fast');
            $(noItemsDiv).hide('fast');
        } else {
            $(gridWrapper).hide('fast');
            $(noItemsDiv).show('fast');
        }
    };

    var setItemsGridVisibility = function (show) {
        if (show) {
            $(orderDetailsContainer).show('fast');
        } else {
            $(orderDetailsContainer).hide('fast');
        }
    };

    var arrayIsNullOrEmpty = function (array) {
        return array == undefined || array == null || array.length == 0
    };

    var fillOrdersGrid = function (orders) {
        orderRows = [];
        for (var i in orders) {
            var order = orders[i];
            var orderRow = new OrderRow(order);
            orderRow.onRowClicked = showOrderDetails;
            ordersGrid.appendChild(orderRow.getRow());
            orderRows[order.OrderID] = orderRow;
        }
    };

    var fillItemsGrid = function (items) {
        itemRows = [];
        for (var i in items) {
            var item = items[i];
            var itemRow = new OrderItemRow(item);
            orderItemsGrid.appendChild(itemRow.getRow());
            itemRows[item.ItemID] = itemRow;
        }
    };

    var showOrderDetails = function (order) {
        currentOrder = order;
        $.getJSON('/api/order/' + order.OrderID, function (order, status, xhr) {
            resetOrderItemsGrid();
            fillItemsGrid(order.Items);

            var totalOrderPrice = 0;
            for (var i in itemRows) {
                totalOrderPrice += itemRows[i].totalPrice;
            }

            var statusLabel = document.getElementById("statusLabel");
            statusLabel.innerText = "סטטוס: " + convertStatus(order.Status);

            var orderDiscountLabel = document.getElementById("orderDiscountLabel");
            orderDiscountLabel.innerText = "הנחה: " + order.DiscountPercentage + "%";

            var orderTotalPriceLabel = document.getElementById("orderTotalPriceLabel");
            totalOrderPrice = totalOrderPrice * (order.DiscountPercentage + 100) / 100;
            orderTotalPriceLabel.innerText = "מחיר כולל: " + totalOrderPrice;

            $(noItemsDiv).hide();
            $(gridWrapper).hide('fast');
            setItemsGridVisibility(true);
        });
    };

    var convertStatus = function (statusID) {
        switch (statusID) {
            case 1:
                return "חדש";
            case 2:
                return "בטיפול";
            case 3:
                return "הוכנס בהצלחה";
            case 4:
                return "נכשל";
            default:
        }
    };

    var onClientChange = function () {
        //if (!currentClientID || currentClientID == 0) {
            currentClientID = _clientsSelector.value;
        //}
        self.loadOrdersForClient(currentClientID);
    };

    var createClientsSelector = function (callback) {
        var selector = document.createElement("select");
        selector.setAttribute("name", "clientsPicker");
        selector.setAttribute("id", "clientsPicker");
        selector.style.width = "300px";
        selector.onchange = onClientChange;

        loadClients(selector, callback);

        return selector;
    };

    var initClients = function () {
        clientsContainer = document.createElement("div");
        clientsContainer.setAttribute("id", "clientsPickerContainer");
        clientsContainer.innerText = "Loading...";
        mainContainer.appendChild(clientsContainer);

        _clientsSelector = createClientsSelector(function () {
            clientsContainer.innerText = "";
            clientsContainer.appendChild(_clientsSelector);
            $("#clientsPicker").select2();
        });
    };

    var initOrdersGrid = function () {
        gridWrapper = document.createElement("div");
        mainContainer.appendChild(gridWrapper);
        $(gridWrapper).hide();

        var button = initButtons();
        gridWrapper.appendChild(button);

        var div = document.createElement("div");
        div.setAttribute("class", "CSSTableGenerator");

        ordersGrid = document.createElement("table");
        ordersGrid.setAttribute("id", "ordersGrid");
        resetGrid();

        div.appendChild(ordersGrid);
        gridWrapper.appendChild(div);

        noItemsDiv = document.createElement("div");
        noItemsDiv.setAttribute("id", "NoItems");
        $(noItemsDiv).hide();
        noItemsDiv.innerText = "לא נמצאו הזמנות";
        mainContainer.appendChild(noItemsDiv);

        //mainContainer.appendChild(pager);

        //ordersGrid.insertRow(0);
        //mainContainer
    };

    var resetOrderItemsGrid = function () {
        $(orderItemsGrid).html("<tr><td>מספר פריט</td><td>שם פריט</td><td>מחיר פריט</td><td>כמות</td><td>מחיר כולל</td></tr>");
    };

    var initOrderDetailsContainer = function () {
        orderDetailsContainer = document.createElement("div");
        orderDetailsContainer.setAttribute("id", "orderDetailsContainer");
        $(orderDetailsContainer).hide();

        mainContainer.appendChild(orderDetailsContainer);

        var backToOrdersButton = document.createElement("input");
        backToOrdersButton.setAttribute("type", "button");
        backToOrdersButton.setAttribute("value", "<-- חזרה לעמוד ההזמנות");
        backToOrdersButton.onclick = goBackToOrdersGrid
        orderDetailsContainer.appendChild(backToOrdersButton);

        var statusLabel = document.createElement("span");
        statusLabel.setAttribute("id", "statusLabel");
        orderDetailsContainer.appendChild(statusLabel);

        var orderDiscountLabel = document.createElement("span");
        orderDiscountLabel.setAttribute("id", "orderDiscountLabel");
        orderDetailsContainer.appendChild(orderDiscountLabel);

        var orderTotalPriceLabel = document.createElement("span");
        orderTotalPriceLabel.setAttribute("id", "orderTotalPriceLabel");
        orderDetailsContainer.appendChild(orderTotalPriceLabel);

        initOrderItemsGrid();
    };

    var goBackToOrdersGrid = function () {
        $(orderDetailsContainer).hide();
        setOrdersGridVisibility(true);
    };

    var initOrderItemsGrid = function () {
        var div = document.createElement("div");
        div.setAttribute("class", "CSSTableGenerator");

        orderItemsGrid = document.createElement("table");
        orderItemsGrid.setAttribute("id", "orderItemsGrid");
        resetOrderItemsGrid();

        div.appendChild(orderItemsGrid);
        orderDetailsContainer.appendChild(div);
    };

    var resetGrid = function () {
        $(ordersGrid).html("<tr><td>מספר הזמנה</td><td>סטטוס</td><td>הנחה באחוזים</td><td>תאריך הזמנה</td></tr>");
    };

    var initButtons = function () {
        var refreshButton = document.createElement("input");
        refreshButton.setAttribute("type", "button");
        refreshButton.setAttribute("value", "רענון");
        refreshButton.onclick = refreshOrders;
        return refreshButton;
        //mainContainer.appendChild(refreshButton);
    };

    var refreshOrders = function () {
        self.loadOrdersForClient(currentClientID);
    };

    this.HideSelector = function () {
        $(_clientsSelector).hide();
    };
    this.init = function () {
        initClients();
        //initButtons();
        initOrdersGrid();
        initOrderDetailsContainer();
    };

    ctor();
}