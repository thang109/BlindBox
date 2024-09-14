let currentQuiz = 1; // Quiz đầu tiên

// Hiển thị quiz theo thứ tự
function showQuiz(quizNumber) {
    const quizzes = document.querySelectorAll('.quiz');
    quizzes.forEach((quiz, index) => {
        if (index === quizNumber - 1) {
            quiz.style.display = 'flex'; // Hiển thị quiz hiện tại
        } else {
            quiz.style.display = 'none'; // Ẩn các quiz khác
        }
    });
}

// Khi trang được tải, hiển thị quiz đầu tiên
document.addEventListener("DOMContentLoaded", () => {
    showQuiz(currentQuiz);
});

// Chuyển đến quiz tiếp theo
function nextQuiz() {
    if (currentQuiz < 5) {
        currentQuiz++;
        showQuiz(currentQuiz);
    }
}

// Chuyển về quiz trước
function previousQuiz() {
    if (currentQuiz > 1) {
        currentQuiz--;
        showQuiz(currentQuiz);
    }
}
