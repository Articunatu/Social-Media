import { useQuery } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import type { ReactedProfilePost } from '../../../models/api/reaction-models';
import type { PagedFeed } from '../../../models/paging-models';
import reactionService from '../../../services/reaction-service';

function getPagedValues<T>(pagedFeed: PagedFeed<T> | (PagedFeed<T> & { Values?: T[] })) {
    if (Array.isArray(pagedFeed.values)) {
        return pagedFeed.values;
    }

    const alternatePagedFeed = pagedFeed as unknown as Record<string, unknown>;
    return Array.isArray(alternatePagedFeed.Values) ? alternatePagedFeed.Values as T[] : [];
}

export function useReactedPostsByUser(userId: UUID, pageIndex: number = 0, enabled: boolean = true) {
    return useQuery<ReactedProfilePost[], Error>({
        queryKey: ['reacted-posts', userId, pageIndex],
        queryFn: async () => {
            const response = await reactionService.getReactedPostsByUser(userId, pageIndex);
            return getPagedValues(response.data);
        },
        enabled: enabled && !!userId,
        staleTime: 1000 * 60,
    });
}
