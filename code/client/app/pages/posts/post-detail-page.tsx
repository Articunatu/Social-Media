"use client";

import Link from 'next/link';
import { useRouter } from 'next/navigation';
import type { UUID } from 'crypto';
import PostCard from './components/post-card';
import { usePostDetails } from './hooks/use-post-details';

interface PostDetailPageProps {
    postId: UUID;
}

export function PostDetailPage({ postId }: PostDetailPageProps) {
    const router = useRouter();
    const { data: post, isError, isLoading } = usePostDetails(postId);

    return (
        <main className="min-h-screen bg-base-200 px-4 py-6">
            <div className="mx-auto flex w-full max-w-2xl flex-col gap-4">
                <Link className="btn btn-sm w-fit" href="/">
                    Back
                </Link>

                {isLoading && <div>Loading post...</div>}
                {isError && <div className="text-error">Failed to load post.</div>}

                {post && (
                    <div className="flex justify-center">
                        <PostCard
                            commentsInitiallyOpen
                            onDeleted={() => router.push('/')}
                            post={post}
                        />
                    </div>
                )}
            </div>
        </main>
    );
}
