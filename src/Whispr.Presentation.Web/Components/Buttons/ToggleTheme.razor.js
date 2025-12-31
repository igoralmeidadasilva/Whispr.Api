export function setTheme(theme) {
    const html = document.getElementById('htmlRoot');
    html.setAttribute('data-bs-theme', theme);
}