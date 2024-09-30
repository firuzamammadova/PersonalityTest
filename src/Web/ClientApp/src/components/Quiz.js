import React, { useState, useEffect } from 'react';
import QuestionComponent from './QuestionComponent';

const Quiz = () => {
  const [questionsData, setQuestionsData] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
  const [loading, setLoading] = useState(true);
  const [answers, setAnswers] = useState([]);
  const [quizComplete, setQuizComplete] = useState(false);
  const [personalityResult, setPersonalityResult] = useState(null); // New state for the result

  // Fetch questions and answers from the backend
  useEffect(() => {
    setCurrentQuestionIndex(0);

    const fetchQuestions = async () => {
      try {
        const response = await fetch('https://localhost:5001/api/PersonalityTest');
        if (!response.ok) {
          throw new Error('Error submitting answers');
        }

        const result = await response.json();
        console.log(result);
        setQuestionsData(result);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching questions:", error);
      }
    };

    fetchQuestions();
  }, []);


  const handleRestart = () => {
    console.log(currentQuestionIndex);
    setCurrentQuestionIndex(0); // Reset to the first question
    setAnswers([]);             // Clear previous answers
    setQuizComplete(false);      // Reset the quiz completion state
  };
  if (quizComplete && personalityResult) {
    return (
      <div className="quiz-complete">
        <h2>Personality Test Result</h2>
        <p>Personality Trait: {personalityResult.personalityTrait}</p> {/* Show Personality Trait */}
        <p>Test Taken On: {new Date(personalityResult.testTakenOn).toLocaleDateString()}</p> {/* Show Test Date */}
        <button onClick={handleRestart}>Start Again</button> {/* Start Again button */}
      </div>
    );
  }
  const handleNext = (selectedOption) => {
    // Save the selected option
    const updatedAnswers = [...answers];
    updatedAnswers[currentQuestionIndex] = {
      questionId: questionsData[currentQuestionIndex].id, // Assuming each question has an id
      answer: selectedOption
    };
    setAnswers(updatedAnswers);

    // Move to the next question
    setCurrentQuestionIndex((prev) => prev + 1);
  };
  const submitAnswers = async () => {
    try {
      console.log('here');
      console.log(answers);
      if (!answers || answers.length === 0) {
        console.error("No answers provided. Please answer the questions before submitting.");
        return;
      }
      console.log('inside');

      const response = await fetch('https://localhost:5001/api/PersonalityTest', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ userAnswers: answers }),
      });

      if (!response.ok) {
        throw new Error('Error submitting answers');
      }

      const result = await response.json();
      console.log('Answers submitted successfully:', result);
      setQuizComplete(true);
      setPersonalityResult(result);
    } catch (error) {
      console.error('Failed to submit answers:', error);
    }
  };

  if (currentQuestionIndex >= questionsData.length) {
    // All questions answered, submit the answers
    submitAnswers();
    return <div className="quiz-complete">You've completed the quiz!</div>;
  }
  const handlePrevious = () => {
    setCurrentQuestionIndex((prev) => prev - 1);
  };

  if (loading) {
    return <div>Loading questions...</div>;
  }
  //if (quizComplete) {
  //  return (
  //    <div className="quiz-complete">
  //      <h2>You've completed the quiz!</h2>
  //      <button onClick={handleRestart}>Start Again</button> {/* Start Again button */}
  //    </div>
  //  );
  //}

  return (
    <div className="quiz-container">
      {currentQuestionIndex < questionsData.length ? (
        <QuestionComponent
          question={questionsData[currentQuestionIndex].text}
          options={questionsData[currentQuestionIndex].answers}
          currentQuestion={currentQuestionIndex + 1}
          totalQuestions={questionsData.length}
          handleNext={handleNext}
          handlePrevious={handlePrevious}
        />
      ) : (
        <div className="quiz-complete">You've com2e32edqw3r4pleted the quiz!</div>
      )}
    </div>
  );
};

export default Quiz;
