import api from './api';
import {
    DeleteAccountCommand,
    FollowCommand,
    ProfileDetails,
    SearchUserQuery,
    UnfollowCommand,
} from '../models/api/user-models';
import { UUID } from 'crypto';

const userUri = '/users';

const userService = {
    deleteAccount: (command: DeleteAccountCommand) =>
        api.delete(`${userUri}/delete`, { data: command }),

    follow: (command: FollowCommand) =>
        api.post(`${userUri}/follow`, command),
    
    getProfile: (userId: UUID) =>
        api.get<ProfileDetails>(`${userUri}/${userId}`),

    searchUsers: (query: SearchUserQuery) =>
        api.post<ProfileDetails[]>(`${userUri}/search`, query),

    unfollow: (command: UnfollowCommand) =>
        api.delete(`${userUri}/unfollow`, { data: command }),
};

export default userService;
