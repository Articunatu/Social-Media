import React from "react";
import Button from "../Shared/Button";
import Text from "../Shared/Text";
type Props = {};

const handleClickButton = () => {
    // Handle button click logic
};

const FeedPage = (props: Props) => {
    return (
        <div className="container mx-auto p-4">
            <h1 className="text-4xl font-bold mb-4">Feed Page</h1>
            <div className="flex justify-between items-center bg-gray-200 p-4 rounded">
                <div className="flex-grow">
                    <Text text="Welcome to the Feed Page"></Text>
                </div>
                <Button onClick={handleClickButton}>Click me</Button>
                <Button onClick={handleClickButton}>Load more</Button>
            </div>
        </div>
    );
};

export default FeedPage;
