/// <reference path="../../services/requester.js" />
angular.module('app')

.controller('LoginController', function ($scope, $rootScope, $location, userRequests, userSession, notyService, helper) {

    if (userSession.getCurrentUser()) {
        $location.path('user/home');
    }

    $scope.username = 'admin';
    $scope.password = 'admin';

    $scope.login = function login() {
        userRequests.login($scope.username, $scope.password)
        .success(function (data) {
            userSession.login(data);
            $location.path('user/home');
            notyService.success("Login successfully.");
        })
        .error(function (error) {
            notyService.error("Login error: Invalid username or password");
            console.log(error);
        });
    };
});