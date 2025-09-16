import axios from 'axios';

export const API_URL = import.meta.url;

const api = axios.create({
    baseURL: `${API_URL}/api`,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true, 
});

export default api;
