
"use client";

import React from 'react';
import Image from 'next/image';
import PostCard from './components/post-card';
import { useProfile } from './hooks/use-profile';
import type { FeedPost } from '@/app/models/api/post-models';
import type { UUID } from 'crypto';
import { useAuth } from '../../authentication/auth-provider';
import { ProfilePhotoManager } from './components/profile-photo-manager';
import { getPhotoDataUrl } from './models/photo-data-url';
import { usePostsByUser } from './hooks/use-posts-by-user';
import { useFollowUser } from './hooks/use-follow-user';
import { useUnfollowUser } from './hooks/use-unfollow-user';

interface ProfilePageProps {
    userId: UUID;
}

const ProfilePage: React.FC<ProfilePageProps> = ({ userId }) => {
    const { data: profile } = useProfile(userId);
    const auth = useAuth();
    const profileInfo = profile?.profile;
    const { data: posts, isLoading, isError } = usePostsByUser(userId, profileInfo);
    const followUser = useFollowUser();
    const unfollowUser = useUnfollowUser();
    const [followError, setFollowError] = React.useState<string | null>(null);
    const profilePhotoSrc = getPhotoDataUrl(profileInfo?.profilePhoto);
    const canEditPhotos = auth?.user?.userId === userId;
    const canFollow = !!auth?.user && auth.user.userId !== userId;
    const isFollowPending = followUser.isPending || unfollowUser.isPending;

    const handleFollowToggle = async () => {
        setFollowError(null);

        if (!auth?.user) {
            setFollowError('You must be logged in to follow users.');
            return;
        }

        try {
            if (profile?.isFollowedByCurrentUser) {
                await unfollowUser.mutateAsync({
                    followerId: auth.user.userId as UUID,
                    followingId: userId,
                });
                return;
            }

            await followUser.mutateAsync({
                followerId: auth.user.userId as UUID,
                followingId: userId,
            });
        } catch (caughtError: unknown) {
            setFollowError(caughtError instanceof Error ? caughtError.message : 'Failed to update follow status.');
        }
    };

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
                {canFollow && (
                    <button
                        className="btn btn-sm btn-primary mt-2"
                        disabled={isFollowPending}
                        onClick={handleFollowToggle}
                        type="button"
                    >
                        {isFollowPending
                            ? 'Saving...'
                            : profile?.isFollowedByCurrentUser
                                ? 'Unfollow'
                                : 'Follow'}
                    </button>
                )}
                {followError && <p className="mt-2 text-sm text-error">{followError}</p>}
                <ProfilePhotoManager canEdit={canEditPhotos} userId={userId} />
            </div>
            <div className="flex flex-col items-center gap-4 p-4">
                {isLoading && <div>Loading...</div>}
                {isError && <div>Failed to load profile posts.</div>}
                
                {posts && posts.map((post: FeedPost) => (
                    <PostCard key={post.postId} post={post} />
                ))}

                {posts && posts.length === 0 && !isLoading && (
                    <div>This user has not posted anything yet.</div>
                )}
            </div>
        </div>
    );
}

export default ProfilePage;
