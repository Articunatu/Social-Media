import api from './api';
import {
  CreatePostCommand,
  CreatePostResponse,
  FeedResponseServer,
  PostDetails,
  ProfileFeedResponseServer,
} from '../models/api/post-models';
import { PagedFeed } from '../models/paging-models';
import { UUID } from 'crypto';

const postUri = '/posts';

const postService = {
  createPost: (command: CreatePostCommand) =>
    api.post<CreatePostResponse>(`${postUri}/create`, command),

  deletePost: (postId: UUID) =>
    api.delete(`${postUri}/delete/${postId}`),

  getFeed: (pageNumber: number) =>
    api.get<PagedFeed<FeedResponseServer>>(`${postUri}/get-feed?pageNumber=${pageNumber}`),

  getPostById: (postId: UUID, pageNumber: number) =>
    api.get<PostDetails>(`${postUri}/${postId}?pageNumber=${pageNumber}`),

  getProfilePosts: (userId: UUID, pageNumber: number) =>
    api.get<ProfileFeedResponseServer>(
      `${postUri}/get-posts-by-user/${userId}?pageNumber=${pageNumber}`
    ),

  getExploredPosts: () =>
    api.get<ProfileFeedResponseServer>(`${postUri}/get-explored-posts`),
};

export default postService;
