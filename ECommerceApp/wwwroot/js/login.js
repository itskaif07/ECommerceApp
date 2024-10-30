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


document.addEventListener("DOMContentLoaded", function () {
    // Get the current page URL
    const currentUrl = window.location.href;


    const registerButton = document.getElementById("registerButton");
    const loginButton = document.getElementById("loginButton");


    // Check if the current URL is for Sign Up or Log In
    if (currentUrl.includes("LogIn")) {
        registerButton.style.opacity = "0.5";
        loginButton.style.opacity = "1";
    }
});
