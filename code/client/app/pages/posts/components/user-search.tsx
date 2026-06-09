"use client";

import Image from 'next/image';
import Link from 'next/link';
import React, { useState } from 'react';
import { useSearchUsers } from '../hooks/use-search-users';
import { getPhotoDataUrl } from '../models/photo-data-url';

export function UserSearch() {
    const [searchText, setSearchText] = useState('');
    const [hasSearched, setHasSearched] = useState(false);
    const searchUsers = useSearchUsers();

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        const trimmedSearchText = searchText.trim();

        if (!trimmedSearchText) {
            searchUsers.reset();
            setHasSearched(false);
            return;
        }

        setHasSearched(true);
        await searchUsers.mutateAsync(trimmedSearchText);
    };

    return (
        <section className="w-full max-w-2xl">
            <form className="flex gap-2" onSubmit={handleSubmit}>
                <input
                    className="input input-bordered w-full border-2 border-black"
                    onChange={(event) => setSearchText(event.target.value)}
                    placeholder="Search users"
                    type="search"
                    value={searchText}
                />
                <button className="btn btn-primary" disabled={searchUsers.isPending} type="submit">
                    {searchUsers.isPending ? 'Searching...' : 'Search'}
                </button>
            </form>

            {searchUsers.isError && (
                <p className="mt-2 text-sm text-error">Failed to search users.</p>
            )}

            {hasSearched && searchUsers.data?.length === 0 && (
                <p className="mt-2 text-sm text-gray-600">No users found.</p>
            )}

            {searchUsers.data && searchUsers.data.length > 0 && (
                <div className="mt-3 overflow-hidden rounded-lg border-2 border-black bg-base-100">
                    {searchUsers.data.map((profile) => {
                        const profilePhotoSrc = getPhotoDataUrl(profile.profilePhoto);

                        return (
                            <Link
                                className="flex items-center gap-3 border-b border-black/10 px-3 py-2 last:border-b-0 hover:bg-base-200"
                                href={`/profile/${profile.id}`}
                                key={profile.id}
                            >
                                <div className="h-10 w-10 overflow-hidden rounded-full border-2 border-black bg-base-200">
                                    {profilePhotoSrc && (
                                        <Image
                                            alt={`${profile.fullName}'s profile`}
                                            className="h-full w-full object-cover"
                                            height={40}
                                            src={profilePhotoSrc}
                                            unoptimized
                                            width={40}
                                        />
                                    )}
                                </div>
                                <div className="min-w-0">
                                    <p className="truncate font-semibold text-black">{profile.fullName}</p>
                                    <p className="truncate text-sm text-gray-700">@{profile.tag}</p>
                                </div>
                            </Link>
                        );
                    })}
                </div>
            )}
        </section>
    );
}
