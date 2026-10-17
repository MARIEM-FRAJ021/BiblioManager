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

### BUG-USER-016 — Admin can modify their own role

**Severity:** High  
**Priority:** High  
**Status:** Fixed  
**Type:** Security / Authorization

**Description:**

An authenticated Admin could modify their own role to `Employe` through the `PUT /api/Utilisateur/{id}/role` endpoint.

**Steps to Reproduce:**

1. Log in with an Admin account.
2. Retrieve the Admin's user ID.
3. Send a `PUT` request to `PUT /api/Utilisateur/{id}/role`, using the Admin's own ID.
4. Set the requested role to `Employe` in the request body.
5. Execute the request.

**Expected Result:**

- The API returns `403 Forbidden`.
- The request is rejected.
- The Admin's role remains unchanged.

**Actual Result:**

- The API accepted the request.
- The Admin's role was changed to `Employe`.

**Root Cause:**

The role modification endpoint did not prevent an Admin from modifying their own role.

**Fix:**

A server-side validation was added to compare the authenticated user's ID with the target user's ID. The API rejects the request if an Admin attempts to modify their own role.

**Regression Test:**

- `AUTHZ-009 — Admin attempts to modify their own role`

### BUG-USER-016 — User deletion fails when refresh tokens exist

**Severity:** Medium  
**Priority:** Medium  
**Status:** Fixed
**Type:** Database / Referential Integrity

**Description:**

Deleting a user failed when one or more refresh tokens were associated with their account. The foreign key constraint `FK_RefreshTokens_Utilisateurs_IdUtilisateur` prevented the deletion because the related records still existed in the `RefreshTokens` table.

**Steps to Reproduce:**

1. Log in or create a user account.
2. Generate and store at least one refresh token for that user.
3. Attempt to delete the user through the API or directly from SQL Server.
4. Execute the deletion request.

**Expected Result:**

- The user is deleted successfully if physical deletion is permitted.
- The associated refresh tokens are automatically deleted.
- No foreign key constraint violation occurs.

**Actual Result:**

- The deletion operation failed.
- SQL Server raised a `SqlException` because the `RefreshTokens` table still contained records referencing the user.
- The foreign key constraint prevented the deletion.

**Root Cause:**

The relationship between `Utilisateur` and `RefreshToken` was configured with `DeleteBehavior.Restrict`, preventing the deletion of a user while associated refresh tokens existed.

**Fix:**

The relationship was updated to use `DeleteBehavior.Cascade`, allowing the automatic deletion of associated refresh tokens when a user is physically deleted. An EF Core migration was created and applied to update the database foreign key constraint.

**Regression Test:**

- `AUTH-XXX — Delete user with associated refresh tokens`

