


var Chart_ModeColor = {
    0: {
        'color': '#141727',
        'semicolor': '#dce1fc'
    },
    1: {
        'color': '#f5f5f5',
        'semicolor': '#424242'
    }
};
var Chart_EmptyColor = "#9e9e9e3b";
var Chart_EmptyCText = '#9e9e9e';
var Chart_EmptyText = 'No data available';
var Chart_EmptyFont = '13px sans-serif';
var Chart_Blockwidth = 100;
var Charts_mode = "light";
var Charts_MainColor = (Charts_mode == 'light' ? Chart_ModeColor[0].color : Chart_ModeColor[1].color);
var Charts_SemiColor = (Charts_mode == 'light' ? Chart_ModeColor[0].semicolor : Chart_ModeColor[1].semicolor);
var Charts_Temp = [];
var colorChart = ['#5899DA', '#E8743B'
    , '#19A979', '#ED4A7B', '#945ECF'
    , '#13A4B4', '#525DF4', '#BF399E'
    , '#6C8893', '#EE6868', '#2F6497'
    , '#ff7f0e', '#2ca02c', '#ffbb78', '#d62728', '#f7b6d2', '#ff9896', '#9467bd', '#dbdb8d', '#c5b0d5'
    , '#8c564b', '#c49c94', '#98df8a', '#e377c2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#17becf', '#a8b8d8'];
var canvas = document.createElement('canvas');
canvas.width = 500;
canvas.height = 400;

// Get the drawing context
var ctx1 = canvas.getContext('2d');
var ctx2 = ctx2 ?? ctx1;
var gradientStroke1 = ctx2.createLinearGradient(0, 230, 0, 50); ctx2

gradientStroke1.addColorStop(1, 'rgba(203,12,159,0.2)');
gradientStroke1.addColorStop(0.2, 'rgba(72,72,176,0.0)');
gradientStroke1.addColorStop(0, 'rgba(203,12,159,0)'); //purple colors

var gradientStroke2 = ctx2.createLinearGradient(0, 230, 0, 50);

gradientStroke2.addColorStop(1, 'rgba(20,23,39,0.2)');
gradientStroke2.addColorStop(0.2, 'rgba(72,72,176,0.0)');
gradientStroke2.addColorStop(0, 'rgba(20,23,39,0)'); //purple colors


async function rp_doughnut(id, data, textName, valueName) {
    new Promise((resolve, reject) => {
        
        if (document.getElementById(id) != null) {
            let ctx3 = document.getElementById(id).getContext("2d");
            let data_Value = [];
            let data_Header = [];
            let data_Color = [];
            let data_BorderColor = [];
 
            let _cutout = 60;
 
            for (let i = 0; i < data.length; i++) {
                data_Header.push(data[i][textName])
                data_Value.push(data[i][valueName]);
                data_Color.push(colorChart[i % colorChart.length] + '78');
                data_BorderColor.push(colorChart[i % colorChart.length]);
            }
            let config = {
                type: "doughnut",
                data: {
                    labels: data_Header,
                    datasets: [{
                        label: "",
                        weight: 9,
                        cutout: _cutout,
                        tension: 0.9,
                        pointRadius: 2,
                        borderWidth: 2,
                        backgroundColor: data_Color,
                        borderColor: data_BorderColor,
                        data: data_Value,
                        fill: false
                    }],
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false,
                        }
                    },
                    interaction: {
                        intersect: false,
                        mode: 'index',
                    },
                    scales: {
                        y: {
                            grid: {
                                drawBorder: false,
                                display: false,
                                drawOnChartArea: false,
                                drawTicks: false,
                            },
                            ticks: {
                                display: false
                            }
                        },
                        x: {
                            grid: {
                                drawBorder: false,
                                display: false,
                                drawOnChartArea: false,
                                drawTicks: false,
                            },
                            ticks: {
                                display: false,
                            }
                        },
                    },
                },
                plugins: [{
                    afterDraw (chart, args, options) {
                        const {datasets} = chart.data;
                        let hasData = false;

                        for (let i = 0; i < datasets.length; i += 1) {
                            const dataset = datasets[i];
                            hasData |= dataset.data.length > 0;
                        }

                        if (!hasData) {
                            const {chartArea: {left, top, right, bottom}, ctx} = chart;
                            const centerX = (left + right) / 2;
                            const centerY = (top + bottom) / 2;
                            const r = Math.min(right - left, bottom - top) / 2;
                            ctx.beginPath();
                            ctx.fillStyle = Chart_EmptyColor;
                            ctx.arc(centerX, centerY, r, 0, 2 * Math.PI, false);
                            ctx.arc(centerX, centerY, _cutout, 0, 2 * Math.PI, true);
                            ctx.fill();
                        }
                    }
                }]
            }
            if (typeof Charts_Temp[id] == "undefined") {
                Charts_Temp[id] = new Chart(ctx3, config);
            }
            else {
                Charts_Temp[id].destroy();
                Charts_Temp[id] = new Chart(ctx3, config);
            }
        }
        resolve()
    });
}
async function rp_pie(id, data, textName, valueName) {
    new Promise((resolve, reject) => {
        setTimeout(() => {
            if (document.getElementById(id) != null) {
               
                let ctx3 = document.getElementById(id).getContext("2d");
                let data_Value = [];
                let data_Header = [];
                let data_Color = [];
                let data_BorderColor = [];
                let _cutout = 55;
 
                for (let i = 0; i < data.length; i++) {
                    data_Header.push(data[i][textName])
                    data_Value.push(data[i][valueName]);
                    data_Color.push(colorChart[i % colorChart.length] + '78');
                    data_BorderColor.push(colorChart[i % colorChart.length]);
                }
                let config = {
                    type: "pie",
                    data: {
                        labels: data_Header,
                        datasets: [{
                            label: "",
                            weight: 9,
                            cutout: _cutout,
                            tension: 0.9,
                            pointRadius: 2,
                            borderWidth: 1,
                            backgroundColor: data_Color,
                            borderColor: data_BorderColor,
                            data: data_Value,
                            fill: false
                        }],
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                display: false,
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        let label = context.dataset.label || '';

                                        if (label) {
                                            label += ': ';
                                        }
                                        
                                        if (context.parsed !== null) {
                                            label += formatNumberToMilion(context.parsed);
                                        }
                                        return label;
                                    }
                                }
                            }
                        },
                        interaction: {
                            intersect: false,
                            mode: 'index',
                        },
                        scales: {
                            y: {
                                grid: {
                                    drawBorder: false,
                                    display: false,
                                    drawOnChartArea: false,
                                    drawTicks: false,
                                },
                                ticks: {
                                    display: false
                                }
                            },
                            x: {
                                grid: {
                                    drawBorder: false,
                                    display: false,
                                    drawOnChartArea: false,
                                    drawTicks: false,
                                },
                                ticks: {
                                    display: false,
                                }
                            },
                        },
                    },
                    plugins: [{
                        afterDraw (chart, args, options) {
                            const {datasets} = chart.data;
                            let hasData = false;
                            let zeroData = 0;
                            for (let i = 0; i < datasets.length; i += 1) {
                                const dataset = datasets[i];
                                hasData |= dataset.data.length > 0;
                                for (let j = 0; j < dataset.data.length; j++) {
                                    if (dataset.data[j] != 0) zeroData = 1;
                                }
                            }

                            if (!hasData || zeroData==0) {
                                const {chartArea: {left, top, right, bottom}, ctx} = chart;
                                const centerX = (left + right) / 2;
                                const centerY = (top + bottom) / 2;
                                const r = Math.min(right - left, bottom - top) / 2;

                                ctx.beginPath();
                                ctx.fillStyle = Chart_EmptyCText;
                                ctx.font = Chart_EmptyFont;
                                ctx.textBaseline = 'middle';
                                ctx.textAlign = "center";
                                ctx.fillText(Chart_EmptyText, centerX, centerY);

                                ctx.beginPath();
                                ctx.fillStyle = Chart_EmptyColor;
                                ctx.arc(centerX, centerY, r, 0, 2 * Math.PI, false);
                                ctx.arc(centerX, centerY, _cutout, 0, 2 * Math.PI, true);
                                ctx.fill();
                            }
                        }
                    }]
                }
                if (typeof Charts_Temp[id] == "undefined") {
                    Charts_Temp[id] = new Chart(ctx3, config);
                }
                else {
                    Charts_Temp[id].destroy();
                    Charts_Temp[id] = new Chart(ctx3, config);
                }
            }
            resolve()
        }, 100)
    });
}
async function rp_bar(id, data, textName, valueName) {
    new Promise((resolve, reject) => {
        if (document.getElementById(id) != null) {
            let ctx3 = document.getElementById(id).getContext("2d");
            let data_Value = [];
            let data_Header = [];
            let data_Color = [];
            let data_BorderColor = [];
            
            for (let i = 0; i < data.length; i++) {
                data_Header.push(data[i][textName])
                data_Value.push(data[i][valueName]);
                data_Color.push(colorChart[i % colorChart.length]+'78');
                data_BorderColor.push(colorChart[i % colorChart.length]);
               
            }
            
            let _header = rp_devidelabel(data_Header, 2);
            let config = {
                type: "bar",
                data: {
                    labels: _header,
                    datasets: [{
                        label: "",
                        weight: 5,
                        borderWidth: 2,
                        borderRadius: 7,
                        backgroundColor: data_Color,
                        borderColor: data_BorderColor,
                        data: data_Value,
                        fill: false,
                        maxBarThickness: 35
                    }],
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false,
                            position: 'top',
                            align: 'start',
                            labels: {
                                color: Charts_MainColor,
                                boxWidth: 20,
                                boxHeight: 20
                            }
                        }
                        ,
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    let label = context.dataset.label || '';

                                    if (label) {
                                        label += ': ';
                                    }

                                    if (context.parsed.y !== null) {
                                        label += formatNumberToMilion(context.parsed.y);
                                    }
                                    return label;
                                }
                            }
                        }
                    },
                    interaction: {
                        intersect: false,
                        mode: 'index',
                    },
                    scales: {
                        y: {
                            grid: {
                                drawBorder: false,
                                display: true,
                                drawOnChartArea: true,
                                drawTicks: false,
                                borderDash: [5, 5],
                                color: Charts_SemiColor,
                                font: {
                                    size: 13,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                }
                            },
                            ticks: {
                                display: true,
                                padding: 10,
                                color: Charts_MainColor
                            }
                        },
                        x: {
                            grid: {
                                drawBorder: false,
                                display: true,
                                drawOnChartArea: true,
                                drawTicks: true,
                                borderDash: [5, 5],
                                color: Charts_SemiColor,
                                font: {
                                    size: 13,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                }
                            },
                            ticks: {
                                display: true,
                                color: Charts_MainColor,
                                maxRotation: 0,
                                minRotation: 0,
                                padding: 10,
                                callback: function (value, index, ticks) {
                                    let label = data_Header[value];
                                    if (label == null) {
                                        return '';
                                    }
                                    if (label.length > 15) {
                                        label = label.match(/(\S{15,}\s)|(.{1,15}(?:\s|$))/g);
                                    }

                                    return label;
                                }
                            }
                        },
                    }
                },
                plugins: [
                    {
                        beforeInit: function (chart) {
                            let minwidth = 0;
                            chart.data.labels.forEach(function (e, i, a) {
                                if (/\n/.test(e)) {
                                    a[i] = e.split(/\n/)
                                }
                                minwidth = minwidth + Chart_Blockwidth;
                            })
                            const originalFit = chart.legend.fit;
                            chart.legend.fit = function fit () {
                                originalFit.bind(chart.legend)();
                                this.height += 30;
                            }
                            $("#" + id).parent().css("minWidth", minwidth + "px");
                        }
                    },
                    {
                        afterDraw (chart, args, options) {
                            const {datasets} = chart.data;
                            let hasData = false;
                            let zeroData = 0;
                            for (let i = 0; i < datasets.length; i += 1) {
                                const dataset = datasets[i];
                                hasData |= dataset.data.length > 0;
                                for (let j = 0; j < dataset.data.length; j++) {
                                    if (dataset.data[j] != 0) zeroData = 1;
                                }
                            }

                            if (!hasData || zeroData == 0) {

                                const {chartArea: {left, top, right, bottom}, ctx} = chart;
                                const centerX = (left + right) / 2;
                                const centerY = (top + bottom) / 2;
                                ctx.fillStyle = Chart_EmptyCText;
                                ctx.font = Chart_EmptyFont;
                                ctx.textBaseline = 'middle';
                                ctx.textAlign = "center";
                                ctx.fillText(Chart_EmptyText, centerX, centerY);
                            }
                        }
                    }
                ]
            }
            if (typeof Charts_Temp[id] == "undefined") {
                Charts_Temp[id] = new Chart(ctx3, config);
            }
            else {
                Charts_Temp[id].destroy();
                Charts_Temp[id] = new Chart(ctx3, config);
            }
            
        }
        resolve()
    });
}

async function rp_barhorizotal(id, data, textName, valueName) {
    new Promise((resolve, reject) => {
        if (document.getElementById(id) != null) {
            let ctx3 = document.getElementById(id).getContext("2d");
            let data_Value = [];
            let data_Header = [];
            let data_Color = [];
            let data_BorderColor = [];
     
            for (let i = 0; i < data.length; i++) {
                data_Header.push(data[i][textName])
                data_Value.push(data[i][valueName]);
                data_Color.push(colorChart[i % colorChart.length] + '78');
                data_BorderColor.push(colorChart[i % colorChart.length]);
                if (Number(data[i][valueName]) != 0) isempty = 1;
            }
            let config = {
                type: "bar",
                data: {
                    labels: data_Header,
                    datasets: [{
                        label: "",
                        weight: 5,
                        borderWidth: 2,
                        borderRadius: 7,
                        backgroundColor: data_Color,
                        borderColor: data_BorderColor,
                        data: data_Value,
                        fill: false,
                        maxBarThickness: 35
                    }],
                },
                options: {
                    indexAxis: 'y',
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false,
                        },
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    let label = context.dataset.label || '';

                                    if (label) {
                                        label += ': ';
                                    }

                                    if (context.parsed.x !== null) {
                                        label += formatNumberToMilion(context.parsed.x);
                                    }
                                    return label;
                                }
                            }
                        }
                    },
                    interaction: {
                        intersect: false,
                        mode: 'index',
                    },
                    scales: {
                        y: {
                            grid: {
                                drawBorder: false,
                                display: true,
                                drawOnChartArea: true,
                                drawTicks: false,
                                borderDash: [5, 5],
                                color: Charts_SemiColor,
                                font: {
                                    size: 13,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                }
                            },
                            ticks: {
                                display: true,
                                padding: 10,
                                color: Charts_MainColor
                            }
                        },
                        x: {
                            grid: {
                                drawBorder: false,
                                display: true,
                                drawOnChartArea: true,
                                drawTicks: true,
                                borderDash: [5, 5],
                                color: Charts_SemiColor,
                                font: {
                                    size: 13,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                }
                            },
                            ticks: {
                                display: true,
                                color: Charts_MainColor,
                                padding: 10
                            }
                        },
                    },
                },
                plugins: [
                    {
                        afterDraw (chart, args, options) {
                            const {datasets} = chart.data;
                            let hasData = false;
                            let zeroData = 0;
                             
                            for (let i = 0; i < datasets.length; i += 1) {
                                const dataset = datasets[i];
                                hasData |= dataset.data.length > 0;
                                for (let j = 0; j < dataset.data.length; j++) {
                                    if (dataset.data[j] != 0) zeroData = 1;
                                }
                            }

                            if (!hasData || zeroData==0) {
                             
                                const {chartArea: {left, top, right, bottom}, ctx} = chart;
                                const centerX = (left + right) / 2;
                                const centerY = (top + bottom) / 2;
                                ctx.fillStyle = Chart_EmptyCText;
                                ctx.font = Chart_EmptyFont;
                                ctx.textBaseline = 'middle';
                                ctx.textAlign = "center";
                                ctx.fillText(Chart_EmptyText, centerX, centerY);
                            }
                        }
                    }
                ]
            }
            if (typeof Charts_Temp[id] == "undefined") {
                Charts_Temp[id] = new Chart(ctx3, config);
            }
            else {
                Charts_Temp[id].destroy();
                Charts_Temp[id] = new Chart(ctx3, config);
            }
             
        }
        resolve()
    });
}
async function rp_barcols(id, data, textName, dataColumn, notitle) {
    new Promise((resolve, reject) => {
          
        if (document.getElementById(id) != null) {
    
            let ctx3 = document.getElementById(id).getContext("2d");
            let barThickness = 30;
            let data_Header = [];
            for (let i = 0; i < data.length; i++) {
                let header = data[i][textName];
                data_Header.push(header)
            }
            let datasets = [];
            for (let i = 0; i < dataColumn.length; i++) {
                let label = dataColumn[i]["label"];
                let value = dataColumn[i]["value"];
                let e = {};
                e.weight = 5;
                e.borderWidth = 2;
                e.borderRadius = 4;
                e.backgroundColor = colorChart[i % colorChart.length]+'78';
                e.borderColor = colorChart[i % colorChart.length];
                e.fill = true;
                e.maxBarThickness = 25;
                let data_Value = [];
                for (let j = 0; j < data.length; j++) {
                    data_Value.push(data[j][value]);
                }
                e.barThickness = barThickness;
                e.maxBarThickness = barThickness;
                e.label = label;
                e.data = data_Value;
                datasets.push(e);
            }
            let config = {
                type: "bar",
                data: {
                    labels: data_Header,
                    datasets: datasets
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: (notitle == undefined || notitle == true),
                            position: 'top',
                            align: 'start',
                            labels: {
                                color: Charts_MainColor,
                                boxWidth: 20,
                                boxHeight: 20
                            }
                        },
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    let label = context.dataset.label || '';

                                    if (label) {
                                        label += ': ';
                                    }

                                    if (context.parsed.y !== null) {
                                        label += formatNumberToMilion(context.parsed.y);
                                    }
                                    return label;
                                }
                            }
                        }
                    },
                    interaction: {
                        intersect: false,
                        mode: 'index',
                    },
                    scales: {
                        y: {
                            grid: {
                                drawBorder: false,
                                display: true,
                                drawOnChartArea: true,
                                drawTicks: false,
                                borderDash: [5, 5],
                                color: Charts_SemiColor
                            },
                            ticks: {
                                display: true,
                                padding: 10,
                                color: Charts_MainColor,
                                font: {
                                    size: 13,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                } 
                            }
                        },
                        x: {
                            grid: {
                                drawBorder: false,
                                display: true,
                                drawOnChartArea: true,
                                drawTicks: true,
                                borderDash: [5, 5],
                                color: Charts_SemiColor
                            },
                            ticks: {
                                display: true,
                                color: Charts_MainColor,
                                font: {
                                    size: 13,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                },
                                maxRotation: 0,
                                minRotation: 0,
                                padding: 10,
                                callback: function (value, index, ticks) {
                                    let label = data_Header[value];
                                    if (label == null) {
                                        return '';
                                    }
                                    if (label.length > 15) {
                                        label = label.match(/(\S{15,}\s)|(.{1,15}(?:\s|$))/g);
                                    }

                                    return label;
                                }
                            }
                        },
                    }
                },
                plugins: [
                    {
                        beforeInit: function (chart, options) {
                            let minwidth = 0;
                            chart.data.labels.forEach(function (e, i, a) {
                                if (/\n/.test(e)) {
                                    a[i] = e.split(/\n/)
                                }
                                minwidth = minwidth + Chart_Blockwidth;
                            })
                            const originalFit = chart.legend.fit;
                            chart.legend.fit = function fit () {
                                originalFit.bind(chart.legend)();
                                this.height += 30;
                            }
                            $("#" + id).parent().css("minWidth", minwidth + "px");
                        }
                    },
                    {
                        afterDraw (chart, args, options) {
                            const {datasets} = chart.data;
                            let hasData = false;
                            let zeroData = 0;
                            for (let i = 0; i < datasets.length; i += 1) {
                                const dataset = datasets[i];
                                hasData |= dataset.data.length > 0;
                                for (let j = 0; j < dataset.data.length; j++) {
                                    if (dataset.data[j] != 0) zeroData = 1;
                                }
                            }

                            if (!hasData || zeroData == 0) {

                                const {chartArea: {left, top, right, bottom}, ctx} = chart;
                                const centerX = (left + right) / 2;
                                const centerY = (top + bottom) / 2;
                                ctx.fillStyle = Chart_EmptyCText;
                                ctx.font = Chart_EmptyFont;
                                ctx.textBaseline = 'middle';
                                ctx.textAlign = "center";
                                ctx.fillText(Chart_EmptyText, centerX, centerY);
                            }
                        }
                    }
                ]
            }
            if (typeof Charts_Temp[id] == "undefined") {

                Charts_Temp[id] = new Chart(ctx3, config);
            }
            else {
                Charts_Temp[id].destroy();
                Charts_Temp[id] = new Chart(ctx3, config);
            }
        }
        resolve()
    });
}
async function rp_mixbarline(id, data, cat) {
    new Promise((resolve, reject) => {

        if (document.getElementById(id) != null) {
            let ctx3 = document.getElementById(id).getContext("2d");

            let listcat = [];
            let labelbar = '', labelline = '';
            let databar = [], dataline = [];
            for (let i = 0; i < data.length; i++) {
                for (j = 0; j < Object.keys(data[i]).length; j++) {
                    let val = data[i][Object.keys(data[i])[j]];

                    if (i == 0 && j != 0) databar.push(val);
                    if (i == 1 && j != 0) dataline.push(val);
                    if (i == 0 && j == 0) labelbar = val;
                    if (i == 1 && j == 0) labelline = val;
                }
            }
            for (var i = 0; i < cat.length; i++) {
                for (j = 0; j < Object.keys(cat[i]).length; j++) {
                    listcat.push(cat[i][Object.keys(cat[i])[j]]);
                }
            }


            let config = {
                type: "bar",
                data: {
                    labels: listcat,
                    datasets: [{
                        type: "bar",
                        label: labelbar,
                        weight: 5,
                        tension: 0.4,
                        borderWidth: 0,
                        pointBackgroundColor: "#3A416F",
                        borderColor: "#3A416F",
                        backgroundColor: '#3A416F',
                        borderRadius: 4,
                        borderSkipped: false,
                        data: databar,
                        maxBarThickness: 10,
                    },
                    {
                        type: "line",
                        label: labelline,
                        tension: 0.4,
                        borderWidth: 0,
                        pointRadius: 0,
                        pointBackgroundColor: "#cb0c9f",
                        borderColor: "#cb0c9f",
                        borderWidth: 3,
                        backgroundColor: gradientStroke1,
                        data: dataline,
                        fill: true,
                    }
                    ],
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false,
                        },
                        tooltip: {
                            callbacks: {
                                label: function (context) {
                                    let label = context.dataset.label || '';
                                    
                                    if (label) {
                                        label += ': ';
                                    }

                                    if (context.parsed.y !== null) {
                                        label += formatNumberToMilion(context.parsed.y);
                                    }
                                    return label;
                                }
                            }
                        }
                    },
                    interaction: {
                        intersect: false,
                        mode: 'index',
                    },
                    scales: {
                        y: {
                            grid: {
                                drawBorder: false,
                                display: true,
                                drawOnChartArea: true,
                                drawTicks: false,
                                borderDash: [5, 5],
                                color: Charts_SemiColor
                            },
                            ticks: {
                                display: true,
                                padding: 0,
                                color: Charts_MainColor,
                                font: {
                                    size: 13,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                },
                            }
                        },
                        x: {
                            grid: {
                                drawBorder: false,
                                display: true,
                                drawOnChartArea: true,
                                drawTicks: true,
                                borderDash: [5, 5],
                                color: Charts_SemiColor
                            },
                            ticks: {
                                display: true,
                                color: Charts_MainColor,
                                padding: 10,
                                font: {
                                    size: 13,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                } 
                            }
                        },
                    },
                },
               
                plugins: [
                    {
                        beforeInit: function (chart) {
                            let minwidth = 0;
                            chart.data.labels.forEach(function (e, i, a) {
                                if (/\n/.test(e)) {
                                    a[i] = e.split(/\n/)
                                }
                                minwidth = minwidth + Chart_Blockwidth;
                            })
                            const originalFit = chart.legend.fit;
                            chart.legend.fit = function fit () {
                                originalFit.bind(chart.legend)();
                                this.height += 30;
                            }
                            $("#" + id).parent().css("minWidth", minwidth + "px");
                        }
                    },
                    {
                        afterDraw (chart, args, options) {
                            const {datasets} = chart.data;
                            let hasData = false;
                            let zeroData = 0;
                            for (let i = 0; i < datasets.length; i += 1) {
                                const dataset = datasets[i];
                                hasData |= dataset.data.length > 0;
                                for (let j = 0; j < dataset.data.length; j++) {
                                    if (dataset.data[j] != 0) zeroData = 1;
                                }
                            }

                            if (!hasData || zeroData == 0) {

                                const {chartArea: {left, top, right, bottom}, ctx} = chart;
                                const centerX = (left + right) / 2;
                                const centerY = (top + bottom) / 2;
                                ctx.fillStyle = Chart_EmptyCText;
                                ctx.font = Chart_EmptyFont;
                                ctx.textBaseline = 'middle';
                                ctx.textAlign = "center";
                                ctx.fillText(Chart_EmptyText, centerX, centerY);
                            }
                        }
                    }
                ]
            }
            if (typeof Charts_Temp[id] == "undefined") {
                Charts_Temp[id] = new Chart(ctx3, config);
            }
            else {
                Charts_Temp[id].destroy();
                Charts_Temp[id] = new Chart(ctx3, config);
            }
        }
        resolve()
    });
}
async function rp_linenolabel(id, data, cat) {
    new Promise((resolve, reject) => {
        if (document.getElementById(id) != null) {
            let ctx3 = document.getElementById(id).getContext("2d");
            let listcat = [];
            let labelline = '';
            let dataline = [];
             
            for (let i = 0; i < data.length; i++) {
                for (j = 0; j < Object.keys(data[i]).length; j++) {
                    let val = data[i][Object.keys(data[i])[j]];
                    if (i == 0 && j != 0) {
                        dataline.push(val);
                        
                    } 
                    if (i == 0 && j == 0) labelline = val;
                   
                }
            }
             
            for (var i = 0; i < cat.length; i++) {
                for (j = 0; j < Object.keys(cat[i]).length; j++) {
                    listcat.push(cat[i][Object.keys(cat[i])[j]]);
                }
            }
            let config = {
                type: "bar",
                data: {
                    labels: listcat,
                    datasets: [
                        {
                            type: "line",
                            label: labelline,
                            tension: 0.4,
                            borderWidth: 0,
                            pointRadius: 0,
                            pointBackgroundColor: "#cb0c9f",
                            borderColor: "#cb0c9f",
                            borderWidth: 3,
                            backgroundColor: gradientStroke1,
                            data: dataline,
                            fill: true,
                        }
                    ],
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: false,
                        }
                    },
                    interaction: {
                        intersect: false,
                        mode: 'index',
                    },
                    scales: {
                        y: {
                            grid: {
                                drawBorder: false,
                                display: false,
                                drawOnChartArea: true,
                                drawTicks: true,
                                borderDash: [5, 5]
                            },
                            ticks: {
                                display: false,
                                padding: 10,
                                color: '#344767',
                                font: {
                                    size: 11,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                },
                            }
                        },
                        x: {
                            grid: {
                                drawBorder: false,
                                display: true,
                                drawOnChartArea: true,
                                drawTicks: true,
                                borderDash: [5, 5]
                            },
                            ticks: {
                                display: true,
                                color: '#344767',
                                padding: 0,
                                font: {
                                    size: 13,
                                    family: "Open Sans",
                                    style: 'normal',
                                    lineHeight: 2
                                }
                            }
                        },
                    },
                },
                plugins: [
                    {
                        afterDraw (chart, args, options) {
                            const {datasets} = chart.data;
                            let hasData = false;
                            let zeroData = 0;
                            for (let i = 0; i < datasets.length; i += 1) {
                                const dataset = datasets[i];
                                hasData |= dataset.data.length > 0;
                                for (let j = 0; j < dataset.data.length; j++) {
                                    if (dataset.data[j] != 0) zeroData = 1;
                                }
                            }

                            if (!hasData || zeroData == 0) {

                                const {chartArea: {left, top, right, bottom}, ctx} = chart;
                                const centerX = (left + right) / 2;
                                const centerY = (top + bottom) / 2;
                                ctx.fillStyle = Chart_EmptyCText;
                                ctx.font = Chart_EmptyFont;
                                ctx.textBaseline = 'middle';
                                ctx.textAlign = "center";
                                ctx.fillText(Chart_EmptyText, centerX, centerY);
                            }
                        }
                    }
                ]
            }
            if (typeof Charts_Temp[id] == "undefined") {
                Charts_Temp[id] = new Chart(ctx3, config);
            }
            else {
                Charts_Temp[id].destroy();
                Charts_Temp[id] = new Chart(ctx3, config);
            }
        }
        resolve()
    });
}
function rp_devidelabel(data, numword) {
    try {

        let result = [];
        for (let i = 0; i < data.length; i++) {
            let e = JSON.parse(JSON.stringify(data[i]));
            let arrname = [];
            let obj = data[i].split(' ');
            let text = '';
            let fitext = 0;
            for (let j = 0; j < obj.length; j++) {
                if (j != obj.length - 1) {
                    if (obj[j] != undefined && obj[j].trim() != "") {

                        if (fitext < numword) {
                            text = text != "" ? (text + " " + obj[j].trim()) : obj[j].trim();
                        }
                        else {
                            fitext = 0;
                            arrname.push(text);
                            text = obj[j].trim();
                        }
                        fitext = fitext + 1;
                    }
                }
                else {
                    text = text != "" ? (text + " " + obj[j].trim()) : obj[j].trim();
                    arrname.push(text);
                }
            }
            e = arrname
            result.push(e);
        }
        return result;
    } catch (e) {
        return data;
    }

}