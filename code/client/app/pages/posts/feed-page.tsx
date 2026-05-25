import React, { useState } from 'react';
import PostCard from '../../components/post-card';
import NewPostForm from '../../components/new-post-form';
import { useAuth } from '../../components/auth-provider';
import { useFeed } from '../../hooks/use-feed';
import { useExploredPosts } from '../../hooks/use-explored-posts';
import { FeedPost } from '@/app/models/api/post-models';

const FeedPage: React.FC = () => {
    const { data: posts, isLoading, isError } = useFeed();
    const auth = useAuth();
    const [showExplored, setShowExplored] = useState(false);
    const { data: exploredPosts, isLoading: isLoadingExplored, isError: isErrorExplored } = useExploredPosts(showExplored);

    const handleLoadExplored = () => setShowExplored(true);

    return (
        <div className="flex flex-col items-center gap-4 p-4">
            {isLoading && <div>Loading...</div>}

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
                    className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
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
