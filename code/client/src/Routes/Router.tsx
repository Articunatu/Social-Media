import React from "react";
import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import FeedPage from "../Components/Feed/FeedPage"

export const Router = createBrowserRouter([
    {
      path: "/",
      element: <App />,
      children: [
        { path: "", element: <FeedPage /> },
      ],
    },
  ]);
  