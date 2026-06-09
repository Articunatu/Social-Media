import { useMutation } from '@tanstack/react-query';
import type { ProfileInfo } from '../../../models/api/user-models';
import userService from '../../../services/user-service';

export function useSearchUsers() {
    return useMutation<ProfileInfo[], Error, string>({
        mutationFn: async (searchText) => {
            const response = await userService.searchUsers({
                filter: {
                    index: 0,
                    order: 'Tag',
                    searchText,
                },
            });

            return response.data;
        },
    });
}
