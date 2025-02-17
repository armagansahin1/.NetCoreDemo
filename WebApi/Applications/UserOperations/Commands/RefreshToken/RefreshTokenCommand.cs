using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using WebApi.DbOperations;
using WebApi.TokenOperations;
using WebApi.TokenOperations.Models;

namespace WebApi.Applications.UserOperations.Commands.RefreshToken
{
    public class RefreshTokenCommand
    {
        private readonly IBookStoreDbContext _dbContext;

        public string RefreshToken { get; set; }
        private readonly IConfiguration _configuration;
        public RefreshTokenCommand(IBookStoreDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;

        }

        public Token Handle()
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.RefreshToken == RefreshToken &&
             x.RefreshTokenExpireDate > DateTime.Now);

            if (user is not null)
            {
                TokenHandler tokenHandler = new TokenHandler(_configuration);
                Token token = tokenHandler.CreateAccessToken(user);

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpireDate = token.Expiration.AddMinutes(5);
                _dbContext.SaveChanges();
                return token;
            }
            else
            {
                throw new InvalidOperationException("Valid bir Refresh Token bulunamadı");
            }
        }
    }
}