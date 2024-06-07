document.addEventListener('DOMContentLoaded', function () {
    var enviar = document.getElementById('submitBtn');

    function validateFields() {
        const email = document.getElementById('usernameL').value.trim();
        const password = document.getElementById('passwordL').value.trim();

        if (email && password) {
            enviar.disabled = false;
        } else {
            enviar.disabled = true;
        }
    }
    document.getElementById('usernameL').addEventListener('input', validateFields);
    document.getElementById('passwordL').addEventListener('input', validateFields);
});

