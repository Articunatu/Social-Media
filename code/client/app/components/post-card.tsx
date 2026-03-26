
import Image from 'next/image';
import { FeedPost } from '../models/api/post-models';

interface PostCardProps {
  post: FeedPost;
}

const PostCard: React.FC<PostCardProps> = ({ post }) => {
  const { profile, content, createdAt } = post;
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
              {profile.profilePhoto && (
                <Image
                  src={profile.profilePhoto}
                  alt={`${profile.fullName}'s profile`}
                  width={48}
                  height={48}
                />
              )}
            </div>
          </div>

          {/* Content */}
          <div className="flex-1">
            <div className="flex items-center gap-2">
              <span className="font-bold text-black">{profile.fullName}</span>
              <span className="text-sm text-gray-700">@{profile.tag}</span>
              <span className="text-xs text-gray-500 ml-2">{new Date(createdAt).toLocaleString()}</span>
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
