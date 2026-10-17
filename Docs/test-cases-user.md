3. User Management — Test Cases

The following cases use the user endpoints identified for the project. Confirm the exact routes against the API's Swagger documentation.

## 3.1 Retrieve Users

### USER-001 — Admin retrieves all users

**Priority:** High
**Type:** Positive

**Preconditions:**

* An Admin account is authenticated.
* At least two users exist.

**Steps:**

1. Send `GET /api/Utilisateur` with the Admin JWT.

**Expected Result:**

* The API returns a successful response.
* The response contains the users allowed by the endpoint's contract.
* User information is returned in the expected format.

**Actual Result:**

* The API returned a successful response.
* The response contained the users allowed by the endpoint's contract.
* User information was returned in the expected format.

**Status:** Pass

---

### USER-002 — Unauthorized user attempts to retrieve all users

**Priority:** Critical
**Type:** Negative / Authorization

**Preconditions:**

* A Utilisateur account is authenticated.

**Steps:**

1. Send `GET /api/Utilisateur` with the Utilisateur JWT.

**Expected Result:**

* The request is rejected if the endpoint is restricted to Admin or Employe.
* No unauthorized user list is returned.

**Actual Result:**

* The request was rejected for an endpoint restricted to Admin or Employe.
* No unauthorized user list was returned.

**Status:** Pass

---

### USER-003 — User retrieves their own profile

**Priority:** High
**Type:** Positive

**Preconditions:**

* A user account exists.
* The user has a valid JWT.

**Steps:**

1. Send `GET /api/Utilisateur/me` with the user's JWT.

**Expected Result:**

* The API returns the authenticated user's profile.
* The returned user ID and email correspond to that account.
* Sensitive data, such as the password hash, is not returned.

**Actual Result:**

* The API returned the authenticated user's profile.
* The returned user ID and email corresponded to that account.
* Sensitive data, such as the password hash, wasn't returned.

**Status:** Pass

---

### USER-004 — User retrieves their profile without a JWT

**Priority:** Critical
**Type:** Negative / Security

**Steps:**

1. Send `GET /api/Utilisateur/me` without an access token.

**Expected Result:**

* The API returns `401 Unauthorized`.
* No user profile is returned.

**Actual Result:**

* The API returned `401 Unauthorized`.
* No user profile was returned.

**Status:** Pass

---

### USER-005 — Admin retrieves an existing user by ID

**Priority:** High
**Type:** Positive

**Preconditions:**

* An Admin account is authenticated.
* A user with the specified ID exists.

**Steps:**

1. Send `GET /api/Utilisateur/{id}` with the Admin JWT.
2. Replace `{id}` with the existing user's ID.

**Expected Result:**

* The API returns the requested user's information.
* The returned ID matches the requested ID.
* Sensitive information is excluded.

**Actual Result:**

* The API returned the requested user's information.
* The returned ID matched the requested ID.
* Sensitive information was excluded.

**Status:** Pass

---

### USER-006 — Retrieve a user with a nonexistent ID

**Priority:** Medium
**Type:** Negative / Validation

**Preconditions:**

* An authorized account is authenticated.
* The specified ID does not exist.

**Steps:**

1. Send `GET /api/Utilisateur/{id}` with a nonexistent ID.

**Expected Result:**

* The API returns `404 Not Found`, or the documented equivalent.
* No unrelated user's information is returned.

**Actual Result:**

* The API returned `404 Not Found`, or the documented equivalent.
* No unrelated user's information was returned.

**Status:** Pass

## 3.2 Create User

### USER-007 — Admin creates a user with valid information

**Priority:** High
**Type:** Positive

**Preconditions:**

* An Admin account is authenticated.
* The email address is not already used.

**Steps:**

1. Send `POST /api/Utilisateur` with valid user information.
2. Include the Admin JWT.

**Expected Result:**

* The API creates the user.
* The response indicates successful creation.
* The new user has the expected default role and account state.
* The password is not stored or returned in plain text.

**Actual Result:**

* The API created the user.
* The response indicated successful creation.
* The new user had the expected default role and account state.
* The password was not stored or returned in plain text.

**Status:** Pass

---

### USER-009 — Create a user with an existing email

**Priority:** High
**Type:** Negative / Validation

**Preconditions:**

* A user already exists with the email being submitted.
* The request is sent by an authorized account.

**Steps:**

1. Send `POST /api/Utilisateur`.
2. Use the existing email.

**Expected Result:**

* The API rejects the request.
* No duplicate account is created.
* The existing account remains unchanged.

**Actual Result:**

* The API rejected the request.
* No duplicate account was created.
* The existing account remained unchanged.

**Status:** Pass

## 3.3 Update User

### USER-010 — Update the authenticated user's own profile

**Priority:** High
**Type:** Positive

**Preconditions:**

* A user is authenticated.
* The update data is valid.

**Steps:**

1. Send `PUT /api/Utilisateur/me`.
2. Provide updated profile information.

**Expected Result:**

* The API updates only the authenticated user's permitted profile fields.
* The changes are persisted.
* The response does not expose sensitive information.

**Actual Result:**

* The API updated only the authenticated user's permitted profile fields.
* The changes persisted.
* The response didn't expose sensitive information.

**Status:** Pass

### USER-011 — Update a user with invalid data

**Priority:** High
**Type:** Negative / Validation

**Preconditions:**

* An authorized account is authenticated.
* A user exists to be updated.

**Steps:**

1. Send an update request with invalid values, such as an invalid email or missing required fields.

**Expected Result:**

* The API returns a validation error, commonly `400 Bad Request`.
* Invalid changes are not saved.
* Existing valid data remains unchanged.

**Actual Result:**

* The API returned a validation error, commonly `400 Bad Request`.
* Invalid changes  weren't saved.
* Existing valid data remained unchanged.

**Status:** Pass

## 3.4 Role Management

### USER-012 — Admin changes a user's role to a permitted role

**Priority:** Critical
**Type:** Positive / Authorization

**Preconditions:**

* An Admin account is authenticated.
* A target user exists.
* The requested role is allowed by the application.

**Steps:**

1. Send `PUT /api/Utilisateur/{id}/role`.
2. Provide the target user's ID and the permitted new role.
3. Include the Admin JWT.
4. Retrieve the user's information again to verify the change.

**Expected Result:**

* The API authorizes the role change.
* The user's role is updated and persisted.
* Subsequent authorization decisions use the updated role according to the application's token and session rules.

**Actual Result:**

* The API authorized the role change.
* The user's role was updated and persisted.
* Subsequent authorization decisions used the updated role according to the application's token and session rules.

**Status:** Pass

---

### USER-013 — Employe attempts to change a user's role

**Priority:** Critical
**Type:** Negative / Authorization

**Preconditions:**

* An Employe account is authenticated.
* A target user exists.

**Steps:**

1. Send `PUT /api/Utilisateur/{id}/role` with the Employe JWT.
2. Provide a valid role change request.

**Expected Result:**

* The API returns `403 Forbidden` or the documented authorization error.
* The user's role remains unchanged.

**Actual Result:**

* The API returned `403 Forbidden` or the documented authorization error.
* The user's role remained unchanged.

**Status:** Pass

---

### USER-014 — Attempt to assign an invalid role

**Priority:** High
**Type:** Negative / Validation

**Preconditions:**

* An Admin account is authenticated.
* A target user exists.

**Steps:**

1. Send `PUT /api/Utilisateur/{id}/role`.
2. Provide a role value that is not supported by the application.

**Expected Result:**

* The API rejects the request.
* The user's role remains unchanged.

**Actual Result:**

* The API rejected the request.
* The user's role remained unchanged.

**Status:** Pass

## 3.5 Delete User

### USER-015 — Admin attempts to delete an Admin account

**Priority:** Critical
**Type:** Negative / Business Rule

**Preconditions:**

* An Admin account exists.
* The application's rule forbids deleting an Admin account.

**Steps:**

1. Send `DELETE /api/Utilisateur/{id}` for the Admin account.

**Expected Result:**

* The API rejected the deletion.
* The Admin account remained available.

**Actual Result:**

* The API rejected the deletion.
* The Admin account remained available.

**Status:** Pass

---

### USER-016 — Admin deletes a permitted user account

**Priority:** High
**Type:** Positive / Business Rule

**Preconditions:**

* An Admin account is authenticated.
* A deletable user account exists.
* The target account is not protected by the Admin-account rule.

**Steps:**

1. Send `DELETE /api/Utilisateur/{id}` with the Admin JWT.
2. Retrieve the target user again.

**Expected Result:**

* The API performs the deletion or deactivation according to the application's implementation.
* The user cannot continue using the account if it has been deactivated.
* Related data remains consistent.

**Actual Result:**

* The API performed the deletion or deactivation according to the application's implementation.
* The user couldn't continue using the account if it has been deactivated.
* Related data remained consistent.

**Status:** Pass

---

### USER-017 — Attempt to delete a nonexistent user

**Priority:** Medium
**Type:** Negative / Validation

**Preconditions:**

* An Admin account is authenticated.
* The specified user ID does not exist.

**Steps:**

1. Send `DELETE /api/Utilisateur/{id}` using a nonexistent ID.

**Expected Result:**

* The API returns `404 Not Found`, or the documented equivalent.
* No other account is affected.

**Actual Result:**

* The API returned `404 Not Found`, or the documented equivalent.
* No other account was affected.

**Status:** Pass

## 3.6 User Security

### USER-018 — User tries to modify their own role through profile update

**Priority:** Critical
**Type:** Negative / Security

**Preconditions:**

* A Utilisateur account is authenticated.

**Steps:**

1. Send `PUT /api/Utilisateur/me`.
2. Include a `RoleUtilisateur` field set to `Admin`, even if the field is not supposed to be editable.
3. Retrieve the profile again.

**Expected Result:**

* The role field is ignored or the request is rejected.
* The user does not gain Admin privileges.
* The stored role remains unchanged.

**Actual Result:**

* The role field was ignored or the request is rejected.
* The user didn't  gain Admin privileges.
* The stored role remained unchanged.

**Status:** Pass

---

### USER-019 — Password hash is not exposed through user endpoints

**Priority:** Critical
**Type:** Security

**Preconditions:**

* An authorized account is authenticated.
* At least one user exists.

**Steps:**

1. Call `GET /api/Utilisateur`.
2. Call `GET /api/Utilisateur/me`.
3. Call `GET /api/Utilisateur/{id}` for an existing user.
4. Inspect all response bodies.

**Expected Result:**

* No response includes `MotDePasse`, a password hash, refresh-token secrets, or other sensitive authentication data.
* Only the fields intended for the API consumer are returned.

**Actual Result:**

* No response included `MotDePasse`, a password hash, refresh-token secrets, or other sensitive authentication data.
* Only the fields intended for the API consumer are returned.

**Status:** Pass
