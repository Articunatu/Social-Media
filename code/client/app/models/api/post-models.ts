import { UUID } from "crypto";
import { ReactionCount } from "./reaction-models";
import { ProfileInfo } from "./user-models";

export interface CreatePostCommand {
    content: string;
}

export interface CreatePostResponse {
    id: UUID;
    content: string;
    timeStamp: string;
    userId: UUID;
}

export interface FeedPost {
    postId: UUID;
    profile: ProfileInfo;
    content: string;
    authorId: UUID;
    createdAt: string;
    commentsCount: number;
    reactionCounts: ReactionCount[];
}

export interface ProfilePost {
    content: string;
    timeStamp: string;
    commentsCount: number;
    reactionCounts: ReactionCount[];
}

export interface PostDetails {
    profileMain: ProfileInfo;
    post: ProfilePostDto;
    comments: PagedFeed<ProfilePostDto>;
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

export interface FeedResponseServer {
    profile: ProfileInfo;
    post: ProfilePostDto;
}

