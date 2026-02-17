const modal = {}

export function initialize(elementRef) {
    if (elementRef) {
        modal.ref = new bootstrap.Modal(elementRef);
        modal.element = elementRef;
    }
}

export function show() {
    if (modal.ref) {
        modal.ref.show();
    }
}

export function hide() {
    if (modal.ref && modal.element) {
        const activeElement = document.activeElement;
        if (activeElement && modal.element.contains(activeElement)) {
            activeElement.blur();
        }
        modal.ref.hide();
    }
}