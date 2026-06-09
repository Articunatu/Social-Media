import type { UUID } from 'crypto';
import type { PhotoResponse, SetProfilePhotoCommand } from '../models/api/photo-models';
import api from './api';

const photoUri = '/photos';

const photoService = {
    getById: (id: UUID) =>
        api.get<PhotoResponse>(`${photoUri}/get-photo-by-id/${id}`),

    getByUserId: (userId: UUID) =>
        api.get<PhotoResponse[]>(`${photoUri}/get-photos-by-userid/${userId}`),

    upload: (userId: UUID, file: File) => {
        const formData = new FormData();
        formData.append('file', file);

        return api.post<UUID>(`${photoUri}/${userId}/upload`, formData);
    },

    setProfilePhoto: (command: SetProfilePhotoCommand) =>
        api.patch<boolean>(`${photoUri}/set-pfp`, command),
};

export default photoService;
