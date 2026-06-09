import { useQuery } from '@tanstack/react-query';
import type { UUID } from 'crypto';
import type { PhotoResponse } from '../../../models/api/photo-models';
import photoService from '../../../services/photo-service';

export function useUserPhotos(userId: UUID, enabled: boolean = true) {
    return useQuery<PhotoResponse[], Error>({
        queryKey: ['user-photos', userId],
        queryFn: async () => {
            const response = await photoService.getByUserId(userId);
            return response.data;
        },
        enabled: enabled && !!userId,
        staleTime: 1000 * 60,
    });
}
