using Moq;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Services;
using BiblioManager.API.Dtos.Auth;
using BiblioManager.API.Models;
using Org.BouncyCastle.Crypto;

namespace BiblioManager.UnitTests;

public class AuthTest
{
    private readonly Mock<IUtilisateurRepository> _utilisateurRepositoryMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IEmailVerificationService> _emailVerifServiceMock;

    private readonly IAuthsService _authService;
    public AuthTest()
    {
        _utilisateurRepositoryMock = new Mock<IUtilisateurRepository>();
        _jwtServiceMock = new Mock<IJwtService>();
        _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _emailVerifServiceMock = new Mock<IEmailVerificationService>();
        _authService = new AuthService(
            _utilisateurRepositoryMock.Object,
            _jwtServiceMock.Object,
            _refreshTokenServiceMock.Object,
            _refreshTokenRepositoryMock.Object,
            _emailVerifServiceMock.Object
        );

    }

    // --------------------------------------------------
    // LOGIN TESTS
    // --------------------------------------------------
    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserDoesNotExist()
    {
        var request = new LoginRequest
        {
            Email = "unkown@example.com",
            Password = "Password123!"
        };

        _utilisateurRepositoryMock.Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((Utilisateur)null);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _authService.LoginAsync(request));

        Assert.Equal("Credentials invalides", exception.Message);

        _jwtServiceMock.Verify(
            s => s.GenerateToken(It.IsAny<Utilisateur>()), Times.Never);

        _refreshTokenRepositoryMock.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Never);
    }
    [Fact]
    public async Task LoginAsync_ShouldReturnAuthResponse_WhenCredentialsAreValid()
    {
        var user = new Utilisateur
        {
            IdUtilisateur = 3,
            Nom = "FRAJ",
            Prenom = "Mariem",
            Email = "test@example.com",
            MotDePasse = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            EmailConfirmed = true,
            RoleUtilisateur = RoleUtilisateurEnum.Utilisateur
        };

        _utilisateurRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);

        _jwtServiceMock.Setup(s => s.GenerateToken(user))
            .Returns("fake-access-token");

        _refreshTokenServiceMock.Setup(s => s.GenerateRefreshToken())
            .Returns("fake-refresh-token");

        var request = new LoginRequest
        {
            Email = user.Email,
            Password = "Password123!"
        };

        var result = await _authService.LoginAsync(request);

        Assert.NotNull(result);

        Assert.Equal("fake-access-token", result.AccessToken);
        Assert.Equal("fake-refresh-token", result.RefreshToken);
        Assert.Equal(user.IdUtilisateur, result.UserId);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.RoleUtilisateur.ToString(), result.Role);

        _jwtServiceMock.Verify(s => s.GenerateToken(user), Times.Once);

        _refreshTokenRepositoryMock.Verify(
            r => r.AddAsync(It.Is<RefreshToken>(
                t => t.IdUtilisateur == user.IdUtilisateur && string.IsNullOrWhiteSpace(t.TokenHash))), Times.Once);

        _refreshTokenRepositoryMock.Verify(
            r => r.SaveChangesAsync(), Times.Once
            );
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordIncorrect()
    {
        var user = new Utilisateur
        {
            IdUtilisateur = 1,
            Nom = "Test",
            Prenom = "User",
            Email = "test@example.com",
            MotDePasse = BCrypt.Net.BCrypt.HashPassword("CorrectPassword123!"),
            EmailConfirmed = true,
            RoleUtilisateur = RoleUtilisateurEnum.Utilisateur
        };

        _utilisateurRepositoryMock.
            Setup(r => r.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);

        var request = new LoginRequest
        {
            Email = user.Email,
            Password = "WrongPassword123!"
        };


        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(request));

        Assert.Equal("Credentials invalides", exception.Message);

        _jwtServiceMock.Verify(s => s.GenerateToken(It.IsAny<Utilisateur>()), Times.Never);

        _refreshTokenRepositoryMock.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenEmailIsNotConfirmed()
    {
        var user = new Utilisateur
        {
            IdUtilisateur = 2,
            Nom = "Test",
            Prenom = "User",
            Email = "Unverified@example.com",
            MotDePasse = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            EmailConfirmed = false,
            RoleUtilisateur = RoleUtilisateurEnum.Utilisateur
        };

        _utilisateurRepositoryMock.Setup(r => r.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);

        var request = new LoginRequest
        {
            Email = user.Email,
            Password = "Password123!"
        };

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(request));

        Assert.Equal("Veuillez verifier votre adresse mail.", exception.Message);

        _jwtServiceMock.Verify(s => s.GenerateToken(It.IsAny<Utilisateur>()), Times.Never);

        _refreshTokenRepositoryMock.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Never);
    }

    // --------------------------------------------------
    // REGISTER TESTS
    // --------------------------------------------------

    [Fact]
    public async Task RegisterAsync_ShouldCreateUserWithCorrectRoleAndHashedPassword()
    {
        var request = new RegisterDto
        {
            Nom = "Test",
            Prenom = "User",
            Email = "newuser@example.com",
            MotDePasse = "Password123!"
        };

        Utilisateur? createdUser = null;

        _utilisateurRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Utilisateur>()))
        .Callback<Utilisateur>(user => createdUser = user)
        .ReturnsAsync(new Utilisateur
        {
            IdUtilisateur = 1,
            Nom = "Test",
            Prenom = "User",
            Email = "newuser@example.com"
        });

        _emailVerifServiceMock.Setup(s => s.SendVerificationEmailAsync(It.IsAny<Utilisateur>()))
            .Returns(Task.CompletedTask);

        await _authService.RegisterAsync(request);

        Assert.NotNull(createdUser);

        Assert.Equal(request.Nom, createdUser.Nom);
        Assert.Equal(request.Prenom, createdUser.Prenom);
        Assert.Equal(request.Email, createdUser.Email);
        Assert.Equal(RoleUtilisateurEnum.Utilisateur, createdUser.RoleUtilisateur);
        Assert.False(createdUser.EmailConfirmed);
        Assert.NotEqual(request.MotDePasse, createdUser.MotDePasse);
        Assert.True(BCrypt.Net.BCrypt.Verify(request.MotDePasse, createdUser.MotDePasse));

        _utilisateurRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Utilisateur>()), Times.Once);
        _emailVerifServiceMock.Verify(s => s.SendVerificationEmailAsync(It.Is<Utilisateur>(u => u.Email == request.Email)), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldNotSendEmail_WhenCreationFails()
    {
        var request = new RegisterDto
        {
            Nom = "Test",
            Prenom = "User",
            Email = "newuser@example.com",
            MotDePasse = "Password123!"
        };

        _utilisateurRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Utilisateur>()))
            .ThrowsAsync(new Exception("Database error"));

        var exception = await Assert.ThrowsAsync<Exception>(
            () => _authService.RegisterAsync(request));

        Assert.Equal("Database error", exception.Message);

        _emailVerifServiceMock.Verify(
            s=> s.SendVerificationEmailAsync(It.IsAny<Utilisateur>()), Times.Never);
    }

    // --------------------------------------------------
    // LOGOUT TESTS
    // --------------------------------------------------

    [Fact]
    public async Task LogoutAsync_ShouldDoNothing_WhenTokenDoesNotExist()
    {
        var refreshToken = "unknown-refresh-token";
        var tokenHash = "unknown-token-hash";

        _refreshTokenServiceMock.Setup(s=> s.HashRefreshToken(refreshToken))
            .Returns(tokenHash);

        _refreshTokenRepositoryMock.Setup(r => r.GetByHashAsync(tokenHash))
            .ReturnsAsync((RefreshToken)null!);

        await _authService.LogoutAsync(refreshToken);

        _refreshTokenRepositoryMock.Verify(r=> r.SaveChangesAsync(), Times.Never);
    }

    [Fact]

    public async Task LogoutAsync_ShouldRevokeToken_WhenTokenExists()
    {
        var refreshToken = "valid-refresh-token";
        var tokenHash = "hashed-refresh-token";

        var storedToken = new RefreshToken
        {
            IdUtilisateur = 1,
            TokenHash = tokenHash,
            DateCreation = DateTime.UtcNow,
            DateExpiration = DateTime.UtcNow.AddDays(7),
            DateRevocation = null
        };

        _refreshTokenServiceMock.Setup(s => s.HashRefreshToken(refreshToken)).Returns(tokenHash);
        _refreshTokenRepositoryMock.Setup(r => r.GetByHashAsync(tokenHash)).ReturnsAsync(storedToken);

        await _authService.LogoutAsync(refreshToken);
        Assert.NotNull(storedToken.DateRevocation);

        _refreshTokenRepositoryMock.Verify(r=> r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LogoutAsync_ShouldNotSaveAgain_WhenTokenIsAlreadyRevoked()
    {
        var refreshToken = "revoked-refresh-tokn";
        var tokenHash = "hashed-revoked-token";
        var revokedAt = DateTime.UtcNow.AddMinutes(-10);

        var storedToken = new RefreshToken
        {
            IdUtilisateur = 1,
            TokenHash = tokenHash,
            DateCreation = DateTime.UtcNow.AddDays(-1),
            DateRevocation = revokedAt,
            DateExpiration = DateTime.UtcNow.AddDays(6),
        };

        _refreshTokenServiceMock.Setup(s => s.HashRefreshToken(refreshToken))
            .Returns(tokenHash);

        _refreshTokenRepositoryMock.Setup(r=> r.GetByHashAsync(tokenHash))
            .ReturnsAsync(storedToken);

        await _authService.LogoutAsync(refreshToken);

        Assert.Equal(revokedAt, storedToken.DateRevocation);

        _refreshTokenRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
