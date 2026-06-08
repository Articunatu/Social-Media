import { ReactionType } from "../../../models/api/reaction-models";
import type { ReactionOption } from "./reaction-option";

export const reactionOptions: ReactionOption[] = [
    { type: ReactionType.Flower, label: "Flower", icon: "🌸" },
    { type: ReactionType.Fire, label: "Fire", icon: "🔥" },
    { type: ReactionType.Raindrop, label: "Raindrop", icon: "💧" },
    { type: ReactionType.Lightning, label: "Lightning", icon: "⚡" },
    { type: ReactionType.Chemical, label: "Chemical", icon: "🧪" },
    { type: ReactionType.Space, label: "Space", icon: "🌌" },
    { type: ReactionType.Ray, label: "Ray", icon: "☀️" },
];
