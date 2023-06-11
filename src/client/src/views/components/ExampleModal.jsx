import React from "react";

export default class ExmapleModal extends React.Component {
    render() {
    
    const { post, onClose } = this.props;
    
        return (
            <div className="modal">
                <div className="modal-content">
                    <button className="close-button" onClick={onClose}>X</button>
                    <h3>{post.title}</h3>
                    <p>{post.content}</p>
                    <img src={post.image} alt={post.title} />
                    <div className="modal-actions">
                        <button className="react-button">React</button>
                        <button className="comment-button">Comment</button>
                        <button className="share-button">Share</button>
                    </div>
                </div>
            </div>
        );
    };
}
