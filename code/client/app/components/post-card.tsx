import Image from 'next/image';

interface PostCardProps {
  profileImage: string;
  displayName: string;
  username: string;
  content: string;
}

const PostCard: React.FC<PostCardProps> = ({ profileImage, displayName, username, content }) => {
  return (
    <div className="w-full max-w-xl border-b border-gray-200 px-4 py-3 bg-gradient-to-b from-white to-slate-50">
      <div className="flex items-start space-x-4">
        {/* Avatar */}
        <Image
          src={profileImage}
          alt={`${displayName}'s profile`}
          width={48}
          height={48}
          className="rounded-full"
        />

        {/* Post Content */}
        <div className="flex-1">
          <div className="flex items-center space-x-2">
            <span className="font-semibold text-gray-900">{displayName}</span>
            <span className="text-sm text-gray-500">@{username}</span>
          </div>

          <p className="mt-1 text-gray-800">{content}</p>

          {/* Buttons */}
          <div className="mt-3 flex space-x-4">
            <button className="px-3 py-1 text-sm text-gray-600 bg-gray-100 rounded-full hover:bg-blue-100 hover:text-blue-600 transition-colors duration-150">
              Like
            </button>
            <button className="btn btn-soft">
              Comment
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PostCard;
