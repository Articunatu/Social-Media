import api from './api';
import {
    CreateCommentCommand,
    CommentDetails,
} from '../models/api/comment-models';
import { UUID } from 'crypto';

const commentUri = '/users';

const commentService = {
    createComment: (command: CreateCommentCommand) =>
        api.post(`${commentUri}/create`, command),

    deleteComment: (id: UUID) =>
        api.post(`${commentUri}/delete/${id}`),

    getComments: (postId: UUID) =>
        api.get<CommentDetails[]>(`${commentUri}/get-by-post-id/${postId}`),

    getCommentById: (id : UUID) =>
        api.get<CommentDetails>(`${commentUri}/get-by-id/${id}`)
};

export default commentService;
