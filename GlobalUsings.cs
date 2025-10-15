// ==========================================================
//  GLOBAL USINGS
//  Consolidated namespace imports for the SocialMedia (Tweeble)
//  application. Makes these namespaces available project-wide
//  without repetitive `using` directives in each file.
//
//  Purpose:
//  • Simplifies maintainability and readability
//  • Reduces boilerplate in Models, Controllers, and Views
//  • Centralizes framework and domain dependencies
// ==========================================================

// ----------------------------------------------------------
//  PROJECT NAMESPACES
//  Core application structure: data layer, models, and viewmodels.
// ----------------------------------------------------------
global using SocialMedia.Data;
global using SocialMedia.Models;
global using SocialMedia.ViewModels;

// ----------------------------------------------------------
//  ASP.NET CORE FRAMEWORK
//  Includes identity, authorization, and MVC essentials.
// ----------------------------------------------------------
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;

// ----------------------------------------------------------
//  SYSTEM NAMESPACES
//  Common base libraries for LINQ, async operations,
//  data annotations, and collections.
// ----------------------------------------------------------
global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.Linq;
global using System.Threading.Tasks;