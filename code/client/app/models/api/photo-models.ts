import type { UUID } from "crypto";

export interface PhotoResponse {
    id: UUID;
    fileName: string;
    contentType: string;
    base64Data: string;
}

export interface SetProfilePhotoCommand {
    photoId: UUID;
}
