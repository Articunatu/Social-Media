// fetchReducer.js
import { CREATE_POST, LIKE_POST, FETCH_POSTS_SUCCESS } from "./actionTypes";
  
  const initialState = {
    loading: false,
    error: null,
  };
  
  const fetchReducer = (state = initialState, action) => {
    switch (action.type) {
      case FETCH_POSTS_REQUEST:
        return {
          ...state,
          loading: true,
          error: null,
        };
      case FETCH_POSTS_SUCCESS:
        return {
          ...state,
          loading: false,
          error: null,
        };
      case FETCH_POSTS_FAILURE:
        return {
          ...state,
          loading: false,
          error: action.payload,
        };
      default:
        return state;
    }
  };
  
  export default fetchReducer;
  