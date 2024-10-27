export class HttpCancelPreviousRequestService {
    constructor() {
        this.pendingRequests = new Map();
    }

    cancelRequestByUrl(url) {
        if (this.pendingRequests.has(url)) {
            const controller = this.pendingRequests.get(url);
            controller.abort();
            this.pendingRequests.delete(url);
        }
    }

    async intercept(url, requestFunction) {
        this.cancelRequestByUrl(url);

        const controller = new AbortController();
        this.pendingRequests.set(url, controller);

        try {
            const response = await requestFunction(controller.signal);
            this.pendingRequests.delete(url);
            const responseData = await response.json();
             
            if (response.ok && responseData.StatusCode === 200) {
                return responseData;  
            } else {
                throw new Error(responseData.Error || "Erro desconhecido");
            }
        } catch (error) {
            if (error.name === 'AbortError') {
                console.log('Requisição cancelada:', url);
            } else {
                console.error('Erro na requisição:', error.message);
            }
            throw error;
        }
    }

    get(url) {
        return this.intercept(url, (signal) => fetch(url, { method: 'GET', signal }));
    }

    post(url, body) {
        return this.intercept(url, (signal) => fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body),
            signal
        }));
    }

    delete(url, body = null) {
        return this.intercept(url, (signal) => fetch(url, {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
            body: body ? JSON.stringify(body) : undefined,
            signal
        }));
    }

    put(url, body) {
        return this.intercept(url, (signal) => fetch(url, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body),
            signal
        }));
    }
}
