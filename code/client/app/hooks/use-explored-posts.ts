import { useQuery } from '@tanstack/react-query';
import { FeedPost } from '../models/api/post-models';
import postService from '../services/post-service';

export function useExploredPosts(enabled: boolean = false) {
  return useQuery<FeedPost[], Error>({
    queryKey: ['explored-posts'],
    queryFn: async () => {
      const response = await postService.getExploredPosts();
      const data = response.data;
      if (Array.isArray(data)) {
        return data;
      }
      return [];
    },
    enabled,
    staleTime: 1000 * 60,
  });
}