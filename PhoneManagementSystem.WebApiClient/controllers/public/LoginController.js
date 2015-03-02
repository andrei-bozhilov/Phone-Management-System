/// <reference path="../../services/requester.js" />
angular.module('app')

.controller('LoginController', function ($scope,$rootScope, $location, userRequests, userSession) {   

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
        })
        .error(function (error) {
            console.log(error);            

        }).then();
    };

    
});