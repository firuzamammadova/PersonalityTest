﻿# WeatherForecastingApp

The PersonalityTestApp is a RESTful service built using .NET Core 8 that allows users to take a simple personality test. This project is designed using a Clean Architecture approach, ensuring separation of concerns and making the application modular, testable, and maintainable. Users can answer a set of questions that are scored to determine their personality traits, such as Introvert or Extrovert.

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd .\src\Web\
dotnet watch run
```

## Default User
There is default User Credentials and Data , If you want to use :
Email : administrator@localhost
Password: Administrator1!



## Features 

- **Simple Personality Test**: : Users can take a quick personality test consisting of 3-5 questions.
- **Trait Mapping**: The application maps the user's responses to a personality trait of either Introvert or Extrovert.
- **Extendable**:The application is designed to be extendable, allowing for future enhancements and additional features.

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

Running database migrations is easy. Ensure you add the following flags to your command (values assume you are executing from repository root)

-  --project src/Infrastructure (optional if in this folder)
- --startup-project src/Web
- --output-dir Data/Migrations

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.


## Test

The solution contains unit, integration, and functional tests.

To run the tests:
```bash
dotnet test
```


## Conclusion

This project demonstrates a practical application of Clean Architecture principles in a .NET environment. The design choices made aim to balance simplicity with maintainability, ensuring that the application is both functional and easy to extend.

Feel free to reach out if you have any questions or need further clarification!
