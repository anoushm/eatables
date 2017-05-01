angular.module('guageApp', ['broadcastService'])
.controller('guage-controller', ['$scope', 'broadcastService', function ($scope, broadcastService) {
    $scope.$on('broadcastService-received-data-event', function (evt, data) {
        $scope.statusMessage = data['time'] + ' Inventory $' + data['inventoryTotal'] + ' (' + data['inventoryCount'] + ') Sales $' + data['salesTotal'] +  ' (' + data['salesCount'] + ')';
        $scope.$apply();
    });

}])
.directive('statusDirective', function (broadcastService) {
    return {
        scope: {
            metric: '@metric'
        },
        templateUrl: 'js/directives/gauge-template.html',
        link: function (scope, el, attrs) {

        scope.initialized = false;
        scope.title = broadcastService.getTitle(scope.metric);
        scope.options = {
            width: 250, height: 250,
            redFrom: 90, redTo: 100,
            yellowFrom: 75, yellowTo: 90,
            minorTicks: 5
        };

        scope.$on('broadcastService-received-data-event', function (evt, data) {

            if (!scope.initialized) {
                scope.data = google.visualization.arrayToDataTable([
                    ['Label', 'Value'],
                    [scope.title, 0]
                ]);

                scope.chart = new google.visualization.Gauge(el[0]);
                scope.initialized = true;
                scope.message = 'hi';
            }
            scope.data.setValue(0, 1, Math.round(data[scope.metric]));
            scope.chart.draw(scope.data, scope.options);
        });
    }
};
});