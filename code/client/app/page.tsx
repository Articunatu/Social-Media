import React from 'react';
import PostCard from '../app/components/post-card';

const HomePage: React.FC = () => {
  return (
    <div className="flex flex-col items-center gap-4 p-4">
      <PostCard
        profileImage="/images/C-ProfileTest2.webp"
        displayName="John Doe"
        username="johndoe"
        content="This is a sample post using DaisyUI's card component for styling. Looks neat, doesn't it?"
      />
      <PostCard
        profileImage="/images/D-ProfileTest.webp"
        displayName="Jane Smith"
        username="janesmith"
        content="Another example of a post card. DaisyUI makes it easy to style!"
      />
    </div>
  );
};

export default HomePage;