document.getElementById('clearImageBtn')?.addEventListener('click', function () {
    const artistIdElement = document.getElementById('artistId');
    const albumIdElement = document.getElementById('albumId');

    const artistId = artistIdElement ? artistIdElement.value : null;
    const albumId = albumIdElement ? albumIdElement.value : null;

    // Get the current image element
    const currentImage = document.getElementById('currentImage');

    // Define default image URLs for artist and album
    const defaultArtistImageUrl = "/img/artist-no-image-available.jpg";
    const defaultAlbumCoverUrl = "/img/album-no-cover-available.jpg";

    if (artistId) {
        // Send a request to delete the artist's image
        fetch(`/Artist/DeleteImage/${artistId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        })
            .then(response => handleResponse(response, 'Artist', defaultArtistImageUrl));
    } else if (albumId) {
        // Send a request to delete the album's image
        fetch(`/Album/DeleteImage/${albumId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        })
            .then(response => handleResponse(response, 'Album', defaultAlbumCoverUrl));
    } else {
        // Handle the case where neither artistId nor albumId is available
        alert('Error: No ID found.');
    }

    function handleResponse(response, type, defaultImageUrl) {
        if (response.ok) {
            // Update the image source to the default image URL
            currentImage.src = defaultImageUrl;
            // Hide the "Clear" button after the image is cleared
            document.getElementById('clearImageBtn').style.display = 'none';
            console.log(`${type} image deleted successfully.`);
        } else {
            // Show an error message if the deletion fails
            alert(`Failed to delete the ${type.toLowerCase()} image.`);
        }
    }
});