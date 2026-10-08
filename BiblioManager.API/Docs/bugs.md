### BUG-AUTH-001 — Refresh token remains valid after password reset

**Severity:** High  
**Priority:** High  
**Status:** Fixed  
**Type:** Security

**Description:**

After resetting the user's password, previously issued refresh tokens remained valid and could still be used to generate new access tokens.

**Steps to Reproduce:**

1. Log in with a valid account.
2. Store the refresh token.
3. Reset the user's password.
4. Use the previously issued refresh token.
5. Send `POST /api/Auth/refresh`.

**Expected Result:**

- The API returns `401 Unauthorized`.
- The old refresh token is rejected.
- No new access token is generated.

**Actual Result:**

- The API generated a new access token using the old refresh token.

**Root Cause:**

Refresh tokens were not revoked after a successful password reset.

**Fix:**

All active refresh tokens associated with the user are revoked after a successful password reset.

**Regression Test:**

- `AUTH-038 — Refresh after password reset`
