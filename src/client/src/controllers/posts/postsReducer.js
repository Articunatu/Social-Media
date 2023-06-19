// postsReducer.js
import { CREATE_POST, LIKE_POST, FETCH_POSTS_SUCCESS } from "./actionTypes";
  
  const initialState = {
    posts: [],
  };
  
  const postsReducer = (state = initialState, action) => {
    switch (action.type) {
      case CREATE_POST:
        return {
          ...state,
          posts: [action.payload, ...state.posts],
        };
      case LIKE_POST:
        const likedPostId = action.payload;
        return {
          ...state,
          posts: state.posts.map((post) => {
            if (post.id === likedPostId) {
              return { ...post, likes: post.likes + 1 };
            }
            return post;
          }),
        };
      case FETCH_POSTS_SUCCESS:
        return {
          ...state,
          posts: action.payload,
        };
      default:
        return state;
    }
  };
  
  export default postsReducer;
  