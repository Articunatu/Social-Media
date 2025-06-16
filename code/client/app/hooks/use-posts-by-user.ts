import { useQuery } from '@tanstack/react-query';
import { fetchPostsByUser } from '../services/post-service';
import { Post } from '../models/post';

export const usePostsByUser = (userId: number) => {
  return useQuery<Post[], Error>({
    queryKey: ['posts', userId], 
    queryFn: () => fetchPostsByUser(userId),
    staleTime: 1000 * 60 * 5
  });
};
