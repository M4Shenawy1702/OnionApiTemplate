using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnionApiTemplate.Application.DOTs.Auth;
using OnionApiTemplate.Application.IServices;
using OnionApiTemplate.Domain.Entities;
using OnionApiTemplate.Domain.Exceptions;
using OnionApiTemplate.Domain.Setting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiTemplate.Application.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwtOptions, IValidator<LoginRequest> loginValidator, IMapper mapper , IValidator<RegisterRequest> registerValidator)
        : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly JWT _jwtOptions = jwtOptions.Value;
        private readonly IValidator<LoginRequest> _loginValidator = loginValidator;
        private readonly IValidator<RegisterRequest> _registerValidator = registerValidator;
        private readonly IMapper _mapper = mapper;

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new InvalidCredentialsException();

            return new AuthResponse(user.Email!, user.UserName!, await CreateJWTTokenAsync(user));
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            if (await _userManager.FindByEmailAsync(request.Email) is not null)
                throw new UserAlreadyExistsException("Email already exists");

            if (await _userManager.FindByNameAsync(request.UserName) is not null)
                throw new UserAlreadyExistsException("Username already exists");

            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.Select(e => e.Description).ToList());

            return new AuthResponse(user.Email!, user.UserName!, await CreateJWTTokenAsync(user));
        }
        private async Task<string> CreateJWTTokenAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }
            .Union(userClaims)
            .Union(userRoles.Select(role => new Claim("role", role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key!));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.DurationInMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
