
export interface ProfileInfo {
    id: string;
    tag: string;
    displayName: string;
    profilePhoto: string;
}

export interface Post {
    id: string;
    text: string;
    datePosted: Date;
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
    id: string,
    icon: string
}