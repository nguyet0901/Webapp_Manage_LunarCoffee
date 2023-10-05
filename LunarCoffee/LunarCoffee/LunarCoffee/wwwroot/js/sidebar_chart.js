'use strict'


var data = {
    labels: ['W1', 'W2', 'W3', 'W4', 'W5', 'W6', 'W7', 'W8', 'W9', 'W10'],
    series: [
        [1, 2, 4, 8, 6, -2, -1, -4, -6, -2]
    ]
};

var options = {
    high: 10,
    low: -10,
    axisX: {
        labelInterpolationFnc: function(value, index) {
            return index % 2 === 0 ? value : null;
        }
    },
    axisY: {
        showLabel: false,
        showGrid: false,
        onlyInteger: true,
        offset: 1
    },
    axisX: {
        showLabel: false,
        showGrid: false,
        offset: 50
    }
};

new Chartist.Bar('.ct-chart-sidebar1', data, options);