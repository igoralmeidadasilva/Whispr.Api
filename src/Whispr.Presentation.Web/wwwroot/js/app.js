window.toggleTheme = () => {
    const html = document.getElementById('htmlRoot');
    const currentTheme = html.getAttribute('data-bs-theme');

    if (currentTheme === 'dark') {
        html.setAttribute('data-bs-theme', 'light');
        localStorage.setItem('theme', 'light');
    } else {
        html.setAttribute('data-bs-theme', 'dark');
        localStorage.setItem('theme', 'dark');
    }
}

window.addEventListener('DOMContentLoaded', () => {
    const savedTheme = localStorage.getItem('theme') || 'light';
    const html = document.getElementById('htmlRoot');
    html.setAttribute('data-bs-theme', savedTheme);
});