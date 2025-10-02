# FarmXpert Platform

This repository contains the **frontend and backend services** for **FarmXpert**, a platform that streamlines the subsidy application process for farmers in the Republic of Moldova. The system enables digital management of subsidy forms and required documents, ensuring compliance with local regulations and improving user experience.

For the Python AI features, access the backend repository here: 
```bash
https://github.com/Klavrin/FarmXpert-backend
```
---

## Features

- **Subsidy Application Management:** Create, edit, and track subsidy applications.
- **Document Upload & Validation:** Upload and validate required documents for each subsidy type.
- **Eligibility Feedback:** View recommendations and eligibility status for available subsidies.
- **User Authentication:** Secure access and user profile management.

---

## Tech Stack

- **Frontend:** Blazor (WebAssembly & Server), MudBlazor UI
- **Backend:** ASP.NET Core, MongoDB
- **Deployment:** Docker

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [MongoDB](https://www.mongodb.com/try/download/community) (local or Docker)

### Installation

```bash
git clone https://github.com/Klavrin/FarmXpert.git
cd farmxpert
dotnet restore
```

### Running the App

```bash
dotnet run --project FarmXpert/FarmXpert/FarmXpert.csproj
```

The app will be available at:

```
http://localhost:8080
```
