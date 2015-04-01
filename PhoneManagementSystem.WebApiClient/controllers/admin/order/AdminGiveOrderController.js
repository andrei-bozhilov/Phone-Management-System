angular.module('app')

.controller('AdminGiveOrderController', function ($scope, $rootScope, adminRequests, transplateNameToEng) {
    $rootScope.location = 'orders';
    $rootScope.subLocation = 'give';

    $scope.departments = [];
    $scope.jobTitles = [];
    $scope.userData = {};
    $scope.availablePhones = [];
    $scope.users = [];
    $scope.phoneObj = {};
    $scope.phoneObj.phone = {};

    $scope.fullname = "";
    $scope.userData.fullname = "";
    $scope.userData.username = "";
    $scope.userData.password = "";

    $scope.userData.departmentId = "";
    $scope.userData.phoneId = "";
    $scope.userData.jobTitleId = "";
    $scope.userData.userId = "";

    $scope.departmentName = "";
    $scope.jobTitleName = "";

    $scope.date = new Date();

    //click
    $scope.giveCard = function () {
        $scope.userData.phoneId = $scope.phoneObj.phone.number;
        console.log($scope.userData);
        adminRequests.givePhoneOrder($scope.userData)
        .success(function (data) {
            console.log(data);
            $scope.userData.fullname = "";
            $scope.userData.username = "";
            $scope.userData.password = "";

            $scope.userData.departmentId = "";
            $scope.userData.jobTitleId = "";
            $scope.userData.userId = "";

            $scope.departmentName = "";
            $scope.jobTitleName = "";
            $scope.fullname = "";

            $scope.getAllFreePhones();
            $scope.getAllUsers(true);
        })
        .error(function (error) {
            console.log(error);
        })
    };

    //change
    $scope.changeDepartment = function (data) {
        $scope.departments.forEach(function (x) {
            if (x.id === data) {
                $scope.departmentName = x.name;
            }
        });
    };

    $scope.changeJobTitle = function (data) {
        $scope.jobTitles.forEach(function (x) {
            if (x.id === data) {
                $scope.jobTitleName = x.jobTitleName;
            }
        });
    };

    $scope.changeFullname = function (data) {
        $scope.userData.username = transplateNameToEng(data);
        $scope.userData.password = $scope.userData.username;
    };

    //requests
    $scope.getAllFreePhones = function () {
        adminRequests.getAllPhones('free')
       .success(function (data) {
           $scope.availablePhones = data;
       })
       .error(function (error) {
           console.log(error);
       });
    };

    $scope.getAllDepartments = function () {
        adminRequests.getAllDepartments()
       .success(function (data) {
           $scope.departments = data;
       })
       .error(function (error) {
           console.log(error);
       });
    };

    $scope.getAllJobTitles = function () {
        adminRequests.getAllJobTitles()
        .success(function (data) {
            $scope.jobTitles = data;
        })
        .error(function (error) {
            console.log(error);
        });
    };

    $scope.getAllUsers = function (forceToReload) {
        if (forceToReload) { // if need to refresh data make request
            adminRequests.getAllUsers()
           .success(function (data) {
               $scope.users = data;
               console.log(data);
           })
           .error(function (error) {
               console.log(error);
           });
        } else {
            if ($scope.users.length === 0) { //this prevent onclick to make request, the request is made only ones
                adminRequests.getAllUsers()
                .success(function (data) {
                    $scope.users = data;
                })
                .error(function (error) {
                    console.log(error);
                });
            }
        }
    };

    $scope.onSelect = function (user, $model, $label) {
        $scope.user = user;
        $scope.userData.fullname = user.fullName;
        $scope.jobTitleName = user.jobTitle;
        $scope.departmentName = user.departmentName;
        $scope.userData.userId = user.id;
        console.log(user);
    };

    $scope.getAllDepartments();
    $scope.getAllFreePhones();
    $scope.getAllJobTitles();
});