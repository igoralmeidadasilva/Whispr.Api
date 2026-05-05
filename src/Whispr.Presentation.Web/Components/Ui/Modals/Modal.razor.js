export function initialize(element, dotnetHelper) {
    if (!element) {
        console.error('Element is null or undefined. Cannot initialize modal events.');
    }

    element.addEventListener('show.bs.modal', function () {
        dotnetHelper.invokeMethodAsync('HandleShow');
    });

    element.addEventListener('shown.bs.modal', function () {
        dotnetHelper.invokeMethodAsync('HandleShown');
    });

    element.addEventListener('hide.bs.modal', function () {
        dotnetHelper.invokeMethodAsync('HandleHide');
    });

    element.addEventListener('hidden.bs.modal', function () {
        dotnetHelper.invokeMethodAsync('HandleHidden');
    });
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
    if (element) {
        const modal = bootstrap.Modal.getOrCreateInstance(element);
        modal?.dispose();
    }
}