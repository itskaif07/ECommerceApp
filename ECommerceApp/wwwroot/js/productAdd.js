const imageUpload = document.querySelector("#imageUpload");
const defaultImage = document.querySelector("#defaultImage");
const removeImageButton = document.querySelector("#removeImageButton");
const addImageButton = document.querySelector("#addImageButton");
const url = document.querySelector("#url")
const urlInput = document.querySelector("#urlInput")

document.addEventListener("DOMContentLoaded", function () {
    if (addImageButton && defaultImage) {
        addImageButton.addEventListener('click', function () {
            imageUpload.click();
        });

        defaultImage.addEventListener('click', function () {
            imageUpload.click();
        });

        imageUpload.addEventListener('change', (e) => {
            let file = e.target.files[0];


            if (file) {
                defaultImage.src = URL.createObjectURL(file);
            }
        });
    }



    if (urlInput) {
        urlInput.addEventListener("input", function () {
            const weblink = urlInput.value;

            if (weblink) {
                defaultImage.src = weblink;
            }
        })
    }



    if (removeImageButton) {
        removeImageButton.addEventListener('click', function () {
            defaultImage.src = '/images/uploadImage.jpg';
            imageUpload.value = '';
            urlInput.value = '';
        });
    }
});

url.addEventListener("click", function () {
    urlInput.classList.toggle("hidden")
})