import type { UseQueryResult } from "@tanstack/react-query";
import type { CommentDetails } from "../../../models/api/comment-models";

type PostCommentsProps = {
    commentsQuery: UseQueryResult<CommentDetails[], Error>;
};

export const PostComments: React.FC<PostCommentsProps> = ({ commentsQuery }) => (
    <div className="mt-3 space-y-2">
        {commentsQuery.isLoading && <div className="text-sm text-gray-700">Loading comments...</div>}
        {commentsQuery.isError && <div className="text-sm text-error">Failed to load comments.</div>}
        {!commentsQuery.isLoading && commentsQuery.data?.length === 0 && (
            <div className="text-sm text-gray-700">No comments yet.</div>
        )}
        {commentsQuery.data?.map((comment) => (
            <div key={comment.postId} className="bg-base-100 border border-black rounded px-2 py-1">
                <p className="text-sm text-black">{comment.content}</p>
                <p className="mt-1 text-xs text-gray-500">{new Date(comment.timeStamp).toLocaleString()}</p>
            </div>
        ))}
    </div>
);
