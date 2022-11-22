using Backend.Infra;
using Backend.Model;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("/api")]
    public class AccountController : ControllerBase
    {
        private IIdentityService _identityService;
        private IAccountService _accountService;
        private ILogRequestService _logRequestService;

        public AccountController(IIdentityService identityService, IAccountService accountService, ILogRequestService logRequestService)
        {
            _identityService = identityService;
            _accountService = accountService;
            _logRequestService = logRequestService;
        }

        [HttpGet]
        [Route("account/{accountId}")]
        public async Task<IActionResult> GetAccount([FromRoute] string accountId)
        {
            var milliseconds = await _logRequestService.AddOneRequest();
            var account = await _accountService.GetAccountByIdUnsecure(accountId);

            if (account == null)
                return NotFound(new ErrorModel { Message = "Account not found" });

            var response = new BankAccountResponse(account, milliseconds);
            return Ok(response);
        }

        [HttpPost]
        [Route("schedule-transfers/")]
        public async Task<IActionResult> ScheduleTransfers([FromBody] List<Transfer> transfers)
        {
            await _logRequestService.AddOneRequest();

            //if (transfers.Count > 10) { return BadRequest(new ErrorModel { Message = "Maximum 10 bank transfers allowed" });}

            var responses = new List<TransferResponse>();
            foreach (var transfer in transfers)
            {
                var transferResult = await _accountService.TransferMoney(transfer);
                var response = new TransferResponse(transferResult);
                responses.Add(response);
            }

            return Ok(responses);
        }

        [HttpPost]
        [Route("withdraw/")]
        public async Task<IActionResult> WithdrawFromAccount([FromBody] Withdrawal withdrawal)
        {
            var milliseconds = await _logRequestService.AddOneRequest();
            var account = await _accountService.WithdrawMoney(null, withdrawal);

            if (account == null)
                return NotFound(new ErrorModel { Message = "Sorry, you cannot withdraw money" });

            var response = new BankAccountResponse(account, milliseconds);
            return Ok(response);
        }

        [HttpPost]
        [Route("audit-account/{accountId}")]
        [Authorize]
        public async Task<IActionResult> AuditAccount([FromRoute] string accountId)
        {
            await _logRequestService.AddOneRequest();
            var user = await GetUser();
            var isAudited = await _accountService.AuditAccountUnsecure(accountId);

            if (!isAudited)
                return NotFound(new ErrorModel { Message = "Account could not be audited" });

            var response = new AuditResponse($"Account {accountId} has been succesfully audited");
            return Ok(response);
        }

        private async Task<UserModel> GetUser()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = await _identityService.GetUserAsync(userName);

            return user;
        }
    }
}