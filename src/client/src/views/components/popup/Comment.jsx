import React from "react"
import Modal from 'react-modal';

export default class Comment extends React.Component {
    render() {
        const { modalIsOpen, afterOpenModal, closeModal, customStyles, subtitle} = this.props;

        return(
            <div>
                <Modal
                    isOpen={modalIsOpen}
                    onAfterOpen={afterOpenModal}
                    onRequestClose={closeModal}
                    style={customStyles}
                    contentLabel="Example Modal"
                >
                    <h2 ref={(_subtitle) => (subtitle = _subtitle)}>Hello</h2>
                    <button onClick={closeModal}>close</button>
                    <div>I am a modal</div>
                    <form>
                                <label>Enter your name:
                                    <input type="text" />
                                </label>
                            </form>
                </Modal>
                        
            </div>
        )
    }
};