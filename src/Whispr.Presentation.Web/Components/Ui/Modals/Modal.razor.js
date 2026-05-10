const _handlers = new Map();

export function initialize(element, dotnetHelper) {
    if (!element) {
        console.error('Element is null or undefined. Cannot initialize modal events.');
        return;
    }

    const onShow = () => dotnetHelper.invokeMethodAsync('HandleShow');
    const onShown = () => dotnetHelper.invokeMethodAsync('HandleShown');
    const onHide = () => dotnetHelper.invokeMethodAsync('HandleHide');
    const onHidden = () => dotnetHelper.invokeMethodAsync('HandleHidden');

    element.addEventListener('show.bs.modal', onShow);
    element.addEventListener('shown.bs.modal', onShown);
    element.addEventListener('hide.bs.modal', onHide);
    element.addEventListener('hidden.bs.modal', onHidden);

    _handlers.set(element, { onShow, onShown, onHide, onHidden });
}

export function show(element) {
    if (element) {
        const modal = bootstrap.Modal.getOrCreateInstance(element);
        modal?.show();
    }
}

export function hide(element) {
    if (element) {
        const activeElement = document.activeElement;
        const modal = bootstrap.Modal.getOrCreateInstance(element);
        if (activeElement && element.contains(activeElement)) {
            activeElement.blur();
        }
        modal?.hide();
    }
}

export function dispose(element) {
    if (!element) {
        return;
    }

    const handlers = _handlers.get(element);
    if (handlers) {
        element.removeEventListener('show.bs.modal', handlers.onShow);
        element.removeEventListener('shown.bs.modal', handlers.onShown);
        element.removeEventListener('hide.bs.modal', handlers.onHide);
        element.removeEventListener('hidden.bs.modal', handlers.onHidden);
        _handlers.delete(element);
    }

    const modal = bootstrap.Modal.getInstance(element);
    modal?.dispose();
}