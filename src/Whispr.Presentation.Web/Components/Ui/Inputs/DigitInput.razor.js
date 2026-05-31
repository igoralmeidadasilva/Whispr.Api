const handlers = new WeakMap();

export function initialize(element) {
    if (!element) {
        console.error('Element is null or undefined. Cannot initialize digit-input events.');
        return;
    }

    if (handlers.has(element)) {
        return;
    }

    const handler = function (e) {
        this.value = this.value.replace(/[^0-9]/g, '');
    };

    handlers.set(element, handler);
    element.addEventListener('input', handler);
}

export function dispose(element) {
    if (!element) {
        console.error('Element is null or undefined. Cannot dispose digit-input events.');
        return;
    }

    const handler = handlers.get(element);
    if (handler) {
        element.removeEventListener('input', handler);
        handlers.delete(element);
    }
}