"use client";

import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import { commentsService } from '../../../services/comment-service';

interface DeleteCommentInput {
    commentId: UUID;
    parentPostId: UUID;
}

export function useDeleteComment() {
    const queryClient = useQueryClient();

    return useMutation<void, Error, DeleteCommentInput>({
        mutationFn: async ({ commentId }) => {
            await commentsService.delete(commentId);
        },
        onSuccess: (_data, { parentPostId }) => {
            queryClient.invalidateQueries({ queryKey: ['comments', parentPostId] });
            queryClient.invalidateQueries({ queryKey: ['feed'] });
            queryClient.invalidateQueries({ queryKey: ['profile-posts'] });
            queryClient.invalidateQueries({ queryKey: ['explored-posts'] });
            queryClient.invalidateQueries({ queryKey: ['post-details', parentPostId] });
        },
    });
}
