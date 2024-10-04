const SmHeader = ({ user, viewPostsFeed, setViewPostsFeed }) => {
    return (
        <header className="p-4">
            <div className="info-container space-y-4">
                <div className="user-info-container flex items-center space-x-4">
                    <div>
                        <h1 className="text-2xl font-bold">{user.username}</h1>
                        <p className="text-gray-500">
                            {user.handle}
                            <span className="text-sm text-gray-400 ml-2">posts.net</span>
                        </p>
                    </div>
                    <div className="img-container h-16 w-16">
                        <img 
                            src={user.img} 
                            alt="profile avatar" 
                            className="h-full w-full rounded-full object-cover"
                        />
                    </div>
                </div>
                <p className="text-gray-700">{user.bio}</p>
                <div className="sub-info-container text-sm text-gray-500">
                    <p className="sub-text">
                        {user.followers.length} followers •{" "}
                        <a 
                            href={user.link} 
                            target="_blank" 
                            rel="noopener noreferrer" 
                            className="text-blue-500 hover:underline"
                        >
                            {user.link.replace("https://www.", "")}
                        </a>
                    </p>
                </div>
            </div>
            <button
                className="primary bg-blue-600 text-white py-2 px-4 mt-4 rounded-lg hover:bg-blue-700"
                onClick={() => navigator.clipboard.writeText('I am a URL')}
            >
                Share Profile
            </button>
            <div className="button-container flex space-x-4 mt-4">
                <button
                    className={`${viewPostsFeed ? "bg-blue-600 text-white" : "text-gray-700"} py-2 px-4 rounded-lg`}
                    onClick={() => setViewPostsFeed(true)}
                >
                    Posts
                </button>
                <button
                    className={`${!viewPostsFeed ? "bg-blue-600 text-white" : "text-gray-700"} py-2 px-4 rounded-lg`}
                    onClick={() => setViewPostsFeed(false)}
                >
                    Replies
                </button>
            </div>
        </header>
    );
};

export default SmHeader;