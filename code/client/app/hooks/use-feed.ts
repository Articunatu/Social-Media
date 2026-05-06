import { useQuery } from '@tanstack/react-query';
import { FeedPost } from '../models/api/post-models';
import postService from '../services/post-service';

interface PagedFeed<T> {
    Values: T[];
    // ...other paging properties if needed
}

export function useFeed(pageNumber: number = 1) {
    return useQuery<FeedPost[], Error>({
        queryKey: ['feed', pageNumber],
        queryFn: async () => {
            const response = await postService.getFeed(pageNumber);
            // Expecting response.data.Values to be the array
            return (response.data as PagedFeed<FeedPost>).Values;
        },
        staleTime: 1000 * 60,
    });
}
