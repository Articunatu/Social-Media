import axios from 'axios';

export const API_URL = "https://localhost:7018/api";

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true, 
});

// Attach JWT from localStorage to every request if available
api.interceptors.request.use((config) => {
    const token = typeof window !== 'undefined' ? localStorage.getItem('jwt') : null;
    if (token) {
        config.headers = config.headers || {};
        config.headers['Authorization'] = `Bearer ${token}`;
    }
    // Debug: log whether a token was attached and the request target
    try {
        console.debug('[api] request', { method: config.method, url: config.url, hasToken: !!token });
    } catch {}
    return config;
});

api.interceptors.response.use(
    (response) => response,
    (error) => {
        try {
            console.error('[api] response error');
            try {
                console.error('status', error && error.response ? error.response.status : null);
            } catch {}
            try {
                console.error('url', error && error.config ? (error.config.url ?? (error.request && error.request.responseURL ? error.request.responseURL : null)) : null);
            } catch {}
            try {
                console.error('data', error && error.response ? error.response.data : null);
            } catch {}
            try {
                console.error('message', error?.message ?? null);
            } catch {}
            try {
                console.error('requestHeaders', error && error.config ? error.config.headers : null);
            } catch {}
        } catch (logErr) {
            try {
                console.error('[api] response error (logging failed)', logErr);
            } catch {}
        }
        return Promise.reject(error);
    }
);

export default api;
