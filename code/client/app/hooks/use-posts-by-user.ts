import { useQuery } from '@tanstack/react-query';
import { fetchPostsByUser, Post } from '../services/post-service';

export const usePostsByUser = (userId: number) => {
  return useQuery<Post[], Error>({
    queryKey: ['posts', userId], 
    queryFn: () => fetchPostsByUser(userId),
    staleTime: 1000 * 60 * 5, 
    keepPreviousData: true, 
  });
};
