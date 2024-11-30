document.addEventListener("DOMContentLoaded", () => {
    const genreDropdown = document.getElementById("genreDropdown");
    const artistDropdown = document.getElementById("artistDropdown");

    // ������������ �� ������� �� ������������, ��� ���� ������ ����
    artistDropdown.disabled = !genreDropdown.value;

    // �������� ��� ��������� �� ����������, ��� ��� ������ ����
    const initialGenreId = genreDropdown.value;
    if (initialGenreId) {
        // ��������� ��������� �� ���� ����
        fetchArtistsByGenre(initialGenreId);
    }

    // ���������� �� ����� �� �����
    genreDropdown.addEventListener("change", async () => {
        const genreId = genreDropdown.value;
        artistDropdown.innerHTML = '<option value="">Select artist</option>'; // Reset artist dropdown

        // ��� ���� ������ ����, ������������ �������� ���� �� �������
        if (!genreId) {
            artistDropdown.disabled = true;
            return;
        }

        // ��� ��� ������ ����, ���������� �������� ���� �� �������
        artistDropdown.disabled = false;
        await fetchArtistsByGenre(genreId);
    });

    // ������� �� ��������� �� ������� �� ����
    async function fetchArtistsByGenre(genreId) {
        try {
            const response = await fetch(`/Album/GetArtistsByGenre?genreId=${genreId}`);
            const data = await response.json();

            // ���������� �������� ���� �� ������� � �������� ������
            artistDropdown.innerHTML = '<option value="">Select artist</option>';
            data.forEach(artist => {
                const option = document.createElement("option");
                option.value = artist.id;
                option.textContent = artist.name;
                artistDropdown.appendChild(option);
            });

            //��� ��� ������������� ������ ������, �� ���������
            const initialArtistId = artistDropdown.getAttribute('data-selected-artist');
            if (initialArtistId) {
                artistDropdown.value = initialArtistId;
            }
        } catch (error) {
            console.error("Error fetching artists:", error);
        }
    }
});