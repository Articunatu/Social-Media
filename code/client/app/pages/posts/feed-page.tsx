import React, { useState } from 'react';
import Link from 'next/link';
import PostCard from './components/post-card';
import NewPostForm from './components/new-post-form';
import { useAuth } from '../../authentication/auth-provider';
import { LogoutButton } from '../../authentication/logout-button';
import { useFeed } from './hooks/use-feed';
import { useExploredPosts } from './hooks/use-explored-posts';
import type { FeedPost } from '@/app/models/api/post-models';
import { UserSearch } from './components/user-search';

const FeedPage: React.FC = () => {
    const { data: posts, isLoading, isError } = useFeed();
    const auth = useAuth();
    const [showExplored, setShowExplored] = useState(false);
    const { data: exploredPosts, isLoading: isLoadingExplored, isError: isErrorExplored } = useExploredPosts(showExplored);

    const handleLoadExplored = () => setShowExplored(true);

    return (
        <div className="flex flex-col items-center gap-4 p-4">
            {auth?.user && (
                <div className="flex w-full max-w-2xl items-center justify-between gap-4">
                    <div className="min-w-0">
                        <p className="truncate text-sm text-gray-600">Signed in as {auth.user.username}</p>
                    </div>
                    <div className="flex shrink-0 items-center gap-2">
                        <Link className="btn btn-sm btn-ghost" href="/change-password">
                            Password
                        </Link>
                        <LogoutButton />
                    </div>
                </div>
            )}

            {isLoading && <div>Loading...</div>}

            {auth?.user && <UserSearch />}

            {auth && auth.user && (
                <div className="w-full flex justify-center">
                    <NewPostForm />
                </div>
            )}
            {isError && <div>Failed to load feed.</div>}

            {posts && posts.length > 0 && posts.map((post: FeedPost) => (
                <PostCard key={post.postId} post={post} />
            ))}

            {posts && posts.length === 0 && !showExplored && (
                <button
                    className="btn btn-primary"
                    onClick={handleLoadExplored}
                >
                    Load Explored Posts
                </button>
            )}

            {showExplored && (
                <>
                    {isLoadingExplored && <div>Loading explored posts...</div>}
                    {isErrorExplored && <div>Failed to load explored posts.</div>}
                    {!exploredPosts && <div>No explored posts found.</div>}
                    {exploredPosts?.length === 0 && <div>No explored posts found.</div>}
                    {exploredPosts && exploredPosts.length > 0 && exploredPosts.map((post: FeedPost) => (
                        <PostCard key={post.postId} post={post} />
                    ))}
                </>
            )}
        </div>
    );
};

export default FeedPage;
