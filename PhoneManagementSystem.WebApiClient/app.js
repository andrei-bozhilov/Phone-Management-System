angular.module('app', [
    'ngRoute'
])

.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
    $routeProvider

    .when('/', {
        templateUrl: 'views/public/welcome-view.html',
        controller: 'WelcomeController'
    })

    .when('/login', {
        templateUrl: 'views/public/login-view.html',
        controller: 'LoginController'
    })

    .when('/logout', {
        templateUrl: 'views/public/welcome-view.html',
        controller: 'LogoutController'
    })

     .when('/admin/home', {
         templateUrl: 'views/admin/admin-home-view.html',
         controller: 'AdminHomeController'
     })

    .when('/admin/users', {
        templateUrl: 'views/admin/admin-home-view.html',
        controller: 'AdminUsersController'
    })

    .when('/admin/orders', {
        templateUrl: 'views/admin/orders/admin-orders-view.html',
        controller: 'AdminOrdersController'
    })

    .when('/admin/orders/give', {
        templateUrl: 'views/admin/orders/admin-give-order-view.html',
        controller: 'AdminGiveOrderController'
    })

    .when('/admin/orders/take', {
        templateUrl: 'views/admin/orders/admin-take-order-view.html',
        controller: 'AdminTakeOrderController'
    })

    .when('/admin/users/add', {
        templateUrl: 'views/admin/admin-add-user-view.html',
        controller: 'AdminAddUserController'
    })

    .when('/admin/users/edit', {
        templateUrl: 'views/admin/admin-edit-user-view.html',
        controller: 'AdminEditUserController'
    })

    .when('/admin/users/delete', {
        templateUrl: 'views/admin/admin-delete-user-view.html',
        controller: 'AdminDeleteUserController'
    })

    .when('/admin/departments', {
        templateUrl: 'views/admin/admin-home-view.html',
        controller: 'AdminDepartmentsController'
    })

     .when('/admin/phones', {
         templateUrl: 'views/admin/admin-phones-view.html',
         controller: 'AdminPhonesController'
     })



}])

.controller('WelcomeController', function ($location, userSession) {
    if (userSession.getCurrentUser()) {
        $location.path('user/home');
    }
})

.controller('LogoutController', function ($location, userSession) {
    userSession.logout();
    $location.path('/');
})

.run(function ($rootScope, $location, userSession) {
    $rootScope.headerUsername = "";
    $rootScope.isAdmin = "";
    $rootScope.location = "";

    $rootScope.$on('$locationChangeStart', function (event) {
        if (userSession.getCurrentUser()) {
            $rootScope.headerUsername = userSession.getCurrentUser().username;
            $rootScope.isAdmin = userSession.getCurrentUser().isAdmin;
        } else {
            $rootScope.headerUsername = "";
            $rootScope.isAdmin = "";
        }

        console.log($rootScope.isAdmin);


        if ($location.path().indexOf("/user/") != -1 && !userSession.getCurrentUser()) {
            // Authorization check: anonymous site visitors cannot access user routes
            $location.path("/");
        }

        if (userSession.isAdmin() && $location.path().indexOf("/user/") != -1) {
            $location.path('/admin/home');
            console.log("admin");
        }

    });

})