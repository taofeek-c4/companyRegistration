# CompanyCardAPI

A secure ASP.NET Core Web API for:

-  Company registration with email OTP verification
-  Password setup after verification
-  Card generation with unique card numbers and secure PINs
- Admin dashboard to view generated cards

## Features

- OTP-based company onboarding
- Secure password hashing with SHA-256
- Card number and PIN generator
- Simple HTML/JS dashboard for testing
- PostgreSQL database (via EF Core)
- Follows best practices and RESTful conventions

## Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core
- sqlServer (local)
- Bootstrap (Frontend)
- MailKit for OTP email delivery

## How to Run

### Backend (API)

```bash
cd CompanyCardAPI
dotnet build
dotnet run
