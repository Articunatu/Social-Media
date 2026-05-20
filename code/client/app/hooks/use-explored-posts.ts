import { useQuery } from '@tanstack/react-query';
import {
  FeedPost,
  ServerResult,
  ProfileFeedResponseServer,
  ProfilePostDto,
} from '../models/api/post-models';
import postService from '../services/post-service';

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

      let values: ProfilePostDto[] = [];

      if (isServerResult(data)) {
        values = data.value.profileFeed.values ?? [];
      } else if (isProfileFeedResponse(data)) {
        values = data.profileFeed.values ?? [];
      } else {
        return [];
      }

      return values.map<FeedPost>((p) => ({
        profile: {
          id: p.postId,
          tag: '',
          fullName: '',
          profilePhoto: '',
        },
        content: p.content,
        authorId: p.postId,
        createdAt: p.timeStamp,
      }));
    },
    enabled,
    staleTime: 1000 * 60,
  });
}