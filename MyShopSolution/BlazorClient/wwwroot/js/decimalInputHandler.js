window.handleDecimalInput = function (inputId) {
    document.getElementById(inputId).addEventListener('input', function (e) {
        let value = e.target.value;
        if (value.includes(',')) {
            value = value.replace(',', '.');
        }
        e.target.value = value;
    });
};
