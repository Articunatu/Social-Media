"use client";
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { CreateCommentCommand, CreatedCommentResponse } from '../models/api/comment-models';
import { commentsService } from '../services/comment-service';
import { AxiosResponse } from 'axios';

export function useCreateComment() {
    const qc = useQueryClient();

    return useMutation<AxiosResponse<CreatedCommentResponse>, Error, CreateCommentCommand>({
        mutationFn: (cmd: CreateCommentCommand) => commentsService.create(cmd),
        onSuccess: (_response, variables) => {
            qc.invalidateQueries({ queryKey: ['comments', variables.parentPostId] });
            qc.invalidateQueries({ queryKey: ['feed'] });
            qc.invalidateQueries({ queryKey: ['explored-posts'] });
            qc.invalidateQueries({ queryKey: ['profile-posts'] });
        },
    });
}
