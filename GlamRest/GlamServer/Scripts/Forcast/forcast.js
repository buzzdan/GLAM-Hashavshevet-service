/// <reference path="../Extentions/jquery.json-2.4.min.js" />
/// <reference path="../NewOrder/GlamProductsService.js" />
function MoedForcastProductRow(item) {
    var self = this;
    this.item = item;
    var row;
    var itemIdCell;
    var quntityCell;

    var ctor = function (item) {
        row = document.createElement('tr');

        itemIdCell = createTableCellWithValue(item.ProductID);
        itemNameCell = createTableCellWithValue(item.ProductName);
        quntityCell = createTableCellWithValue(item.Quantity);

        row.appendChild(itemIdCell);
        row.appendChild(itemNameCell);
        row.appendChild(quntityCell);
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

function MonthlyProductForcastRow(month, quantity) {
    var self = this;
    var row;
    var monthCell;
    var quntityCell;

    var ctor = function (month, quantity) {
        row = document.createElement('tr');

        var monthName = getMonthNameBy(month);
        monthCell = createTableCellWithValue(monthName);
        quntityCell = createTableCellWithValue(quantity);

        row.appendChild(monthCell);
        row.appendChild(quntityCell);
    };

    var getMonthNameBy = function (monthNumber) {
        switch (monthNumber) {
            case "1":
            case "13":
                return "ינואר";
            case "2":
            case "14":
                return "פברואר";
            case "3":
            case "15":
                return "מרץ";
            case "4":
            case "16":
                return "אפריל";
            case "5":
            case "17":
                return "מאי";
            case "6":
            case "18":
                return "יוני";
            case "7":
            case "19":
                return "יולי";
            case "8":
            case "20":
                return "אוגוסט";
            case "9":
            case "21":
                return "ספטמבר";
            case "10":
            case "22":
                return "אוקטובר";
            case "11":
            case "23":
                return "נובמבר";
            case "12":
            case "24":
                return "דצמבר";
        }
    };
    this.getRow = function () {
        return row;
    };

    var createTableCellWithValue = function (value) {
        var cell = document.createElement('td');
        cell.innerText = value;
        return cell;
    };

    ctor(month, quantity);
}

function forcastPage() {
    var _products;
    var productsLoader;
    var holidaysSelector;
    var productsForcastSelector;
    var dateProductForcastSelector;

    this.initPage = function () {
        productsLoader = new GlamProductsService();
        loadProducts();
        loadHolidays();
        registerToSubmitForcastsEvents();
    };

    var registerToSubmitForcastsEvents = function () {
        holidaysSelector.onchange = RunSubmitMoed;
        productsForcastSelector.onchange = RunSubmitForcastByProduct;
        var button = document.getElementById("doForcastByProductDates");
        button.onclick = RunDatesProductForcast;
    };

    var RunDatesProductForcast = function () {
        var forcastDetails = collectForcast();
        var resultsContainer = document.getElementById('productDatesForcastResults');
        $(resultsContainer).hide('fast');

        $.post('/api/Forcast/ForcastService/ProductsByDates', forcastDetails, function (results) {
            if (results.length > 0) {
                var resultGrid = generateProductForcastResultTable(results);
                resultsContainer.innerHTML = "";
                resultsContainer.appendChild(resultGrid);
                $(resultsContainer).removeClass('noItems');
            }
            else {
                $(resultsContainer).addClass("noItems");
                var classes = $(resultsContainer).attr('class');
                resultsContainer.innerHTML = "לא נמצאו תחזיות למוצר זה";
            }
            $(resultsContainer).show('fast');
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

    var collectForcast = function () {
        var productID = $(dateProductForcastSelector).val();
        var startDate = $("#from").datepicker('getDate').toJSON();
        var endDate = $("#to").datepicker('getDate').toJSON();

        return {
            ProductID: productID,
            StartDate: startDate,
            EndDate: endDate
        };
    };
    var generateProductForcastResultTable = function (results) {
        var div = document.createElement("div");
        div.setAttribute("class", "CSSTableGenerator");

        var resultsTable = document.createElement("table");
        resultsTable.setAttribute("class", "MoedForcastResultsGrid");

        resultsTable.innerHTML = "<tr><td>חודש</td><td>חיזוי כמות</td></tr>";
        var quantities = results[0];
        for (var month in quantities) {
            var quantity = quantities[month];
            var productRow = new MonthlyProductForcastRow(month, quantity);
            resultsTable.appendChild(productRow.getRow());
        }

        div.appendChild(resultsTable);
        return div;
    };

    var RunSubmitForcastByProduct = function () {
        var productID = $(productsForcastSelector).val();
        var resultsContainer = document.getElementById('productForcastResults');
        $(resultsContainer).hide('fast');

        $.getJSON('/api/Forcast/ForcastService/Products/' + productID, function (results) {
            if (results.length > 0) {
                var resultGrid = generateProductForcastResultTable(results);
                resultsContainer.innerHTML = "";
                resultsContainer.appendChild(resultGrid);
                $(resultsContainer).removeClass('noItems');
            }
            else {
                $(resultsContainer).addClass("noItems");
                var classes = $(resultsContainer).attr('class');
                resultsContainer.innerHTML = "לא נמצאו תחזיות למוצר זה";
            }
            $(resultsContainer).show('fast');
        });
    };

    var generateMoedResultTable = function (products) {
        var div = document.createElement("div");
        div.setAttribute("class", "CSSTableGenerator");

        var resultsTable = document.createElement("table");
        resultsTable.setAttribute("id", "MoedForcastResultsGrid");

        resultsTable.innerHTML = "<tr><td>מספר מוצר</td><td>שם מוצר</td><td>חיזוי כמות</td></tr>";
        for (var product in products) {
            var productRow = new MoedForcastProductRow(products[product]);
            resultsTable.appendChild(productRow.getRow());
        }

        div.appendChild(resultsTable);
        return div;
    };

    var RunSubmitMoed = function () {
        var resultsContainer = document.getElementById('moedResults');
        $(resultsContainer).hide('fast');

        var moedID = $(holidaysSelector).val();
        $.getJSON('/api/Forcast/ForcastService/Moed/' + moedID, function (results) {
            if (results.length > 0) {
                var resultGrid = generateMoedResultTable(results);
                resultsContainer.innerHTML = "";
                resultsContainer.appendChild(resultGrid);
                $(resultsContainer).removeClass('noItems');
            }
            else {
                $(resultsContainer).addClass("noItems");
                var classes = $(resultsContainer).attr('class');
                resultsContainer.innerHTML = "לא נמצאו מוצרים תחת המועד הנבחר";
            }
            $(resultsContainer).show('fast');
        });
    };

    var loadHolidays = function () {
        holidaysSelector = document.getElementById("selectMe2");
        productsLoader.loadHolidays(function (holidays) {
            for (var i in holidays) {
                holidaysSelector.add(new Option(holidays[i].HolidayName, holidays[i].HolidayID));
            }
        });
    };

    var loadProducts = function () {
        var selector = document.createElement("select");
        dateProductForcastSelector = document.getElementById("selectMe0");
        productsForcastSelector = document.getElementById("selectMe1");

        productsLoader.loadProducts(function (products) {
            _products = products;
            for (var i in _products) {
                selector.add(new Option(_products[i].ProductID + " - " + _products[i].ProductName, _products[i].ProductID));
            }

            dateProductForcastSelector.innerHTML = selector.innerHTML;
            productsForcastSelector.innerHTML = selector.innerHTML;
        });
    };

    this.initJquryUIItems = function () {
        $("#from").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            showOn: "button",
            buttonImage: "../../Content/images/calendar.gif",
            buttonImageOnly: false,
            dateFormat: "dd-mm-yy",
            numberOfMonths: 2,
            minDate: "today",
            maxDate: "+12M",
            onClose: function (selectedDate) {
                $("#to").datepicker("option", "minDate", selectedDate);
            }
        });

        $("#to").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            dateFormat: "dd-mm-yy",
            showOn: "button",
            buttonImage: "../../Content/images/calendar.gif",
            buttonImageOnly: false,
            numberOfMonths: 2,
            minDate: "today",
            maxDate: "+12M",
            onClose: function (selectedDate) {
                $("#from").datepicker("option", "maxDate", selectedDate);
            }
        });
        $("#tabs").tabs();
    };
}