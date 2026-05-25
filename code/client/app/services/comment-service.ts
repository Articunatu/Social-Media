import { CommentDetails, CreateCommentCommand, CreatedCommentResponse } from '../models/api/comment-models';
import { PagedFeed, PageFilter } from '../models/paging-models';
import api from './api';
import { UUID } from 'crypto';

const commentsUri = '/comments';

export const commentsService = {
    create: (cmd: CreateCommentCommand) =>
        api.post<CreatedCommentResponse>(`${commentsUri}/create`, cmd),

    delete: (id: UUID) =>
        api.delete(`${commentsUri}/delete/${id}`),

    getByPost: (postId: UUID, filter: PageFilter) =>
        api.get<PagedFeed<CommentDetails>>(
        `${commentsUri}/get-by-post-id/${postId}?index=${filter.index}`
        + (filter.order ? `&order=${encodeURIComponent(filter.order)}` : '')
        + (filter.searchText ? `&searchText=${encodeURIComponent(filter.searchText)}` : '')
    ),

    getById: (id: UUID) =>
        api.get<CommentDetails>(`${commentsUri}/${id}`)
};
