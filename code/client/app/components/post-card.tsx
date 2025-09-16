import Image from 'next/image';

interface PostCardProps {
  profileImage: string | null;
  displayName: string;
  username: string;
  content: string;
}

const PostCard: React.FC<PostCardProps> = ({ profileImage, displayName, username, content }) => {
  return (
    <li className="list-none w-full max-w-xl border-b border-gray-200 px-4 py-3 bg-gradient-to-b from-white to-slate-50">
      <div className="flex items-start gap-4">
        <div className="avatar">
          <div className="w-12 rounded-full overflow-hidden">
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
        <div className="flex-1">
          <div className="flex items-center gap-2">
            <span className="font-semibold text-gray-900">{displayName}</span>
            <span className="text-sm text-gray-500">@{username}</span>
          </div>
          <p className="mt-1 text-gray-800">{content}</p>
          <div className="mt-3 flex gap-4">
            <button className="btn-circle btn-sm ">
              <span className="material-symbols-outlined text-[1.3em]">star</span>
            </button>
            <button className="btn-circle btn-sm">
              <span className="material-symbols-outlined text-[1.3em]">comment</span>
            </button>
          </div>
        </div>
      </div>
    </li>
  );
};

export default PostCard;
