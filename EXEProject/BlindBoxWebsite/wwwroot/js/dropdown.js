    document.addEventListener('DOMContentLoaded', function() {
    var dropdown = document.querySelector('.dropdown');
    var dropdownBtn = dropdown.querySelector('.dropdown-btn');
    var dropdownContent = dropdown.querySelector('.dropdown-content');

    dropdownBtn.addEventListener('click', function() {
        dropdownContent.classList.toggle('show');
    });

    window.addEventListener('click', function(event) {
        if (!event.target.matches('.dropdown-btn')) {
            var openDropdowns = document.querySelectorAll('.dropdown-content.show');
    openDropdowns.forEach(function(dropdown) {
        dropdown.classList.remove('show');
            });
        }
    });
});
