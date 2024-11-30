document.addEventListener("DOMContentLoaded", () => {
    const genreDropdown = document.getElementById("genreDropdown");
    const artistDropdown = document.getElementById("artistDropdown");

    // Деактивиране на артисти по подразбиране, ако няма избран жанр
    artistDropdown.disabled = !genreDropdown.value;

    // Проверка при зареждане на страницата, ако има избран жанр
    const initialGenreId = genreDropdown.value;
    if (initialGenreId) {
        // Зареждаме артистите от този жанр
        fetchArtistsByGenre(initialGenreId);
    }

    // Обработчик за смяна на жанра
    genreDropdown.addEventListener("change", async () => {
        const genreId = genreDropdown.value;
        artistDropdown.innerHTML = '<option value="">Select artist</option>'; // Reset artist dropdown

        // Ако няма избран жанр, деактивираме падащото меню за артисти
        if (!genreId) {
            artistDropdown.disabled = true;
            return;
        }

        // Ако има избран жанр, активираме падащото меню за артисти
        artistDropdown.disabled = false;
        await fetchArtistsByGenre(genreId);
    });

    // Функция за зареждане на артисти по жанр
    async function fetchArtistsByGenre(genreId) {
        try {
            const response = await fetch(`/Album/GetArtistsByGenre?genreId=${genreId}`);
            const data = await response.json();

            // Изчистваме падащото меню за артисти и добавяме новите
            artistDropdown.innerHTML = '<option value="">Select artist</option>';
            data.forEach(artist => {
                const option = document.createElement("option");
                option.value = artist.id;
                option.textContent = artist.name;
                artistDropdown.appendChild(option);
            });

            //Ако има предварително избран артист, го задържаме
            const initialArtistId = artistDropdown.getAttribute('data-selected-artist');
            if (initialArtistId) {
                artistDropdown.value = initialArtistId;
            }
        } catch (error) {
            console.error("Error fetching artists:", error);
        }
    }
});