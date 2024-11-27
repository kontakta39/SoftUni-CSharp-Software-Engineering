document.addEventListener('DOMContentLoaded', function () {
    const quantityInputs = document.querySelectorAll('.quantity-input');

    quantityInputs.forEach(function (input) {
        input.addEventListener('change', function () {
            const parentRow = this.closest('.row');
            const orderId = parentRow.querySelector('.order-id').value;
            const albumId = parentRow.querySelector('.album-id').value;
            const quantity = parseInt(this.value);

            if (quantity < 1 || isNaN(quantity)) {
                alert('Quantity must be at least 1.');
                this.value = 1; // Reset to 1 if invalid
                return;
            }

            const formData = new FormData();
            formData.append('id', orderId);
            formData.append('albumId', albumId);
            formData.append('quantity', quantity);

            // Get the anti-forgery token from the page
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            formData.append('__RequestVerificationToken', token);

            // Send the FormData with the anti-forgery token included
            fetch('/Order/UpdateQuantity', {
                method: 'POST',
                body: formData // No need for 'Content-Type' header
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        const totalPrice = data.updatedPrice;
                        parentRow.querySelector('.total-price').textContent = `${totalPrice} лв.`;
                    } else {
                        alert(data.error || 'Could not update quantity.');
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('An error occurred while updating the quantity.');
                });
        });
    });
});