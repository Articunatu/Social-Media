"use client";
import { useMutation, useQueryClient } from '@tanstack/react-query';
import postService from '../../../services/post-service';
import type { CreatePostCommand, CreatePostResponse } from '../../../models/api/post-models';
import type { AxiosResponse } from 'axios';

export function useCreatePost() {
    const qc = useQueryClient();
    return useMutation<AxiosResponse<CreatePostResponse>, Error, CreatePostCommand>({
        mutationFn: (cmd: CreatePostCommand) => postService.createPost(cmd),
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ['feed'] });
            qc.invalidateQueries({ queryKey: ['explored-posts'] });
            qc.invalidateQueries({ queryKey: ['profile-posts'] });
        },
    });
}
