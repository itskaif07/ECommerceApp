
//Slideshow
console.log("Script loaded!");

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

const slideData = [
    {
        image: "/images/foodbg.jpg",
        title: "Foods",
        description: "Foods that boost your energy and delight your taste buds — perfect for every craving!",
        categoryId: 6
    },
    {
        image: "/images/beverbg.jpg",
        title: "Beverages",
        description: "Refreshing beverages to keep you energized all day long.",
        categoryId: 7
    }
];

let currentIndex2 = 0;
let slideShow2 = document.querySelector("#slideShow2");
let titleElement = slideShow2.querySelector("h2");
let descriptionElement = slideShow2.querySelector("p");
let categoryLink = document.querySelector("#shopNowLink");

function changeContentAndBackground() {
    const currentSlide = slideData[currentIndex2];

    slideShow2.style.backgroundImage = `url(${currentSlide.image})`;

    titleElement.textContent = currentSlide.title;
    descriptionElement.textContent = currentSlide.description;

    categoryLink.setAttribute("href", `/Products/ProductsIndex?categoryId=${currentSlide.categoryId}`);

    currentIndex2 = (currentIndex2 + 1) % slideData.length;
}

changeContentAndBackground();

setInterval(changeContentAndBackground, 5000);

const completed = document.querySelector("#Completed");

if (completed) {
    setTimeout(() => {
        completed.classList.add("fade-out");
        setTimeout(() => {
            completed.remove();
        }, 500);
    }, 2000);
}


// Search bar

document.addEventListener("DOMContentLoaded", () => {
    const searchBox = document.getElementById('searchBox');
    const searchButton = document.getElementById('searchButton');
    const searchResults = document.getElementById('searchResults');

    // Function to handle search
    const performSearch = () => {
        const query = searchBox.value.trim();

        if (query === "") {
            searchResults.innerHTML = `
            <div class="w-full  py-3 border-[1px] overflow-hidden text-white flex items-center justify-center bg-[#222222]">
                <p class="text-gray-400">Please enter a search query.</p>
            </div>
            `;
            return;
        }

        fetch(`/Home/Search?query=${encodeURIComponent(query)}`)
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    const products = data.data.$values || [];
                    searchResults.innerHTML = "";
                    if (products.length > 0) {
                        products.forEach(product => {
                            const imageUrl = product.imageUrl || '/images/noImage.jpg';

                            const productCard = `
                            <a href="/Products/ProductDetails/${product.id}" class="w-full h-24  overflow-hidden flex items-center justify-start bg-[#222222] px-2 no-underline text-white">
                                <div class="w-24 h-10">
                                    <img src="${imageUrl}" class="object-scale-down h-full w-full" alt="${product.name}">
                                </div>
                                <div class="space-y-2">
                                    <h3 class="w-full text-left">${product.name}</h3>
                                    <p class="text-sm text-gray-500">${product.categoryName || "Uncategorized"}</p>
                                </div>
                            </a>
                            `;
                            searchResults.innerHTML += productCard;
                        });
                    } else {
                        searchResults.innerHTML = `
                        <div class="w-full  py-3 border-[1px] overflow-hidden text-white flex items-center justify-center bg-[#222222]">
                            <p class='text-gray-500'>No products found.</p>
                        </div>
                        `;
                    }
                } else {
                    searchResults.innerHTML = `
                    <div class="w-full  py-3 border-[1px] overflow-hidden text-white flex items-center justify-center bg-[#222222]">
                        <p class='text-red-500'>Error: ${data.message}</p>
                    </div>
                    `;
                }
            })
            .catch(error => {
                console.error("Error fetching search results:", error);
                searchResults.innerHTML = `
                <div class="w-full  py-3 border-[1px] overflow-hidden text-white flex items-center justify-center bg-[#222222]">
                    <p class='text-red-500'>An error occurred. Please try again.</p>
                </div>
                `;
            });
    };

    searchButton.addEventListener("click", performSearch);

    searchBox.addEventListener("keydown", (event) => {
        if (event.key == "Enter") {
            event.preventDefault();
            performSearch();
        }
    })

});


