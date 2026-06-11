"use client";

import { PostCommentForm } from "./post-comment-form";
import { PostComments } from "./post-comments";
import { PostReactionButtons } from "./post-reaction-buttons";
import { usePostCard } from "../hooks/use-post-card";
import type { PostCardProps } from "../models/post-card-props";
import { ReactionDetailModal } from "./reaction-detail-modal";
import React from "react";
import { useAuth } from "../../../authentication/auth-provider";
import { useDeletePost } from "../hooks/use-delete-post";
import { PostActions } from "./post-actions";
import { PostAuthorLink } from "./post-author-link";
import { PostDeleteButton } from "./post-delete-button";

const PostCard: React.FC<PostCardProps> = ({ post, commentsInitiallyOpen = false, onDeleted }) => {
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
    const canDelete = auth?.user?.userId === post.authorId;

    const handleDeletePost = async () => {
        await deletePost.mutateAsync(post.postId);
        onDeleted?.();
    };

    return (
        <li className="list-none w-full max-w-xl px-4 py-3">
            <article className="card bordered bg-base-200 border-4 border-black pokeshadow pokeshadow-hover transition-all duration-150">
                <PostAuthorLink
                    action={
                        canDelete ? (
                            <PostDeleteButton
                                disabled={deletePost.isPending}
                                onDelete={handleDeletePost}
                            />
                        ) : undefined
                    }
                    createdAt={createdAt}
                    profile={profile}
                >
                    <p className="mt-1 text-black">{content}</p>

                    <PostActions
                        commentsCount={commentsCount}
                        onShowReactionDetails={() => setShowReactionDetails(true)}
                        postId={post.postId}
                        totalReactions={totalReactions}
                    />

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
                </PostAuthorLink>
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
