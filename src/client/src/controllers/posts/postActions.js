// actionTypes.js
export const CREATE_POST = "CREATE_POST";
export const LIKE_POST = "LIKE_POST";
export const FETCH_POSTS_REQUEST = "FETCH_POSTS_REQUEST";
export const FETCH_POSTS_SUCCESS = "FETCH_POSTS_SUCCESS";
export const FETCH_POSTS_FAILURE = "FETCH_POSTS_FAILURE";

// actions.js
export const createPost = (post) => ({
  type: CREATE_POST,
  payload: post,
});

export const likePost = (postId) => ({
  type: LIKE_POST,
  payload: postId,
});

export const fetchPostsRequest = () => ({
  type: FETCH_POSTS_REQUEST,
});

export const fetchPostsSuccess = (posts) => ({
  type: FETCH_POSTS_SUCCESS,
  payload: posts,
});

export const fetchPostsFailure = (error) => ({
  type: FETCH_POSTS_FAILURE,
  payload: error,
});
