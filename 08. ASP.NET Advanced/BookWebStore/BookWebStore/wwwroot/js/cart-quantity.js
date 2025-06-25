document.addEventListener('DOMContentLoaded', function () {
    const quantityInputs = document.querySelectorAll('.quantity-input');
    const totalPriceElem = document.querySelector('.total-price');

    quantityInputs.forEach(function (input) {
        input.defaultValue = input.value;

        input.addEventListener('change', async function () {
            const newQuantity = parseInt(this.value);
            const row = this.closest('.row');
            const orderId = row.querySelector('.order-id')?.value;
            const bookId = row.querySelector('.book-id')?.value;

            if (!orderId || !bookId) {
                showError("Something went wrong while updating the quantity. Please refresh the page and try again.");
                return;
            }

            try {
                const response = await fetch('/Order/UpdateQuantity', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    body: new URLSearchParams({
                        orderId: orderId,
                        bookId: bookId,
                        quantity: newQuantity
                    })
                });

                const data = await response.json();

                if (data.success) {
                    const itemTotalElem = row.querySelector('.item-total');
                    itemTotalElem.textContent = `${data.itemTotal} lv.`;
                    totalPriceElem.textContent = `Total Price: ${data.totalPrice} lv.`;
                    this.defaultValue = this.value;
                } else {
                    showError(data.error);
                    if (data.resetTo !== undefined) {
                        this.value = data.resetTo;
                        this.defaultValue = data.resetTo;
                    } else {
                        this.value = this.defaultValue;
                    }
                }
            } catch (error) {
                showError("Unexpected error occurred. Please try again later.");
            }
        });
    });

    function showError(message) {
        const alert = document.getElementById('errorAlert');
        if (!alert) return;

        const paragraph = alert.querySelector('p');
        if (paragraph) {
            paragraph.textContent = message;
        }

        alert.classList.remove('d-none');
        alert.style.opacity = '1';

        setTimeout(() => {
            alert.style.transition = 'opacity 0.5s';
            alert.style.opacity = '0';
            setTimeout(() => {
                alert.classList.add('d-none');
                alert.style.opacity = '1';
            }, 500);
        }, 3000);
    }
});