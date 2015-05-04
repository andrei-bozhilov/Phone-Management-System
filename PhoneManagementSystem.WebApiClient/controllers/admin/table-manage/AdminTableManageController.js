angular.module('app')

.controller('AdminTableManageController',
function ($scope, $route, $rootScope, $location, helper, adminRequests, pagination, datepicker, userSession) {
    if (!userSession.getCurrentUser()) {
        $location.path('/login');
    }

    $rootScope.location = 'table-manage';
    $rootScope.subLocation = $route.current.params['table'];

    $scope.pagination = pagination;
    $scope.pagination.bigCurrentPage = 1;  //setup for Pagination (ui.bootstrap.pagination)


    $scope.data = {};
    $scope.request = {
        pageSize: 10,
        startPage: 1
    };

    $scope.getData = function () {
        switch ($rootScope.subLocation) {
            case "phones":
                adminRequests.getAllPhonesWithUserInfo($scope.request)
                .success(function (data) {
                    $scope.data = data;
                    $scope.pagination.bigTotalItems = $scope.data.numPages * 10;  //setup for Pagination (ui.bootstrap.pagination)
                })
                .error(function (error) {
                    console.log(error);
                })
                break;
            case "users":
                adminRequests.getAllUsersWithPaging($scope.request)
                .success(function (data) {
                    $scope.data = data;
                    $scope.pagination.bigTotalItems = $scope.data.numPages * 10;  //setup for Pagination (ui.bootstrap.pagination)
                })
                .error(function (error) {
                    console.log(error);
                })
                break;
            case "departments":
                adminRequests.getAllDepartmentsWithPaging($scope.request)
                .success(function (data) {
                    $scope.data = data;
                    $scope.pagination.bigTotalItems = $scope.data.numPages * 10;  //setup for Pagination (ui.bootstrap.pagination)
                })
                .error(function (error) {
                    console.log(error);
                })
                break;
            case "job-titles":
                adminRequests.getAllJobTitlesWithPaging($scope.request)
                .success(function (data) {
                    $scope.data = data;
                    $scope.pagination.bigTotalItems = $scope.data.numPages * 10;  //setup for Pagination (ui.bootstrap.pagination)
                })
                .error(function (error) {
                    console.log(error);
                })


            default:
                break;
        }
    }





    $scope.getData();
});