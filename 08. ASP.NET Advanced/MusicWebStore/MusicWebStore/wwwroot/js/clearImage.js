document.addEventListener('DOMContentLoaded', function () {
    const clearButton = document.getElementById('clearImageBtn');
    const uploadImageSection = document.getElementById('uploadImageSection');
    const currentImage = document.getElementById('currentImage');
    const fileNameField = document.getElementById('fileName');
    const currentImageDiv = clearButton ? clearButton.closest('div.mb-2') : null;
    const albumIdElement = document.getElementById('albumId');

    if (clearButton) {
        clearButton.addEventListener('click', function () {
            clearImage(albumIdElement, currentImage, fileNameField, currentImageDiv, uploadImageSection);
        });
    }

    function clearImage(albumIdElement, currentImage, fileNameField, currentImageDiv, uploadImageSection) {
        const albumId = albumIdElement ? albumIdElement.value : null;
        const defaultAlbumCoverUrl = "/img/album-no-cover-available.jpg";

        if (albumId) {
            // Send request to delete the album image
            fetch(`/Album/DeleteImage/${albumId}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            })
                .then(response => {
                    if (response.ok) {
                        // Successfully deleted the image
                        updateUIForImageClear(currentImage, fileNameField, currentImageDiv, uploadImageSection, defaultAlbumCoverUrl);
                    } else {
                        alert("Failed to delete the album image.");
                    }
                })
                .catch(error => {
                    console.error("Error deleting album image:", error);
                    alert("An error occurred while trying to delete the album image.");
                });
        } else {
            alert("Error: No album ID found.");
        }
    }

    function updateUIForImageClear(currentImage, fileNameField, currentImageDiv, uploadImageSection, defaultAlbumCoverUrl) {
        // Update image to default cover
        currentImage.src = defaultAlbumCoverUrl;

        // Reset file name field
        fileNameField.value = "No file chosen";

        // Hide the image and clear button section
        if (currentImageDiv) {
            currentImageDiv.style.display = 'none';
        }

        // Show the upload image section again
        uploadImageSection.style.display = 'block';
    }
});