
#1. Project name
# VotingManagementSystem
A web-based platform that allows administrators to create and manage elections while enabling registered voters to securely cast their votes and view results.

#2. Problem statement
Conducting elections manually or through informal means is often prone to errors, fraud, and inefficiency. Whether for national polls, school elections, or organizational decisions, paper-based or unstructured voting makes it difficult to ensure transparency, security, and accurate result tabulation. There is a need for a reliable, centralized platform that manages the entire voting process digitally.

#3. Objective
Develop a web-based Voting Management System using ASP.NET Core MVC that allows administrators to create and manage elections, register candidates, and oversee the voting process — while enabling verified voters to cast their votes securely and view results after voting closes.

This application can include features like:
-Voter registration and authentication (login/logout)
-Admin panel to create and manage elections and candidates
-Voters can cast one vote per election
-Result display after voting closes
-Election scheduling with start and end dates

#4. Functional and non functional requirements
Functional Requirements:
-Voter registration and login
-Admin can create, edit, and delete elections
-Admin can add and remove candidates
-Voter can cast one vote per election
-Results are displayed after the election closes
-Election scheduling with start and end dates

Non-Functional Requirements:
-Security: one vote per registered voter enforced at database level
-Performance: results load within 2 seconds
-Usability: simple, mobile-friendly interface
-Availability: system accessible 24/7 during active elections
-Scalability: supports multiple simultaneous elections

#5. Usecase diagram

<img width="900" height="675" alt="image" src="https://github.com/user-attachments/assets/b85ca03e-7fe6-4012-98df-758b04b28b40" />


#6. Database diagram

<img width="900" height="1120" alt="image" src="https://github.com/user-attachments/assets/83f58cd5-f629-432e-8fb0-bf397569b038" />



#7. Project timeline
Week	Tasks
Week 1	Project setup, DB design, authentication, admin panel — create/manage elections & candidates
Week 2	Voter dashboard, cast vote, results page, one-vote enforcement, UI polish, testing & documentation
