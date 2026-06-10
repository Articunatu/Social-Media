"use client";

import Image from "next/image";
import Link from "next/link";
import { PostCommentForm } from "./post-comment-form";
import { PostComments } from "./post-comments";
import { PostReactionButtons } from "./post-reaction-buttons";
import { usePostCard } from "../hooks/use-post-card";
import type { PostCardProps } from "../models/post-card-props";
import { getPhotoDataUrl } from "../models/photo-data-url";
import { ReactionDetailModal } from "./reaction-detail-modal";
import React from "react";
import { useAuth } from "../../../authentication/auth-provider";
import { useDeletePost } from "../hooks/use-delete-post";

const PostCard: React.FC<PostCardProps> = ({ post, commentsInitiallyOpen = false }) => {
    const [showReactionDetails, setShowReactionDetails] = React.useState(false);
    const auth = useAuth();
    const deletePost = useDeletePost();
    const {
        commentContent,
        commentError,
        commentsQuery,
        createComment,
        handleCommentContentChange,
        handleCreateComment,
        handleReact,
        myReactionQuery,
        reactToPost,
        reactionError,
        showComments,
        totalReactions,
    } = usePostCard(post, commentsInitiallyOpen);

    const { profile, content, createdAt, commentsCount } = post;
    const profilePhotoSrc = getPhotoDataUrl(profile.profilePhoto);
    const canDelete = auth?.user?.userId === post.authorId;

    const handleDeletePost = async () => {
        await deletePost.mutateAsync(post.postId);
    };

    return (
        <li className="list-none w-full max-w-xl px-4 py-3">
            <article className="card bordered bg-base-200 border-4 border-black pokeshadow pokeshadow-hover transition-all duration-150">
                <div className="card-body p-4 flex-row gap-4 items-start">
                    <Link className="avatar shrink-0" href={`/profile/${profile.id}`}>
                        <div className="w-12 h-12 rounded-full border-2 border-black pokeshadow">
                            {profilePhotoSrc && (
                                <Image
                                    src={profilePhotoSrc}
                                    alt={`${profile.fullName}'s profile`}
                                    width={48}
                                    height={48}
                                    unoptimized
                                />
                            )}
                        </div>
                    </Link>

                    <div className="flex-1">
                        <div className="flex items-center gap-2">
                            <Link className="font-bold text-black hover:underline" href={`/profile/${profile.id}`}>
                                {profile.fullName}
                            </Link>
                            <Link className="text-sm text-gray-700 hover:underline" href={`/profile/${profile.id}`}>
                                @{profile.tag}
                            </Link>
                            <span className="ml-2 text-xs text-gray-500">{new Date(createdAt).toLocaleString()}</span>
                            {canDelete && (
                                <button
                                    className="btn btn-xs btn-error ml-auto"
                                    disabled={deletePost.isPending}
                                    onClick={handleDeletePost}
                                    type="button"
                                >
                                    Delete
                                </button>
                            )}
                        </div>
                        <p className="mt-1 text-black">{content}</p>

                        <div className="mt-3 flex items-center gap-3">
                            <span className="text-sm text-gray-700">{totalReactions}</span>
                            <button
                                className="btn btn-xs bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none"
                                disabled={totalReactions === 0}
                                onClick={() => setShowReactionDetails(true)}
                                type="button"
                            >
                                Details
                            </button>
                            <Link
                                className="btn btn-sm bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none"
                                href={`/posts/${post.postId}`}
                            >
                                <span className="material-symbols-outlined text-[1.2em]">comment</span>
                            </Link>
                            <span className="text-sm text-gray-700">{commentsCount}</span>
                        </div>

                        <PostReactionButtons
                            currentReaction={myReactionQuery.data ?? null}
                            disabled={reactToPost.isPending || myReactionQuery.isLoading}
                            onReact={handleReact}
                        />
                        {reactionError && <p className="mt-1 text-sm text-error">{reactionError}</p>}

                        <PostCommentForm
                            content={commentContent}
                            error={commentError}
                            isPending={createComment.isPending}
                            onChange={handleCommentContentChange}
                            onSubmit={handleCreateComment}
                        />

                        {showComments && <PostComments commentsQuery={commentsQuery} />}
                    </div>
                </div>
            </article>
            <ReactionDetailModal
                isOpen={showReactionDetails}
                onClose={() => setShowReactionDetails(false)}
                postId={post.postId}
                reactionCounts={post.reactionCounts}
            />
        </li>
    );
};

export default PostCard;
