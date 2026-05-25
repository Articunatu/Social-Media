import { UUID } from "crypto";
import { ReactionCount } from "./reaction-models";

export interface CommentDetails {
    postId: UUID;
    commentsCount: number;
    reactionCounts: ReactionCount[];
    parentPostId: UUID;
    timeStamp: string;
    content: string;
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
