import { useQuery } from '@tanstack/react-query';
import type { FeedPost, FeedResponseServer } from '../../../models/api/post-models';
import type { PagedFeed } from '../../../models/paging-models';
import postService from '../../../services/post-service';

function mapFeedResponseToFeedPost(item: FeedResponseServer): FeedPost {
    return {
        postId: item.post.postId,
        profile: item.profile,
        content: item.post.content,
        authorId: item.profile.id,
        createdAt: item.post.timeStamp,
        commentsCount: item.post.commentsCount,
        reactionCounts: item.post.reactionCounts ?? [],
    };
}

export function useFeed(pageNumber: number = 0) {
    return useQuery<FeedPost[], Error>({
        queryKey: ['feed', pageNumber],
        queryFn: async () => {
            const response = await postService.getFeed(pageNumber);
            const data = response.data as unknown;

            if (!data || typeof data !== 'object') {
                return [];
            }

            const paged = data as PagedFeed<FeedResponseServer> & { Values?: FeedResponseServer[] };
            const values = Array.isArray(paged.values) ? paged.values : Array.isArray(paged.Values) ? paged.Values : [];

            return values.map(mapFeedResponseToFeedPost);
        },
        staleTime: 1000 * 60,
    });
}
