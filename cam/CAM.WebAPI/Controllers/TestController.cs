using CAM.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CAM.BusinessManager.Entity;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Repository;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly DesignComponentFamilyManager _manager;
        private readonly DesignComponentManager designComponentManager;
        private readonly SystemTypeManager _systemTypeManager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        public TestController(ICurrentUserService currentUser, ILoggerManager logger, IHttpContextAccessor contextAccessor, DesignComponentFamilyManager manager, DesignComponentManager designComponentManager, IRepositoryWrapper repositoryWrapper) : base(logger, contextAccessor)
        {
            _currentUser = currentUser;
            _manager = manager;
            this.designComponentManager = designComponentManager;
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var tra = await _repositoryWrapper.BeginTransactionAsync();
            try
            {
                
                await _manager.GenerateAssociationForDcf();
                await designComponentManager.Rebuild();
                await tra.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                await tra.RollbackAsync();
                throw;
            }
        }
        [HttpGet("SystemTypeNameOem")]
        public async Task<IActionResult> Calcola()
        {
            try
            {
                await _systemTypeManager.RefreshSystemTypeNameOem();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }




    }
}
