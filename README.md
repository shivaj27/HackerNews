HackerNews – Full Stack Project

This project contains two parts:
1. Backend (ASP.NET Core Web API)
2. Frontend (Angular Application)

Prerequisites: 
1. Node.js (v18 or above)
2. npm (comes with Node.js)
3. .NET 6 SDK (or later)
4. Angular CLI (v16 or above)


Backend setup:
1. Navigate to the folder /Back-end/HackerNews
2. Restore dependencies using: dotnet restore
3. Run the app: dotnet run
4. The backend should start here:
http://localhost:5000

Run tests:
1. Navigate to the tests folder /Back-end/HackerNews.Tests
2. dotnet test


Frontend setup:
1. Navigate to the folder /Front-end/HackerNews
2. intall dependencies using: npm install
3. Start server: ng serve
4. The Frontend should start here:
http://localhost:4200

Run tests:
1. Navigate to the folder /Front-end/HackerNews
2. Run : ng test
3. For code coverage: ng test --code-coverage