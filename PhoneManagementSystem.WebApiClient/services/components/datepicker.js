angular.module('app')

.factory('datepicker', function () {
    var datepicker = {
        dateOptions: {
            formatYear: 'yy',
            startingDay: 1
        },

        format: "dd.MM.yyyy"
    }

    return datepicker
})