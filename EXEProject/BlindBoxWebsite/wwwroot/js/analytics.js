    const visitsCtx = document.getElementById("visitsChart").getContext("2d");
    const visitsChart = new Chart(visitsCtx, {
        type: "line",
    data: {
        labels: ["January", "February", "March", "April", "May", "June"],
    datasets: [
    {
        label: "Visits",
    data: [300, 400, 350, 500, 600, 700],
    backgroundColor: "rgba(52, 152, 219, 0.2)",
    borderColor: "rgba(52, 152, 219, 1)",
    borderWidth: 2,
            },
    ],
        },
    options: {
        responsive: true,
    maintainAspectRatio: false,
        },
      });

    const revenueCtx = document
    .getElementById("revenueChart")
    .getContext("2d");
    const revenueChart = new Chart(revenueCtx, {
        type: "bar",
    data: {
        labels: ["January", "February", "March", "April", "May", "June"],
    datasets: [
    {
        label: "Revenue",
    data: [15000, 20000, 18000, 22000, 24000, 26000],
    backgroundColor: "rgba(46, 204, 113, 0.6)",
    borderColor: "rgba(46, 204, 113, 1)",
    borderWidth: 2,
            },
    ],
        },
    options: {
        responsive: true,
    maintainAspectRatio: false,
        },
      });
