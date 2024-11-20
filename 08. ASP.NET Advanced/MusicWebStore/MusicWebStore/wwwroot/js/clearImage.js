document.getElementById('clearImageBtn')?.addEventListener('click', function () {
    // Get the artist's ID from a hidden input field or the image element itself
    var artistId = document.getElementById('artistId').value; // You can use a hidden field or another way to pass the artist ID

    fetch(`/Artist/DeleteImage/${artistId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                // Update the image on the page to the default image
                document.getElementById('currentImage').src = '/img/artist-no-image-available.jpg';
                // Optionally disable the "Clear" button after it's clicked
                document.getElementById('clearImageBtn').style.display = 'none';
            } else {
                alert('Failed to delete the image.');
            }
        })
        .catch(error => {
            alert('Error: ' + error);
        });
});