import React, { useState } from "react";
import PostModal from "./PostModal";

function PostButton() {
  const [isOpen, setIsOpen] = useState(false);

  const openModal = () => {
    setIsOpen(true);
  };

  const closeModal = () => {
    setIsOpen(false);
  };

  const handleSubmit = (message) => {
    console.log(message);
    // TODO: Submit the message to the server
  };

  return (

    <button  type="button"
      className="inline-flex items-center px-4 py-2 border border-transparent rounded-md font"
    ></button>
  )
}