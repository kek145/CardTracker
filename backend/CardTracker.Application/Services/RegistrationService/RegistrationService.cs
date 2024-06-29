using MediatR;
using System.Net;
using FluentValidation;
using System.Threading.Tasks;
using CardTracker.Domain.Responses;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Registration;
using CardTracker.Application.Commands.UserCommands.AddUser;

namespace CardTracker.Application.Services.RegistrationService;

public class RegistrationService(IMediator mediator, IValidator<RegistrationRequest> validator) : IRegistrationService
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<RegistrationRequest> _validator = validator;
    public async Task<IBaseResponse<int>> RegistrationUserAsync(RegistrationRequest request)
    {
        var validation = await _validator.ValidateAsync(request);

        if (!validation.IsValid)
            return new BaseResponse<int>().Failure($"{validation}", HttpStatusCode.BadRequest);

        if (request.Password != request.ConfirmPassword)
            return new BaseResponse<int>().Failure("Password mismatch", HttpStatusCode.BadRequest);

        var command = new AddUserCommand(request);

        var result = await _mediator.Send(command);
        
        return result.Data <= 0 ? 
            new BaseResponse<int>().Failure($"{result.ErrorMessage}", HttpStatusCode.BadRequest) :
            new BaseResponse<int>().Success("Success", HttpStatusCode.Created, result.Data);
    }
}