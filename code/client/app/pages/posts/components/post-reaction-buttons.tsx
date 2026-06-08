import { reactionOptions } from "../models/reaction-options";
import type { ReactionResponse, ReactionType } from "../../../models/api/reaction-models";

type PostReactionButtonsProps = {
    currentReaction: ReactionResponse | null;
    disabled: boolean;
    onReact: (type: ReactionType) => void;
};

export const PostReactionButtons: React.FC<PostReactionButtonsProps> = ({
    currentReaction,
    disabled,
    onReact,
}) => (
    <div className="mt-2 flex flex-wrap gap-2">
        {reactionOptions.map((option) => {
            const isActive = currentReaction?.type === option.type;

            return (
                <button
                    key={option.type}
                    type="button"
                    onClick={() => onReact(option.type)}
                    disabled={disabled}
                    title={option.label}
                    className={`btn btn-xs border-2 border-black pokeshadow ${isActive ? "bg-yellow-200" : "bg-base-100"}`}
                >
                    <span>{option.icon}</span>
                    <span>{option.label}</span>
                </button>
            );
        })}
    </div>
);
