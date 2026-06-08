"use client";
import React, { useState } from 'react';
import { useAuth } from '../../../authentication/auth-provider';
import { useCreatePost } from '../hooks/use-create-post';
import type { CreatePostCommand } from '../../../models/api/post-models';

const NewPostForm: React.FC = () => {
    const auth = useAuth();
    const mutation = useCreatePost();
    const { mutateAsync } = mutation;
    const isLoading = mutation.status === 'pending';
    const [content, setContent] = useState('');
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        if (!auth || !auth.user) {
        setError('You must be logged in to post.');
        return;
        }

        const cmd: CreatePostCommand = {
        content,
        };

        try {
        await mutateAsync(cmd);
        setContent('');
        } catch (err: unknown) {
        setError((err as Error)?.message ?? 'Failed to create post');
        }
    };

    return (
        <form onSubmit={handleSubmit} className="w-full max-w-xl">
        <div className="mb-2">
            <textarea
            className="textarea textarea-bordered w-full"
            placeholder="What's on your mind?"
            value={content}
            onChange={(e) => setContent(e.target.value)}
            rows={4}
            required
            />
        </div>
        {error && <div className="mb-2 text-error">{error}</div>}
        <div className="flex justify-end">
            <button
            type="submit"
            className="btn btn-success"
            disabled={isLoading}
            >
            {isLoading ? 'Posting...' : 'Post'}
            </button>
        </div>
        </form>
    );
};

export default NewPostForm;
