"use client";
import { useQuery } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import type { CommentDetails } from '../../../models/api/comment-models';
import type { PagedFeed } from '../../../models/paging-models';
import { commentsService } from '../../../services/comment-service';

export function useCommentsByPost(postId: UUID, page: number = 0, enabled: boolean = true) {
    return useQuery<CommentDetails[], Error>({
        queryKey: ['comments', postId, page],
        enabled: enabled && !!postId,
        queryFn: async () => {
            const response = await commentsService.getByPost(postId, { index: page });
            const data = response.data as PagedFeed<CommentDetails> & { Values?: CommentDetails[] };

            if (Array.isArray(data.values)) {
                return data.values;
            }

            if (Array.isArray(data.Values)) {
                return data.Values;
            }

            return [];
        },
        staleTime: 1000 * 30,
    });
}
