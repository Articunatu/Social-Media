import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import photoService from '../../../services/photo-service';

interface SetProfilePhotoInput {
    userId: UUID;
    photoId: UUID;
}

export function useSetProfilePhoto() {
    const queryClient = useQueryClient();

    return useMutation<boolean, Error, SetProfilePhotoInput>({
        mutationFn: async ({ photoId }) => {
            const response = await photoService.setProfilePhoto({ photoId });
            return response.data;
        },
        onSuccess: (_isUpdated, { userId }) => {
            queryClient.invalidateQueries({ queryKey: ['profile', userId] });
            queryClient.invalidateQueries({ queryKey: ['user-photos', userId] });
            queryClient.invalidateQueries({ queryKey: ['feed'] });
            queryClient.invalidateQueries({ queryKey: ['explored-posts'] });
        },
    });
}
