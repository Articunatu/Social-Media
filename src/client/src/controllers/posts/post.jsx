import React, { useEffect } from "react";
import { connect } from "react-redux";


const PostList = ({
  posts,
  loading,
  error,
  createPost,
  likePost,
  fetchPostsRequest,
}) => {
  useEffect(() => {
    fetchPostsRequest();
  }, [fetchPostsRequest]);

  const handleCreatePost = () => {
    const newPost = {
      id: Date.now(),
      content: "New post",
      likes: 0,
    };
    createPost(newPost);
  };

  return (
    <div>
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p>Error: {error}</p>
      ) : (
        <div>
          <button onClick={handleCreatePost}>Create Post</button>
          <ul>
            {posts.map((post) => (
              <li key={post.id}>
                <p>{post.content}</p>
                <p>Likes: {post.likes}</p>
                <button onClick={() => likePost(post.id)}>Like</button>
              </li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};

const mapStateToProps = (state) => ({
  posts: state.posts.posts,
  loading: state.fetch.loading,
  error: state.fetch.error,
});

const mapDispatchToProps = {
  createPost,
  likePost,
  fetchPostsRequest,
};

export default connect(mapStateToProps, mapDispatchToProps)(PostList);
