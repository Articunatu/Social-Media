import React from "react";
import BaseModal from "../GPT_Modal";

export default class Modal extends React.Component {
    render() {
    
    const { post, onClose } = this.props;
    
        return (
            <BaseModal 
                post={post}
                onClose={onClose}>
            </BaseModal>
        );
    };
}