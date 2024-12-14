document.addEventListener('DOMContentLoaded', function () {
    const quantityInputs = document.querySelectorAll('.quantity-input');

    quantityInputs.forEach(function (input) {
        let previousValue = input.value; //Store the initial value

        input.addEventListener('focus', function () {
            //Save the current value when the field is focused
            previousValue = this.value;
        });

        input.addEventListener('change', function () {
            const parentRow = this.closest('.row');
            const orderId = parentRow.querySelector('.order-id').value;
            const albumId = parentRow.querySelector('.album-id').value;
            const quantity = parseInt(this.value);

            if (quantity < 1 || isNaN(quantity)) {
                alert('Quantity must be at least 1.');
                this.value = previousValue; //Reset to previous value if invalid
                return;
            }

            const formData = new FormData();
            formData.append('id', orderId);
            formData.append('albumId', albumId);
            formData.append('quantity', quantity);

            //Get the anti-forgery token from the page
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
            formData.append('__RequestVerificationToken', token);

            //Send the FormData with the anti-forgery token included
            fetch('/Order/UpdateQuantity', {
                method: 'POST',
                body: formData //No need for 'Content-Type' header
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        const totalPrice = data.updatedPrice;
                        parentRow.querySelector('.total-price').textContent = `${totalPrice} lv.`;
                        updateTotalCartPrice(); //Update the total price of the cart
                        previousValue = quantity; //Update the stored value to the new value
                    } else {
                        alert(data.error || 'Could not update quantity.');
                        this.value = previousValue; //Reset to previous value if the update fails
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert(`There are only ${quantity} units of the album in stock.`);
                    this.value = previousValue; //Reset to previous value in case of error
                });
        });
    });

    function updateTotalCartPrice() {
        let totalCartPrice = 0;
        const totalPrices = document.querySelectorAll('.total-price');
        totalPrices.forEach(function (priceElement) {
            const price = parseFloat(priceElement.textContent.replace(' lv.', '').trim());
            totalCartPrice += price;
        });

        const totalPriceElement = document.querySelector('.total-price-cart');
        if (totalPriceElement) {
            totalPriceElement.textContent = `${totalCartPrice.toFixed(2)} lv.`;
        }
    }
});