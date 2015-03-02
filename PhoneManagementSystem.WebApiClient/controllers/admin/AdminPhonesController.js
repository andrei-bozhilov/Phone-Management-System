angular.module('app')

.controller('AdminPhonesController', function ($scope, $rootScope, adminRequests) {
    $rootScope.location = 'phones';
    $scope.phones = {};

    adminRequests.getAllPhones()
    .success(function (data) {
        $scope.phones = data;
    })
    .error(function (error) {
        console.log(error);
    })
});