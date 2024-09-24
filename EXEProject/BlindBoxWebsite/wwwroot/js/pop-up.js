function scrollUp() {
            window.scrollBy(0, -200);
        }

function scrollDown() {
    window.scrollBy(0, 200);
}

window.onload = function () {
    setTimeout(function () {
        document.getElementById("popup").style.display = "block";
    }, 5000); // Hiển thị sau 10 giây

    document.getElementById("close-popup").onclick = function () {
        document.getElementById("popup").style.display = "none";
    };
};        