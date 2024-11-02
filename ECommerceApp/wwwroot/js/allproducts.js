const SearchInput = document.querySelector("#SearchInput");
const ProductContainer = document.querySelector("#ProductContainer");
const products = ProductContainer.querySelectorAll("a"); 

// Search function to filter products
function search() {
    const searchValue = SearchInput.value.toLowerCase(); // Get input value
    products.forEach(product => {
        const productName = product.getAttribute("data-product-name"); // Get the product name from data attribute
        if (productName.includes(searchValue)) {
            product.style.display = ""; // Show product
        } else {
            product.style.display = "none"; // Hide product
        }
    });
}


SearchInput.addEventListener("input", search);