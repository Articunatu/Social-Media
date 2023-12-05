
const PostInput = ({user, text, setText, sendPost}) => {
    return (
        <>
            <p>{user.handle}</p>
            <input value={text} onChange={e => setText(e.target.value)}/>
            <button className="primary" onClick={sendPost}>Send</button>
        </>
    );
}

export default PostInput;