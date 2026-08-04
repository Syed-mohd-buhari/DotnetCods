using CAM.BusinessManager.Entity;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using CAM.WebAPI.Helper;
using CAM.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager _userManager;
        private readonly TokenManager _tokenManager;
        protected readonly ILoggerManager _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserLoggingLevelManager _userLoggingLevelManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        int UserID = 0;
        string userLogLevels = "";
        private string _UserRole = string.Empty;
        private string _SessionID = string.Empty;
        private string _IPAddress = string.Empty;
        private string _RequestPath = string.Empty;
        private string _UserName = string.Empty;
        private string roles = string.Empty;
        private string _Method = string.Empty;
        private string _RequestPathNoParams = string.Empty;


        public UserController(ILoggerManager logger, UserManager userManager, TokenManager tokenManager,
            ICurrentUserService currentUserService, IUserLoggingLevelManager userLoggingLevelManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userManager = userManager;
            _tokenManager = tokenManager;
            _currentUserService = currentUserService;
            _userLoggingLevelManager = userLoggingLevelManager;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpGet(template: "Base64Decode")]
        public async Task<string> Base64Decode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Base64Encode")]
        public async Task<string> Base64Encode(string plainText)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);

                return System.Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GenerateToken")]
        public async Task<IActionResult> GenerateToken(string email, bool isAuthenicated)
        {
           
            if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
            {
                _SessionID = _httpContextAccessor.HttpContext.Session != null ? _httpContextAccessor.HttpContext.Session.Id : "";
                _IPAddress = _httpContextAccessor.HttpContext.Connection.LocalIpAddress != null ? _httpContextAccessor.HttpContext.Connection.LocalIpAddress.ToString() : "";
                _RequestPath = _httpContextAccessor.HttpContext.Request.Path.Value != null ? (_httpContextAccessor.HttpContext.Request.Path.Value.Any(char.IsDigit) ? CustomizeHttpRequestPath.CustomizeRequestPath(_httpContextAccessor.HttpContext.Request.Path.Value) : _httpContextAccessor.HttpContext.Request.Path.Value) : "";
                _UserName = _httpContextAccessor.HttpContext?.User?.FindFirstValue("Email") ?? "";
                userLogLevels = await _userLoggingLevelManager.GetCurrentLogLevel();
                _Method = _httpContextAccessor.HttpContext.Request.Method;
            }

            UserInfoModel userInfo;
            try
            {
                if (email == null || !isAuthenicated)
                {
                    if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{_UserName}][{roles}][{_SessionID}]" +
                        $"[{_IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                        $"[{"User Authentication failed"}]");

                        _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{_UserName}][{roles}][{_SessionID}]" +
                        $"[{_IPAddress}][{_Method} Response][{_RequestPathNoParams}]" +
                        $"[{"User Authentication failed"}]");
                    }


                    return BadRequest("Invalid client request");
                }
                else
                {
                    email = await Base64Decode(email);
                    UserID = _userManager.GetUserIdByMail(email);
                    userInfo = _userManager.GetUserInfo(email);
                    roles = userInfo.RolesName != null ? string.Join(", ", userInfo.RolesName) : "";
                    string _RequestPathNoParams = new String(_RequestPath.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
                    if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && userInfo.IsExist && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CurrentLogLevelConfig._CurrentLogLevel.Contains("Info") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Trace") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Debug"))
                        {
                            _logger.LogInfo($"[INFO][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                            $"[{_IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                                $"[successful login]" + "User : " + email + " try to logged in at : " + DateTime.Now.ToString());
                            _logger.LogInfo($"[INFO][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                            $"[{_IPAddress}][{_Method} Response][{_RequestPathNoParams}]" +
                                $"[successful login]" + "User : " + email + " try to logged in at : " + DateTime.Now.ToString());
                        }
                    }
                    if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && !userInfo.IsExist && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CurrentLogLevelConfig._CurrentLogLevel.Contains("Error") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Info") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Trace") ||
                            CurrentLogLevelConfig._CurrentLogLevel.Contains("Debug") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Warn"))
                        {
                            _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                            $"[{_IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                                $"[failed login]" + "User :  " + email + "tried to login at :  " + DateTime.Now.ToString() + "  does not exist");
                            _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                                $"[{_IPAddress}][{_Method} Response][{_RequestPathNoParams}]" +
                                    $"[failed login]");
                        }
                    }

                    if (userInfo.IsExist)
                    {
                        var tokenModel = JWTToken.GenerateJwtToken(userInfo);
                        if (tokenModel != null)
                        {
                            _tokenManager.AddToken(tokenModel.RefreshToken);
                            if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && CurrentLogLevelConfig._CurrentLogLevel.Contains("Info")
                                && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                            {

                                _logger.LogInfo($"[INFO][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                                $"[{_IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                                    $"[Successful Login User : {email} Logged In at : {DateTime.Now}]");
                                _logger.LogInfo($"[INFO][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                                $"[{_IPAddress}][{_Method} Response][{_RequestPathNoParams}]" +
                                    $"[Successful Login]");
                            }
                            return Ok(new { Token = tokenModel.Token, RefreshToken = tokenModel.RefreshToken, Period = tokenModel.ExpirationPeriod, ErrorMessage = string.Empty });
                        }
                        else
                        {
                            if ((CurrentLogLevelConfig._CurrentLogLevel.Contains("Error") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Trace") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Debug") ||
                                CurrentLogLevelConfig._CurrentLogLevel.Contains("Info") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Warn")
                                || CurrentLogLevelConfig._CurrentLogLevel.Contains("Error")) && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off")
                                && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                            {
                                _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                               $"[{_IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                                   $"[{"Failed to login please contact administrators " + email}]");
                                _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                                $"[{_IPAddress}][{_Method} Response][{_RequestPathNoParams}]" +
                                   $"[Failed to login]");

                            }
                            return Ok(new { Token = string.Empty, ErrorMessage = "Failed to login please contact administrators" });
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off")
                            && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                        {
                            if (CurrentLogLevelConfig._CurrentLogLevel.Contains("Error") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Trace") ||
                                CurrentLogLevelConfig._CurrentLogLevel.Contains("Debug") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Info") ||
                                CurrentLogLevelConfig._CurrentLogLevel.Contains("Warn") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Error"))
                            {
                                _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                                $"[{_IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                                   $"[{" Failed to login with user account =  " + email + "  --  " + userInfo.ErrorMessage}]");
                                _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                                $"[{_IPAddress}][{_Method} Response][{_RequestPathNoParams}]" +
                                   $"[Failed to login]");
                            }
                        }

                        var isremoved = GlobalDbMode.DbMode.Remove(email.ToLower());
                        return Ok(new { Token = string.Empty, ErrorMessage = userInfo.ErrorMessage });
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalDbMode.DbMode.Remove(email);
                var message = ex.InnerException == null
              ? string.Join("{0}{1}", "Message = " + ex.Message, "Stack Trace = " + ex.StackTrace)
              : string.Join("{0}{1}{2}", "Message = " + ex.Message, "Inner Exception= " + ex.InnerException.Message, "Stack Trace = " + ex.StackTrace);
                if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && CurrentLogLevelConfig._CurrentLogLevel.Contains("Error")
                    && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                {

                    _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                    $"[{_IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                       $"[{ex.Message}]");
                    _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{roles}][{_SessionID}]" +
                    $"[{_IPAddress}][{_Method} Response][{_RequestPathNoParams}]" +
                       $"[{ex.Message}]");
                }
                return BadRequest(message);
            }
        }

        [HttpGet(template: "Logout")]
        public async Task<IActionResult> Logout(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInfo($"[INFO][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{_SessionID}]" +
                    $"[{_IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                    $"[{$"User :{email} Logged out at :{DateTime.Now}"}]");
                    _logger.LogInfo($"[INFO][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{email}][{_SessionID}]" +
                    $"[{_IPAddress}][{_Method} Response][{_RequestPathNoParams}]" +
                       $"[{$"User :{email} Logged out at :{DateTime.Now} Successfully"}]");
                }

                HttpContext.Session.Clear();
                return Ok(new { Message = "The Current User Is logged out" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await VerifyToken(tokenRequest);

                    if (res == null)
                    {
                        return BadRequest(new AuthResult()
                        {
                            Errors = new List<string>() { "Invalid tokens" },
                            Success = false
                        });
                    }

                    return Ok(res);
                }

                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() { "Invalid payload" },
                    Success = false
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        private async Task<AuthResult> VerifyToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenVal = tokenRequest.Token.Replace("Bearer ", "");
                // This validation function will make sure that the token meets the validation parameters
                // and its an actual jwt token not just a random string
                var principal = jwtTokenHandler.ValidateToken(tokenVal, JwtTokenParameters.RetrieveTokenParamters(), out var validatedToken);

                // Now we need to check if the token has a valid security algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.Aes128KW, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Will get the time stamp in unix time
                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                // we convert the expiry date from seconds to the date
                var expDate = UnixTimeStampToDateTime(utcExpiryDate);

                var utcDate = DateTime.UtcNow;

                var x = (expDate - utcDate).TotalMinutes;
                if ((expDate - utcDate).TotalMinutes > 2)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "We cannot refresh this since the token has not expired" },
                        Success = false
                    };
                }

                // Check the token we got if its saved in the db
                var storedRefreshToken = _tokenManager.FindToken(tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "refresh token doesnt exist" },
                        Success = false
                    };
                }

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has expired, user needs to relogin" },
                        Success = false
                    };
                }

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been used" },
                        Success = false
                    };
                }

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "token has been revoked" },
                        Success = false
                    };
                }

                // we are getting here the jwt token id
                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                // check the id that the recieved token has against the id saved in the db
                if (storedRefreshToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Errors = new List<string>() { "the token doenst mateched the saved token" },
                        Success = false
                    };
                }

                storedRefreshToken.IsUsed = true;
                _tokenManager.UpdateToken(storedRefreshToken);


                var dbUser = _userManager.GetUserInfo(storedRefreshToken.UserId);
                var newToken = JWTToken.GenerateJwtToken(dbUser);
                if (newToken != null)
                {
                    _tokenManager.AddToken(newToken.RefreshToken);
                    return new AuthResult() { Token = newToken.Token, RefreshToken = newToken.RefreshToken, Success = true };
                }
                else
                {
                    return new AuthResult() { Success = false, Errors = new List<string>() { "Failed to generate token" } };
                }
            }
            catch (Exception ex)
            {
                return (new AuthResult()
                {
                    Errors = new List<string>() { "Invalid payload" },
                    Success = false
                });
            }
        }



        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
           
                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
                return dtDateTime;
            
            
        }
    }
}
