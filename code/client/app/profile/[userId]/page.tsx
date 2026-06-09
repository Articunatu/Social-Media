"use client";

import { useParams } from 'next/navigation';
import type { UUID } from 'crypto';
import ProfilePage from '../../pages/posts/profile-page';

export default function UserProfileRoute() {
    const params = useParams<{ userId: string }>();

    if (!params.userId) {
        return <div>Invalid user ID</div>;
    }

    return <ProfilePage userId={params.userId as UUID} />;
}
