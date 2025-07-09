
export interface CreatePostCommand {
    content: string;
    mediaUrls?: string[];
    tags?: string[];
}

export interface FeedPost {
    id: string;
    content: string;
    authorId: string;
    createdAt: string;
}

export interface PostSummary {
    id: string;
    content: string;
    createdAt: string;
}

export interface FullPostDetails extends PostSummary {
    comments: CommentModel[];
    likesCount: number;
}

export interface DeletePostCommand {
    id: string;
}
