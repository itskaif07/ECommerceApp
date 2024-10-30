
const passwordInput = document.querySelector('#password');
const openEye = document.querySelector('#openEye');
const closedEye = document.querySelector('#closedEye');

openEye.addEventListener('click', function (event) {
    event.preventDefault();
    passwordInput.setAttribute('type', 'text');

    openEye.classList.add('hidden');
    closedEye.classList.remove('hidden');
});

closedEye.addEventListener('click', function (event) {
    event.preventDefault();
    passwordInput.setAttribute('type', 'password');

    closedEye.classList.add('hidden');
    openEye.classList.remove('hidden');
});




const passwordInput1 = document.querySelector('#password1');
const openEye1 = document.querySelector('#openEye1');
const closedEye1 = document.querySelector('#closedEye1');

openEye1.addEventListener('click', function (event) {
    event.preventDefault();
    passwordInput1.setAttribute('type', 'text');
    openEye1.classList.add('hidden');
    closedEye1.classList.remove('hidden');
});

closedEye1.addEventListener('click', function (event) {
    event.preventDefault();
    passwordInput1.setAttribute('type', 'password');
    closedEye1.classList.add('hidden'); 
    openEye1.classList.remove('hidden');
});

document.addEventListener("DOMContentLoaded", function () {
    const currentUrl = window.location.href;

    const registerButton = document.getElementById("registerButton");
    const loginButton = document.getElementById("loginButton");


    if (currentUrl.includes("SignUp")) {
        registerButton.style.opacity = "1";
        loginButton.style.opacity = "0.5";
    }
});

