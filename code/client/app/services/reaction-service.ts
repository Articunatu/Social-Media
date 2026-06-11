import api from './api';
import { UUID } from 'crypto';
import { AddReactionCommand, 
    ReactedProfilePost, 
    ReactionResponse, 
    ReactionType, 
    UpdateReactionCommand 
} from '../models/api/reaction-models';
import { PagedFeed } from '../models/paging-models';

const reactionUri = '/reactions';

const reactionService = {
    getMyReaction: (postId: UUID) =>
        api.get<ReactionResponse>(`${reactionUri}/my-reaction/${postId}`),

    getReactionsByPost: (postId: UUID, pageNumber: number, type?: ReactionType) =>
        api.get<PagedFeed<ReactionResponse>>(
        `${reactionUri}/get-by-post/${postId}?pageNumber=${pageNumber}${type !== undefined ? `&type=${type}` : ''}`
    ),

    getReactedPostsByUser: (userId: UUID, pageIndex: number) =>
        api.get<PagedFeed<ReactedProfilePost>>(
        `${reactionUri}/users-reactions/${userId}?index=${pageIndex}&order=`
    ),

    addReaction: (command: AddReactionCommand) =>
        api.post<ReactionResponse>(`${reactionUri}/react-to-post`, command),

    updateReaction: (command: UpdateReactionCommand) =>
        api.patch<ReactionResponse>(`${reactionUri}/update-reaction`, command),

    removeReaction: (id: UUID) =>
        api.delete<ReactionResponse>(`${reactionUri}/delete/${id}`)
};

export default reactionService;
