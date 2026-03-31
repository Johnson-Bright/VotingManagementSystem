# Voting Management System

A comprehensive web-based platform that empowers administrators to create and manage elections while enabling registered voters to securely cast their votes and view results in real-time.

---

## 📋 Problem Statement

Conducting elections manually or through informal means is often prone to errors, fraud, and inefficiency. Whether for national polls, school elections, or organizational decisions, paper-based or unstructured voting makes it difficult to ensure transparency, security, and accurate result tabulation. There is a critical need for a reliable, centralized platform that manages the entire voting process digitally.

---

## 🎯 Objectives

Develop a robust web-based Voting Management System using **ASP.NET Core MVC** that enables:

- **Administrators** to create, manage, and oversee elections seamlessly
- **Registration and management** of candidates for each election
- **Verified voters** to cast their votes securely
- **Real-time result** display and tracking after voting closes

### Key Features:
✅ Voter registration and authentication (login/logout)  
✅ Admin panel to create, edit, and manage elections  
✅ Admin capability to add and remove candidates  
✅ One-vote-per-election enforcement for voters  
✅ Result display after election closure  
✅ Election scheduling with configurable start and end dates  
✅ Mobile-friendly, intuitive user interface  

---

## 📊 Requirements

### Functional Requirements:
- ✓ Secure voter registration and login system
- ✓ Admin can create, edit, and delete elections
- ✓ Admin can add and remove candidates
- ✓ One vote per voter per election (strictly enforced)
- ✓ Results displayed after election closes
- ✓ Election scheduling with start and end dates
- ✓ Role-based access control (Admin/Voter)

### Non-Functional Requirements:
- 🔒 **Security**: One vote per registered voter enforced at database level
- ⚡ **Performance**: Results load within 2 seconds
- 📱 **Usability**: Simple, intuitive, mobile-friendly interface
- 🌐 **Availability**: System accessible 24/7 during active elections
- 📈 **Scalability**: Supports multiple simultaneous elections
- 🛡️ **Data Integrity**: Database constraints prevent duplicate votes

---

## 📐 Use Case Diagram

![Use Case Diagram](https://github.com/user-attachments/assets/b85ca03e-7fe6-4012-98df-758b04b28b40)

---

## 🗄️ Database Schema

![Database Diagram](https://github.com/user-attachments/assets/83f58cd5-f629-432e-8fb0-bf397569b038)

---

## 📅 Project Timeline

| Week | Tasks |
|------|-------|
| **Week 1** | Project setup, database design, authentication implementation, admin panel development (create/manage elections & candidates) |
| **Week 2** | Voter dashboard development, vote casting functionality, results page, one-vote enforcement, UI polish, comprehensive testing & documentation |

---

## 🛠️ Technology Stack

- **Backend**: ASP.NET Core MVC
- **Frontend**: HTML, CSS, JavaScript
- **Database**: SQL Server
- **Architecture**: MVC Pattern

---



