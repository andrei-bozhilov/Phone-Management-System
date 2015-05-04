angular.module('app')

.factory('adminRequests', function (requester, baseUrl, userSession, helper) {

    var url = '';
    var data = {};
    var headers = {};

    var adminRequests = {
        getAllPhones: function (obj) {
            url = baseUrl + 'phones' + helper.objectToUrlParams(obj);
            headers = {
                Authorization: 'Bearer ' + userSession.getToken()
            };

            return requester.get(url, headers);
        },

        getAllPhonesWithUserInfo: function (obj) {
            url = baseUrl + 'phones/UserInfo' + helper.objectToUrlParams(obj);
            headers = {
                Authorization: 'Bearer ' + userSession.getToken()
            };

            return requester.get(url, headers);
        },

        getAllDepartments: function (obj) {
            url = baseUrl + 'departments';

            return requester.get(url);
        },

        getAllDepartmentsWithPaging: function (obj) {
            url = baseUrl + 'departments/paging' + helper.objectToUrlParams(obj);

            headers = {
                Authorization: 'Bearer ' + userSession.getToken()
            };

            return requester.get(url, headers);
        },

        getAllJobTitles: function (obj) {
            url = baseUrl + 'jobTitles';

            return requester.get(url);
        },

        getAllJobTitlesWithPaging: function (obj) {
            url = baseUrl + 'jobTitles/paging' + helper.objectToUrlParams(obj);
            headers = {
                Authorization: 'Bearer ' + userSession.getToken()
            };

            return requester.get(url, headers);
        },

        getAllOrders: function (obj) {
            url = baseUrl + 'admin/orders' + helper.objectToUrlParams(obj);
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

        getAllUsersWithPaging: function (obj) {
            url = baseUrl + 'admin/users/paging' + helper.objectToUrlParams(obj);
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
        },

        getUserInfo: function (obj) {
            url = baseUrl + 'admin/users/UserInfo' + helper.objectToUrlParams(obj);
            headers = {
                Authorization: 'Bearer ' + userSession.getToken()
            };

            return requester.get(url, headers);
        }
    }


    return adminRequests;
});