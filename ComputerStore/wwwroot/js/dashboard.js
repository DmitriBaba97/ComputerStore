/*Creates income chart*/
function createIncomeChart() {
    var ctx = $("#incomeChart");
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            datasets: [{
                data: [12440, 10360, 6980, 7654, 12234, 9086, 6874, 13594, 13978, 9012, 5078, 20789],
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1
            }]
        },
        options: {
            title: {
                text: 'Monthly Income',
                display: true,
                position: 'top',
                fontSize: 24,
                fontStyle: 'bold',
                fontFamily: 'Numans, sans-serif'
            },
            legend: {
                display: false
            },
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}

/*Creates visitors map*/
function createVisitorsMap() {
    $("#visitors_map").vectorMap({
        map: 'world_mill',
        markerStyle: {
            initial: {
                fill: '#F8E23B',
                stroke: '#383f47'
            }
        },
        backgroundColor: '#383f47',
        markers: [
            { latLng: [55.75, 37.61], name: 'Moscow' },
            { latLng: [55.01, 82.93], name: 'Novosibirsk' },
            { latLng: [52.51, 13.37], name: 'Brandenburg' },
            { latLng: [45.76, 4.83], name: 'Lyon' },
            { latLng: [43.93, 12.46], name: 'San Marino' },
            { latLng: [47.14, 9.52], name: 'Liechtenstein' },
            { latLng: [53.48, -2.24], name: 'Manchester' },
            { latLng: [53.35, -6.26], name: 'Dublin' },
            { latLng: [41.01, 28.97], name: 'Istanbul' },
            { latLng: [42.36, -71.05], name: 'Boston' },
            { latLng: [52.23, 21.01], name: 'Warsaw' },
            { latLng: [13.16, -59.55], name: 'Barbados' }
        ]
    });
}

$(document).ready(function () {
    if (window.location.pathname == "/Admin") {
        createIncomeChart();

        createVisitorsMap();
    }
});