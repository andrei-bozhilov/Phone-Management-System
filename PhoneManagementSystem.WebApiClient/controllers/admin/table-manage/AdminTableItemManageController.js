angular.module('app')

.controller('AdminTableItemManageController',
function ($scope, $route, $rootScope, $location, helper, adminRequests, userSession) {
    if (!userSession.getCurrentUser()) {
        $location.path('/login');
    }

    //$rootScope.location = 'table-manage';
    // $rootScope.subLocation = $route.current.params['table'];


});