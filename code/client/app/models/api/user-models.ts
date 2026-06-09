import type { UUID } from "crypto";
import type { PhotoResponse } from "./photo-models";

export interface DeleteAccountCommand {
    userId: UUID;
}

export interface FollowCommand {
    followerId: UUID;
    followedId: UUID;
}

export interface ProfileDetails {
    profile: ProfileInfo;
    aboutMe: string;
    followingCount: number;
    followersCount: number;
    backgroundPhoto: PhotoResponse | null;
}

export interface ProfileInfo {
    id: UUID;
    tag: string;
    fullName: string;
    profilePhoto: PhotoResponse | null;
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
