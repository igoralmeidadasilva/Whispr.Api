export function show(id, dotnetHelper) {
    const element = document.getElementById(id);
    if (element == null) {
        console.error(`Element with id ${id} not found.`);
        return;
    }

    element.addEventListener('show.bs.toast', function () {
        dotnetHelper.invokeMethodAsync('OnShow');
    });

    element.addEventListener('shown.bs.toast', function () {
        dotnetHelper.invokeMethodAsync('OnShown');
    });

    element.addEventListener('hide.bs.toast', function () {
        dotnetHelper.invokeMethodAsync('OnHide');
    });

    element.addEventListener('hidden.bs.toast', function () {
        dotnetHelper.invokeMethodAsync('OnHidden');
    });

    const toast = bootstrap.Toast.getOrCreateInstance(element);
    toast?.show();
}

export function hide(id) {
    const element = document.getElementById(id);
    if (element == null) {
        console.error(`Element with id ${id} not found.`);
        return;
    }
    const toast = bootstrap.Toast.getOrCreateInstance(element);
    toast?.hide();
}

export function dispose(id) {
    const element = document.getElementById(id);
    if (element == null) {
        console.error(`Element with id ${id} not found.`);
        return;
    }
    const toast = bootstrap.Toast.getOrCreateInstance(element);
    toast?.dispose();
}