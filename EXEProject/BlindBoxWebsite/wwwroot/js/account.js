var errorPopup = document.getElementById('popup-error');
var successPopup = document.getElementById('popup-success');

if (errorPopup) {
    errorPopup.style.display = 'block';
    setTimeout(function () {
        errorPopup.style.display = 'none';
    }, 5000);
}

if (successPopup) {
    successPopup.style.display = 'block';
    setTimeout(function () {
        successPopup.style.display = 'none';
    }, 5000);
}

function closePopup(id) {
    document.getElementById(id).style.display = 'none';
}