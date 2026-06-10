import { useQuery } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import type { FeedPost } from '../../../models/api/post-models';
import postService from '../../../services/post-service';

function mapPostDetailsToFeedPost(postId: UUID, data: Awaited<ReturnType<typeof postService.getPostById>>['data']): FeedPost {
    return {
        postId,
        profile: data.profileMain,
        content: data.post.content,
        authorId: data.profileMain.id,
        createdAt: data.post.timeStamp,
        commentsCount: data.post.commentsCount,
        reactionCounts: data.post.reactionCounts ?? [],
    };
}

export function usePostDetails(postId: UUID, pageNumber: number = 0) {
    return useQuery<FeedPost, Error>({
        queryKey: ['post-details', postId, pageNumber],
        queryFn: async () => {
            const response = await postService.getPostById(postId, pageNumber);
            return mapPostDetailsToFeedPost(postId, response.data);
        },
        enabled: !!postId,
        staleTime: 1000 * 60,
    });
}
