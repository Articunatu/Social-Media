"use client";

import { useParams } from 'next/navigation';
import type { UUID } from 'crypto';
import ProfilePage from '../../pages/posts/profile-page';

export default function UserProfileRoute() {
    const params = useParams<{ userId: string }>();
    const userId = params.userId;

    if (!userId || userId === 'undefined') {
        return <div>Invalid user ID</div>;
    }

    return <ProfilePage userId={userId as UUID} />;
}
