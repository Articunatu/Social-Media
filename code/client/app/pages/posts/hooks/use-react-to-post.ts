"use client";
import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import type { AxiosResponse } from 'axios';
import type { ReactionResponse, ReactToPostInput } from '../../../models/api/reaction-models';
import { useAuth } from '../../../authentication/auth-provider';
import reactionService from '../../../services/reaction-service';

export function useReactToPost(postId: UUID) {
    const qc = useQueryClient();
    const auth = useAuth();
    const currentUserId = auth?.user?.userId as UUID | undefined;

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
            qc.invalidateQueries({ queryKey: ['post-reactions', postId] });
            qc.invalidateQueries({ queryKey: ['feed'] });
            qc.invalidateQueries({ queryKey: ['explored-posts'] });
            qc.invalidateQueries({ queryKey: ['profile-posts'] });

            if (currentUserId) {
                qc.invalidateQueries({ queryKey: ['reacted-posts', currentUserId] });
            }
        },
    });
}
