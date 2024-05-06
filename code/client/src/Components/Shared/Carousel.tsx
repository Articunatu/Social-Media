import React, { useState } from 'react';

interface CarouselProps {
    items: JSX.Element[];
    defaultItems?: JSX.Element[]; // Add defaultItems prop
    interval?: number;
}

const defaultInterval = 5000;

const Carousel: React.FC<CarouselProps> = ({ items, defaultItems = [], interval = defaultInterval }) => {
    const [currentSlide, setCurrentSlide] = useState(0);

    const nextSlide = () => {
        const nextIndex = (currentSlide + 1) % items.length;
        setCurrentSlide(nextIndex);
    };

    const prevSlide = () => {
        const prevIndex = (currentSlide - 1 + items.length) % items.length;
        setCurrentSlide(prevIndex);
    };

    // Automatic sliding
    React.useEffect(() => {
        const intervalId = setInterval(nextSlide, interval);
        return () => clearInterval(intervalId);
    }, [currentSlide, interval]);

    // Combine items and defaultItems
    const allItems = defaultItems.concat(items);

    return (
        <div className="relative">
            {allItems.map((item, index) => (
                <div
                key={index}
                className={`absolute top-0 left-0 w-full h-full transition-opacity duration-500 ${
                    index === currentSlide ? 'opacity-100' : 'opacity-0'
                }`}
                >
                {item}
                </div>
            ))}
            <button className="absolute top-1/2 left-4 transform -translate-y-1/2" onClick={prevSlide}>
                Prev
            </button>
            <button className="absolute top-1/2 right-4 transform -translate-y-1/2" onClick={nextSlide}>
                Next
            </button>
        </div>
    );
};

export default Carousel;
