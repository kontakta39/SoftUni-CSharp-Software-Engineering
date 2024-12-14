document.addEventListener("DOMContentLoaded", () => {
    const genreDropdown = document.getElementById("genreDropdown");
    const artistDropdown = document.getElementById("artistDropdown");

    //Store the initial genre and artist
    const initialGenreId = genreDropdown.value;
    const initialArtistId = artistDropdown.getAttribute("data-selected-artist");

    //Disable artist dropdown by default if no genre is selected
    artistDropdown.disabled = !genreDropdown.value;

    //Check on page load if a genre is already selected
    if (initialGenreId) {
        fetchArtistsByGenre(initialGenreId, initialArtistId);
    }

    //Event handler for genre change
    genreDropdown.addEventListener("change", async () => {
        const genreId = genreDropdown.value;
        artistDropdown.innerHTML = '<option value="">Select artist</option>'; //Reset artist dropdown

        //Disable artist dropdown if no genre is selected
        if (!genreId) {
            artistDropdown.disabled = true;
            return;
        }

        // Enable artist dropdown if a genre is selected
        artistDropdown.disabled = false;

        //If the selected genre is the initial one, restore the associated artist
        if (genreId === initialGenreId && initialArtistId) {
            await fetchArtistsByGenre(genreId, initialArtistId);
        } else {
            await fetchArtistsByGenre(genreId);
        }
    });

    //Function to fetch artists by genre
    async function fetchArtistsByGenre(genreId, selectedArtistId = "") {
        try {
            const response = await fetch(`/Album/GetArtistsByGenre?genreId=${genreId}`);
            const data = await response.json();

            //Clear the artist dropdown and populate it with new data
            artistDropdown.innerHTML = '<option value="">Select artist</option>';
            data.forEach(artist => {
                const option = document.createElement("option");
                option.value = artist.id;
                option.textContent = artist.name;
                artistDropdown.appendChild(option);
            });

            //If there is a preselected artist, set it
            if (selectedArtistId) {
                artistDropdown.value = selectedArtistId;
            }
        } catch (error) {
            console.error("Error fetching artists:", error);
        }
    }
});