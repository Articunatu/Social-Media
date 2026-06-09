import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import userService from '../../../services/user-service';

interface UnfollowUserInput {
    followerId: UUID;
    followingId: UUID;
}

export function useUnfollowUser() {
    const queryClient = useQueryClient();

    return useMutation<void, Error, UnfollowUserInput>({
        mutationFn: async (command) => {
            await userService.unfollow(command);
        },
        onSuccess: (_data, { followerId, followingId }) => {
            queryClient.invalidateQueries({ queryKey: ['profile', followingId] });
            queryClient.invalidateQueries({ queryKey: ['profile', followerId] });
            queryClient.invalidateQueries({ queryKey: ['feed'] });
        },
    });
}
