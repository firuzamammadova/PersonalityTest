import React, { useState } from 'react';
import './QuestionComponent.css'; 

const QuestionComponent = ({ question, options, currentQuestion, totalQuestions, handleNext, handlePrevious }) => {
  const [selectedOption, setSelectedOption] = useState(null);

  const handleOptionChange = (option) => {
    setSelectedOption(option);
  };

  return (
    <div className="question-container">
      <h2>Question {currentQuestion}/{totalQuestions}</h2>
      <p>{question}</p>
      <form className="options-form">
        {options.map((option) => (
          <div key={option.id} className="option">
            <label>
              <input
                type="radio"
                name="option"
                value={option.id} // Use option.id or another unique identifier
                checked={selectedOption?.id === option.id} // Adjust this comparison if needed
                onChange={() => handleOptionChange(option)} // Pass the full option object
              />
              {option.answerText} {/* Render the actual answer text */}
            </label>
          </div>
        ))}
      </form>
      <div className="navigation-buttons">
        <button onClick={handlePrevious} disabled={currentQuestion === 1}>Previous</button>
        <button onClick={() => handleNext(selectedOption)} disabled={!selectedOption}>Next question</button>
      </div>
    </div>
  );
};

export default QuestionComponent;
