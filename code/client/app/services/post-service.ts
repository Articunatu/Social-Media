import api from './api';
import {
  CreatePostCommand,
  FeedPost,
  FullPostDetails,
  PostSummary,
} from '../models/api/post-models';
import { UUID } from 'crypto';

const postUri = '/posts';

const postService = {
  createPost: (command: CreatePostCommand) =>
    api.post<FullPostDetails>(`${postUri}/create`, command),

  deletePost: (postId: UUID) =>
    api.delete(`${postUri}/delete/${postId}`),

  getFeed: (pageNumber: number) =>
    api.get<FeedPost[]>(`${postUri}/get-feed?pageNumber=${pageNumber}`),

  getPostById: (postId: UUID, pageNumber: number) =>
    api.get<FullPostDetails>(`${postUri}/${postId}?pageNumber=${pageNumber}`),

  getProfilePosts: (userId: UUID, pageNumber: number) =>
    api.get<PostSummary[]>(`${postUri}/get-posts-by-user/${userId}?pageNumber=${pageNumber}`),
};

export default postService;
