

const zoomOverlay = document.getElementById("zoomOverlay");
const productImage = document.querySelector("#productImage");
console.log(productImage)

productImage.addEventListener("mousemove", (e) => {
    const rect = productImage.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;

    zoomOverlay.style.left = `${x - zoomOverlay.offsetWidth / 2}px`;
    zoomOverlay.style.top = `${y - zoomOverlay.offsetHeight / 2}px`;
    zoomOverlay.style.backgroundImage = `url('${productImage.src}')`;
    zoomOverlay.style.backgroundSize = `${productImage.width * 2}px ${productImage.height * 2}px`;
    zoomOverlay.style.backgroundPosition = `-${x * 2 - zoomOverlay.offsetWidth / 2}px -${y * 2 - zoomOverlay.offsetHeight / 2}px`;
});

productImage.addEventListener("mouseleave", () => {
    zoomOverlay.classList.add("hidden");
});

productImage.addEventListener("mouseenter", () => {
    zoomOverlay.classList.remove("hidden");
});

const short = document.querySelector("#shortAddress");
const full = document.querySelector("#fullAddress");

short.addEventListener('click', function () {
    full.classList.remove("hidden");
    short.classList.add("hidden");
});

full.addEventListener('click', function () {
    short.classList.remove("hidden");
    full.classList.add("hidden");
});

