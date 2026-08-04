//using CAM.Contracts.DBModels;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Entities.Models;
using CAM.Identity;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class TokenManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public TokenManager(IEnumerable<IRepositoryWrapper> wrappers, IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public int AddToken(RefreshTokenModel model)
        {
            var token = new Refreshtoken()
            {
                Addeddate = model.AddedDate,
                Expirydate = model.ExpiryDate,
                Id = model.Id,
                Isrevoked = model.IsRevoked,
                Isused = model.IsUsed,
                Jwtid = model.JwtId,
                Token = model.Token,
                Userid = model.UserId,
            };
            _repositoryWrapper.TokenRepository.Create(token);
            _repositoryWrapper.SaveAsync();
            return token.Id;
        }

        public int UpdateToken(RefreshTokenModel model)
        {

            var currentToken = _repositoryWrapper.TokenRepository.FindByCondition(p => p.Id == model.Id).FirstOrDefault();
            if (currentToken != null)
            {
                currentToken = new Refreshtoken()
                {
                    Addeddate = model.AddedDate,
                    Expirydate = model.ExpiryDate,
                    Id = model.Id,
                    Isrevoked = model.IsRevoked,
                    Isused = model.IsUsed,
                    Jwtid = model.JwtId,
                    Token = model.Token,
                    Userid = model.UserId,
                };
                _repositoryWrapper.TokenRepository.Update(currentToken);
                _repositoryWrapper.SaveAsync();
            }
            return currentToken.Id;
        }

        public RefreshTokenModel FindToken(string refreshToken)
        {
            var token = _repositoryWrapper.TokenRepository.FindByCondition(p => p.Token == refreshToken).FirstOrDefault();
            return token != null
                ? new RefreshTokenModel()
                {
                    AddedDate = token.Addeddate.Value,
                    ExpiryDate = token.Expirydate.Value,
                    Id = token.Id,
                    IsRevoked = token.Isrevoked.Value,
                    IsUsed = token.Isused.Value,
                    JwtId = token.Jwtid,
                    Token = token.Token,
                    UserId = token.Userid,
                }
                : null;
        }
    }
}
