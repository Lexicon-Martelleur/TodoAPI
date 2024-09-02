using AutoMapper;
using TodoAPI.Entities;
using TodoAPI.Models.DTO;
using TodoAPI.Models.Repositories;

namespace TodoAPI.Models.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationRepository _repository;
    private readonly IMapper _mapper;
    private readonly ISecurityService _securityService;

    public AuthenticationService (
        IAuthenticationRepository repository,
        IMapper mapper,
        ISecurityService securityService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
    }

    public async Task<TokenDTO?> AuthenticateByPassword(
        UserAuthenticationDTO userAuthenticationDTO)
    {
        var userAuthentication = _mapper.Map<UserAuthenticationEntity>(userAuthenticationDTO);

        var storedUser = await _repository.GetUserByEmail(userAuthentication.Email);
        if (storedUser == null) { return null; }

        var isValidPassword = _securityService.VerifyPassword(
            storedUser,
            storedUser.Password,
            userAuthentication.Password
        );

        if (!isValidPassword) { return null; }
        
        var token = _securityService.GenerateToken(userAuthentication);
        if (token == null) { return null; }
        
        return new TokenDTO() { Token = token };
    }
}
