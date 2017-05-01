'use strict';

angular.module('broadcastService', []).factory('broadcastService', [
    '$rootScope',
    function ($rootScope) {

        //Set the hubs URL for the connection
        $.connection.hub.url = "http://aleatablesweb.azurewebsites.net/inventory-signalr/hubs";
        var inventoryHub = $.connection.inventoryHub;

        inventoryHub.client.broadcastStatus = function (time,
            inventoryCount,
            salesCount,
            inventoryTotal,
            salesTotal) {

            var message = 'Inventory $' + inventoryTotal + ' (' + inventoryCount + ') Sales $' + salesCount + ' (' + salesTotal + ')';
            console.log(message);
            $rootScope.$broadcast('broadcastService-received-data-event',
                {
                    'time': time,
                    'inventoryCount': inventoryCount,
                    'salesCount': salesCount,
                    'inventoryTotal': inventoryTotal,
                    'salesTotal': salesTotal
                });
        };

        $.connection.hub.start()
            .done()
            .fail(function (data) {
                alert(data);
            }
        );

        var getMessage = function () {
            return message;
        }
        var getTitle = function (metric) {
            switch (metric) {
                case 'time':
                    return 'Time';
                case 'inventoryCount':
                    return 'Inventory count';
                case 'salesCount':
                    return 'Sales count';
                case 'inventoryTotal':
                    return 'Inventory $';
                case 'salesTotal':
                    return 'Sales $';
            }
            return undefined;
        };

        return {
            getTitle: getTitle
        };
    }
]);