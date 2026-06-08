import React from 'react';
import ProfilePage from './profile-page';
import { useRouter } from 'next/router';
import type { UUID } from 'crypto';

const ProfilePageWrapper: React.FC = () => {
    const router = useRouter();
    const { userId } = router.query;

    // Validate userId as UUID if needed
    if (!userId || typeof userId !== 'string') {
        return <div>Invalid user ID</div>;
    }

    return <ProfilePage userId={userId as UUID} />;
};

export default ProfilePageWrapper;
