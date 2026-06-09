import type { PhotoResponse } from "../../../models/api/photo-models";

export function getPhotoDataUrl(photo: PhotoResponse | null | undefined) {
    if (!photo?.base64Data) {
        return "";
    }

    return `data:${photo.contentType};base64,${photo.base64Data}`;
}
