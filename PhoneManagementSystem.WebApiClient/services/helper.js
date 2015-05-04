angular.module('app')

.factory('helper', function (userSession, $location) {
    var helper = {
        objectToUrlParams: function (obj) {
            var result = "?";
            for (var prop in obj) {
                result += (prop + "=");
                result += (obj[prop] + "&");

            }

            result[result.length - 1] = "";

            return result;
        },
        getResponseMessage: function (obj) {
            var responceMessage = obj.Message + "\n";

            if (obj.ModelState) {
                for (var state in obj.ModelState) {
                    console.log(state);
                    for (var prop in obj.ModelState[state]) {
                        console.log(prop);
                        responceMessage += obj.ModelState[state][prop] + "\n";
                    }
                }
            }
            return responceMessage;
        },

        redirectNotLoginUser: function () {
            if (!userSession.getCurrentUser()) {
                $location.path('#/');
            }
        }
    };

    return helper
})