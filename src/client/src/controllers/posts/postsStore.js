// store.js
import { createStore, applyMiddleware, combineReducers } from "redux";
import thunk from "redux-thunk";
import postsReducer from "./postsReducer";
import fetchReducer from "./fetchReducer";

const rootReducer = combineReducers({
  posts: postsReducer,
  fetch: fetchReducer,
});

const store = createStore(rootReducer, applyMiddleware(thunk));

export default store;
