using MediatR;
using System.Net;
using FluentValidation;
using System.Threading.Tasks;
using CardTracker.Domain.Requests.Account;
using CardTracker.Domain.Responses.Common;
using CardTracker.Application.Commands.UserCommands.UpdateUser;
using CardTracker.Application.Commands.UserCommands.ResetPassword;
using CardTracker.Application.Queries.UserQueries.GetByResetToken;
using CardTracker.Application.Commands.UserCommands.ForgotPassword;
using CardTracker.Application.Queries.UserQueries.GetUserVerificationToken;

namespace CardTracker.Application.Services.AccountService;

public class AccountService(IMediator mediator, IValidator<ResetPasswordRequest> validator) : IAccountService
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<ResetPasswordRequest> _validator = validator;

    public async Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var validation = await _validator.ValidateAsync(request);

        if (!validation.IsValid)
            return BaseResponse.Failure("Error", HttpStatusCode.BadRequest, [$"{validation}"]);
        
        if (request.Password != request.ConfirmPassword)
            return BaseResponse.Failure("Error", HttpStatusCode.BadRequest, ["Password mismatch!"]);

        var query = new GetUserByResetTokenQuery(request.ResetToken);
        var user = await _mediator.Send(query);

        if (!user.IsSuccess || user.Data is null)
            return BaseResponse.Failure("Error", HttpStatusCode.NotFound, [user.ErrorMessage]);
        
        var command = new ResetPasswordCommand(user.Data.UserId, request.Password);
        var result = await _mediator.Send(command);

        return !result.IsSuccess 
            ? BaseResponse.Failure("Error", HttpStatusCode.NotFound, [result.ErrorMessage]) 
            : BaseResponse.Success("Success", HttpStatusCode.OK);
    }

    public async Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var command = new ForgotPasswordCommand(request);

        var result = await _mediator.Send(command);

        return !result.IsSuccess 
            ? BaseResponse.Failure("Error", HttpStatusCode.NotFound, [result.ErrorMessage]) 
            : BaseResponse.Success("Success", HttpStatusCode.OK);
    }

    public async Task<BaseResponse> VerifyAccountAsync(VerificationTokenRequest request)
    {
        var token = new GetUserVerificationTokenQuery(request);

        var dataToken = await _mediator.Send(token);

        if (!dataToken.IsSuccess || dataToken.Data is null)
            return BaseResponse.Failure("Error", HttpStatusCode.BadRequest, [dataToken.ErrorMessage]);
        
        var command = new UpdateVerificationStatusCommand(dataToken.Data.UserId);

        var result = await _mediator.Send(command);

        return !result.IsSuccess 
            ? BaseResponse.Failure("Error", HttpStatusCode.BadRequest, [result.ErrorMessage]) 
            : BaseResponse.Success("Success", HttpStatusCode.OK);
    }
}