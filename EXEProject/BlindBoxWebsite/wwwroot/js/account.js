var errorPopup = document.getElementById('popup-error');
var successPopup = document.getElementById('popup-success');

if (errorPopup) {
    errorPopup.style.display = 'block';
    setTimeout(function () {
        errorPopup.style.display = 'none';
    }, 7000);
}

if (successPopup) {
    successPopup.style.display = 'block';
    setTimeout(function () {
        successPopup.style.display = 'none';
    }, 7000);
}