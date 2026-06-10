"use client";

import { useParams } from 'next/navigation';
import type { UUID } from 'crypto';
import { PostDetailPage } from '../../pages/posts/post-detail-page';

export default function PostDetailRoute() {
    const params = useParams<{ postId: string }>();

    if (!params.postId) {
        return <div>Invalid post ID</div>;
    }

    return <PostDetailPage postId={params.postId as UUID} />;
}
