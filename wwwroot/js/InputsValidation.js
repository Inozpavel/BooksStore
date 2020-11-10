const form = document.querySelector("form")
const inputs = form.querySelectorAll("input.form-control");
const passwordInputs = document.querySelectorAll(`input[type = "password"`);

form.addEventListener("submit", (event) => {
    if (checkInputs() === false)
        event.preventDefault();
});

function checkInputs() {
    let validState = checkPasswordsInputs();
    for (let input of inputs) {
        if (checkIsNotEmpty(input) === false) {
            setErrorForElement(input, "is-invalid", "is-valid", "Значение не может быть пустым!")
            validState = false;
        }
    }
    return validState;
}

function checkIsNotEmpty(element) {
    return element.value.trim() != "";
}

function checkElementIsPasswordInput(element) {
    return element.type === "password";
}

function checkPasswordsInputs() {
    if (passwordInputs.length > 0) {

        if (checkPasswords() === false) {
            for (let element of passwordInputs) {
                setErrorForElement(element, "is-invalid", "is-valid", "Пароли не совпадают!")
            }
            return false;
        }
        else {
            for (let element of passwordInputs) {
                setErrorForElement(element, "is-valid", "is-invalid", "")
            }
            return true;
        }
    }
}

function checkPasswords() {
    const password = passwordInputs[0].value;
    for (let i = 1; i < passwordInputs.length; i++) {
        if (passwordInputs[i].value != password) {
            return false;
        }
    }
    return true;
}

function setErrorForElement(element, classToAdd = "", classToRemove = "", errorText = "") {
    element.classList.add(classToAdd)
    element.classList.remove(classToRemove);

    let errorField = element.parentElement.querySelector("span");
    if (errorField) {
        errorField.innerText = errorText;
    }
    return true;
}