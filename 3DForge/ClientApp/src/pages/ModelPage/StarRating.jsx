import React, { useState, useEffect } from 'react';
import cl from "./.module.css";

const StarRating = ({ rating, onRatingChange }) => {
    const maxRating = 5; // Максимальна оцінка
    const [filledStars, setFilledStars] = useState(Math.floor(rating)); // Кількість заповнених зірок

    // Генерація заповнених та порожніх зірок
    const starsArray = Array.from({ length: maxRating }, (_, index) => (
        <img
            key={index}
            className={index < filledStars ? cl.filled_star : cl.empty_star}
            alt={index < filledStars ? "Filled Star" : "Empty Star"}
            onClick={() => handleStarClick(index + 1)}
        />
    ));

    // Обробник натискання на зірку
    const handleStarClick = (clickedRating) => {
        setFilledStars(clickedRating);
        onRatingChange(clickedRating); // Викликати зміну рейтингу
    };

    // Log the updated rating when it changes
    useEffect(() => {
        console.log(filledStars);
    }, [filledStars]);

    return (
        <div className={cl.star_rating}>
            {starsArray}

        </div>
    );
};

export default StarRating;