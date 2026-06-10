"use client";

import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import postService from '../../../services/post-service';

export function useDeletePost() {
    const queryClient = useQueryClient();

    return useMutation<void, Error, UUID>({
        mutationFn: async (postId) => {
            await postService.deletePost(postId);
        },
        onSuccess: (_data, postId) => {
            queryClient.invalidateQueries({ queryKey: ['feed'] });
            queryClient.invalidateQueries({ queryKey: ['profile-posts'] });
            queryClient.invalidateQueries({ queryKey: ['explored-posts'] });
            queryClient.invalidateQueries({ queryKey: ['post-details', postId] });
            queryClient.invalidateQueries({ queryKey: ['comments', postId] });
        },
    });
}
