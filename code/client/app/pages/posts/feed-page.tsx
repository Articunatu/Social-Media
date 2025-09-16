import React from 'react';
import PostCard from '../../components/post-card';

const FeedPage: React.FC = () => {
    return (
        <div className="flex flex-col items-center gap-4 p-4">
        <PostCard
            profileImage={null}
            displayName="Heinrich Lunge"
            username="polizei_nr1"
            content="Someone was here, and nobody can do anything without leaving some sort of trace of themselves. If there were such a person, he could hardly be considered human."
        />
        <PostCard
            profileImage={null}
            displayName="Tenjou Utena"
            username="roseduelist"
            content="I don’t want the power to revolutionize the world, but Himemiya needs me!"
        />
        <button className="btn btn-primary">Primary</button>
        <p className="text-foreground">Normal text</p>
        <div className="bg-background">Panel</div>
        </div>
    );
};

export default FeedPage;