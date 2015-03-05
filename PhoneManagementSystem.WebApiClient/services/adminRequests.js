angular.module('app')

.factory('adminRequests', function (requester, baseUrl, userSession) {

    var url = '';
    var data = {};
    var headers = {};

    var adminRequests = {
        getAllPhones: function (status) {
            if (!status) {
                status = "";
            }
            url = baseUrl + 'admin/phones?phoneStatus=' + status;
            headers = {
                Authorization: 'Bearer ' + userSession.getToken()
            };

            return requester.get(url, headers);
        },

        getAllDepartments: function () {
            url = baseUrl + 'departments';

            return requester.get(url);
        },

        getAllJobTitles: function () {
            url = baseUrl + 'jobTitles';

            return requester.get(url);
        },

        getAllOrders: function () {
            url = baseUrl + 'admin/orders';
            headers = {
                Authorization: 'Bearer ' + userSession.getToken()
            };

            return requester.get(url, headers);
        },
        getAllUsers: function () {
            url = baseUrl + 'admin/users';
            headers = {
                Authorization: 'Bearer ' + userSession.getToken()
            };

            return requester.get(url, headers);
        },
        givePhoneOrder: function (data) {
            url = baseUrl + 'admin/orders/GivePhone';
            headers = {
                Authorization: 'Bearer ' + userSession.getToken()
            };

            return requester.post(url, data, headers);
        }
    }


    return adminRequests;
});