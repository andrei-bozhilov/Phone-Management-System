angular.module('app')

.controller('AdminGiveOrderController', function ($scope, $rootScope, adminRequests, transplateNameToEng) {
    $rootScope.location = 'orders';

    $scope.departments = {};
    $scope.userData = {};
    $scope.availablePhones = {}

    $scope.userData.fullname = "";
    $scope.userData.username = "";
    $scope.userData.password = "";

    $scope.userData.departmentId = "";

    $scope.change = function (data) {
        $scope.userData.username = transplateNameToEng(data);
        $scope.userData.password = $scope.userData.username;
    };

    adminRequests.getAllPhones('free')
    .success(function (data) {
        $scope.availablePhones = data;
    })
    .error(function (error) {
        console.log(error);
    });

    adminRequests.getAllDepartments()
    .success(function (data) {
        $scope.departments = data;
    })
    .error(function (error) {
        console.log(error);
    });


});