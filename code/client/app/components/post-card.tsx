import Image from 'next/image';

interface PostCardProps {
  profileImage: string | null;
  displayName: string;
  username: string;
  content: string;
}

const PostCard: React.FC<PostCardProps> = ({ profileImage, displayName, username, content }) => {
  return (
    <li className="list-none w-full max-w-xl px-4 py-3">
      <div
        className="
          card bordered bg-base-200
          border-4 border-black
          pokeshadow pokeshadow-hover
          transition-all duration-150
          cursor-pointer
        "
      >
        <div className="card-body p-4 flex-row gap-4 items-start">
          {/* Avatar */}
          <div className="avatar">
            <div className="w-12 h-12 rounded-full border-2 border-black pokeshadow">
              {profileImage && (
                <Image
                  src={profileImage}
                  alt={`${displayName}'s profile`}
                  width={48}
                  height={48}
                />
              )}
            </div>
          </div>

          {/* Content */}
          <div className="flex-1">
            <div className="flex items-center gap-2">
              <span className="font-bold text-black">{displayName}</span>
              <span className="text-sm text-gray-700">@{username}</span>
            </div>
            <p className="mt-1 text-black">{content}</p>

            {/* Buttons */}
            <div className="mt-3 flex gap-3">
              <button className="btn btn-sm bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none">
                <span className="material-symbols-outlined text-[1.2em]">star</span>
              </button>
              <button className="btn btn-sm bg-base-100 border-2 border-black pokeshadow active:translate-x-px active:translate-y-px active:shadow-none">
                <span className="material-symbols-outlined text-[1.2em]">comment</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </li>
  );
};


export default PostCard;
