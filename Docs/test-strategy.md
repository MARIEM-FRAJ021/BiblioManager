# BiblioManager — Test Strategy

## 1. Objective

The objective of this test strategy is to verify the reliability, security, and functional correctness of the BiblioManager API.

Testing aims to verify:

* Business requirements and core functionalities;
* JWT authentication and role-based authorization;
* Membership and active/inactive member rules;
* Book borrowing and return workflows;
* Data consistency after operations;
* Input validation and error handling;
* Protection against unauthorized access to resources.

---

## 2. Scope

### In Scope

The following areas are covered by testing:

* Authentication and JWT handling;
* Role-based authorization (`Admin`, `Employe`, `Utilisateur`);
* Member and membership management;
* Book management;
* Author management;
* Category management;
* Loan management;
* Book returns;
* Late penalties;
* Payment processing and Stripe integration;
* Input validation;
* HTTP error handling;
* Access control and security scenarios.

### Out of Scope

The following are currently outside the scope of this testing phase:

* Angular frontend;
* UI testing;
* External services other than the Stripe integration;
* Advanced performance testing;
* Infrastructure and deployment testing.

---

## 3. Test Types

| Test Type           | Tool / Approach                     | Objective                                                   |
| ------------------- | ----------------------------------- | ----------------------------------------------------------- |
| Manual API Testing  | Swagger / Postman                   | Validate functional scenarios and negative cases            |
| Unit Testing        | xUnit, Moq                          | Verify business rules implemented in services               |
| Integration Testing | WebApplicationFactory               | Verify interactions between API components and the database |
| Security Testing    | Manual API tests                    | Verify authentication, authorization, and resource access   |
| Regression Testing  | Swagger / Postman + automated tests | Ensure that changes do not break existing functionality     |
| Validation Testing  | API                                 | Verify invalid inputs, missing fields, and boundary values  |

Performance and advanced security testing may be added in a later phase.

---

## 4. Test Environment

Testing is initially performed in a local development environment.

### Main Components

* ASP.NET Core Web API;
* SQL Server;
* Swagger / OpenAPI;
* Stripe CLI for payment testing;
* Git for version control.

A dedicated test dataset is used to reduce side effects on development data.

---

## 5. Test Data

Testing uses different user profiles and business data to cover the main application scenarios.

### User Accounts

* Administrator;
* Employee;
* Non-member user;
* Active member;
* Inactive member.

### Business Data

* Available books;
* Unavailable books;
* Existing and non-existing authors;
* Existing categories;
* Active and inactive members;
* Active and completed loans;
* Pending and validated payments.

---

## 6. Entry Criteria

Testing can start when:

* The application builds successfully;
* The API starts without errors;
* The database is available;
* Required database migrations have been applied;
* Swagger is accessible;
* Required test data is available.

---

## 7. Exit Criteria

The test campaign is considered complete when:

* All critical test cases have been executed;
* Core functionalities have been validated;
* No critical or blocking defects remain open;
* Identified defects have been fixed or documented;
* Required regression tests have been executed;
* Test results have been documented in `test-cases.md`.

---

## 8. Risk Management

| Risk                                    | Probability | Impact | Mitigation / Test Approach            |
| --------------------------------------- | ----------- | ------ | ------------------------------------- |
| Unauthorized access to resources        | Medium      | High   | Dedicated 401/403 authorization tests |
| Access to another member's data         | Medium      | High   | Resource ownership and IDOR tests     |
| Loan created for an inactive member     | Medium      | High   | Business rule tests                   |
| Loan created when a book is unavailable | Medium      | High   | Book availability tests               |
| Incorrect book stock after loan/return  | Medium      | High   | Data consistency tests                |
| Incorrect payment validation            | Low         | High   | Stripe payment scenario tests         |
| Invalid data accepted by the API        | Medium      | Medium | Validation and boundary tests         |
| Regression after code changes           | Medium      | Medium | Regression test suite                 |

---

## 9. Test Results and Defect Management

Test cases are documented in:

`docs/test-cases.md`

Each test case includes:

* Test case ID;
* Feature;
* Preconditions;
* Steps;
* Expected result;
* Actual result;
* Status.

Defects are documented in:

`docs/bugs.md`

Each defect includes:

* Defect ID;
* Description;
* Steps to reproduce;
* Expected result;
* Actual result;
* Severity;
* Status;
* Fix, when applicable.

---

## 10. Testing Process

Testing follows the following process:

1. Prepare the test environment;
2. Execute manual API tests;
3. Identify and fix defects;
4. Perform regression testing;
5. Implement unit tests for critical business rules;
6. Implement integration tests;
7. Gradually automate API scenarios;
8. Integrate automated tests into the CI/CD pipeline.

---

## 11. Test Priorities

Testing priorities are based on business and security risks.

### High Priority

* Authentication;
* Authorization;
* Member management;
* Loan management;
* Book returns;
* Payments;
* Access to personal resources.

### Medium Priority

* Books;
* Authors;
* Categories;
* Input validation.

### Low Priority

* Secondary scenarios;
* Less critical edge cases;
* Advanced performance testing.

---

## 12. Test Deliverables

The main testing deliverables are:

* `docs/test-strategy.md` — Test strategy;
* `docs/test-cases.md` — Test cases;
* `docs/bugs.md` — Defect reports;
* Unit tests;
* Integration tests;
* Regression test results;
* Automated test reports.

