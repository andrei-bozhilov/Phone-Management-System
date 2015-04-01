angular.module('app')

.controller('AdminOrdersController', function ($scope, $rootScope, adminRequests) {
    $rootScope.location = 'orders';
    $rootScope.subLocation = 'orders';

    $scope.orders = [];
    $scope.admins = [];
    $scope.actions = [];

    $scope.request = {};

    $scope.request.adminId = "";
    $scope.request.action = "";
    $scope.request.fromDate = "";
    $scope.request.toDate = "";

    //click
    $scope.getOrdersWithFilter = function () {
        console.log($scope.request);
    }

    //requests
    adminRequests.getAllOrders()
    .success(function (data) {
        $scope.orders = data;
    })
    .error(function (error) {
        console.log(error);
    })
    .then(function () {
        $('table').stickyTableHeaders({ fixedOffset: $('header') });
    });

    //datepicker
    $scope.openFrom = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedFrom = true;
    };

    $scope.openTo = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.openedTo = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };
});