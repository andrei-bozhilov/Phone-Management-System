angular.module('app')

.factory('pagination', function () {
    var pagination = {
        bigTotalItems: 0,
        bigCurrentPage: 1,  //setup for Pagination (ui.bootstrap.pagination)
        maxSize: 5, //setup for Pagination (ui.bootstrap.pagination)
        setPage: function (pageNo) {  //setup for Pagination (ui.bootstrap.pagination)
            pagination.bigCurrentPage = pageNo;
        },
        pageChanged: function (requestFunc, requestObject) {  //setup for Pagination (ui.bootstrap.pagination)
            requestObject.startPage = pagination.bigCurrentPage;
            requestFunc();
        }
    }

    return pagination
})