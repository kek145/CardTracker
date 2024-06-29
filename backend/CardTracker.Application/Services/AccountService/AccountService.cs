using System.Net;
using MediatR;
using System.Threading.Tasks;
using CardTracker.Application.Commands.UserCommands.UpdateUser;
using CardTracker.Application.Queries.UserQueries.GetUserVerificationToken;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Account;
using CardTracker.Domain.Responses;

namespace CardTracker.Application.Services.AccountService;

public class AccountService(IMediator mediator) : IAccountService
{
    private readonly IMediator _mediator = mediator;
    
    public async Task<IBaseResponse<string>> VerifyAccountAsync(VerificationTokenRequest request)
    {
        var token = new GetUserVerificationTokenQuery(request);

        var dataToken = await _mediator.Send(token);

        if (!dataToken.IsSuccess)
            return new BaseResponse<string>().Failure(dataToken.ErrorMessage, HttpStatusCode.BadRequest);

        var command = new UpdateVerificationStatusCommand(dataToken.Data.UserId);

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return new BaseResponse<string>().Failure(result.ErrorMessage, HttpStatusCode.BadRequest);

        const string message = "Success";
        
        return new BaseResponse<string>().Success(message, HttpStatusCode.OK, data: "");
    }
}