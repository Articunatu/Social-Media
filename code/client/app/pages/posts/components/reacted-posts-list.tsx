"use client";

import type { UUID } from 'crypto';
import { useReactedPostsByUser } from '../hooks/use-reacted-posts-by-user';
import { reactionOptions } from '../models/reaction-options';

interface ReactedPostsListProps {
    enabled: boolean;
    userId: UUID;
}

function getReactionLabel(type: number) {
    return reactionOptions.find((option) => option.type === type)?.label ?? 'Reaction';
}

export function ReactedPostsList({ enabled, userId }: ReactedPostsListProps) {
    const reactedPostsQuery = useReactedPostsByUser(userId, 0, enabled);

    if (!enabled) {
        return null;
    }

    return (
        <div className="flex w-full max-w-xl flex-col gap-3">
            {reactedPostsQuery.isLoading && <div>Loading reacted posts...</div>}
            {reactedPostsQuery.isError && <div>Failed to load reacted posts.</div>}
            {reactedPostsQuery.data?.length === 0 && <div>This user has not reacted to any posts yet.</div>}

            {reactedPostsQuery.data?.map((post) => (
                <article
                    className="rounded-lg border-2 border-black bg-base-200 p-4 pokeshadow"
                    key={post.reactionId}
                >
                    <div className="flex flex-wrap items-center justify-between gap-2">
                        <span className="badge badge-primary">{getReactionLabel(post.type)}</span>
                        <span className="text-xs text-gray-600">{new Date(post.timeStamp).toLocaleString()}</span>
                    </div>
                    <p className="mt-2 text-black">{post.content}</p>
                    <div className="mt-3 flex gap-3 text-sm text-gray-700">
                        <span>{post.commentsCount} comments</span>
                        <span>
                            {post.reactionCounts.reduce((sum, reactionCount) => sum + reactionCount.amount, 0)} reactions
                        </span>
                    </div>
                </article>
            ))}
        </div>
    );
}
