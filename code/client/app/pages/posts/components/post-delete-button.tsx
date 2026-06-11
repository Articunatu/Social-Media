"use client";

interface PostDeleteButtonProps {
    disabled: boolean;
    onDelete: () => void;
}

export function PostDeleteButton({ disabled, onDelete }: PostDeleteButtonProps) {
    return (
        <button
            className="btn btn-xs btn-error ml-auto"
            disabled={disabled}
            onClick={onDelete}
            type="button"
        >
            Delete
        </button>
    );
}
