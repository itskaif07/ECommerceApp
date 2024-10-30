
const images = [
    "/images/bg3.jpg",
    "/images/bg1.jpg",
    "/images/bg2.jpg",
    "/images/bg4.jpg",
    "/images/bg5.jpg",
    "/images/bg6.jpg",
    "/images/bg7.jpg",
    "/images/bg8.jpg",
    "/images/bg9.jpg",
    "/images/bg10.jpg",
];

let currentIndex = 0;

let slideShow = document.querySelector("#slideShow")

function changeBackground() {
    currentIndex = (currentIndex + 1) % images.length;
    slideShow.style.backgroundImage = `url(${images[currentIndex]})`;
}

changeBackground();

setInterval(changeBackground, 5000);