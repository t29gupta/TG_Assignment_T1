# TG_Web_Extraction
- Web extractor for local html file
- It is a console application
- The projects are in .Net5.
- I'm using HtmlAgilityPack for HTML dom operations

## Prerequisites:
1. Visual Studio 2019
2. .Net5 installed

## How to run :
1. Open the solution
2. Run the default console project by the name TG_Web_Extraction
3. Enter the location of the html file.
4. Output is printed in the console window and saved in the same folder as input file.

## Run Unit tests:
1. Go to TG_Web_Extraction.Tests project
2. Run all tests

## Improvements 

There are several things we could enhance:

- **Error Handling**: We can add custom error handling according to the requirement. Currently I'm just returning empty data if a value is missing. In a real world application, we would be returning some sort of message and logging the same.
- **UX**: I've used a console application for running the logic. Same could have been done through a WPF/Web app/Web API. Which would open ways to send different types of data and testing
- **Testing**:  File IO and Exception tests should be added.

## Comments
I have added some comments, notes, etc every where in code to explain my mindset while taking that decision.
Usually I want my code to be self-explanatory and avoid comments unless absolutely needed. 
My approach in this project was to start with minimum required and then extend when needed.

Looking forward to the feedback. Many Thanks!
