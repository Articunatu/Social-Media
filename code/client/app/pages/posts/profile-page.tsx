
import React from 'react';
import Image from 'next/image';
import PostCard from '../../components/post-card';
import { useFeed } from '../../hooks/use-feed';
import { useProfile } from '../../hooks/use-profile';
import { FeedPost } from '@/app/models/api/post-models';
import { UUID } from 'crypto';

interface ProfilePageProps {
    userId: UUID;
}

const ProfilePage: React.FC<ProfilePageProps> = ({ userId }) => {
    const { data: posts, isLoading, isError } = useFeed();
    const { data: profile } = useProfile(userId);

    return (
        <div className=''>
            <div>
                <div className="avatar">
                    <div className="w-12 h-12 rounded-full border-2 border-black pokeshadow">
                        {profile && profile.profileInfo && profile.profileInfo.profilePhoto && (
                            <Image
                                src={profile.profileInfo.profilePhoto}
                                alt={`${profile.profileInfo.fullName}'s profile`}
                                width={48}
                                height={48}
                            />
                        )}
                    </div>
                </div>
                <div>
                    <div>Following: {profile ? profile.followingCount : 0}</div>
                    <div>Followers: {profile ? profile.followersCount : 0}</div>
                </div>
                <button>Follow</button>
            </div>
            <div className="flex flex-col items-center gap-4 p-4">
                {isLoading && <div>Loading...</div>}
                {isError && <div>Failed to load feed.</div>}
                {posts && posts.map((post: FeedPost) => (
                    <PostCard key={post.createdAt + post.authorId} post={post} />
                ))}
            </div>
        </div>
    );
}

export default ProfilePage;