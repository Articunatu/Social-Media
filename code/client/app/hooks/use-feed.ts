import { useQuery } from '@tanstack/react-query';
import postService from '../services/post-service';
import { FeedPost } from '../models/api/post-models';

export function useFeed(pageNumber: number = 1) {
    return useQuery<FeedPost[], Error>({
        queryKey: ['feed', pageNumber],
        queryFn: async () => {
            const response = await postService.getFeed(pageNumber);
            return response.data;
        },
        staleTime: 1000 * 60,
    });
}
