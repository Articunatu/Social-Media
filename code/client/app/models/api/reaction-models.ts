export interface ReactionCount {
    type: ReactionType;
    amount: number;
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