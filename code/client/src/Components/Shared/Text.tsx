import React, { ReactNode } from 'react';

interface props {
    text : string;
}

const Text: React.FC<props> = ({ text }) => {
    return (
        <p className="font-mono text-base font-medium tracking-tighter text-gray-900">{text}</p>
    );
};

export default Text;
