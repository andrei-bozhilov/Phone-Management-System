angular.module('app')

.controller('AdminTakeOrderController',
function ($scope, $rootScope, adminRequests, userSession) {
    if (!userSession.getCurrentUser()) {
        $location.path('/login');
    }

    $rootScope.location = 'orders';
    $rootScope.subLocation = 'take';
    $scope.users = [];
    $scope.phones = [];
    $scope.userInfo = {};
    $scope.fullname = "";
    $scope.phone = "";
    $scope.request = {};




    $scope.getAllUsers = function (forceToReload) {
        if (forceToReload) { // if need to refresh data make request, pass true
            adminRequests.getAllUsers()
           .success(function (data) {
               $scope.users = data;
           })
           .error(function (error) {
               console.log(error);
               notyService.error(helper.getResponseMessage(error));
           });
        } else {
            if ($scope.users.length === 0) { //this prevent onclick to make request, the request is made only ones
                adminRequests.getAllUsers()
                .success(function (data) {
                    $scope.users = data;
                })
                .error(function (error) {
                    console.log(error);
                    notyService.error(helper.getResponseMessage(error));
                });
            }
        }
    };

    $scope.getAllPhones = function (forceToReload) {
        if (forceToReload) { // if need to refresh data make request, pass true
            adminRequests.getAllPhones()
           .success(function (data) {
               $scope.phones = data;
           })
           .error(function (error) {
               console.log(error);
               notyService.error(helper.getResponseMessage(error));
           });
        } else {
            if ($scope.phones.length === 0) { //this prevent onclick to make request, the request is made only ones
                adminRequests.getAllPhones()
                .success(function (data) {
                    $scope.phones = data;
                })
                .error(function (error) {
                    console.log(error);
                    notyService.error(helper.getResponseMessage(error));
                });
            }
        }
    };

    $scope.search = function () {
        $scope.request.phone = $scope.phone;
        $scope.request.fullname = $scope.fullname;

        adminRequests.getUserInfo($scope.request)
        .success(function (data) {
            $scope.userInfo = data;
        })
        .error(function (error) {
            console.log(error);
            notyService.error(helper.getResponseMessage(error));
        })

    };

    $scope.phone = "0884933030";
    $scope.search();

});