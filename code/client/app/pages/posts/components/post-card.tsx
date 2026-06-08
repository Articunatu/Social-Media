"use client";

import Image from "next/image";
import { PostCommentForm } from "./post-comment-form";
import { PostComments } from "./post-comments";
import { PostReactionButtons } from "./post-reaction-buttons";
import { usePostCard } from "../hooks/use-post-card";
import type { PostCardProps } from "../models/post-card-props";

const PostCard: React.FC<PostCardProps> = ({ post }) => {
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
        toggleComments,
        totalReactions,
    } = usePostCard(post);

    const { profile, content, createdAt, commentsCount } = post;

    return (
        <li className="list-none w-full max-w-xl px-4 py-3">
            <article className="card bordered bg-base-200 border-4 border-black pokeshadow pokeshadow-hover transition-all duration-150">
                <div className="card-body p-4 flex-row gap-4 items-start">
                    <div className="avatar">
                        <div className="w-12 h-12 rounded-full border-2 border-black pokeshadow">
                            {profile.profilePhoto && (
                                <Image
                                    src={profile.profilePhoto}
                                    alt={`${profile.fullName}'s profile`}
                                    width={48}
                                    height={48}
                                />
                            )}
                        </div>
                    </div>

                    <div className="flex-1">
                        <div className="flex items-center gap-2">
                            <span className="font-bold text-black">{profile.fullName}</span>
                            <span className="text-sm text-gray-700">@{profile.tag}</span>
                            <span className="ml-2 text-xs text-gray-500">{new Date(createdAt).toLocaleString()}</span>
                        </div>
                        <p className="mt-1 text-black">{content}</p>

                        <div className="mt-3 flex items-center gap-3">
                            <span className="text-sm text-gray-700">{totalReactions}</span>
                            <button
                                className="btn btn-sm bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none"
                                onClick={toggleComments}
                                type="button"
                            >
                                <span className="material-symbols-outlined text-[1.2em]">comment</span>
                            </button>
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
        </li>
    );
};

export default PostCard;
