# Learning Tracker

A web application for managing learners, learning activities, and
assignments. It provides a Coordinator-facing dashboard and allows
learners to track the activities assigned to them.

## Tech Stack

-   ASP.NET Core MVC (.NET 10)
-   C#
-   Entity Framework Core
-   PostgreSQL
-   Razor Views, HTML, CSS, and JavaScript

## Project Structure

The solution is split into multiple projects to separate
responsibilities:

  -----------------------------------------------------------------------
  Project                             Purpose
  ----------------------------------- -----------------------------------
  `LearningTracker.Web`               MVC controllers, Razor views, and
                                      web application configuration

  `LearningTracker.Services`          Application business logic

  `LearningTracker.Repositories`      Database access and repository
                                      implementations

  `LearningTracker.Data`              Entity Framework Core database
                                      context and database configuration

  `LearningTracker.Models`            Shared entities and enums
  -----------------------------------------------------------------------

## Features

-   **Authentication:** Sign in and sign out using cookie-based
    authentication.
-   **Role-based access:** Separate access for Coordinators and
    Learners.
-   **Password management:** Supports password changes and
    temporary-password handling.
-   **Coordinator dashboard:** Displays learner, activity, and
    assignment statistics, including cohort progress.
-   **Learner management:** Create and manage learner accounts.
-   **Activity management:** Create, view, and edit learning activities.
-   **Assignment management:** Assign activities to learners and track
    assignment status.
-   **Learner progress:** Learners can view their assignments and update
    their progress.

## Prerequisites

Make sure the following are installed:

-   .NET 10 SDK
-   PostgreSQL
-   An IDE or editor such as Visual Studio or Visual Studio Code

## Configuration

Before running the application, configure the following settings.

### PostgreSQL Connection String

Update `DefaultConnection` in your local configuration file, such as
`appsettings.Development.json`:

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LearningTrackerDb;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
  },
  "Scheduler": {
    "HealthUrl": "http://localhost:5272/health"
  },
  "COORDINATOR_REGISTRATION_SECRET": "YOUR PASSWORD"
}
```

Replace the database username and password with your PostgreSQL
credentials. Set `Scheduler:HealthUrl` to the health endpoint of the
scheduler service you are using.
Add coordinator registration secret for coordination creation

The scheduler setting can also be supplied as an environment variable:

``` text
Scheduler__HealthUrl
```

In ASP.NET Core configuration, the double underscore (`__`) represents a
colon (`:`). Therefore, `Scheduler__HealthUrl` overrides the
`Scheduler:HealthUrl` setting in `appsettings.json`.

### Coordinator Registration Secret

Set the following environment variable:

``` text
COORDINATOR_REGISTRATION_SECRET
```

This value is used as the shared registration secret on the Coordinator
registration page. Do not commit real secrets or database passwords to
the repository.

## Run Locally

1.  Clone the repository:

    ``` bash
    git clone https://github.com/s-sammm-y/LearningProgressTracker
    cd LearningProgressTracker
    ```

2.  Restore the dependencies:

    ``` bash
    dotnet restore
    ```

3.  Configure the PostgreSQL connection string, the
    `Scheduler__HealthUrl` environment variable (if configuring the
    scheduler URL through the environment), and the
    `COORDINATOR_REGISTRATION_SECRET` environment variable.

4.  Run the MVC application:

    ``` bash
    dotnet run --project LearningTracker.Web
    ```

5.  Open the local URL shown in the terminal.

The application is configured to apply pending Entity Framework Core
migrations during startup. Make sure the configured PostgreSQL server is
running and the database credentials are valid. Also make sure the
scheduler health URL points to the scheduler service's health endpoint.

## Demo Note: Coordinator Account Creation

Coordinator self-registration is included **for demonstration and
testing purposes only**.

To create a Coordinator account:

1.  Set the `COORDINATOR_REGISTRATION_SECRET` environment variable.
2.  Open the Coordinator registration page at `/Coordinator/Register`.
3.  Enter an email address, password, and the configured registration
    secret.
4.  Submit the form. If the details and secret are valid, the
    Coordinator account is created.
5.  Go to the login page and sign in using the newly created
    credentials.

The registration secret is checked by the application before the account
is created. The new account is assigned the `Coordinator` role, and its
password is stored as a hash.

This is a demo registration mechanism, not a production-ready
Coordinator onboarding process. In a production system, Coordinator
account creation should be restricted to an authorized administrative
process, and the registration secret should not be distributed publicly.
