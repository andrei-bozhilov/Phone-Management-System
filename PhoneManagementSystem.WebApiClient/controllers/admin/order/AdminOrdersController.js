angular.module('app')

.controller('AdminOrdersController', function ($scope, $rootScope, adminRequests) {
    $rootScope.location = 'orders';
    $scope.orders = [];



    adminRequests.getAllOrders()
    .success(function (data) {
        $scope.orders = data;
    })
    .error(function (error) {
        console.log(error);
    })
    .then(function () {
        $('table').stickyTableHeaders({ fixedOffset: $('header') });
    })

});