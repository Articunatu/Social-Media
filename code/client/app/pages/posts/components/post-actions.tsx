"use client";

import Link from "next/link";
import type { UUID } from "crypto";

interface PostActionsProps {
    commentsCount: number;
    onShowReactionDetails: () => void;
    postId: UUID;
    totalReactions: number;
}

export function PostActions({
    commentsCount,
    onShowReactionDetails,
    postId,
    totalReactions,
}: PostActionsProps) {
    return (
        <div className="mt-3 flex items-center gap-3">
            <span className="text-sm text-gray-700">{totalReactions}</span>
            <button
                className="btn btn-xs bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none"
                disabled={totalReactions === 0}
                onClick={onShowReactionDetails}
                type="button"
            >
                Details
            </button>
            <Link
                className="btn btn-sm bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none"
                href={`/posts/${postId}`}
            >
                <span className="material-symbols-outlined text-[1.2em]">comment</span>
            </Link>
            <span className="text-sm text-gray-700">{commentsCount}</span>
        </div>
    );
}
