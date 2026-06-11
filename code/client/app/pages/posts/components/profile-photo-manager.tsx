"use client";

import Image from 'next/image';
import React, { useId, useState } from 'react';
import type { UUID } from 'crypto';
import { getPhotoDataUrl } from '../models/photo-data-url';
import { useSetProfilePhoto } from '../hooks/use-set-profile-photo';
import { useUploadPhoto } from '../hooks/use-upload-photo';
import { useUserPhotos } from '../hooks/use-user-photos';

interface ProfilePhotoManagerProps {
    canEdit: boolean;
    userId: UUID;
}

export function ProfilePhotoManager({ canEdit, userId }: ProfilePhotoManagerProps) {
    const inputId = useId();
    const [error, setError] = useState<string | null>(null);
    const photosQuery = useUserPhotos(userId, canEdit);
    const uploadPhoto = useUploadPhoto();
    const setProfilePhoto = useSetProfilePhoto();

    if (!canEdit) {
        return null;
    }

    const handleFileChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
        setError(null);
        const file = event.target.files?.[0];

        if (!file) {
            return;
        }

        try {
            const photoId = await uploadPhoto.mutateAsync({ userId, file });
            await setProfilePhoto.mutateAsync({ userId, photoId });
            event.target.value = "";
        } catch (caughtError: unknown) {
            setError(caughtError instanceof Error ? caughtError.message : "Failed to upload photo.");
        }
    };

    const handleSetProfilePhoto = async (photoId: UUID) => {
        setError(null);

        try {
            await setProfilePhoto.mutateAsync({ userId, photoId });
        } catch (caughtError: unknown) {
            setError(caughtError instanceof Error ? caughtError.message : "Failed to set profile photo.");
        }
    };

    return (
        <section className="mt-4 w-full">
            <div className="flex flex-wrap items-center gap-3">
                <label className="btn btn-sm btn-primary" htmlFor={inputId}>
                    Upload photo
                </label>
                <input
                    accept="image/png,image/jpeg,image/gif"
                    className="hidden"
                    id={inputId}
                    onChange={handleFileChange}
                    type="file"
                />
                {(uploadPhoto.isPending || setProfilePhoto.isPending) && (
                    <span className="text-sm text-gray-600">Saving photo...</span>
                )}
            </div>

            {error && <p className="mt-2 text-sm text-error">{error}</p>}
            {photosQuery.isError && <p className="mt-2 text-sm text-error">Failed to load photos.</p>}

            {photosQuery.data && photosQuery.data.length > 0 && (
                <div className="mt-3 flex flex-wrap gap-3">
                    {photosQuery.data.map((photo) => {
                        const src = getPhotoDataUrl(photo);

                        return (
                            <button
                                aria-label={`Use ${photo.fileName} as profile photo`}
                                className="h-16 w-16 overflow-hidden rounded-full border-2 border-black bg-base-100 pokeshadow disabled:opacity-60"
                                disabled={setProfilePhoto.isPending}
                                key={photo.id}
                                onClick={() => handleSetProfilePhoto(photo.id)}
                                type="button"
                            >
                                {src && (
                                    <Image
                                        alt={photo.fileName}
                                        className="h-full w-full object-cover"
                                        height={64}
                                        src={src}
                                        unoptimized
                                        width={64}
                                    />
                                )}
                            </button>
                        );
                    })}
                </div>
            )}
        </section>
    );
}
