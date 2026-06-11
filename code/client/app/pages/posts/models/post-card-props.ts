import type { FeedPost } from "../../../models/api/post-models";

export type PostCardProps = {
    post: FeedPost;
    commentsInitiallyOpen?: boolean;
    onDeleted?: () => void;
};
