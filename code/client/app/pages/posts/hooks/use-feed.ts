import { useQuery } from '@tanstack/react-query';
import type { FeedPost, FeedResponseServer } from '../../../models/api/post-models';
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
            return response.data.values.map(mapFeedResponseToFeedPost);
        },
        staleTime: 1000 * 60,
    });
}
