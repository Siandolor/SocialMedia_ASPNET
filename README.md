# Tweeble (SocialMedia)

A lightweight **ASP.NET Core MVC web application** that simulates a minimal social media platform. Users can register, log in, post short messages (called *Chirps*), tag topics using *Peeps* (similar to hashtags), and like or explore trending content. Built entirely with **C#**, **Entity Framework Core (SQLite)**, and **ASP.NET Identity**.

---

## Features

- **User Accounts & Authentication**  
  Registration and login system powered by ASP.NET Identity.
- **Chirps**  
  Post short text messages (max. 123 characters) similar to tweets.
- **Peeps (Hashtags)**  
  Auto-detected tags using the `<name>` syntax, stored and queryable via EF Core.
- **Likes**  
  Toggle-based like system with live counter per chirp.
- **Profiles**  
  View user details, statistics, and recent chirps.
- **Trending Topics**  
  Displays the top 5 Peeps from the last 24 hours.
- **Bootstrap UI**  
  Clean, responsive front-end using Bootstrap and custom CSS.
- **Auto-Migration**  
  Database schema updates automatically on application start.

---

## Project Structure

```
SocialMedia/
├── Controllers/
│   ├── AccountController.cs      # Handles registration, login, logout
│   ├── HomeController.cs         # Main feed, chirp creation, likes
│   ├── PeepsController.cs        # Peeps-based topic filtering
│   └── ProfileController.cs      # User profile and statistics
│
├── Data/
│   └── ApplicationDbContext.cs   # EF Core DB context and model mappings
│
├── Models/
│   ├── ApplicationUser.cs        # Extends IdentityUser with Description, CreatedAt
│   ├── Chirp.cs                  # Main post entity
│   ├── Peep.cs                   # Hashtag-equivalent entity
│   ├── ChirpPeep.cs              # Many-to-many relation Chirp <-> Peep
│   ├── Like.cs                   # Like relation (User <-> Chirp)
│   └── ErrorViewModel.cs         # Error model for exception pages
│
├── ViewModels/
│   ├── ChirpViewModel.cs         # Projection for UI rendering
│   ├── LoginViewModel.cs         # Login data model
│   └── RegisterViewModel.cs      # Registration form model
│
├── Views/
│   ├── Account/                  # Login & Register pages
│   ├── Home/                     # Feed & Chirp creation
│   ├── Peeps/                    # Peeps overview
│   ├── Profile/                  # Profile page
│   └── Shared/                   # Layout, validation partials, etc.
│
├── wwwroot/css/                  # CSS styles
│   └── site.css                  # styles for page-design
├── Program.cs                    # Application configuration & middleware pipeline
├── GlobalUsings.cs               # Shared using declarations
└── appsettings.json              # Config (DB, Logging)
```

---

## How It Works

1. **User Registration & Identity**  
   ASP.NET Identity provides user creation, authentication cookies, and password policies.  
   Custom fields like `Description` and `CreatedAt` are added via the `ApplicationUser` model.

2. **Data Persistence**  
   The database uses **SQLite** (`SocialMedia.db`).  
   The `ApplicationDbContext` defines relationships and keys for Chirps, Likes, and Peeps.

3. **Posting Chirps**  
   Authenticated users can submit short messages (max. 123 chars).  
   `<TagName>` entries are automatically extracted and stored as Peeps.

4. **Likes & Interaction**  
   Likes are toggled using a composite key `(UserId, ChirpId)` with EF tracking.

5. **Trending Peeps**  
   The 5 most-used Peeps from the last 24 hours are shown in the sidebar.

6. **Database Migration**  
   On startup, EF Core ensures the database schema is up-to-date via `db.Database.Migrate()`.

---

## Setup & Installation

### Prerequisites
- .NET 6.0 or later
- SQLite (preinstalled with .NET runtime)

### Build & Run
```bash
  git clone https://github.com/Siandolor/SocialMedia_ASPNET_MVC_EF_CSharp.git
  cd SocialMedia
  dotnet restore
  dotnet ef database update
  dotnet run
```

Then open your browser and navigate to:
```
https://localhost:5001
```

---

## Technical Notes

- **Framework:** ASP.NET Core MVC (.NET 6+)
- **Database:** SQLite + Entity Framework Core
- **Authentication:** ASP.NET Core Identity
- **Frontend:** Razor Views + Bootstrap + Custom CSS
- **Security:** HTTPS enforced, secure cookies, 7-day sliding expiration

---

## License
Licensed under the **MIT License**.  
(c) 2025 Daniel Fitz

---

## Notes
- Designed for educational and experimental use — not production.
- Identity & database logic are self-contained and easily extendable.
- All controllers and models are fully documented for clarity and maintainability.

> “Tweeble – minimal social noise, maximum signal.”  
> — Daniel Fitz, 2025
