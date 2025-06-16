import Image from 'next/image';

interface PostCardProps {
  profileImage: string;
  displayName: string;
  username: string;
  content: string;
}

const PostCard: React.FC<PostCardProps> = ({ profileImage, displayName, username, content }) => {
  return (
    <div className="card card-bordered bg-base-100 w-96 shadow-xl">
      <figure className="flex items-center px-4 pt-4">
        <div className="avatar">
          <div className="w-16 rounded-full">
            <Image src={profileImage} alt={`${displayName}'s profile`} />
          </div>
        </div>
        <div className="ml-4">
          <h2 className="card-title">{displayName}</h2>
          <p className="text-sm text-gray-500">@{username}</p>
        </div>
      </figure>
      
      <div className="card-body">
        <p>{content}</p>
        <div className="card-actions justify-end">
          <button className="btn btn-primary btn-sm">Like</button>
          <button className="btn btn-outline btn-sm">Comment</button>
        </div>
      </div>
    </div>
  );
};

export default PostCard;
