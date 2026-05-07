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
                const data = response.data;
                if (Array.isArray(data)) {
                    return data;
                } 
                if (data && Array.isArray((data as PagedFeed<FeedPost>).Values)) {
                    return (data as PagedFeed<FeedPost>).Values;
                } 
                return [];
        },
        staleTime: 1000 * 60,
    });
}
