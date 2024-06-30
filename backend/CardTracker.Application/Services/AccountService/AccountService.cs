using System;
using MediatR;
using System.Net;
using System.Threading.Tasks;
using CardTracker.Domain.Responses;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Account;
using CardTracker.Application.Queries.UserQueries.GetUserById;
using CardTracker.Application.Commands.UserCommands.UpdateUser;
using CardTracker.Application.Commands.UserCommands.ResetPassword;
using CardTracker.Application.Commands.UserCommands.ForgotPassword;
using CardTracker.Application.Queries.UserQueries.GetUserVerificationToken;

namespace CardTracker.Application.Services.AccountService;

public class AccountService(IMediator mediator) : IAccountService
{
    private readonly IMediator _mediator = mediator;

    public async Task<IBaseResponse<string>> ResetPasswordAsync(int userId, ResetPasswordRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            return new BaseResponse<string>().Failure("Password mismatch!", HttpStatusCode.BadRequest);

        var query = new GetUserByIdQuery(userId);

        var user = await _mediator.Send(query);

        if (!user.IsSuccess)
            return new BaseResponse<string>().Failure(user.ErrorMessage, HttpStatusCode.NotFound);

        if (user.Data.ResetTokenExpiry < DateTime.UtcNow)
            return new BaseResponse<string>().Failure("Token expires!", HttpStatusCode.BadRequest);

        if (user.Data.ResetToken == null)
            return new BaseResponse<string>().Failure("Token is null", HttpStatusCode.NotFound);
        
        var command = new ResetPasswordCommand(userId, request.Password);

        var result = await _mediator.Send(command);

        return !result.IsSuccess 
            ? new BaseResponse<string>().Failure(result.ErrorMessage, HttpStatusCode.NotFound) 
            : new BaseResponse<string>().Success("Password reset successfully", HttpStatusCode.OK, "");
    }

    public async Task<IBaseResponse<string>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var command = new ForgotPasswordCommand(request);

        var result = await _mediator.Send(command);

        return !result.IsSuccess 
            ? new BaseResponse<string>().Failure(result.ErrorMessage, HttpStatusCode.NotFound) 
            : new BaseResponse<string>().Success("You may now reset your password", HttpStatusCode.OK, "");
    }

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