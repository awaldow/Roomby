export class AuthClient {
    constructor(private roomsApiUrl: string) {
    }

    getBaseUrl(defaultUrl: string, requestedUrl?: string) {
        return requestedUrl ? requestedUrl : this.roomsApiUrl;
    }

    transformHttpRequestOptions(options: RequestInit): Promise<RequestInit> {
        return Promise.resolve(options);
    }
}

export class OidcClientSettings {
    static create(settings: {
        serverUrl: string, stsServerUrl: string, clientId: string, customerAlias: string,
        customerId: string, scope: string, redirectServerUrl?: string, logoutServerUrl?: string
    }) {

        return {
            client_id: settings.clientId,
            scope: settings.scope,
            authority: settings.stsServerUrl,
            response_type: "id_token token",
            filterProtocolClaims: true,
            loadUserInfo: true,
            redirect_uri: settings.redirectServerUrl ? settings.redirectServerUrl : settings.serverUrl + '/auth-callback',
            post_logout_redirect_uri: settings.logoutServerUrl ? settings.logoutServerUrl : settings.serverUrl,
            acr_values: 'tenant:{"id":"' +
                settings.customerId + '","alias":"' +
                settings.customerAlias + '"}'
        }
    }
}

export class AccessTokenAuthClient extends AuthClient {
    constructor(roomsApiUrl: string, private accessToken: string) {
        super(roomsApiUrl);
    }

    transformHttpRequestOptions(options: RequestInit): Promise<RequestInit> {
        if (options.headers && this.accessToken) {
            options.headers['Authorization'] = 'Bearer ' + this.accessToken;
        }

        return super.transformHttpRequestOptions(options);
    }
}

export class AuthorizedApiBase {
    protected constructor(private authClient: AuthClient) {

    }

    getBaseUrl(defaultUrl: string, baseUrl: string) {
        return this.authClient ? this.authClient.getBaseUrl(defaultUrl) : baseUrl;
    }

    transformOptions(options: RequestInit): Promise<RequestInit> {
        return this.authClient ? this.authClient.transformHttpRequestOptions(options) : Promise.resolve(options);
    }
}