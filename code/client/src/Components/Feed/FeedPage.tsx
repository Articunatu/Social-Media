import React from 'react';
import Carousel from '../Shared/Carousel'; 

interface Props {}

const FeedPage: React.FC<Props> = () => {
    return (
        <div className="container mx-auto p-4">
        <Carousel items={[]} />
        </div>
    );
};

export default FeedPage;
