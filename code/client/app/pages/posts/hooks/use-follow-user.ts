import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import userService from '../../../services/user-service';

interface FollowUserInput {
    followerId: UUID;
    followingId: UUID;
}

export function useFollowUser() {
    const queryClient = useQueryClient();

    return useMutation<void, Error, FollowUserInput>({
        mutationFn: async (command) => {
            await userService.follow(command);
        },
        onSuccess: (_data, { followerId, followingId }) => {
            queryClient.invalidateQueries({ queryKey: ['profile', followingId] });
            queryClient.invalidateQueries({ queryKey: ['profile', followerId] });
            queryClient.invalidateQueries({ queryKey: ['feed'] });
        },
    });
}
