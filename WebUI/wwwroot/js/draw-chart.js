
function drawChart(data) {

    var labels = [];
    for (var i = 1; i <= data.length; i++) {
        labels.push(i.toString());
    }

    var ctx = document.getElementById("myChart").getContext('2d');

    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'transformed signal',
                data: data,
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                lineTension: 0.1
            }]
        },
        options: {}
    });
}

function fetchData() {

    var obj = readSignal();
    var content = JSON.stringify(obj);

    var headers = new Headers({
        "Content-Type": "application/json",
        "Content-Length": content.length.toString()
    });

    var parameters = {
        method: 'POST',
        headers: headers,
        body: content
    };

    fetch('api/dft/getTransformed', parameters).then(function (response) {
        return response.json();
    })
    .then(function (array) {
        if (array) {
            drawChart(array);
        }
    });

}

function readSignal() {
    var input = document.getElementById('input-signal');

    var textArray = input.value.split(',');
    var result = [];

    for (var i = 0; i < textArray.length; i++) {
        var num = parseInt(textArray[i].trim());
        if (!isNaN(num)) {
            result.push(num);
        } else {
            alert("Wrong input. [5, 7, 3, 3, 0, 0, 15, 32, 13, 7] will be send as input signal");

            return [5, 7, 3, 3, 0, 0, 15, 32, 13, 7];
        }
    }

    return result;
}

document.getElementById('go-button').addEventListener('click', fetchData);