# SPC - Statistical Process Control System

## 🔒 Private Repository - Proprietary Software

**© 2025 - All Rights Reserved**  
This is a proprietary, closed-source software project.  
Unauthorized copying, distribution, or modification is strictly prohibited.

---

## 📋 Description

Système de Contrôle Statistique des Processus (SPC) pour la production industrielle de câblage.

Application WPF moderne avec architecture async/await pour le contrôle qualité en temps réel.

---

## ✨ Fonctionnalités

- ✅ Enregistrement séries de production (6 types de machines)
- ✅ Contrôle dimensions automatique (HA, HI, Traction)
- ✅ Validation tolérances temps réel
- ✅ Gestion maintenance préventive des outils
- ✅ Historique complet et traçabilité
- ✅ Interface utilisateur responsive

---

## 🏗️ Architecture

- **Frontend:** WPF .NET 8.0
- **Pattern:** MVVM (Model-View-ViewModel)
- **Data Access:** Async/Await throughout
- **Database:** Microsoft Access (évolutif SQL Server)
- **Authentication:** JWT Ready

---

## 🚀 Technologies

- C# 12
- .NET 8.0
- WPF (Windows Presentation Foundation)
- XAML
- Async/Await
- LINQ
- Entity Framework Ready

---

## 📊 Structure Projet

SPC/
├── Models/ # Entités et modèles de données
├── Services/ # Couche service (ISpcDataService)
├── ViewModel/ # ViewModels MVVM (8 VMs)
├── Views/ # Interfaces XAML WPF
├── Managers/ # Accès données (legacy async)
├── Tools/ # Utilitaires (Commands, Helpers)
├── API.DTOs/ # Data Transfer Objects
└── App.xaml # Point d'entrée application

---

## 🔧 Prérequis

- Windows 10/11 (64-bit)
- .NET 8.0 SDK
- Visual Studio 2022 ou supérieur
- Microsoft Access Database Engine (si Access)

---

## 📦 Installation (Développement)

1. Cloner le repository (accès autorisé uniquement)
2. Ouvrir `SPC.sln` dans Visual Studio
3. Restaurer les packages NuGet
4. Configurer la connection string
5. Build & Run

---

## ⚙️ Configuration

Configurer `App.config`:
<connectionStrings> <add name="SPCConnection" connectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Path\To\Database.accdb" /> </connectionStrings> ```
Développeur
[Mohammed Batar]
Email: [batarmohamed.03@gmail.com]
LinkedIn: [https://www.linkedin.com/in/mohammed-batar-262a74344/]

