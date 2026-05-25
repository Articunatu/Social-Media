"use client";
import Image from 'next/image';
import { useState } from 'react';
import { FeedPost } from '../models/api/post-models';
import { useCommentsByPost } from '../hooks/use-comments-by-post';
import { useCreateComment } from '../hooks/use-create-comment';

interface PostCardProps {
  post: FeedPost;
}

const PostCard: React.FC<PostCardProps> = ({ post }) => {
  const { profile, content, createdAt, commentsCount, reactionCounts, postId } = post;
  const [showComments, setShowComments] = useState(false);
  const [commentContent, setCommentContent] = useState('');
  const [commentError, setCommentError] = useState<string | null>(null);

  const commentsQuery = useCommentsByPost(postId, 0, showComments);
  const createComment = useCreateComment();
  const totalReactions = reactionCounts.reduce((sum, rc) => sum + rc.amount, 0);

  const handleCreateComment = async (e: React.FormEvent) => {
    e.preventDefault();
    setCommentError(null);

    const trimmed = commentContent.trim();
    if (!trimmed) {
      setCommentError('Comment cannot be empty.');
      return;
    }

    try {
      await createComment.mutateAsync({
        content: trimmed,
        parentPostId: postId,
      });
      setCommentContent('');
      setShowComments(true);
    } catch (err: unknown) {
      setCommentError((err as Error)?.message ?? 'Failed to create comment');
    }
  };

  return (
    <li className="list-none w-full max-w-xl px-4 py-3">
      <div
        className="
          card bordered bg-base-200
          border-4 border-black
          pokeshadow pokeshadow-hover
          transition-all duration-150
          cursor-pointer
        "
      >
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
              <span className="text-xs text-gray-500 ml-2">{new Date(createdAt).toLocaleString()}</span>
            </div>
            <p className="mt-1 text-black">{content}</p>

            <div className="mt-3 flex gap-3 items-center">
              <button className="btn btn-sm bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none">
                <span className="material-symbols-outlined text-[1.2em]">star</span>
              </button>
              <span className="text-sm text-gray-700">{totalReactions}</span>
              <button
                className="btn btn-sm bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none"
                onClick={() => setShowComments((v) => !v)}
                type="button"
              >
                <span className="material-symbols-outlined text-[1.2em]">comment</span>
              </button>
              <span className="text-sm text-gray-700">{commentsCount}</span>
            </div>

            <form onSubmit={handleCreateComment} className="mt-3">
              <div className="flex gap-2">
                <input
                  className="input input-bordered w-full"
                  placeholder="Write a comment"
                  value={commentContent}
                  onChange={(e) => setCommentContent(e.target.value)}
                />
                <button
                  type="submit"
                  className="btn btn-sm bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none"
                  disabled={createComment.isPending}
                >
                  {createComment.isPending ? 'Posting...' : 'Send'}
                </button>
              </div>
              {commentError && <p className="text-red-500 text-sm mt-1">{commentError}</p>}
            </form>

            {showComments && (
              <div className="mt-3 space-y-2">
                {commentsQuery.isLoading && <div className="text-sm text-gray-700">Loading comments...</div>}
                {commentsQuery.isError && <div className="text-sm text-red-500">Failed to load comments.</div>}
                {!commentsQuery.isLoading && commentsQuery.data?.length === 0 && (
                  <div className="text-sm text-gray-700">No comments yet.</div>
                )}
                {commentsQuery.data?.map((comment) => (
                  <div key={comment.postId} className="bg-base-100 border border-black rounded px-2 py-1">
                    <p className="text-sm text-black">{comment.content}</p>
                    <p className="text-xs text-gray-500 mt-1">{new Date(comment.timeStamp).toLocaleString()}</p>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </li>
  );
};

export default PostCard;
