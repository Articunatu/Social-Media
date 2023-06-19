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

import React, { useState } from 'react';

function MyComponent() {
  const [isOpen, setIsOpen] = useState(false);

  const toggleModal = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div>
      <button onClick={toggleModal}>Open Modal</button>
      {isOpen && (
        <div className="modal">
          <div className="modal-content">
            <h2>Modal Title</h2>
            <p>This is the content of the modal.</p>
            <button onClick={toggleModal}>Close Modal</button>
          </div>
        </div>
      )}
    </div>
  );
}
