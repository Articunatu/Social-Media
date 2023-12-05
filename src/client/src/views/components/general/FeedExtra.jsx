import Thread from './thread';

const Feed = ({user, setOpenPopup, filteredPosts, getPosts,}) => {
    return (
        <div className="feed">
            {filteredPosts?.map(filteredPost => 
            <Post 
                key={filteredPost.id} 
                user={user} 
                setOpenPopup={setOpenPopup}
                filteredPost={filteredPost}
                getPosts={getPosts}
            />)}
        </div>
    );
}

export default Feed;
