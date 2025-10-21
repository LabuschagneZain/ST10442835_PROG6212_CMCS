# CMCS Lecturer Claim Management System

## Overview

The CMCS Lecturer Claim Management System is a web application built with **ASP.NET Core MVC** that allows lecturers to submit claims for hours worked. Claims can include uploaded supporting documents (PDFs, images, etc.), which are stored in **Azure Blob Storage**. Claim data is stored in **Azure Table Storage**, and lecturers can view their submitted claims with their current status. Managers can approve or reject claims.

---

## Features

- Lecturer dashboard for submitting new claims
- Upload supporting documents to Azure Blob Storage
- View previously submitted claims
- Management dashboard for approving/rejecting claims
- Claims stored in Azure Table Storage for persistence
- Session-based user identity management
- Responsive interface using Bootstrap 5

---

## Technologies Used

- **ASP.NET Core MVC** (.NET 7)
- **Azure Blob Storage** – for storing uploaded files
- **Azure Table Storage** – for storing claim metadata
- **Bootstrap 5** – for UI styling
- **C# 11** – backend logic
- **Visual Studio 2022** – development environment

---

## Prerequisites

- Visual Studio 2022 or later
- .NET 7 SDK
- Azure Storage Account (with Blob and Table Storage)
- Internet connection for CDN (Bootstrap)