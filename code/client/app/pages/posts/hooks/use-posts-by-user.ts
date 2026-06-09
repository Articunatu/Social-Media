import { useQuery } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import type {
    FeedPost,
    ProfileFeedResponseServer,
    ProfilePostDto,
    ServerResult,
} from '../../../models/api/post-models';
import type { ProfileInfo } from '../../../models/api/user-models';
import postService from '../../../services/post-service';

function isServerResult(value: unknown): value is ServerResult<ProfileFeedResponseServer> {
    if (typeof value !== 'object' || value === null) {
        return false;
    }

    return 'value' in value;
}

function isProfileFeedResponse(value: unknown): value is ProfileFeedResponseServer {
    if (typeof value !== 'object' || value === null) {
        return false;
    }

    return 'profileFeed' in value;
}

function mapProfilePostToFeedPost(post: ProfilePostDto, profile: ProfileInfo): FeedPost {
    return {
        postId: post.postId,
        profile,
        content: post.content,
        authorId: profile.id,
        createdAt: post.timeStamp,
        commentsCount: post.commentsCount,
        reactionCounts: post.reactionCounts ?? [],
    };
}

export function usePostsByUser(userId: UUID, profile: ProfileInfo | undefined, pageNumber: number = 0) {
    return useQuery<FeedPost[], Error>({
        queryKey: ['profile-posts', userId, pageNumber],
        queryFn: async () => {
            const response = await postService.getProfilePosts(userId, pageNumber);
            const data = response.data as unknown;

            const profileFeed = isServerResult(data)
                ? data.value?.profileFeed
                : isProfileFeedResponse(data)
                    ? data.profileFeed
                    : undefined;

            if (!profile || !profileFeed?.values) {
                return [];
            }

            return profileFeed.values.map((post) => mapProfilePostToFeedPost(post, profile));
        },
        enabled: !!userId && !!profile,
        staleTime: 1000 * 60,
    });
}
