using MediatR;
using System.Net;
using FluentValidation;
using System.Threading.Tasks;
using CardTracker.Domain.Responses.Common;
using CardTracker.Domain.Requests.Registration;
using CardTracker.Application.Commands.UserCommands.AddUser;

namespace CardTracker.Application.Services.RegistrationService;

public class RegistrationService(IMediator mediator, IValidator<RegistrationRequest> validator) : IRegistrationService
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<RegistrationRequest> _validator = validator;
    public async Task<BaseResponse<int>> RegistrationUserAsync(RegistrationRequest request)
    {
        var validation = await _validator.ValidateAsync(request);
        
        if (!validation.IsValid)
            return BaseResponse<int>.Failure($"Error", HttpStatusCode.BadRequest, [$"{validation}"]);

        if (request.Password != request.ConfirmPassword)
            return BaseResponse<int>.Failure("Error", HttpStatusCode.BadRequest, ["Password mismatch!"]);

        var command = new AddUserCommand(request);

        var result = await _mediator.Send(command);
        
        return result.Data <= 0 ? 
            BaseResponse<int>.Failure("Success", HttpStatusCode.BadRequest, [$"{result.ErrorMessage}"]) :
            BaseResponse<int>.Success("Success", HttpStatusCode.Created, result.Data);
    }
}