
import React from 'react';
import PostCard from '../../components/post-card';
import { useFeed } from '../../hooks/use-feed';
import { FeedPost } from '@/app/models/api/post-models';

const FeedPage: React.FC = () => {
    const { data: posts, isLoading, isError } = useFeed(1);

    return (
        <div className="flex flex-col items-center gap-4 p-4">
            {isLoading && <div>Loading...</div>}
            {isError && <div>Failed to load feed.</div>}
            {posts && posts.map((post: FeedPost) => (
                <PostCard key={post.createdAt + post.authorId} post={post} />
            ))}
        </div>
    );
};

export default FeedPage;