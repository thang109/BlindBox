// Object to store selected answers
const answers = {};

// Function to highlight selected option and store answer
function selectOption(quizId, optionElement, optionText) {
    const options = document.querySelectorAll(`#${quizId} .option`);
    options.forEach(option => option.classList.remove('selected'));

    optionElement.classList.add('selected');

    answers[quizId] = optionText;

    console.log(answers); 
}

// Go to the next quiz
function nextQuiz() {
    const currentQuiz = document.querySelector('.quiz:not(.hidden)');
    const nextQuiz = currentQuiz.nextElementSibling;
    if (nextQuiz && nextQuiz.classList.contains('quiz')) {
        currentQuiz.classList.add('hidden');
        nextQuiz.classList.remove('hidden');
    }
}

// Go to the previous quiz
function previousQuiz() {
    const currentQuiz = document.querySelector('.quiz:not(.hidden)');
    const previousQuiz = currentQuiz.previousElementSibling;
    if (previousQuiz && previousQuiz.classList.contains('quiz')) {
        currentQuiz.classList.add('hidden');
        previousQuiz.classList.remove('hidden');
    }
}
