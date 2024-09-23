let selectedAnswers = {};

function selectOption(quizId, element, answer) {
    // Store the selected answer
    selectedAnswers[quizId] = answer;

    // Remove the 'selected' class from all options in the current quiz
    const options = document.querySelectorAll(`#${quizId} .option`);
    options.forEach(option => option.classList.remove('selected'));

    // Add the 'selected' class to the clicked option
    element.classList.add('selected');
}

function nextQuiz() {
    const currentQuiz = document.querySelector('.quiz:not(.hidden)');
    const nextQuiz = currentQuiz.nextElementSibling;

    if (nextQuiz && nextQuiz.classList.contains('quiz')) {
        currentQuiz.classList.add('hidden');
        nextQuiz.classList.remove('hidden');

        // Display previously selected option, if any
        const nextQuizId = nextQuiz.getAttribute('id');
        const selectedAnswer = selectedAnswers[nextQuizId];
        if (selectedAnswer) {
            const options = document.querySelectorAll(`#${nextQuizId} .option`);
            options.forEach(option => {
                if (option.textContent.trim() === selectedAnswer) {
                    option.classList.add('selected');
                }
            });
        }
    }
}

function previousQuiz() {
    const currentQuiz = document.querySelector('.quiz:not(.hidden)');
    const previousQuiz = currentQuiz.previousElementSibling;

    if (previousQuiz && previousQuiz.classList.contains('quiz')) {
        currentQuiz.classList.add('hidden');
        previousQuiz.classList.remove('hidden');

        // Display previously selected option, if any
        const prevQuizId = previousQuiz.getAttribute('id');
        const selectedAnswer = selectedAnswers[prevQuizId];
        if (selectedAnswer) {
            const options = document.querySelectorAll(`#${prevQuizId} .option`);
            options.forEach(option => {
                if (option.textContent.trim() === selectedAnswer) {
                    option.classList.add('selected');
                }
            });
        }
    }
}
