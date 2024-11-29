<script>
    const imageUpload = document.querySelector("#imageUpload");
    const defaultImage = document.querySelector("#defaultImage");
    const removeImageButton = document.querySelector("#removeImageButton");
    const addImageButton = document.querySelector("#addImageButton");

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
        // Show selected image as default image
        defaultImage.src = URL.createObjectURL(file);
                }
            });
        }

    if (removeImageButton) {
        removeImageButton.addEventListener('click', function () {
            // Reset the image and file input when removing
            defaultImage.src = '/images/uploadImage.jpg';
            // Clear the file input (this is how we reset it)
            imageUpload.value = '';
        });
        }
    });
</script>
