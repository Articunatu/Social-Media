import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import photoService from '../../../services/photo-service';

interface UploadPhotoInput {
    userId: UUID;
    file: File;
}

export function useUploadPhoto() {
    const queryClient = useQueryClient();

    return useMutation<UUID, Error, UploadPhotoInput>({
        mutationFn: async ({ userId, file }) => {
            const response = await photoService.upload(userId, file);
            return response.data;
        },
        onSuccess: (_photoId, { userId }) => {
            queryClient.invalidateQueries({ queryKey: ['user-photos', userId] });
        },
    });
}
