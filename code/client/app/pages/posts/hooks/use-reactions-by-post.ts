import { useQuery } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import type { ReactionResponse, ReactionType } from '../../../models/api/reaction-models';
import type { PagedFeed } from '../../../models/paging-models';
import reactionService from '../../../services/reaction-service';

function getPagedValues<T>(pagedFeed: PagedFeed<T> | (PagedFeed<T> & { Values?: T[] })) {
    if (Array.isArray(pagedFeed.values)) {
        return pagedFeed.values;
    }

    const alternatePagedFeed = pagedFeed as unknown as Record<string, unknown>;
    return Array.isArray(alternatePagedFeed.Values) ? alternatePagedFeed.Values as T[] : [];
}

export function useReactionsByPost(
    postId: UUID,
    pageNumber: number = 0,
    type?: ReactionType,
    enabled: boolean = true
) {
    return useQuery<ReactionResponse[], Error>({
        queryKey: ['post-reactions', postId, pageNumber, type],
        queryFn: async () => {
            const response = await reactionService.getReactionsByPost(postId, pageNumber, type);
            return getPagedValues(response.data);
        },
        enabled: enabled && !!postId,
        staleTime: 1000 * 60,
    });
}
