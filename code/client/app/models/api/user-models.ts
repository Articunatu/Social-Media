import { UUID } from "crypto";

export interface DeleteAccountCommand {
    userId: UUID;
}

export interface FollowCommand {
    followerId: UUID;
    followedId: UUID;
}

export interface ProfileDetails {
    profileInfo: ProfileInfo;
    aboutMe: string;
    followingCount: number;
    followersCount: number;
    backgroundPhoto: string;
}

export interface ProfileInfo {
    id: UUID;
    tag: string;
    fullName: string;
    profilePhoto: string;
}

export interface SearchUserQuery {
    searchText: string;
    index: number;
    sortOrder: string;
}

export interface UnfollowCommand {
    followerId: UUID;
    unfollowedId: UUID;
}