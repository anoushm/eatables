angular.module('chartApp', ['broadcastService'])
.controller('linechart-controller', ['$scope', 'broadcastService', function ($scope, broadcastService) {
    $scope.$on('broadcastService-received-data-event', function (evt, data) {
        $scope.statusMessage = data['time'] + ' Inventory $' + data['inventoryTotal'] + ' (' + data['inventoryCount'] + ') Sales $' + data['salesTotal'] + ' (' + data['salesCount'] + ')';
        $scope.$apply();
    });

}])
.directive('linechartDirective', function (broadcastService) {
    return {
        scope: {
            metric: '@metric'
        },
        template: '',
        link: function (scope, el, attrs) {

            scope.options = {
                width: 700,
                height: 400,
                legend: {
                    position: 'none'
                },
                vAxis: {
                    maxValue: 100,
                    minValue: 0
                },
                hAxis: {
                    slantedText: false,
                    format: 'h:mm:ss',
                    maxTextLines: 1,
                    gridlines: {
                        count: 20
                    }
                }
            };

            scope.options.title = broadcastService.getTitle(scope.metric);
            scope.initialized = false;

            scope.$on('broadcastService-received-data-event', function (evt, data) {

                if (!scope.initialized) {
                    scope.data = new google.visualization.DataTable();
                    scope.data.addColumn('timeofday', 'Time of Day');
                    scope.data.addColumn('number', 'Value');

                    scope.chart = new google.visualization.LineChart(el[0]);
                    scope.initialized = true;
                }
                var d = new Date(data.time);
                scope.data.addRow([[d.getHours(), d.getMinutes(), d.getSeconds()], Math.round(data[scope.metric])]);

                var rowCount = scope.data.getNumberOfRows();
                if (rowCount < 20) {
                    scope.options.hAxis.baseline = [d.getHours(),
                        d.getMinutes(),
                        d.getSeconds() + 20 - rowCount];
                }
                else {
                    scope.options.hAxis.baseline = [
                        d.getHours(),
                        d.getMinutes(),
                        d.getSeconds()
                    ];
                }
                if (rowCount > 20)
                    scope.data.removeRow(0);

                scope.chart.draw(scope.data, scope.options);

            });
        }
    };
});