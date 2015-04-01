angular.module('app')

.controller('AdminPhonesController', function ($scope, $rootScope, adminRequests) {
    $rootScope.location = 'table-manage';
    $rootScope.subLocation = 'phones';
    console.log($rootScope.location);

    $scope.phones = {};

    adminRequests.getAllPhones()
    .success(function (data) {
        $scope.phones = data;
    })
    .error(function (error) {
        console.log(error);
    })
});