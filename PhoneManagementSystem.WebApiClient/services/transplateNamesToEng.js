angular.module('app')

.factory('transplateNameToEng', function () {
    function translate(data) {
        //var data = "";
        var input = data.toLocaleLowerCase().replace('-', ' ').split(' ');
        //input = [];
        var firstLetter = input[0][0];

        var lastName = input[input.length - 1];
        var result = firstLetter + lastName;



        function translateChar(name) {
            var translateResult = "";
            var chResult = "";

            for (var ch in name) {

                console.log(name);
                switch (name[ch]) {
                    //bg
                    case 'а': chResult = "a"; translateResult += chResult; break;
                    case 'б': chResult = "b"; translateResult += chResult; break;
                    case 'в': chResult = "v"; translateResult += chResult; break;
                    case 'г': chResult = "g"; translateResult += chResult; break;
                    case 'д': chResult = "d"; translateResult += chResult; break;
                    case 'е': chResult = "e"; translateResult += chResult; break;
                    case 'ж': chResult = "zh"; translateResult += chResult; break;
                    case 'з': chResult = "z"; translateResult += chResult; break;
                    case 'и': chResult = "i"; translateResult += chResult; break;
                    case 'й': chResult = "y"; translateResult += chResult; break;
                    case 'к': chResult = "k"; translateResult += chResult; break;
                    case 'л': chResult = "l"; translateResult += chResult; break;
                    case 'м': chResult = "m"; translateResult += chResult; break;
                    case 'н': chResult = "n"; translateResult += chResult; break;
                    case 'о': chResult = "o"; translateResult += chResult; break;
                    case 'п': chResult = "p"; translateResult += chResult; break;
                    case 'р': chResult = "r"; translateResult += chResult; break;
                    case 'с': chResult = "s"; translateResult += chResult; break;
                    case 'т': chResult = "t"; translateResult += chResult; break;
                    case 'у': chResult = "u"; translateResult += chResult; break;
                    case 'ф': chResult = "f"; translateResult += chResult; break;
                    case 'х': chResult = "h"; translateResult += chResult; break;
                    case 'ч': chResult = "ch"; translateResult += chResult; break;
                    case 'ш': chResult = "sh"; translateResult += chResult; break;
                    case 'щ': chResult = "sht"; translateResult += chResult; break;
                    case 'ц': chResult = "c"; translateResult += chResult; break;
                    case 'ъ': chResult = "a"; translateResult += chResult; break;
                    case 'ь': chResult = "y"; translateResult += chResult; break;
                    case 'ю': chResult = "yu"; translateResult += chResult; break;
                    case 'я': chResult = "ya"; translateResult += chResult; break;
                    case ' ': chResult = " "; translateResult += chResult; break;
                    case '-': chResult = "-"; translateResult += chResult; break;
                        //eng

                    case 'a': chResult = "a"; translateResult += chResult; break;
                    case 'b': chResult = "b"; translateResult += chResult; break;
                    case 'c': chResult = "c"; translateResult += chResult; break;
                    case 'd': chResult = "d"; translateResult += chResult; break;
                    case 'e': chResult = "e"; translateResult += chResult; break;
                    case 'f': chResult = "f"; translateResult += chResult; break;
                    case 'g': chResult = "g"; translateResult += chResult; break;
                    case 'h': chResult = "h"; translateResult += chResult; break;
                    case 'i': chResult = "i"; translateResult += chResult; break;
                    case 'j': chResult = "j"; translateResult += chResult; break;
                    case 'k': chResult = "k"; translateResult += chResult; break;
                    case 'l': chResult = "l"; translateResult += chResult; break;
                    case 'm': chResult = "m"; translateResult += chResult; break;
                    case 'n': chResult = "n"; translateResult += chResult; break;
                    case 'o': chResult = "o"; translateResult += chResult; break;
                    case 'p': chResult = "p"; translateResult += chResult; break;
                    case 'q': chResult = "q"; translateResult += chResult; break;
                    case 'r': chResult = "r"; translateResult += chResult; break;
                    case 's': chResult = "s"; translateResult += chResult; break;
                    case 't': chResult = "t"; translateResult += chResult; break;
                    case 'u': chResult = "u"; translateResult += chResult; break;
                    case 'v': chResult = "v"; translateResult += chResult; break;
                    case 'w': chResult = "w"; translateResult += chResult; break;
                    case 'x': chResult = "x"; translateResult += chResult; break;
                    case 'y': chResult = "y"; translateResult += chResult; break;
                    case 'z': chResult = "z"; translateResult += chResult; break;

                    default: translateResult += "Unknown symbol";
                        break;
                }
            }
            return translateResult;

        }

        console.log(translateChar(input));
        console.log(translateChar(firstLetter));
        console.log(lastName);

        return result != "undefined" ? translateChar(result) : "No value";
    }


    return translate;
})