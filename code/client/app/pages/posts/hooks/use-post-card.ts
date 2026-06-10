import React, { useState } from "react";
import { useAuth } from "../../../authentication/auth-provider";
import type { FeedPost } from "../../../models/api/post-models";
import type { ReactionType } from "../../../models/api/reaction-models";
import { useCommentsByPost } from "./use-comments-by-post";
import { useCreateComment } from "./use-create-comment";
import { useMyReaction } from "./use-my-reaction";
import { useReactToPost } from "./use-react-to-post";

export function usePostCard(post: FeedPost, commentsInitiallyOpen: boolean = false) {
    const { postId, reactionCounts } = post;
    const [showComments, setShowComments] = useState(commentsInitiallyOpen);
    const [commentContent, setCommentContent] = useState("");
    const [commentError, setCommentError] = useState<string | null>(null);
    const [reactionError, setReactionError] = useState<string | null>(null);

    const auth = useAuth();
    const commentsQuery = useCommentsByPost(postId, 0, showComments);
    const createComment = useCreateComment();
    const myReactionQuery = useMyReaction(postId, !!auth?.user);
    const reactToPost = useReactToPost(postId);
    const totalReactions = reactionCounts.reduce((sum, rc) => sum + rc.amount, 0);

    const handleCreateComment = async (event: React.FormEvent) => {
        event.preventDefault();
        setCommentError(null);

        const trimmed = commentContent.trim();
        if (!trimmed) {
            setCommentError("Comment cannot be empty.");
            return;
        }

        try {
            await createComment.mutateAsync({ content: trimmed, parentPostId: postId });
            setCommentContent("");
            setShowComments(true);
        } catch (error: unknown) {
            setCommentError(error instanceof Error ? error.message : "Failed to create comment");
        }
    };

    const handleReact = async (type: ReactionType) => {
        setReactionError(null);
        if (!auth?.user) {
            setReactionError("You must be logged in to react.");
            return;
        }

        try {
            await reactToPost.mutateAsync({ type, currentReaction: myReactionQuery.data ?? null });
        } catch (error: unknown) {
            setReactionError(error instanceof Error ? error.message : "Failed to update reaction");
        }
    };

    return {
        commentContent,
        commentError,
        commentsQuery,
        createComment,
        handleCommentContentChange: setCommentContent,
        handleCreateComment,
        handleReact,
        myReactionQuery,
        reactToPost,
        reactionError,
        showComments,
        toggleComments: () => setShowComments((value) => !value),
        totalReactions,
    };
}
