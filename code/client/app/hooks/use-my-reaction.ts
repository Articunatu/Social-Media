"use client";
import { useQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { UUID } from 'crypto';
import { ReactionResponse } from '../models/api/reaction-models';
import reactionService from '../services/reaction-service';

export function useMyReaction(postId: UUID, enabled: boolean = true) {
    return useQuery<ReactionResponse | null, Error>({
        queryKey: ['my-reaction', postId],
        enabled: enabled && !!postId,
        queryFn: async () => {
            try {
                const response = await reactionService.getMyReaction(postId);
                const data = response.data as unknown;
                if (!data || typeof data !== 'object') {
                    return null;
                }
                return data as ReactionResponse;
            } catch (error) {
                const axiosError = error as AxiosError;
                if (axiosError.response?.status === 204 || axiosError.response?.status === 404) {
                    return null;
                }
                throw error as Error;
            }
        },
        staleTime: 1000 * 30,
    });
}
