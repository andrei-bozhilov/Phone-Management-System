angular.module('app')

.factory('transplateNameToEng', function () {
    function translate(data) {
        //var data = "";
        var input = data.toLocaleLowerCase().replace('-', ' ').split(' ');
        //input = [];
        var firstLetter = input[0][0];

        var lastName = input[input.length - 1];
        var result = firstLetter + lastName;

        

        function translateChar(char) {
            var result = "";
            //to do
            switch(char) {
                case "a":
                    result = "a"
                    break;
                case "a":
                    result = "a"
                    break;
                case "a":
                    result = "a"
                    break;
                default:
               
            }

        }

        console.log(input);
        console.log(firstLetter);
        console.log(lastName);

        return result != "undefined" ? result : "No value";
    }


    return translate;
})