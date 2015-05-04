angular.module('app')

.directive('tableGrid', function () {

    var controller = function ($scope, $location, $route) {
        var table = angular.element('table');

        table.on('click', 'tr', function ($ev) {
            var id = $ev.currentTarget.cells[0].innerText;
            var currentPath = $location.path();
            $location.path(currentPath + '/' + id);
            $scope.$apply();
        });

    };
    return {
        restrict: 'EA', //E = element, A = attribute, C = class, M = comment
        scope: {
            datasource: '=',
        },
        controller: controller,
        templateUrl: 'directives/tableGrid.html',
        link: function ($scope, element, attrs) {

        }
    }
})