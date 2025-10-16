# Bookstore API Tests

[![API Tests Workflow](https://github.com/brunokutacho/Bookstore.ApiTests/actions/workflows/run-tests.yml/badge.svg)](https://github.com/brunokutacho/Bookstore.ApiTests/actions/workflows/run-tests.yml)

## Project Description

This project contains **automated API tests** for the **Bookstore** application.  
It validates the functionality of **Authors** and **Books** endpoints, including both success and failure scenarios.

The tests are implemented using **NUnit** and generate **Allure Reports** for detailed analysis of test results.

---

## Technologies Used

- [.NET 8](https://dotnet.microsoft.com/)  
- [NUnit](https://nunit.org/)  
- [Allure](https://docs.qameta.io/allure/) for test reporting  
- [GitHub Actions](https://github.com/features/actions) for CI/CD  
- [Docker](https://www.docker.com/) (optional, to run tests inside a container)

---

## Running Locally

1. Clone this repository:

```bash
git clone https://github.com/brunokutacho/Bookstore.ApiTests.git
cd Bookstore.ApiTests
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build --configuration Release
```

4. Run the tests:
```bash
dotnet test --configuration Release --logger "trx;LogFileName=test_results.trx" --results-directory ./allure-results
```

5. Generate the Allure report (make sure Allure CLI is installed):
```bash
allure generate ./allure-results --clean -o ./allure-report
allure open ./allure-report
```


## Running Using Docker

1. Build the Docker image:
```bash
docker build -t bookstore-api-tests .
```

2. Run the tests inside the container:
```bash
docker run --rm bookstore-api-tests
```
##### The Allure report will be generated inside the container. You can map volumes to access it outside if needed.


## Running Using GitHub Actions

This project has a configured workflow to automatically run tests and generate an Allure Report published via GitHub Pages.

Latest Allure report: [View Allure Report](https://brunokutacho.github.io/Bookstore.ApiTests/)

Workflow status badge: [![API Tests Workflow](https://github.com/brunokutacho/Bookstore.ApiTests/actions/workflows/run-tests.yml/badge.svg)](https://github.com/brunokutacho/Bookstore.ApiTests/actions/workflows/run-tests.yml)


### You can also trigger the workflow manually:

Go to the Actions tab on GitHub.

Select the workflow Run API Tests and Generate Allure Report.

Click Run workflow and choose the branch if needed.


## Allure Report

The Allure report provides:

Overview: Summary of total, passed, and failed tests.

Graphs and charts: Visual representation of test execution.

Detailed results: Step-by-step logs for each test.

To view locally, run:
```bash
allure open ./allure-report
```

You can navigate through the report to see detailed execution for each test, attachments, and statuses.


## Project Structure
```bash
Bookstore.ApiTests/
│
├─ ApiTests/
│   ├─ Clients/       # Classes to consume the API
│   ├─ Data/          # Test data factories
│   ├─ Models/        # Author and Book models
│   └─ Tests/         # NUnit test classes (AuthorsTests, BooksTests)
│
├─ Dockerfile         # Container to run the tests
├─ allure-results/    # Test results
├─ allure-report/     # Allure reports
└─ .github/workflows/ # CI/CD workflow file
```
