import { useQuery } from '@tanstack/react-query';
import type {
  FeedPost,
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
      return mapProfilePosts(response.data.profileFeed.values ?? []);
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
      profilePhoto: null,
    },
    content: post.content,
    authorId: EMPTY_UUID,
    createdAt: post.timeStamp,
    commentsCount: post.commentsCount,
    reactionCounts: post.reactionCounts ?? [],
  }));
}
