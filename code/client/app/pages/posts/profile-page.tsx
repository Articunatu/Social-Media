
import React from 'react';
import Image from 'next/image';
import PostCard from './components/post-card';
import { useFeed } from './hooks/use-feed';
import { useProfile } from './hooks/use-profile';
import type { FeedPost } from '@/app/models/api/post-models';
import type { UUID } from 'crypto';
import { useAuth } from '../../authentication/auth-provider';
import { ProfilePhotoManager } from './components/profile-photo-manager';
import { getPhotoDataUrl } from './models/photo-data-url';

interface ProfilePageProps {
    userId: UUID;
}

const ProfilePage: React.FC<ProfilePageProps> = ({ userId }) => {
    const { data: posts, isLoading, isError } = useFeed();
    const { data: profile } = useProfile(userId);
    const auth = useAuth();
    const profileInfo = profile?.profile;
    const profilePhotoSrc = getPhotoDataUrl(profileInfo?.profilePhoto);
    const canEditPhotos = auth?.user?.userId === userId;

    return (
        <div className=''>
            <div>
                <div className="avatar">
                    <div className="w-12 h-12 rounded-full border-2 border-black pokeshadow">
                        {profilePhotoSrc && profileInfo && (
                            <Image
                                src={profilePhotoSrc}
                                alt={`${profileInfo.fullName}'s profile`}
                                width={48}
                                height={48}
                                unoptimized
                            />
                        )}
                    </div>
                </div>
                <div>
                    <div>Following: {profile ? profile.followingCount : 0}</div>
                    <div>Followers: {profile ? profile.followersCount : 0}</div>
                </div>
                <button>Follow</button>
                <ProfilePhotoManager canEdit={canEditPhotos} userId={userId} />
            </div>
            <div className="flex flex-col items-center gap-4 p-4">
                {isLoading && <div>Loading...</div>}
                {isError && <div>Failed to load feed.</div>}
                
                {posts && posts.map((post: FeedPost) => (
                    <PostCard key={post.postId} post={post} />
                ))}
            </div>
        </div>
    );
}

export default ProfilePage;
