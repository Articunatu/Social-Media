import React, { ReactNode } from 'react';

interface TextCProps {
    children: ReactNode;
}

const SmText: React.FC<TextCProps> = ({ children }) => {
    return (
        <p className="font-mono text-base font-medium tracking-tighter text-gray-900">
            {children}
        </p>
    );
};

export default SmText;
