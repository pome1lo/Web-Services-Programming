var isVal = true;

submitButton.setAttribute('disabled', 'disabled');

const offsetInput = document.getElementById('offset');
const errorMessageElement = document.getElementById('error-offset');

offsetInput.addEventListener('input', function () {
    let inputValue = offsetInput.value;

    if (!/^\d+$/.test(inputValue)) {
        isVal = false;
        errorMessageElement.innerText = 'Enter integer number';
        offsetInput.classList.add('error-input');
    } else {
        isVal = true;
        errorMessageElement.innerText = '';
        offsetInput.classList.remove('error-input');
    }
});

offsetInput.addEventListener('focus', function () {
    offsetInput.classList.remove('error-input');
});

const limitInput = document.getElementById('limit');
const limitErrorMessageElement = document.getElementById('error-limit');

limitInput.addEventListener('input', function () {
    let inputValue = limitInput.value;

    if (!/^\d+$/.test(inputValue)) {
        isVal = false;
        limitErrorMessageElement.innerText = 'Enter integer number';
        limitInput.classList.add('error-input');
    } else {
        isVal = true;
        limitErrorMessageElement.innerText = '';
        limitInput.classList.remove('error-input');
    }
});

limitInput.addEventListener('focus', function () {
    limitInput.classList.remove('error-input');
});

const minidInput = document.getElementById('minid');
const maxidInput = document.getElementById('maxid');
const minidErrorMessageElement = document.getElementById('error-minid');
const maxidErrorMessageElement = document.getElementById('error-maxid');

function setupInputValidation(input, errorMessageElement, otherInput) {
    input.addEventListener('input', function () {
        let inputValue = input.value;

        if (!/^\d+$/.test(inputValue)) {
            errorMessageElement.innerText = 'Enter integer number';
            input.classList.add('error-input');
        } else {
            errorMessageElement.innerText = '';
            input.classList.remove('error-input');
        }

        validateMinidMaxid();
    });

    input.addEventListener('focus', function () {
        input.classList.remove('error-input');
        otherInput.classList.remove('error-input');
    });
}

function validateMinidMaxid() {
    const minidValue = parseInt(minidInput.value, 10);
    const maxidValue = parseInt(maxidInput.value, 10);

    if (!isNaN(minidValue) && !isNaN(maxidValue) && minidValue >= maxidValue) {
        minidErrorMessageElement.innerText = 'MinId should be less than MaxId';
        maxidErrorMessageElement.innerText = 'MaxId should be greater MinId';
        minidInput.classList.add('error-input');
        maxidInput.classList.add('error-input');
        return false;
    } else {
        minidErrorMessageElement.innerText = '';
        maxidErrorMessageElement.innerText = '';
        minidInput.classList.remove('error-input');
        maxidInput.classList.remove('error-input');
        return true;
    }
}

setupInputValidation(document.getElementById('minid'), minidErrorMessageElement, maxidInput);

setupInputValidation(maxidInput, maxidErrorMessageElement, minidInput);