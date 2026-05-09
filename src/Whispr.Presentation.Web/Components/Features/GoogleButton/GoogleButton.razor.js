export function initialize(clientId, dotNetRef) {
    google.accounts.id.initialize({
        client_id: clientId,
        callback: (response) => {
            dotNetRef?.invokeMethodAsync('OnGoogleCredentialReceived', response.credential);
        },
        auto_select: false,
        cancel_on_tap_outside: true,
    });
}

export function signIn(dotNetRef) {
    google.accounts.id.prompt((notification) => {

        if (notification.isDismissedMoment()) {
            dotNetRef?.invokeMethodAsync('OnGoogleLoginDismissed');
        }

        if (notification.isNotDisplayed() || notification.isSkippedMoment()) {
            const container = document.getElementById('google-signin-fallback');
            if (container && container.childElementCount === 0) {
                google.accounts.id.renderButton(container, {
                    type: 'standard',
                    theme: 'outline',
                    size: 'large',
                    text: 'signin_with',
                    locale: 'pt-BR',
                });
                container.querySelector('div[role=button]')?.click();
            }
        }
    });
}