angular.module("umbraco").controller("dexter.reindex", [
    "$scope", "$http", "$timeout", function ($scope, $http, $timeout) {
        $scope.indexes = [];

        $http.get("/umbraco/backoffice/api/DexterBackofficeApi/GetIndexes").then(function (resp) {
            $scope.indexes = resp.data;
        });

        $scope.reindex = function(index) {
            index.isProcessing = true;
            $http.get("/umbraco/backoffice/api/DexterBackofficeApi/Reindex?index=" + index.Name).then(function (r) {
                $timeout(function () {
                    $http.get("/umbraco/backoffice/api/DexterBackofficeApi/GetIndexes?indexName=" + index.Name).then(function (resp) {
                        index.DocumentsIndexed = resp.data[0].DocumentsIndexed;
                        index.isProcessing = false;
                    });
                }, 2000);
            });
        }
    }
]);