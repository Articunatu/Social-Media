import axios from 'axios';

export const API_URL = "http://localhost:5041/api";

const api = axios.create({
    baseURL: `${API_URL}/api`,
    headers: {
        'Content-Type': 'application/json',
    },
    withCredentials: true, 
});

export default api;
