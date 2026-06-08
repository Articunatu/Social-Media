import type { ReactionType } from "../../../models/api/reaction-models";

export type ReactionOption = {
    type: ReactionType;
    label: string;
    icon: string;
};
