import { ReactionCount } from "./reaction-models";
import { ProfileInfo } from "./user-models";

export interface CreatePostCommand {
    content: string;
    mediaUrls?: string[];
    tags?: string[];
}

export interface FeedPost {
    profile: ProfileInfo;
    content: string;
    authorId: string;
    createdAt: string;
}

export interface ProfilePost {
    content: string;
    timeStamp: string;
    commentsCount: number;
    reactionCounts: ReactionCount[];
}

export interface PostDetails {
    post: ProfilePost;
    profile: ProfileInfo;
    comments: ProfilePost[];
}

export interface DeletePostCommand {
    id: string;
}
