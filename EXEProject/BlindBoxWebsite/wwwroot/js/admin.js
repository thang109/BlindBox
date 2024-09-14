const ctx = document.getElementById("myChart").getContext("2d");
const myChart = new Chart(ctx, {
  type: "bar",
  data: {
    labels: ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"],
    datasets: [
      {
        label: "# of Signups",
        data: [12, 19, 3, 5, 2],
        backgroundColor: [
          "rgba(52, 152, 219, 0.6)",
          "rgba(52, 152, 219, 0.6)",
          "rgba(52, 152, 219, 0.6)",
          "rgba(52, 152, 219, 0.6)",
          "rgba(52, 152, 219, 0.6)",
        ],
        borderColor: [
          "rgba(52, 152, 219, 1)",
          "rgba(52, 152, 219, 1)",
          "rgba(52, 152, 219, 1)",
          "rgba(52, 152, 219, 1)",
          "rgba(52, 152, 219, 1)",
        ],
        borderWidth: 1,
      },
    ],
  },
  options: {
    scales: {
      y: {
        beginAtZero: true,
      },
    },
  },
});
