import { UUID } from "crypto";
import { ReactionCount } from "./reaction-models";
import type { ProfileInfo } from "./user-models";

export interface CommentDetails {
    postId: UUID;
    authorId: UUID;
    author: ProfileInfo;
    commentsCount: number;
    reactionCounts: ReactionCount[];
    parentPostId: UUID;
    timeStamp: string;
    content: string;
    replies: CommentDetails[];
}

export interface CreatedCommentResponse {
    id: UUID;
    content: string;
    userId: UUID;
    timeStamp: string;
    parentPostId: UUID;
}

export interface CreateCommentCommand {
    content: string;
    parentPostId: UUID;
    parentCommentId?: UUID;
}
