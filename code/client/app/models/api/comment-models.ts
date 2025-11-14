import { UUID } from "crypto";

export interface CommentDetails {
    id: UUID;
    content: string;
    userId: UUID;
    timestamp: Date;
    parentPostId: UUID;
}

export interface CreateCommentCommand {
    content: string;
    authorId: UUID;
    parentPostId: UUID;
    parentCommentId?: UUID;
}