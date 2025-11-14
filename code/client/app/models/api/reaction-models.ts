import { UUID } from "crypto";
import { ProfileInfo } from "./user-models";

export interface ReactionCount {
    type: ReactionType;
    amount: number;
}

export interface ReactionResponse {
    id: UUID,
    type: ReactionType,
    profile: ProfileInfo
};

export enum ReactionType {
    Flower = 0,
    Fire = 1,
    Raindrop = 2,
    Lightning = 3,
    Chemical = 4,
    Space = 5,
    Ray = 6
}