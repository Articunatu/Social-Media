"use client";

import Image from 'next/image';
import type { UUID } from 'crypto';
import type { ProfileDetails } from '@/app/models/api/user-models';
import { getPhotoDataUrl } from '../models/photo-data-url';
import { ProfilePhotoManager } from './profile-photo-manager';

interface ProfileHeaderProps {
    canEditPhotos: boolean;
    canFollow: boolean;
    followError: string | null;
    isFollowPending: boolean;
    onFollowToggle: () => void;
    profile: ProfileDetails | undefined;
    userId: UUID;
}

function getInitials(fullName: string) {
    return fullName
        .split(' ')
        .filter(Boolean)
        .slice(0, 2)
        .map((part) => part[0]?.toUpperCase())
        .join('');
}

export function ProfileHeader({
    canEditPhotos,
    canFollow,
    followError,
    isFollowPending,
    onFollowToggle,
    profile,
    userId,
}: ProfileHeaderProps) {
    const profileInfo = profile?.profile;
    const displayName = profileInfo?.fullName ?? 'Profile';
    const profilePhotoSrc = getPhotoDataUrl(profileInfo?.profilePhoto);
    const tag = profileInfo?.tag ? `@${profileInfo.tag}` : '';

    return (
        <section className="w-full border-b-2 border-black bg-base-100">
            <div className="mx-auto flex max-w-5xl flex-col gap-5 px-4 py-6 sm:flex-row sm:items-end sm:justify-between">
                <div className="flex min-w-0 flex-col gap-4 sm:flex-row sm:items-end">
                    <div className="avatar">
                        <div className="flex h-28 w-28 items-center justify-center overflow-hidden rounded-full border-2 border-black bg-base-200 text-3xl font-bold pokeshadow">
                            {profilePhotoSrc && profileInfo ? (
                                <Image
                                    alt={`${displayName}'s profile`}
                                    className="h-full w-full object-cover"
                                    height={112}
                                    src={profilePhotoSrc}
                                    unoptimized
                                    width={112}
                                />
                            ) : (
                                <span>{getInitials(displayName) || '?'}</span>
                            )}
                        </div>
                    </div>

                    <div className="min-w-0">
                        <p className="truncate text-3xl font-bold leading-tight text-black">{displayName}</p>
                        {tag && <p className="mt-1 text-sm font-semibold text-gray-600">{tag}</p>}
                        {profile?.aboutMe && (
                            <p className="mt-3 max-w-xl text-sm leading-6 text-gray-700">{profile.aboutMe}</p>
                        )}
                    </div>
                </div>

                <div className="flex flex-col gap-3 sm:items-end">
                    <div className="stats stats-horizontal border-2 border-black bg-base-200 shadow-none">
                        <div className="stat px-4 py-2">
                            <div className="stat-title text-xs">Followers</div>
                            <div className="stat-value text-xl">{profile?.followersCount ?? 0}</div>
                        </div>
                        <div className="stat px-4 py-2">
                            <div className="stat-title text-xs">Following</div>
                            <div className="stat-value text-xl">{profile?.followingCount ?? 0}</div>
                        </div>
                    </div>

                    {canFollow && (
                        <button
                            className="btn btn-primary min-w-32"
                            disabled={isFollowPending}
                            onClick={onFollowToggle}
                            type="button"
                        >
                            {isFollowPending
                                ? 'Saving...'
                                : profile?.isFollowedByCurrentUser
                                    ? 'Unfollow'
                                    : 'Follow'}
                        </button>
                    )}

                    {followError && <p className="text-sm text-error">{followError}</p>}
                </div>
            </div>

            <div className="mx-auto max-w-5xl px-4 pb-6">
                <ProfilePhotoManager canEdit={canEditPhotos} userId={userId} />
            </div>
        </section>
    );
}
