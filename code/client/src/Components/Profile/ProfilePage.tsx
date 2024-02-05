import React from "react";

type Props = {};

const ProfilePage = (props: Props) => {
    return (
        <div className="container mx-auto p-4">
            <h1 className="text-4xl font-bold mb-4">Feed Page</h1>
            <div className="flex justify-between items-center bg-gray-200 p-4 rounded">
                <div className="flex-grow">
                    <p>dskfdnsfoubdfi9ubdfsdf</p>
                    <p>dskfdnsfoubdfi9ubdfsdf</p>
                    <p>dskfdnsfoubdfi9ubdfsdf</p>
                    <p>dskfdnsfoubdfi9ubdfsdf</p>
                    <p>dskfdnsfoubdfi9ubdfsdf</p>
                    <p>dskfdnsfoubdfi9ubdfsdf</p>
                    <p className="text-lg">Welcome to the feed page!</p>
                </div>
                <button className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">
                    Load More
                </button>
            </div>
        </div>
    );
};

export default ProfilePage;
