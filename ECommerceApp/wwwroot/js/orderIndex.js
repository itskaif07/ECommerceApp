const notification = document.querySelector("#notification");

if (notification) {
    setTimeout(() => {
        notification.classList.add("fade-out");
        setTimeout(() => {
            notification.remove();
        }, 500);
    }, 2000);
}