"use client";

import Image from "next/image";
import Link from "next/link";
import type { ReactNode } from "react";
import type { ProfileInfo } from "../../../models/api/user-models";
import { getPhotoDataUrl } from "../models/photo-data-url";

interface PostAuthorLinkProps {
    action?: ReactNode;
    children: ReactNode;
    createdAt: string;
    profile: ProfileInfo;
}

export function PostAuthorLink({ action, children, createdAt, profile }: PostAuthorLinkProps) {
    const profilePhotoSrc = getPhotoDataUrl(profile.profilePhoto);

    return (
        <div className="card-body p-4 flex-row gap-4 items-start">
            <Link className="avatar shrink-0" href={`/profile/${profile.id}`}>
                <div className="w-12 h-12 rounded-full border-2 border-black pokeshadow">
                    {profilePhotoSrc && (
                        <Image
                            src={profilePhotoSrc}
                            alt={`${profile.fullName}'s profile`}
                            width={48}
                            height={48}
                            unoptimized
                        />
                    )}
                </div>
            </Link>

            <div className="flex-1">
                <div className="flex items-center gap-2">
                    <Link className="font-bold text-black hover:underline" href={`/profile/${profile.id}`}>
                        {profile.fullName}
                    </Link>
                    <Link className="text-sm text-gray-700 hover:underline" href={`/profile/${profile.id}`}>
                        @{profile.tag}
                    </Link>
                    <span className="ml-2 text-xs text-gray-500">{new Date(createdAt).toLocaleString()}</span>
                    {action}
                </div>
                {children}
            </div>
        </div>
    );
}
