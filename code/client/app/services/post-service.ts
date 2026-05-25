import api from './api';
import {
  CreatePostCommand,
  CreatePostResponse,
  FeedPost,
  FeedResponseServer,
  ProfilePost,
  ProfileFeedResponseServer,
  ServerResult,
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
    api.get<PagedFeed<FeedResponseServer> | FeedPost[]>(`${postUri}/get-feed?pageNumber=${pageNumber}`),

  getPostById: (postId: UUID, pageNumber: number) =>
    api.get<ProfilePost>(`${postUri}/${postId}?pageNumber=${pageNumber}`),

  getProfilePosts: (userId: UUID, pageNumber: number) =>
    api.get<ProfilePost[]>(`${postUri}/get-posts-by-user/${userId}?pageNumber=${pageNumber}`),

  getExploredPosts: () =>
    api.get<ServerResult<ProfileFeedResponseServer>>(`${postUri}/get-explored-posts`),
};

export default postService;
