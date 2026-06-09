"use client";

import Image from 'next/image';
import Link from 'next/link';
import React, { useState } from 'react';
import type { UUID } from 'crypto';
import type { ReactionCount, ReactionType } from '../../../models/api/reaction-models';
import { useReactionsByPost } from '../hooks/use-reactions-by-post';
import { getPhotoDataUrl } from '../models/photo-data-url';
import { reactionOptions } from '../models/reaction-options';

interface ReactionDetailModalProps {
    isOpen: boolean;
    onClose: () => void;
    postId: UUID;
    reactionCounts: ReactionCount[];
}

function getReactionLabel(type: ReactionType) {
    return reactionOptions.find((option) => option.type === type)?.label ?? 'Reaction';
}

export function ReactionDetailModal({
    isOpen,
    onClose,
    postId,
    reactionCounts,
}: ReactionDetailModalProps) {
    const [selectedType, setSelectedType] = useState<ReactionType | undefined>();
    const reactionsQuery = useReactionsByPost(postId, 0, selectedType, isOpen);
    const totalReactions = reactionCounts.reduce((sum, reactionCount) => sum + reactionCount.amount, 0);

    if (!isOpen) {
        return null;
    }

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
            <section className="w-full max-w-md rounded-lg border-2 border-black bg-base-100 p-4 shadow-xl">
                <div className="flex items-center justify-between gap-3">
                    <h2 className="text-lg font-semibold text-black">Reactions</h2>
                    <button className="btn btn-sm btn-ghost" onClick={onClose} type="button">
                        Close
                    </button>
                </div>

                <div className="mt-3 flex flex-wrap gap-2">
                    <button
                        className={`btn btn-xs border-2 border-black ${selectedType === undefined ? 'btn-primary' : 'bg-base-100'}`}
                        onClick={() => setSelectedType(undefined)}
                        type="button"
                    >
                        All {totalReactions}
                    </button>
                    {reactionCounts.map((reactionCount) => (
                        <button
                            className={`btn btn-xs border-2 border-black ${selectedType === reactionCount.type ? 'btn-primary' : 'bg-base-100'}`}
                            key={reactionCount.type}
                            onClick={() => setSelectedType(reactionCount.type)}
                            type="button"
                        >
                            {getReactionLabel(reactionCount.type)} {reactionCount.amount}
                        </button>
                    ))}
                </div>

                <div className="mt-4 max-h-80 overflow-y-auto">
                    {reactionsQuery.isLoading && <p className="text-sm text-gray-600">Loading reactions...</p>}
                    {reactionsQuery.isError && <p className="text-sm text-error">Failed to load reactions.</p>}
                    {reactionsQuery.data?.length === 0 && <p className="text-sm text-gray-600">No reactions found.</p>}

                    {reactionsQuery.data?.map((reaction) => {
                        const profilePhotoSrc = getPhotoDataUrl(reaction.profile.profilePhoto);

                        return (
                            <Link
                                className="flex items-center gap-3 border-b border-black/10 py-2 last:border-b-0 hover:bg-base-200"
                                href={`/profile/${reaction.profile.id}`}
                                key={reaction.id}
                            >
                                <div className="h-10 w-10 overflow-hidden rounded-full border-2 border-black bg-base-200">
                                    {profilePhotoSrc && (
                                        <Image
                                            alt={`${reaction.profile.fullName}'s profile`}
                                            className="h-full w-full object-cover"
                                            height={40}
                                            src={profilePhotoSrc}
                                            unoptimized
                                            width={40}
                                        />
                                    )}
                                </div>
                                <div className="min-w-0 flex-1">
                                    <p className="truncate font-semibold text-black">{reaction.profile.fullName}</p>
                                    <p className="truncate text-sm text-gray-700">@{reaction.profile.tag}</p>
                                </div>
                                <span className="text-sm text-gray-700">{getReactionLabel(reaction.type)}</span>
                            </Link>
                        );
                    })}
                </div>
            </section>
        </div>
    );
}
