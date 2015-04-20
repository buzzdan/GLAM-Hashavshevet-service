/// <reference path="../jquery-1.9.1.js" />
/// <reference path="jquery.json-2.4.min.js" />

$.postJSON = function (url, data, callback) {
    return jQuery.ajax({
        'type': 'POST',
        'url': url,
        'contentType': 'application/json',
        'data': $.toJSON(data),
        'dataType': 'json',
        'success': callback
    });
};