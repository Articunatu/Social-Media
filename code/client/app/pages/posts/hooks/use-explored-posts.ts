import { useQuery } from '@tanstack/react-query';
import type {
  FeedPost,
  ServerResult,
  ProfileFeedResponseServer,
  ProfilePostDto,
} from '../../../models/api/post-models';
import type { UUID } from 'crypto';
import postService from '../../../services/post-service';

const EMPTY_UUID = '00000000-0000-0000-0000-000000000000' as unknown as UUID;

export function useExploredPosts(enabled: boolean = false) {
  return useQuery<FeedPost[], Error>({
    queryKey: ['explored-posts'],
    queryFn: async () => {
      const response = await postService.getExploredPosts();
      const data = response.data as unknown;

      const isServerResult = (
        x: unknown
      ): x is ServerResult<ProfileFeedResponseServer> => {
        if (typeof x !== 'object' || x === null) return false;
        const r = x as Record<string, unknown>;
        if (!('value' in r)) return false;
        const val = r.value as Record<string, unknown> | undefined;
        return typeof val === 'object' && val !== undefined && 'profileFeed' in val;
      };

      const isProfileFeedResponse = (x: unknown): x is ProfileFeedResponseServer => {
        if (typeof x !== 'object' || x === null) return false;
        const r = x as Record<string, unknown>;
        return 'profileFeed' in r && typeof r.profileFeed === 'object';
      };

      if (isServerResult(data)) {
        return mapProfilePosts(data.value.profileFeed.values ?? []);
      }

      if (isProfileFeedResponse(data)) {
        return mapProfilePosts(data.profileFeed.values ?? []);
      }

      return [];
    },
    enabled,
    staleTime: 1000 * 60,
  });
}

function mapProfilePosts(values: ProfilePostDto[]): FeedPost[] {
  return values.map<FeedPost>((post) => ({
    postId: post.postId,
    profile: {
      id: EMPTY_UUID,
      tag: '',
      fullName: '',
      profilePhoto: '',
    },
    content: post.content,
    authorId: EMPTY_UUID,
    createdAt: post.timeStamp,
    commentsCount: post.commentsCount,
    reactionCounts: post.reactionCounts ?? [],
  }));
}
