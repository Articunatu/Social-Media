import Image from "next/image";
import Link from "next/link";
import type { UseQueryResult } from "@tanstack/react-query";
import type { UUID } from "crypto";
import type { CommentDetails } from "../../../models/api/comment-models";
import { useAuth } from "../../../authentication/auth-provider";
import { useDeleteComment } from "../hooks/use-delete-comment";
import { getPhotoDataUrl } from "../models/photo-data-url";

type PostCommentsProps = {
    commentsQuery: UseQueryResult<CommentDetails[], Error>;
};

type CommentRowProps = {
    comment: CommentDetails;
    isDeleting: boolean;
    onDelete: (comment: CommentDetails) => void;
};

function CommentRow({ comment, isDeleting, onDelete }: CommentRowProps) {
    const auth = useAuth();
    const canDelete = auth?.user?.userId === (comment.authorId as UUID);
    const profilePhotoSrc = getPhotoDataUrl(comment.author.profilePhoto);

    return (
        <div className="bg-base-100 border border-black rounded px-2 py-2">
            <div className="flex items-start gap-2">
                <Link className="shrink-0" href={`/profile/${comment.author.id}`}>
                    <div className="h-8 w-8 overflow-hidden rounded-full border border-black bg-base-200">
                        {profilePhotoSrc && (
                            <Image
                                alt={`${comment.author.fullName}'s profile`}
                                className="h-full w-full object-cover"
                                height={32}
                                src={profilePhotoSrc}
                                unoptimized
                                width={32}
                            />
                        )}
                    </div>
                </Link>

                <div className="min-w-0 flex-1">
                    <div className="flex items-start justify-between gap-2">
                        <div className="min-w-0">
                            <Link className="font-semibold text-black hover:underline" href={`/profile/${comment.author.id}`}>
                                {comment.author.fullName}
                            </Link>
                            <Link className="ml-2 text-xs text-gray-600 hover:underline" href={`/profile/${comment.author.id}`}>
                                @{comment.author.tag}
                            </Link>
                        </div>
                        {canDelete && (
                            <button
                                className="btn btn-xs btn-error"
                                disabled={isDeleting}
                                onClick={() => onDelete(comment)}
                                type="button"
                            >
                                Delete
                            </button>
                        )}
                    </div>
                    <p className="mt-1 text-sm text-black">{comment.content}</p>
                    <p className="mt-1 text-xs text-gray-500">{new Date(comment.timeStamp).toLocaleString()}</p>

                    {comment.replies.length > 0 && (
                        <div className="mt-2 space-y-2 border-l-2 border-black/20 pl-3">
                            {comment.replies.map((reply) => (
                                <CommentRow
                                    comment={reply}
                                    isDeleting={isDeleting}
                                    key={reply.postId}
                                    onDelete={onDelete}
                                />
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}

export const PostComments: React.FC<PostCommentsProps> = ({ commentsQuery }) => {
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
            {commentsQuery.data?.map((comment) => (
                <CommentRow
                    comment={comment}
                    isDeleting={deleteComment.isPending}
                    key={comment.postId}
                    onDelete={handleDeleteComment}
                />
            ))}
        </div>
    );
};
