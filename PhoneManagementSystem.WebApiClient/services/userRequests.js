angular.module('app')

.factory('userRequests', function (baseUrl, requester) {

    var url = "";
    var data = {};

    var userRequests = {
        login: function (username, password) {

            url = baseUrl + 'user/login'
            data = {
                username: username,
                password: password
            };

            return requester.post(url, data);
        }
    };


    return userRequests;
});