import type { UseQueryResult } from "@tanstack/react-query";
import type { UUID } from "crypto";
import type { CommentDetails } from "../../../models/api/comment-models";
import { useAuth } from "../../../authentication/auth-provider";
import { useDeleteComment } from "../hooks/use-delete-comment";

type PostCommentsProps = {
    commentsQuery: UseQueryResult<CommentDetails[], Error>;
};

export const PostComments: React.FC<PostCommentsProps> = ({ commentsQuery }) => {
    const auth = useAuth();
    const deleteComment = useDeleteComment();

    const handleDeleteComment = async (comment: CommentDetails) => {
        await deleteComment.mutateAsync({
            commentId: comment.postId,
            parentPostId: comment.parentPostId,
        });
    };

    return (
        <div className="mt-3 space-y-2">
            {commentsQuery.isLoading && <div className="text-sm text-gray-700">Loading comments...</div>}
            {commentsQuery.isError && <div className="text-sm text-error">Failed to load comments.</div>}
            {!commentsQuery.isLoading && commentsQuery.data?.length === 0 && (
                <div className="text-sm text-gray-700">No comments yet.</div>
            )}
            {commentsQuery.data?.map((comment) => {
                const canDelete = auth?.user?.userId === (comment.authorId as UUID);

                return (
                    <div key={comment.postId} className="bg-base-100 border border-black rounded px-2 py-1">
                        <div className="flex items-start justify-between gap-2">
                            <p className="text-sm text-black">{comment.content}</p>
                            {canDelete && (
                                <button
                                    className="btn btn-xs btn-error"
                                    disabled={deleteComment.isPending}
                                    onClick={() => handleDeleteComment(comment)}
                                    type="button"
                                >
                                    Delete
                                </button>
                            )}
                        </div>
                        <p className="mt-1 text-xs text-gray-500">{new Date(comment.timeStamp).toLocaleString()}</p>
                    </div>
                );
            })}
        </div>
    );
};
