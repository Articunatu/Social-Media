import type { UUID } from "crypto";
import type { PhotoResponse } from "./photo-models";
import type { PageFilter } from "../paging-models";

export interface DeleteAccountCommand {
    userId: UUID;
}

export interface FollowCommand {
    followerId: UUID;
    followingId: UUID;
}

export interface ProfileDetails {
    profile: ProfileInfo;
    aboutMe: string;
    followingCount: number;
    followersCount: number;
    isFollowedByCurrentUser: boolean;
    backgroundPhoto: PhotoResponse | null;
}

export interface ProfileInfo {
    id: UUID;
    tag: string;
    fullName: string;
    profilePhoto: PhotoResponse | null;
}

export interface SearchUserQuery {
    filter: PageFilter;
}

export interface UnfollowCommand {
    followerId: UUID;
    followingId: UUID;
}
