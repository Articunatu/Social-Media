import { UUID } from "crypto";
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
    authorId: UUID;
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
    id: UUID;
}

// Server DTOs for paged/profile feed responses
export interface ProfilePostDto {
    postId: UUID;
    content: string;
    timeStamp: string;
    commentsCount: number;
    reactionCounts: { type: number; amount: number }[];
}

export interface PagedFeed<T> {
    values: T[];
    index?: number;
    order?: string;
    searchText?: string;
}

export interface ProfileFeedResponseServer {
    profileFeed: PagedFeed<ProfilePostDto>;
}

export interface ServerResult<T> {
    value: T;
    isSuccess: boolean;
    isFailure: boolean;
    error: { header: string; message: string } | null;
    status: number;
}
