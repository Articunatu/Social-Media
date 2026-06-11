
"use client";

import React from 'react';
import PostCard from './components/post-card';
import { useProfile } from './hooks/use-profile';
import type { FeedPost } from '@/app/models/api/post-models';
import type { UUID } from 'crypto';
import { useAuth } from '../../authentication/auth-provider';
import { usePostsByUser } from './hooks/use-posts-by-user';
import { useFollowUser } from './hooks/use-follow-user';
import { useUnfollowUser } from './hooks/use-unfollow-user';
import { ReactedPostsList } from './components/reacted-posts-list';
import { ProfileHeader } from './components/profile-header';

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
    const [activeTab, setActiveTab] = React.useState<'posts' | 'reacted'>('posts');
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
        <main className="min-h-screen bg-base-200">
            <ProfileHeader
                canEditPhotos={canEditPhotos}
                canFollow={canFollow}
                followError={followError}
                isFollowPending={isFollowPending}
                onFollowToggle={handleFollowToggle}
                profile={profile}
                userId={userId}
            />

            <section className="border-b border-base-300 bg-base-100">
                <div className="mx-auto flex max-w-5xl justify-center px-4 py-3">
                    <div className="tabs tabs-boxed border border-black bg-base-100">
                        <button
                            className={`tab ${activeTab === 'posts' ? 'tab-active' : ''}`}
                            onClick={() => setActiveTab('posts')}
                            type="button"
                        >
                            Posts
                        </button>
                        <button
                            className={`tab ${activeTab === 'reacted' ? 'tab-active' : ''}`}
                            onClick={() => setActiveTab('reacted')}
                            type="button"
                        >
                            Reacted
                        </button>
                    </div>
                </div>
            </section>

            <section className="mx-auto flex w-full max-w-2xl flex-col items-center gap-4 px-4 py-6">
                {activeTab === 'posts' && isLoading && <div>Loading...</div>}
                {activeTab === 'posts' && isError && <div>Failed to load profile posts.</div>}
                
                {activeTab === 'posts' && posts && posts.map((post: FeedPost) => (
                    <PostCard key={post.postId} post={post} />
                ))}

                {activeTab === 'posts' && posts && posts.length === 0 && !isLoading && (
                    <div>This user has not posted anything yet.</div>
                )}

                <ReactedPostsList enabled={activeTab === 'reacted'} userId={userId} />
            </section>
        </main>
    );
}

export default ProfilePage;
