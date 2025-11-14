import api from './api';
import {
    ReactionCount,
    ReactionResponse,
} from '../models/api/reaction-models';
import { UUID } from 'crypto';

const reactionUri = '/users';

const reactionService = {
    getReactionsByUser: (postId: UUID) =>
        api.post(`${reactionUri}/get-by-post/${postId}`, command),

    deleteComment: (id: UUID) =>
        api.post(`${reactionUri}/delete/${id}`),

    getComments: (postId: UUID) =>
        api.get<CommentDetails[]>(`${reactionUri}/get-by-post-id/${postId}`),

    getCommentById: (id : UUID) =>
        api.get<CommentDetails>(`${reactionUri}/get-by-id/${id}`)
};

export default reactionService;