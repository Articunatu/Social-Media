import { UUID } from "crypto";

export interface ProfileInfo {
    id: UUID;
    tag: string;
    displayName: string;
    profilePhoto: string;
}

export interface Post {
    id: UUID;
    text: string;
    timestamp: Date;
    profileInfo : ProfileInfo,
    replyAmount: number,
    reactionAmount: number
}

export interface PagedPosts {
    posts: Post[];
    currentPage: number;
    totalPages: number;
    totalPosts: number;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

export interface Reply extends Omit<Post, 'replyAmount'> {
    parentPostId: string
}

export interface Reaction {
    id: UUID,
    icon: string
}