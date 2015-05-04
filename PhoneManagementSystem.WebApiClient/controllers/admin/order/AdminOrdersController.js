angular.module('app')

.controller('AdminOrdersController',
function ($scope, $rootScope, adminRequests, pagination, datepicker, userSession) {
    if (!userSession.getCurrentUser()) {
        $location.path('/login');
    }
    $rootScope.location = 'orders';
    $rootScope.subLocation = 'orders';

    $scope.orders = {};
    $scope.datepicker = datepicker; //datepicker
    $scope.pagination = pagination;

    $scope.request = {
        pageSize: 10,
        startPage: 1
    };

    $scope.request.phoneAction = "";
    $scope.request.fromDate = "";
    $scope.request.toDate = "";

    $scope.takePhone = false;
    $scope.giveBackPhone = false;
    $scope.takeForPrivateUse = false;


    //click
    $scope.clearFilter = function () {
        $scope.request = {
            pageSize: 10,
            startPage: 1
        };

        $scope.request.phoneAction = "";
        $scope.request.fromDate = "";
        $scope.request.toDate = "";

        $scope.takePhone = false;
        $scope.giveBackPhone = false;
        $scope.takeForPrivateUse = false;

        $scope.getOrdersWithFilter();
    };

    $scope.getOrdersWithFilter = function () {
        $scope.request.fromDate = ($scope.request.fromDate == ""
            || $scope.request.fromDate == null
            || $scope.request.fromDate == undefined) ? "" : (new Date($scope.request.fromDate)).toISOString();

        $scope.request.toDate = ($scope.request.toDate == ""
            || $scope.request.toDate == null
            || $scope.request.toDate == undefined) ? "" : (new Date($scope.request.toDate)).toISOString();

        $scope.request.phoneAction = "";
        if ($scope.takePhone) {
            $scope.request.phoneAction += "TakePhone|";
        }

        if ($scope.giveBackPhone) {
            $scope.request.phoneAction += "GiveBackPhone|";
        }

        if ($scope.takeForPrivateUse) {
            $scope.request.phoneAction += "GetPhoneForPrivateUse|";
        }

        //get rid of the last "|";
        $scope.request.phoneAction = $scope.request.phoneAction.substr(0, $scope.request.phoneAction.length - 1);

        console.log($scope.request);
        $scope.getOrders();
    };

    //requests
    $scope.getOrders = function getOrders() {
        adminRequests.getAllOrders($scope.request)
        .success(function (data) {
            $scope.orders = data;
            $scope.pagination.bigTotalItems = data.numPages * 10;  //setup for Pagination (ui.bootstrap.pagination)

        })
        .error(function (error) {
            console.log(error);
        });
    };

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

    $scope.getOrders();
});