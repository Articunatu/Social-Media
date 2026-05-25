"use client";
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { UUID } from 'crypto';
import { AxiosResponse } from 'axios';
import { ReactionResponse, ReactToPostInput } from '../models/api/reaction-models';
import reactionService from '../services/reaction-service';

export function useReactToPost(postId: UUID) {
    const qc = useQueryClient();

    return useMutation<AxiosResponse<ReactionResponse>, Error, ReactToPostInput>({
        mutationFn: ({ type, currentReaction }) => {
            if (!currentReaction) {
                return reactionService.addReaction({
                    type,
                    messageId: postId,
                });
            }

            if (currentReaction.type === type) {
                return reactionService.removeReaction(currentReaction.id);
            }

            return reactionService.updateReaction({
                id: currentReaction.id,
                type,
            });
        },
        onSuccess: () => {
            qc.invalidateQueries({ queryKey: ['my-reaction', postId] });
            qc.invalidateQueries({ queryKey: ['feed'] });
            qc.invalidateQueries({ queryKey: ['explored-posts'] });
            qc.invalidateQueries({ queryKey: ['profile-posts'] });
        },
    });
}
