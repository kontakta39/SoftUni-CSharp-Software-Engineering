document.addEventListener('DOMContentLoaded', function () {
    const carousel = document.querySelector('#bookCarousel');
    const indicators = document.querySelectorAll('.carousel-indicators button');

    if (!carousel || indicators.length === 0) return;

    carousel.addEventListener('slid.bs.carousel', () => {
        const activeIndex = Array.from(carousel.querySelectorAll('.carousel-item')).findIndex(item => item.classList.contains('active'));
        indicators.forEach((btn, idx) => {
            if (idx === activeIndex) {
                btn.classList.add('active');
                btn.style.opacity = '1';
            } else {
                btn.classList.remove('active');
                btn.style.opacity = '0.5';
            }
        });
    });
});
