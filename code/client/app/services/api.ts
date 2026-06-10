import axios, { AxiosError, type InternalAxiosRequestConfig } from 'axios';

export const API_URL = "https://localhost:7018/api";

type RetriableRequestConfig = InternalAxiosRequestConfig & {
    _retry?: boolean;
};

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
    return config;
});

async function refreshAccessToken() {
    const response = await axios.post<string>(
        `${API_URL}/auth/refresh-token`,
        {},
        { withCredentials: true }
    );

    return response.data;
}

function clearSessionAndRedirect() {
    if (typeof window === 'undefined') {
        return;
    }

    localStorage.removeItem('jwt');

    if (window.location.pathname !== '/') {
        window.location.assign('/');
    }
}

function shouldSkipRefresh(url: string | undefined) {
    return !url
        || url.includes('/auth/login')
        || url.includes('/auth/signup')
        || url.includes('/auth/refresh-token');
}

api.interceptors.response.use(
    (response) => response,
    async (error: AxiosError) => {
        const originalRequest = error.config as RetriableRequestConfig | undefined;

        if (
            error.response?.status !== 401
            || !originalRequest
            || originalRequest._retry
            || shouldSkipRefresh(originalRequest.url)
        ) {
            return Promise.reject(error);
        }

        originalRequest._retry = true;

        try {
            const accessToken = await refreshAccessToken();
            localStorage.setItem('jwt', accessToken);
            originalRequest.headers = originalRequest.headers || {};
            originalRequest.headers.Authorization = `Bearer ${accessToken}`;

            return api(originalRequest);
        } catch (refreshError) {
            clearSessionAndRedirect();
            return Promise.reject(refreshError);
        }
    }
);

export default api;
