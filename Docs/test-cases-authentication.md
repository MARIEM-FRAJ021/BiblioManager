# 1. Authentication — Test Cases

## Purpose
Validate registration, email verification, login, token refresh, logout and password reset.

## Scope
- Endpoints: `/api/Auth/register`, `verify-email`, `resend-verification-email`, `login`, `refresh`, `logout`, `forgot-password`, `reset-password`
- Covers: input validation, token lifecycle (JWT, refresh token rotation), anti-enumeration, session security

## Prerequisites
- A test user with verified email and one with unverified email
- Access to the verification and reset emails (or tokens in the database)

## 1.1 Registration

### AUTH-001 — Register with valid information

**Priority:** High
**Type:** Positive

**Preconditions:**

* The email does not already exist.
* The registration data is valid.

**Steps:**

1. Send `POST /api/Auth/register`.
2. Provide a valid first name, last name, email, and password.

**Expected Result:**

* The API returns `200 OK`.
* The user account is created.
* The user's role is set to `Utilisateur`.
* The password is stored hashed.
* `EmailConfirmed` is set to `false`.
* A verification email is sent.

**Actual Result:**
- The API returned `200 OK`.
- The user account was created successfully.
- The user was assigned the `Utilisateur` role.
- The account email remains unverified.
- A verification email was sent.

**Status:** Pass

---

### AUTH-002 — Register with an existing email

**Priority:** High
**Type:** Negative / Validation

**Preconditions:**

* A user already exists with the provided email.

**Steps:**

1. Send `POST /api/Auth/register`.
2. Use the existing email.

**Expected Result:**
* The API returns `400 Bad Request`.
* The registration is rejected.
* No duplicate account is created.

**Actual Result:**
- The API returned `400 Bad Request`.
- The registration was rejected.
- No duplicate account was created.

**Status:** Pass

---

### AUTH-003 — Register with invalid data

**Priority:** High
**Type:** Negative / Validation

**Steps:**

1. Send `POST /api/Auth/register`.
2. Provide missing or invalid required fields.

**Expected Result:**

* The API returns `400 Bad Request`.
* Validation errors are returned.
* No account is created.

**Actual Result:**

* The API returned `400 Bad Request`.
* Validation errors were returned.
* No account was created.

**Status:** Pass

---

### AUTH-004 — Register with a weak or invalid password

**Priority:** Medium
**Type:** Negative / Validation

**Steps:**

1. Send `POST /api/Auth/register`.
2. Provide a password that does not satisfy the password validation rules.

**Expected Result:**

* The API returns `400 Bad Request`.
* No account is created.

**Actual Result:**
* The API returned `400 Bad Request`.
* No account was created.

**Status:** Pass

## 1.2 Email Verification

### AUTH-005 — Verify email with a valid token

**Priority:** High
**Type:** Positive

**Preconditions:**

* A registered user exists.
* The user's email is not verified.
* A valid verification token exists.

**Steps:**

1. Send `POST /api/Auth/verify-email`.
2. Provide the valid verification token.

**Expected Result:**

* The API returns `200 OK`.
* The user's email is marked as verified.
* The user can subsequently log in.

**Actual Result:**

* The API returned `200 OK`.
* The user's email was marked as verified.
* The user was subsequently able to log in.


**Status:** Pass

---

### AUTH-006 — Verify email with an invalid token

**Priority:** High
**Type:** Negative / Security

**Steps:**

1. Send `POST /api/Auth/verify-email`.
2. Provide an invalid or unknown token.

**Expected Result:**

* The request is rejected.
* The email is not verified.

**Actual Result:**

* The request was rejected.
* The email was not verified.

**Status:** Pass

---

### AUTH-007 — Verify email with an expired token

**Priority:** High
**Type:** Negative / Security

**Preconditions:**

* A verification token exists but has expired.

**Steps:**

1. Send `POST /api/Auth/verify-email`.
2. Provide the expired token.

**Expected Result:**

* The request is rejected.
* The email remains unverified.

**Actual Result**
* The email still unverified.
* The request was rejected.

**Status:** Pass

---

### AUTH-008 — Verify email with missing or invalid request data

**Priority:** Medium
**Type:** Negative / Validation

**Steps:**

1. Send `POST /api/Auth/verify-email`.
2. Omit the token or provide invalid request data.

**Expected Result:**

* The API returns `400 Bad Request`.
* No verification occurs.

**Actual Result**
* The API returned `400 Bad Request`.
* No verification occurred.

**Status:** Pass

## 1.3 Resend Verification Email

### AUTH-009 — Resend verification email for an existing unverified account

**Priority:** High
**Type:** Positive

**Preconditions:**

* The user exists.
* The user's email is not verified.

**Steps:**

1. Send `POST /api/Auth/resend-verification-email`.
2. Provide the user's email.

**Expected Result:**

* The API returns `200 OK`.
* A new verification email is sent.


**Actual Result**

*The API returned 200 OK.
A new verification email was sent.

**Status:** Pass

---

### AUTH-010 — Resend verification email for a non-existing email

**Priority:** Medium
**Type:** Negative / Security

**Steps:**

1. Send `POST /api/Auth/resend-verification-email`.
2. Provide an email that does not belong to an account.

**Expected Result:**

* The API does not expose whether the account exists.
* The request is handled according to the application's anti-enumeration policy.

**Actual Result:**

* The API didn't expose whether the account exists.
* The request was handled according to the application's anti-enumeration policy.

**Status:** Pass

---

### AUTH-011 — Resend verification email with invalid data

**Priority:** Medium
**Type:** Negative / Validation

**Steps:**

1. Send `POST /api/Auth/resend-verification-email`.
2. Provide an invalid or empty email.

**Expected Result:**

* The API returns `400 Bad Request`.
* No verification email is sent.

**Actual Result:**

* The API returned `400 Bad Request`.
* No verification email was sent.

**Status:** Pass

## 1.4 Login

### AUTH-012 — Login with valid verified credentials

**Priority:** Critical
**Type:** Positive

**Preconditions:**

* The user exists.
* The email is verified.
* The password is correct.

**Steps:**

1. Send `POST /api/Auth/login`.
2. Provide the user's email and password.

**Expected Result:**

* The API returns `200 OK`.
* A valid access token is returned.
* A refresh token is stored in an HttpOnly cookie.
* The refresh token is not returned in the response body.
* User information is returned.
* The user's role is returned.
* Access token expiration is returned.

**Expected Result:**

* The API returned `200 OK`.
* A valid access token was returned.
* A refresh token was stored in an HttpOnly cookie.
* The refresh token was not returned in the response body.
* User information was returned.
* The user's role was returned.
* Access token expiration was returned.

**Status:** Pass

---

### AUTH-013 — Login with incorrect password

**Priority:** Critical
**Type:** Negative / Security

**Preconditions:**

* The user exists.
* The email is verified.

**Steps:**

1. Send `POST /api/Auth/login`.
2. Provide the correct email and an incorrect password.

**Expected Result:**

* The API rejects the authentication attempt.
* No access token is issued.
* No valid authenticated session is created.

* **Actual Result:**

* The API rejected the authentication attempt.
* No access token was issued.
* No valid authenticated session was created.

**Status:** Pass

---

### AUTH-014 — Login with non-existing email

**Priority:** High
**Type:** Negative / Security

**Steps:**

1. Send `POST /api/Auth/login`.
2. Provide an email that does not exist.

**Expected Result:**

* The API rejects the authentication attempt.
* No access token is issued.

**Actual Result:**

* The API rejected the authentication attempt.
* No access token was issued.

**Status:** Pass

---

### AUTH-015 — Login with an unverified email

**Priority:** Critical
**Type:** Negative / Business Rule

**Preconditions:**

* The user exists.
* `EmailConfirmed = false`.

**Steps:**

1. Send `POST /api/Auth/login`.
2. Provide valid credentials.

**Expected Result:**

* The API rejects the login.
* No access token is issued.
* The user is informed that email verification is required.

* **Actual Result:**

* The API rejected the login.
* No access token was issued.
* The user was informed that email verification was required.

**Status:** Pass

---

### AUTH-016 — Login with invalid request data

**Priority:** Medium
**Type:** Negative / Validation

**Steps:**

1. Send `POST /api/Auth/login`.
2. Omit the email or password, or provide invalid values.

**Expected Result:**

* The request is rejected.
* No access token is issued.

**Actual Result:**

* The request was rejected.
* No access token was issued.

**Status:** Pass

## 1.5 Refresh Token

### AUTH-017 — Refresh with a valid refresh token

**Priority:** Critical
**Type:** Positive / Security

**Preconditions:**

* A valid refresh token exists.
* The refresh token is not revoked.
* The refresh token has not expired.
* The associated user exists.

**Steps:**

1. Send `POST /api/Auth/refresh`.
2. Include the valid refresh token cookie.

**Expected Result:**

* The API returns `200 OK`.
* A new access token is returned.
* A new refresh token is issued in the HttpOnly cookie.
* The previous refresh token is revoked.
* The new refresh token has a new expiration date.

**Actual Result:**

* The API returned `200 OK`.
* A new access token was returned.
* A new refresh token was issued in the HttpOnly cookie.
* The previous refresh token was revoked.
* The new refresh token had a new expiration date.

**Status:** Pass

---

### AUTH-018 — Refresh without a refresh token

**Priority:** Critical
**Type:** Negative / Security

**Steps:**

1. Send `POST /api/Auth/refresh` without the `refreshToken` cookie.

**Expected Result:**

* The request is rejected.
* No access token is issued.

**Actual Result:**

* The request was rejected.
* No access token was issued.

**Status:** Pass

---

### AUTH-019 — Refresh with an invalid refresh token

**Priority:** Critical
**Type:** Negative / Security

**Steps:**

1. Send `POST /api/Auth/refresh`.
2. Provide an unknown or invalid refresh token.

**Expected Result:**

* The request is rejected.
* No access token is issued.

**Actual Result:**

* The request was rejected.
* No access token was issued.

**Status:** Pass

---

### AUTH-020 — Refresh with a revoked refresh token

**Priority:** Critical
**Type:** Negative / Security

**Preconditions:**

* The refresh token exists.
* The refresh token has been revoked.

**Steps:**

1. Send `POST /api/Auth/refresh`.
2. Use the revoked refresh token.

**Expected Result:**

* The request is rejected.
* No new access token is issued.

**Actual Result:**

* The request was rejected.
* No new access token was issued.

**Status:** Pass

---

### AUTH-021 — Refresh with an expired refresh token

**Priority:** Critical
**Type:** Negative / Security

**Preconditions:**

* The refresh token exists.
* The expiration date has passed.

**Steps:**

1. Send `POST /api/Auth/refresh`.
2. Use the expired refresh token.

**Expected Result:**

* The request is rejected.
* No new access token is issued.

**Actual Result:**

* The request was rejected.
* No new access token was issued.

**Status:** Pass

---

### AUTH-022 — Refresh after user membership status changes

**Priority:** High
**Type:** Positive / Security

**Preconditions:**

* The user has an existing refresh token.
* The user's current database state differs from the state when the original access token was issued.

**Steps:**

1. Change the user's membership state in the database.
2. Use the refresh token.
3. Send `POST /api/Auth/refresh`.

**Expected Result:**

* A new access token is generated using the user's current database state.
* Current claims are reflected in the new access token.

**Actual Result:**

* A new access token was generated using the user's current database state.
* Current claims were reflected in the new access token.

**Status:** Pass

## 1.6 Logout

### AUTH-023 — Logout with a valid refresh token

**Priority:** High
**Type:** Positive / Security

**Preconditions:**

* A valid refresh token exists.

**Steps:**

1. Send `POST /api/Auth/logout`.
2. Include the refresh token cookie.

**Expected Result:**

* The API returns `200 OK`.
* The refresh token is revoked.
* The refresh token cookie is deleted.
* The revoked refresh token cannot be used again.

**Actual Result:**

* The API returned `200 OK`.
* The refresh token was revoked.
* The refresh token cookie was deleted.
* The revoked refresh token cannot be used again.

**Status:** Pass

---

### AUTH-024 — Logout without a refresh token

**Priority:** Medium
**Type:** Positive / Edge Case

**Steps:**

1. Send `POST /api/Auth/logout` without a refresh token cookie.

**Expected Result:**

* The API returns `200 OK`.
* The logout operation completes without an error.
* The refresh token cookie is deleted if present.

**Actual Result:**

* The API returned `200 OK`.
* The logout operation completed without an error.
* The refresh token cookie was deleted if present.

**Status:** Pass

---

### AUTH-025 — Logout with an invalid refresh token

**Priority:** Medium
**Type:** Edge Case / Security

**Steps:**

1. Send `POST /api/Auth/logout`.
2. Provide an invalid refresh token.

**Expected Result:**

* The API returns `200 OK`.
* No valid refresh token is revoked.
* The cookie is deleted.

**Actual Result:**

* The API returned`200 OK`.
* No valid refresh token was revoked.
* The cookie was deleted.

**Status:** Pass

## 1.7 Forgot Password

### AUTH-026 — Request password reset for an existing user

**Priority:** High
**Type:** Positive

**Preconditions:**

* The user exists.

**Steps:**

1. Send `POST /api/Auth/forgot-password`.
2. Provide the user's email.

**Expected Result:**

* The API returns `200 OK`.
* A password reset token is generated and stored hashed.
* A password reset email is sent.
* The response does not expose the reset token.

**Actual Result:**

* The API returns `200 OK`.
* A password reset token was generated and stored hashed.
* A password reset email was sent.
* The response didn't expose the reset token.


**Status:** Pass

---

### AUTH-027 — Request password reset for a non-existing email

**Priority:** High
**Type:** Security / Anti-enumeration

**Steps:**

1. Send `POST /api/Auth/forgot-password`.
2. Provide an email that does not exist.

**Expected Result:**

* The API returns the same generic response as for an existing account.
* The API does not reveal whether the email is registered.
* No reset token is created.

**Actual Result:**

* The API returned the same generic response as for an existing account.
* The API didn't not reveal whether the email was registered.
* No reset token was created.

**Status:** pass

---

### AUTH-028 — Forgot password with invalid email

**Priority:** Medium
**Type:** Negative / Validation

**Steps:**

1. Send `POST /api/Auth/forgot-password`.
2. Provide an invalid or empty email.

**Expected Result:**

* The API returns `400 Bad Request`.

**Actual Result:**

* The API returned `400 Bad Request`.

**Status:** Pass

## 1.8 Reset Password

### AUTH-029 — Reset password with a valid token

**Priority:** Critical
**Type:** Positive / Security

**Preconditions:**

* A valid password reset token exists.
* The token has not expired.
* The token has not been used.

**Steps:**

1. Send `POST /api/Auth/reset-password`.
2. Provide the valid token.
3. Provide a valid new password.

**Expected Result:**

* The API returns `200 OK`.
* The password is replaced by a hashed password.
* The reset token is marked as used.
* The old password can no longer be used to log in.
* The new password can be used to log in after email verification requirements are satisfied.

**Actual Result:**

* The API returned `200 OK`.
* The password was replaced by a hashed password.
* The reset token was marked as used.
* The old password could no longer be used to log in.
* The new password could be used to log in after email verification requirements are satisfied.

**Status:** Pass

---

### AUTH-030 — Reset password with an invalid token

**Priority:** Critical
**Type:** Negative / Security

**Steps:**

1. Send `POST /api/Auth/reset-password`.
2. Provide an invalid or unknown token.

**Expected Result:**

* The request is rejected.
* The password remains unchanged.

**Expected Result:**

* The request was rejected.
* The password remained unchanged.

**Status:** Pass

---

### AUTH-031 — Reset password with an expired token

**Priority:** Critical
**Type:** Negative / Security

**Preconditions:**

* The reset token has expired.

**Steps:**

1. Send `POST /api/Auth/reset-password`.
2. Provide the expired token and a new password.

**Expected Result:**

* The request is rejected.
* The password remains unchanged.

**Actual Result:**

* The request was rejected.
* The password remained unchanged.

**Status:** Pass

---

### AUTH-032 — Reset password with an already-used token

**Priority:** Critical
**Type:** Negative / Security

**Preconditions:**

* The reset token has already been used.

**Steps:**

1. Send `POST /api/Auth/reset-password`.
2. Reuse the same token.

**Expected Result:**

* The request is rejected.
* The password is not changed again.

**Actual Result:**

* The request was rejected.
* The password was not changed again.

**Status:** Pass

---

### AUTH-033 — Reset password with invalid new password

**Priority:** High
**Type:** Negative / Validation

**Steps:**

1. Send `POST /api/Auth/reset-password`.
2. Provide a valid token.
3. Provide an invalid or weak password.

**Expected Result:**

* The API returns `400 Bad Request`.
* The password remains unchanged.

**Actual Result:**

* The API returned `400 Bad Request`.
* The password remained unchanged.

**Status:** Pass

## 1.9 Authentication Security

### AUTH-034 — Access protected endpoint without access token

**Priority:** Critical
**Type:** Negative / Security

**Steps:**

1. Send a request to a protected endpoint without an access token.

**Expected Result:**

* The API returns `401 Unauthorized`.

**Actual Result:**

* The API returned `401 Unauthorized`.

**Status:** Pass

---

### AUTH-035 — Access protected endpoint with an invalid access token

**Priority:** Critical
**Type:** Negative / Security

**Steps:**

1. Send a request to a protected endpoint using an invalid or corrupted JWT.

**Expected Result:**

* The API returns `401 Unauthorized`.

**Actual Result:**

* The API returned `401 Unauthorized`.

**Status:** Pass

---

### AUTH-036 — Access protected endpoint with an expired access token

**Priority:** Critical
**Type:** Negative / Security

**Steps:**

1. Use an expired access token.
2. Send a request to a protected endpoint.

**Expected Result:**

* The API returns `401 Unauthorized`.

**Actual Result:**

* The API returned `401 Unauthorized`.

**Status:** Pass

---

### AUTH-037 — Refresh token rotation prevents reuse

**Priority:** Critical
**Type:** Negative / Security

**Preconditions:**

* A valid refresh token exists.

**Steps:**

1. Use the refresh token to obtain a new access token and refresh token.
2. Attempt to use the original refresh token again.

**Expected Result:**

* The second refresh attempt is rejected.
* The original refresh token cannot be reused.

**Actual Result:**

* The second refresh attempt was rejected.
* The original refresh token couldn't be reused.

**Status:** Pass
### AUTH-038 — Refresh after password reset

**Priority:** High  
**Type:** Security / Regression

**Preconditions:**
- The user has a valid refresh token.
- The user resets their password successfully.

**Steps:**
1. Reset the user's password.
2. Use the refresh token issued before the password reset.
3. Send `POST /api/Auth/refresh`.

**Expected Result:**
- The API returns `401 Unauthorized`.
- No new access token is generated.
- The refresh token can no longer be used.

**Actual Result:**
- The API returned `401 Unauthorized`.
- No new access token was generated.
- The refresh token could no longer be used.

**Status:** Pass

