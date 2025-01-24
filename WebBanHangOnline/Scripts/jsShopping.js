$(document).ready(function () {
    $('body').on('click', '.btnAddToCard', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quantity = parseInt(tQuantity);
        }
        alert(id + " " + quantity);
    });
});