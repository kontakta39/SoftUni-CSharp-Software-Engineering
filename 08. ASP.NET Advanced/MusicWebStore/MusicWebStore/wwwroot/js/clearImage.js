document.getElementById('clearImageBtn')?.addEventListener('click', function () {
    const albumIdElement = document.getElementById('albumId');
    const artistIdElement = document.getElementById('artistId');
    const currentImage = document.getElementById('currentImage');
    const fileNameField = document.getElementById('fileName'); // ���������� ���� �� ����� �� �����

    const albumId = albumIdElement ? albumIdElement.value : null;
    const artistId = artistIdElement ? artistIdElement.value : null;

    // ���������� �� ��������� �� ������������
    const defaultAlbumCoverUrl = "/img/album-no-cover-available.jpg";
    const defaultArtistImageUrl = "/img/artist-no-image-available.jpg";

    if (albumId) {
        // ������ �� ��������� �� ����������� �� �����
        fetch(`/Album/DeleteImage/${albumId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        })
            .then(response => {
                if (response.ok) {
                    // ������� ���������
                    currentImage.src = defaultAlbumCoverUrl; // �������� �� �������������
                    fileNameField.value = "No file chosen"; // �������� �� ����� �� �����
                    document.getElementById('clearImageBtn').style.display = 'none'; // �������� �� ������
                    console.log("Album image deleted successfully.");
                } else {
                    alert("Failed to delete the album image.");
                }
            })
            .catch(error => {
                console.error("Error deleting album image:", error);
                alert("An error occurred while trying to delete the album image.");
            });
    } else if (artistId) {
        // ������ �� ��������� �� ����������� �� ������
        fetch(`/Artist/DeleteImage/${artistId}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        })
            .then(response => {
                if (response.ok) {
                    // ������� ���������
                    currentImage.src = defaultArtistImageUrl; // �������� �� �������������
                    fileNameField.value = "No file chosen"; // �������� �� ����� �� �����
                    document.getElementById('clearImageBtn').style.display = 'none'; // �������� �� ������
                    console.log("Artist image deleted successfully.");
                } else {
                    alert("Failed to delete the artist image.");
                }
            })
            .catch(error => {
                console.error("Error deleting artist image:", error);
                alert("An error occurred while trying to delete the artist image.");
            });
    } else {
        alert("Error: No album or artist ID found.");
    }
});