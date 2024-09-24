var errorPopup = document.getElementById('popup-error');
var successPopup = document.getElementById('popup-success');

if (errorPopup) {
    errorPopup.style.display = 'flex';
    setTimeout(function () {
        errorPopup.style.display = 'none';
    }, 5000);
}

if (successPopup) {
    successPopup.style.display = 'flex';
    setTimeout(function () {
        successPopup.style.display = 'none';
    }, 5000);
}

function closePopup(id) {
    document.getElementById(id).style.display = 'none';
}