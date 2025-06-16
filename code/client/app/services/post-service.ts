import axios from 'axios';
import { Post } from '../models/post';
import { API_URL } from './api';

export const fetchPostsByUser = async (userId: number): Promise<Post[]> => {

  try {
    const response = await axios.get<Post[]>(`${API_URL}/posts`, {
      params: { userId },
    });
    return response.data;
  } 
  catch (error) {
    console.error('Error fetching posts:', error);
    throw error;
  }
};

