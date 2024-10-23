var nameInput = document.getElementById('name-post');
var phoneInput = document.getElementById('phone-post');
var submitButton = document.getElementById('submitButton');
var phoneError = document.getElementById('phoneError');
var nameError = document.getElementById('nameError');

nameInput.addEventListener('input', function () {
    validateName();
});

phoneInput.addEventListener('input', function () {
    validatePhone();
});

nameInput.addEventListener('focus', function () {
    validateName();
});

phoneInput.addEventListener('focus', function () {
    validatePhone();
});

function clearField(fieldId) {
    var field = document.getElementById(fieldId);
    field.value = '';

    var errorId = fieldId + 'Error';
    var errorElement = document.getElementById(errorId);
    if (errorElement) {
        errorElement.textContent = '';
    }

    validateForm();
}

function validateName() {
    var nameValue = nameInput.value.trim();
    var nameRegex = /^[a-zA-Zа-яА-Я\s]+$/;
    var isNameValid = nameRegex.test(nameValue);

    if (nameValue !== '') {
        nameError.textContent = isNameValid ? '' : 'Invalid name. ';
    } else {
        nameError.textContent = 'Enter Name.';
    }

    validateForm();
}

function validatePhone() {
    var phoneValue = phoneInput.value.trim();
    var phoneRegex = /^\+375\d{9}$/;
    var isPhoneValid = phoneRegex.test(phoneValue);

    if (phoneValue !== '') {
        phoneError.textContent = isPhoneValid ? '' : 'Please enter a valid number.';
    } else {
        phoneError.textContent = 'Enter number';
    }

    validateForm();
}

function validateForm() {
    var nameValue = nameInput.value.trim();
    var phoneValue = phoneInput.value.trim();
    var isNameValid = /^[a-zA-Zа-яА-Я\s]+$/.test(nameValue);
    var isPhoneValid = /^\+375\d{9}$/.test(phoneValue);

    if (nameValue !== '' && isNameValid && phoneValue !== '' && isPhoneValid) {
        submitButton.removeAttribute('disabled');
    } else {
        submitButton.setAttribute('disabled', 'disabled');
    }
}

