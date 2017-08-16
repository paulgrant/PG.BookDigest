var app = angular.module('CTM_PG', ['ngResource']);

//global configuration of the resources that are listed above
app.config(function ($httpProvider, $resourceProvider) {
    $resourceProvider.defaults.stripTrailingSlashes = false;

});

app.controller('BookReaderController', function ($scope, BookReaderService) {

    $scope.bookreader = BookReaderService;

    $scope.readbook1 = function () {
        $scope.bookreader.readBook(1);
    };

    $scope.readbook2 = function () {
        $scope.bookreader.readBook(2);
    }

});

//wrap the api call into a factory
app.factory("BookReader", function ($resource) {

    var factory = {};

    factory.readBook1 = function(){
        $resource("readbook1", {id: '@id'}, {
            update: {
                method: 'PUT'
            }
        });
    };

    factory.readBook2 = function(){
        $resource("readbook2", {id: '@id'}, {
            update: {
                method: 'PUT'
            }
        });
    };


    return factory;

});

app.service('BookReaderService', function (BookReader, $http) {
    var self={
        'isLoading': false,
        'results':[],
        'responseDetails': "",
        'readBook': function (bookreadingmethod) {
                if (!self.isLoading) {
                    self.isLoading = true;

                    var params = {
                        'method': bookreadingmethod
                    };

                    $http.get('/home/readbook1')
                    .success(function (data, status, headers, config) {
                        self.results = data;
                        self.isLoading = false;
                    })
                    .error(function (data, status, header, config) {
                        self.responseDetails = "Data: " + data +
                            "<br />status: " + status +
                            "<br />headers: " + jsonFilter(header) +
                            "<br />config: " + jsonFilter(config);
                            self.isLoading = false;
                    });

                    /*BookReader.readBook1(params, function (data) {
                        console.log(data);
                        angular.forEach(data.results, function (item) {
                            self.results.push(new BookReader(item));
                        });

                        self.isLoading = false;
                    });*/
                }

            },

    }


    return self;

});
