angular.module('app')

.factory('requester', ['$http', 'baseUrl', function ($http, baseUrl) {
    var requester = {};

    function baseRequest(url, method, data, headers, params) {
        var http = $http(
                    {
                        url: url,
                        method: method,
                        data: JSON.stringify(data) || undefined,
                        headers: headers,
                        params: params || undefined

                    });

        return http;
    }

    var makeGetRequest = function (url, headers, params) {
        return baseRequest(url, 'GET', null, headers, params);
    }

    var makePostRequest = function (url, data, headers, params) {
        return baseRequest(url, 'POST', data, headers, params);
    }

    var makePutRequest = function (url, data, headers, params) {
        return baseRequest(url, 'PUT', data, headers, params);
    }

    var makeDeleteRequest = function (url, headers, params) {
        return baseRequest(url, 'DELETE', null, headers, params);
    }

    requester = {
        get: makeGetRequest,
        post: makePostRequest,
        put: makePutRequest,
        delete: makeDeleteRequest
    }

    return requester;
}]);

