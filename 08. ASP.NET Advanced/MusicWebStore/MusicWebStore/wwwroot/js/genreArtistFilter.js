document.addEventListener("DOMContentLoaded", () => {
    const genreDropdown = document.getElementById("genreDropdown");
    const artistDropdown = document.getElementById("artistDropdown");

    genreDropdown.addEventListener("change", async () => {
        const genreId = genreDropdown.value;
        artistDropdown.innerHTML = '<option value="">Select artist</option>'; // Reset artist dropdown

        if (genreId) {
            try {
                const response = await fetch(`/Album/GetArtistsByGenre?genreId=${genreId}`);
                const data = await response.json();

                data.forEach(artist => {
                    const option = document.createElement("option");
                    option.value = artist.id;
                    option.textContent = artist.name;
                    artistDropdown.appendChild(option);
                });
            } catch (error) {
                console.error("Error fetching artists:", error);
            }
        }
    });
});