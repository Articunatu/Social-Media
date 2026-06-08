// import { useQuery } from '@tanstack/react-query';
// import { getProfilePosts } from  "../services/post-service";
// import { Post } from '../models/post';

// export const usePostsByUser = (userId: number) => {
//   return useQuery<Post[], Error>({
//     queryKey: ['posts', userId], 
//     queryFn: () => getProfilePosts(userId),
//     staleTime: 1000 * 60 * 5
//   });
// };
