# LecturesApp

### What Is It About

The application allows ones to create Lectures and others to register on Lectures to visit them.

### Technologies

The application is based on the following:
- C#
- ASP.NET Core MVC 3
- Entity Framework Core
- Identity

### Application Structure

There are two domain models: **User** and **Lecture**.
It is supposed to divide access between **Users** via **Identity Roles**.
One role is **Host**. It can create **Lectures**.
Another role is **Member**. It can register on **Lectures**.
There are two types of relationships between models.
One-to-many between **Lecture** and **User** as **Host**.
Many-to-many between **Lecture** and **User** as **Member**.
In other words, **Lecture** has one **Host** and many **Members**.

![Diagram](https://imgur.com/nP4U5dy)

*At this time there are no roles. Users can both create lectures and register on them.*

### What is next

There are a lot of things to do:
- Divide LectureController into two controllers for two roles
- Add roles and divide access
- Add check for a count of registered members
- Add check for age limit
- Deny editing a lecture if it is over
- Add sorting
- Add fancy design
