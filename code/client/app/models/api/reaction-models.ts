import { UUID } from "crypto";
import { ProfileInfo } from "./user-models";
import { ProfilePost } from "./post-models";

export interface ReactionCount {
    type: ReactionType;
    amount: number;
}

export interface ReactionResponse {
    id: UUID,
    type: ReactionType,
    profile: ProfileInfo
};

export interface ReactedProfilePost extends ProfilePost {
    type: ReactionType;
}

export interface AddReactionCommand {
    type: ReactionType;
    userId: UUID;
    messageId: UUID;
}

export interface UpdateReactionCommand {
    id: UUID;
    type: ReactionType;
}

export enum ReactionType {
    Flower = 0,
    Fire = 1,
    Raindrop = 2,
    Lightning = 3,
    Chemical = 4,
    Space = 5,
    Ray = 6
}