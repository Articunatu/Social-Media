import React from "react";

type PostCommentFormProps = {
    content: string;
    error: string | null;
    isPending: boolean;
    onChange: (value: string) => void;
    onSubmit: (event: React.FormEvent) => void;
};

export const PostCommentForm: React.FC<PostCommentFormProps> = ({
    content,
    error,
    isPending,
    onChange,
    onSubmit,
}) => (
    <form onSubmit={onSubmit} className="mt-3">
        <div className="flex gap-2">
            <input
                className="input input-bordered w-full"
                placeholder="Write a comment"
                value={content}
                onChange={(event) => onChange(event.target.value)}
            />
            <button type="submit" className="btn btn-sm bg-base-100 border-2 border-black pokeshadow" disabled={isPending}>
                {isPending ? "Posting..." : "Send"}
            </button>
        </div>
        {error && <p className="mt-1 text-sm text-error">{error}</p>}
    </form>
);
