const getGraphData = async () => {
    const params = new URLSearchParams({ handler: 'StatisticsGraphData' });

    try {
        const response = await fetch(`?${params}`);

        if (!response.ok)
            throw new Error(response.status);

        return await response.json();
    }
    catch (e) {
        alert('An error occurred getting the graph data');
        console.error(e);
        return null;
    }
}

const renderGraph = async () => {
    const graphData = await getGraphData();

    if (!graphData)
        return;

    const canvas = document
        .getElementById('ScoreOverTimeChart')
        .getContext('2d');

    new Chart(canvas, {
        type: 'line',
        data: {
            labels: graphData.titlesAndDates,
            datasets: [
                {
                    label: 'Scores',
                    data: graphData.scores,
                    borderWidth: 3,
                    borderColor: '#00FFFF',
                    fill: false,
                    tension: 0.1
                },
                {
                    label: 'Par',
                    data: graphData.parLine,
                    borderWidth: 3,
                    borderColor: '#00AA00',
                    fill: false,
                    tension: 0.1,
                    pointRadius: 0,
                    pointHoverRadius: 0
                }
            ]
        },
        options: {
            plugins: {
                legend: {
                    labels: {
                        color: '#000',
                        font: {
                            weight: 'bold',
                            size: 14
                        }
                    }
                }
            },
            scales: {
                y: {
                    title: {
                        display: true,
                        text: 'Score',
                        color: '#000',
                        font: {
                            weight: 'bold',
                            size: 12
                        }
                    },
                    min: -10,
                    max: 26,
                    ticks: {
                        color: '#000',
                        font: {
                            size: 12
                        },
                        stepSize: 2
                    }
                },
                x: {
                    title: {
                        display: true,
                        text: 'Round',
                        color: '#000',
                        font: {
                            weight: 'bold',
                            size: 12
                        }
                    },
                    ticks: {
                        color: '#000',
                        font: {
                            size: 12
                        }
                    }
                }
            }
        }
    });
}

renderGraph();