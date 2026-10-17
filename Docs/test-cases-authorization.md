# 2. Authorization — Test Cases

These tests verify that each role can access only the operations permitted to it.

**Test accounts:** Prepare authenticated accounts with the `Admin`, `Employe`, and `Utilisateur` roles, and obtain a valid JWT for each.

## 2.1 Authentication Required

### AUTHZ-001 — Access protected endpoint without authentication

**Priority:** Critical
**Type:** Negative / Security

**Preconditions:**

* No access token is provided.

**Steps:**

1. Send `GET /api/Utilisateur` without an `Authorization` header.
2. Repeat with another protected endpoint.

**Expected Result:**

* The API returns `401 Unauthorized`.
* No protected data is returned.

**Actual Result:**

* The API returned `401 Unauthorized`.
* No protected data was returned.

**Status:** Pass

## 2.2 Role-Based Access

### AUTHZ-002 — Admin accesses an Admin-only operation

**Priority:** Critical
**Type:** Positive / Authorization

**Preconditions:**

* An authenticated Admin account exists.
* The Admin JWT is valid.
* An existing user account is available.
* The target user ID is valid.
* The requested role is supported by the API.

**Steps:**

1. Send a PUT request to `PUT /api/Utilisateur/{id}/role`, replacing {id} with the target user's ID.
2. Include the Admin JWT in the Authorization: Bearer <token> header.
3. Provide a valid role in the request body, according to the API's expected format.
4. Execute the request.
5. Verify that the user's role has been updated.

**Expected Result:**

* The API authorizes the request.
* The user's role is updated successfully.
* The API returns the expected success status code.
* The updated role is correctly persisted in the database.

**Actual Result:**

* The API authorized the request.
* The user's role was updated successfully.
* The API returned the expected success status code.
* The updated role was correctly persisted in the database.

**Status:** Pass

---

### AUTHZ-003 — Employe attempts an Admin-only operation

**Priority:** Critical
**Type:** Negative / Authorization

**Preconditions:**

* An authenticated Employe account exists.
* The Employe JWT is valid.

**Steps:**

1. Send a request to an Admin-only endpoint.
2. Include the Employe JWT.
3. Provide otherwise valid request data.

**Expected Result:**

* The API returns `403 Forbidden`.
* No employee or other record is created or modified.

**Actual Result:**

* The API returned `403 Forbidden`.
* No employee or other record was created or modified.

**Status:** Pass

---

### AUTHZ-004 — Utilisateur attempts an Admin-only operation

**Priority:** Critical
**Type:** Negative / Authorization

**Preconditions:**

* An authenticated Utilisateur account exists.

**Steps:**

1. Send a request to an Admin-only endpoint.
2. Include the Utilisateur JWT.

**Expected Result:**

* The API returns `403 Forbidden`.
* No unauthorized operation is performed.

**Actual Result:**

* The API returns `403 Forbidden`.
* No unauthorized operation is performed.

**Status:** Pass

---

### AUTHZ-005 — Employe accesses an endpoint allowed to employees

**Priority:** High
**Type:** Positive / Authorization

**Preconditions:**

* An Employe account exists.
* The endpoint is configured to allow Employe access.

**Steps:**

1. Send a request to an endpoint permitted for Employe.
2. Include the Employe JWT.

**Expected Result:**

* The API authorizes the request.
* The operation succeeds when the input is valid.

**Actual Result:**

* The API authorized the request.
* The operation succeeded when the input is valid.

**Status:** Pass

---

### AUTHZ-006 — Utilisateur accesses an endpoint restricted to Admin or Employe

**Priority:** Critical
**Type:** Negative / Authorization

**Preconditions:**

* A Utilisateur account exists.
* The selected endpoint is restricted to Admin and/or Employe.

**Steps:**

1. Send a request to the restricted endpoint.
2. Include the Utilisateur JWT.

**Expected Result:**

* The API returns `403 Forbidden`.
* No protected information is disclosed.
* No data is modified.

**Actual Result:**

* The API returned `403 Forbidden`.
* No protected information was disclosed.
* No data was modified.

**Status:** Pass

## 2.3 JWT Integrity

### AUTHZ-007 — Access endpoint with a tampered JWT

**Priority:** Critical
**Type:** Negative / Security

**Steps:**

1. Obtain a valid JWT.
2. Modify its payload or signature.
3. Send a request to a protected endpoint using the modified token.

**Expected Result:**

* The API returns `401 Unauthorized`.
* The modified token is not accepted.

**Actuak Result:**

* The API returned `401 Unauthorized`.
* The modified token was not accepted.

**Status:** Pass

---

### AUTHZ-008 — Verify that a user's role cannot be changed by editing the JWT

**Priority:** Critical
**Type:** Negative / Security

**Preconditions:**

* A valid Utilisateur JWT is available.

**Steps:**

1. Modify the JWT payload to claim the Admin role without generating a valid signature.
2. Send a request to an Admin-only endpoint.

**Expected Result:**

* The API returns `401 Unauthorized`.
* The user does not gain Admin privileges.

**Actual Result:**

* The API returned `401 Unauthorized`.
* The user didn't gain Admin privileges.

**Status:** Pass

## 2.4 Business Rules and Data Isolation

### AUTHZ-009 — Admin cannot assign a forbidden role to themselves

**Priority:** High
**Type:** Negative / Business Rule

**Preconditions:**

* An Admin account exists.
* The application forbids changing the Admin's role or deleting the Admin account.

**Steps:**

1. Authenticate as Admin.
2. Attempt to change the Admin's own role using `PUT /api/Utilisateur/{id}/role`.
3. Attempt to delete the Admin account using `DELETE /api/Utilisateur/{id}`.

**Expected Result:**

* The API rejects the forbidden changes.
* The Admin role is preserved.
* The Admin account is not deleted.

**Actual Result:**

* The API rejected the forbidden changes.
* The Admin role was preserved.
* The Admin account was not deleted.

**Status:** Pass

---

### AUTHZ-010 — User cannot access another user's self-service data

**Priority:** Critical
**Type:** Negative / Authorization

**Preconditions:**

* Two distinct user accounts exist.
* Both accounts have valid JWTs.

**Steps:**

1. Authenticate as User A.
2. Send a request to a self-service endpoint using User B's ID, if the endpoint accepts an ID.
3. Try to retrieve or modify User B's information.

**Expected Result:**

* The API does not disclose or modify User B's data.
* Self-service operations use the authenticated user's identity rather than trusting an arbitrary client-supplied ID.

**Expected Result:**

* The API didn't disclose or modify User B's data.
* Self-service operations used the authenticated user's identity rather than trusting an arbitrary client-supplied ID.

**Status:** Pass