import { CommentDetails, CreateCommentCommand } from '../models/api/comment-models';
import { PagedFeed, PageFilter } from '../models/paging-models';
import api from './api';

const commentsUri = '/comments';

export const commentsService = {
    create: (cmd: CreateCommentCommand) =>
        api.post<CommentDetails>(`${commentsUri}/create`, cmd),

    delete: (id: string) =>
        api.delete(`${commentsUri}/delete/${id}`),

    getByPost: (postId: string, filter: PageFilter) =>
        api.get<PagedFeed<CommentDetails>>(
        `${commentsUri}/get-by-post-id/${postId}?index=${filter.index}`
        + (filter.order ? `&order=${encodeURIComponent(filter.order)}` : '')
        + (filter.searchText ? `&searchText=${encodeURIComponent(filter.searchText)}` : '')
    ),

    getById: (id: string) =>
        api.get<CommentDetails>(`${commentsUri}/${id}`)
};
