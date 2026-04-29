import axios from 'axios';

export const API_URL = "https://localhost:7018/api";

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true, 
});

export default api;
