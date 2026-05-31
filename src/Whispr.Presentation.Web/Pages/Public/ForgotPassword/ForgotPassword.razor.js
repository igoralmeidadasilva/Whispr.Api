export function configureDigitInputs() {
    const inputs = document.querySelectorAll('.digit-control');

    inputs.forEach((input, index) => {
        input.addEventListener('keydown', (event) => {
            if (event.key === 'Backspace') {
                if (input.value === '' && index > 0) {
                    inputs[index - 1].focus();
                }
            }
        });

        input.addEventListener('input', () => {
            if (input.value.length >= input.maxLength) {
                if (index + 1 < inputs.length) {
                    inputs[index + 1].focus();
                }
            }
        });

        input.addEventListener('paste', (e) => {
            e.preventDefault();

            const pastedData = (e.clipboardData || window.clipboardData).getData('text');
            const digitsOnly = pastedData.replace(/\D/g, '');

            let fillIndex = index;
            for (let i = 0; i < digitsOnly.length && fillIndex < inputs.length; i++) {
                inputs[fillIndex].value = digitsOnly[i];

                inputs[fillIndex].dispatchEvent(new Event('change', { bubbles: true }));

                fillIndex++;
            }

            const focusIndex = Math.min(fillIndex, inputs.length - 1);
            inputs[focusIndex].focus();
        });
    });
}