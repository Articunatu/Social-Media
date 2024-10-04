import React, { useState, useEffect } from "react";
import { Post } from "../../Routes/Models"; // Assuming Post interface is correctly defined
import SmText from "../Shared/SmText";
import API from "../../Routes/Api";

const Feed: React.FC = () => {
    // Initialize posts state as an empty array
    const [posts, setPosts] = useState<Post[]>([]);

    // Example: Fetch posts (simulated with useEffect)
    useEffect(() => {
        const fetchedPosts: Post[] = API.feed.get10posts();
        setPosts(fetchedPosts);
    }, []); // Empty dependency array means this runs once when the component mounts

    return (
        <div className="feed-container mx-auto p-4">
            <h1 className="text-2xl font-bold mb-4">Feed</h1>

            {/* Render each post */}
            {posts.length > 0 ? (
                posts.map((post) => (
                    <div key={post.id} className="post mb-6 p-4 border rounded-lg shadow-md">
                        <h2 className="text-xl font-semibold mb-2">{post.title}</h2>
                        <SmText>{post.content}</SmText>
                    </div>
                ))
            ) : (
                <p>No posts available</p> // Display if no posts are present
            )}
        </div>
    );
};

export default Feed;
