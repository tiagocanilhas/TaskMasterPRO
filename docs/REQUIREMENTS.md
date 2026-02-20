# Project Specification: TaskMaster Pro

## 1. Introduction
The goal of this project is the design and implementation of a professional-grade desktop task management system named **TaskMaster Pro**. 

This project serves as a practical sandbox to master the .NET Ecosystem, specifically focusing on building graphical interfaces with **WPF (Windows Presentation Foundation)**, ensuring data persistence with **SQLite**, and strictly separating business logic from the UI using the **MVVM (Model-View-ViewModel)** architectural pattern.

## 2. Domain Model
The application manages two main entities:
* **Category:** Characterized by a unique identifier (Id), a Name (e.g., "Work", "Personal", "Study"), and a Hex Color Code for visual identification.
* **Task:** Characterized by a unique identifier (Id), a Title, an optional Description, a Creation Date, a Deadline (Date/Time), and is associated with one **Category**. It also maintains a Status (Pending or Completed) and a Priority level (Low, Medium, High).

## 3. Technical Stack & Constraints
* **Language & Framework:** C# 12.0 and .NET 8.0 (or higher).
* **Frontend:** WPF using XAML for the UI.
* **Database:** SQLite (local database file).
* **Data Access:** Entity Framework Core (EF Core) as the ORM.
* **Architecture:** MVVM. 
  * *Constraint:* No database calls or complex logic are allowed in the XAML Code-Behind (`.xaml.cs`). 
  * *Constraint:* All button clicks and UI interactions must be handled via `ICommand` in the ViewModels.
  * *Constraint:* The UI must update automatically when data changes using `INotifyPropertyChanged` and `ObservableCollection`.

---

## 4. Testing & Quality Assurance
To guarantee the reliability of the system, the project must include a robust automated testing strategy. All tests will reside in a separate project within the solution (e.g., `TaskMasterPro.Tests`).

* **Testing Framework:** xUnit.
* **Database Testing:** Use the `Microsoft.EntityFrameworkCore.InMemory` package to test Data Access layers without corrupting the real SQLite database.
* **UI/Logic Testing (MVVM):** The ViewModels must be unit-testable. Use the `Moq` library to mock database services. You must be able to test user actions (Commands) and state changes (Properties) purely through C# code, without spinning up the WPF UI.

---

## 5. Phased Development Plan & Schedule

The project is divided into 4 sequential phases. Do not move to the next phase until the acceptance criteria of the current one are met.

### Phase 1: Data Access and Backend Logic
**Goal:** Establish the database, the models, and the basic CRUD operations using asynchronous programming (`async / await`) to ensure UI responsiveness.
* Create the `Task` and `Category` C# classes.
* Configure the EF Core `DbContext` to connect to a local SQLite file (`tasks.db`) and generate the schema via EF Core Migrations.
* Create an interface `ITaskDataService` and its implementation to handle database operations asynchronously.
* **Testing:** Create the xUnit test project. Write tests for the CRUD operations using the InMemory database provider.
* **Acceptance Criteria:**
  * The SQLite database file is successfully created.
  * All Unit Tests for creating, reading, updating, and deleting tasks/categories pass successfully (Green).

### Phase 2: Basic User Interface and MVVM Integration
**Goal:** Build the first visual screens and connect them to the ViewModels using Dependency Injection.
* **Main Window:** Create a screen that displays a list of all existing tasks.
* **Add Task View:** Create a form to input a Title, select a Priority, and save a new Task.
* Create the `MainViewModel` utilizing `ObservableCollection` and `ICommand`. Inject `ITaskDataService` via the constructor.
* **Testing:** Write unit tests for the `MainViewModel`. Mock the `ITaskDataService` using `Moq` so the ViewModel tests run in isolation from the database.
* **Acceptance Criteria:**
  * The UI launches and displays tasks. Adding a task updates the UI immediately.
  * Unit tests prove that executing the "Add Task Command" successfully adds an item to the `ObservableCollection`.
  * No database logic exists in the View or ViewModel.

### Phase 3: Advanced Functionalities & Navigation
**Goal:** Implement filtering, related data (Categories), and task actions.
* **Category Management:** Add the ability to create new Categories and assign them to Tasks.
* **Task Actions:** Add the ability to mark a task as "Completed" (e.g., via a CheckBox) and to delete a task.
* **Filtering:** Add UI controls (like ComboBoxes or RadioButtons) to filter the visible task list by Status and Priority.
* **Testing:** Write tests to verify that changing the filter criteria correctly updates the visible task list in the ViewModel.
* **Acceptance Criteria:**
  * Checking a task as completed updates the database immediately.
  * Changing a filter instantly updates the displayed list without needing to restart the application or query the database unnecessarily.
  * All new features are covered by passing xUnit tests.

### Phase 4: Polish, Analytics, and UX
**Goal:** Improve the user experience and finalize the application.
* **Progress Analytics:** Add a Progress Bar to the Main Window that calculates the percentage of completed tasks (Completed Tasks / Total Tasks * 100).
* **Theming:** Implement a "Dark Mode" toggle. Use WPF Resource Dictionaries to switch between light and dark color schemes.
* **Validation:** Prevent the user from saving a task with an empty Title. Show a visual error if they try.
* **Testing:** Write tests to ensure the Progress percentage calculation is strictly accurate based on the task list state.
* **Acceptance Criteria:**
  * The Progress Bar updates dynamically.
  * Dark Mode toggles seamlessly.
  * Empty tasks are rejected, and unit tests verify this rejection logic.

---

*Disclaimer: The structure, requirements, and formatting of this specification document were created with the assistance of AI (Google Gemini).*
