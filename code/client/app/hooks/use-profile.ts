import { useQuery } from '@tanstack/react-query';
import userService from '../services/user-service';
import { ProfileDetails } from '../models/api/user-models';
import { UUID } from 'crypto';

export function useProfile(userId: UUID) {
    return useQuery<ProfileDetails, Error>({
        queryKey: ['profile', userId],
        queryFn: async () => {
            const response = await userService.getProfile(userId);
            return response.data;
        },
        enabled: !!userId,
        staleTime: 1000 * 60,
    });
}
