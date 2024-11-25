
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

    console.log("SlideShow element:", document.querySelector("#slideShow"));
    console.log("SearchBox element:", document.getElementById("searchBox"));
    console.log("SearchButton element:", document.getElementById("searchButton"));
    console.log("SearchResults element:", document.getElementById("searchResults"));


    searchButton.addEventListener('click', () => {
        const query = searchBox.value.trim();

        if (query === "") {
            searchResults.innerHTML = `<p class='text-gray-500'>Please enter a search query.</p>`;
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
                            const productCard = `
                                <div class="w-full border rounded-md overflow-hidden shadow-md flex items-center justify-center mx-auto bg-[#222222]">
                                    <div class="p-4">
                                        <h3 class="text-lg font-semibold">${product.name}</h3>
                                        <p class="text-sm text-gray-500">${product.categoryName || "Uncategorized"}</p>
                                        <div class="flex items-center space-x-2 mt-2">
                                            <p class="text-green-600 font-semibold">₹${product.discountedPrice.toFixed(2)}</p>
                                            ${product.discount > 0
                                    ? `<p class="text-sm line-through text-gray-400">₹${product.price.toFixed(2)}</p>`
                                    : ""
                                }
                                        </div>
                                    </div>
                                </div>
                            `;
                            searchResults.innerHTML += productCard;
                        });
                    } else {
                        searchResults.innerHTML = `
                            <div class="w-full border rounded-md overflow-hidden shadow-md flex items-center justify-center mx-auto bg-[#222222]">
                                <p class='text-gray-500'>No products found.</p>
                            </div>
                        `;
                    }
                } else {
                    searchResults.innerHTML = `
                        <div class="w-full border rounded-md overflow-hidden shadow-md flex items-center justify-center mx-auto bg-[#222222]">
                            <p class='text-red-500'>Error: ${data.message}</p>
                        </div>
                    `;
                }
            })
            .catch(error => {
                console.error("Error fetching search results:", error);
                searchResults.innerHTML = `
                    <div class="w-full border rounded-md overflow-hidden shadow-md flex items-center justify-center mx-auto bg-[#222222]">
                        <p class='text-red-500'>An error occurred. Please try again.</p>
                    </div>
                `;
            });
    });
});
