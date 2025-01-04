import React from 'react';
import PostCard from '../app/components/post-card';

const HomePage: React.FC = () => {
  return (
    <div className="flex flex-col items-center gap-4 p-4">
      <PostCard
        profileImage="https://media.istockphoto.com/id/1300972574/sv/foto/millennial-manlig-teamledare-organiserar-virtuell-workshop-med-anst%C3%A4llda-online.webp?s=1024x1024&w=is&k=20&c=SzJN_PPugtXvvpU735tKtPUDaQwnC-Z77bze9jIDhtk="
        displayName="John Doe"
        username="johndoe"
        content="This is a sample post using DaisyUI's card component for styling. Looks neat, doesn't it?"
      />
      <PostCard
        profileImage="https://media.istockphoto.com/id/1437816897/sv/foto/business-woman-manager-or-human-resources-portrait-for-career-success-company-we-are-hiring-or.jpg?s=1024x1024&w=is&k=20&c=xnH_Rm9Ci_EfTq9XP7qHH3ZdV4NO4BNfdcEJgiD9nkQ="
        displayName="Jane Smith"
        username="janesmith"
        content="Another example of a post card. DaisyUI makes it easy to style!"
      />
    </div>
  );
};

export default HomePage;