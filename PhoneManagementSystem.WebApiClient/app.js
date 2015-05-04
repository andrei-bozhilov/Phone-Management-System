angular.module('app', [
    'ngRoute',
    'ngAnimate',
    'ui.bootstrap'
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

    .when('/admin/table-manage/:table', {
        templateUrl: 'views/admin/table-manage/admin-tables-view.html',
        controller: 'AdminTableManageController'
    })

    .when('/admin/table-manage/:table/:id', {
        templateUrl: 'views/admin/table-manage/admin-tables-item-view.html',
        controller: 'AdminTableItemManageController'
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
    $rootScope.subLocation = "";

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