using ImageEditor.Business.Interfaces;
using ImageEditor.Business.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ImageEditor.Api.Controllers;
[ApiController]
public class BaseController : ControllerBase
{
    private readonly INotifier _notifier;
    public readonly IIdentityUser IdentityUser;
    protected Guid UserId { get; set; }
    protected bool AuthenticatedUser { get; set; }

    public BaseController(INotifier notifier, IIdentityUser identityUser)
    {
        _notifier = notifier;
        IdentityUser = identityUser;

        if (IdentityUser.IsUserAuthenticated())
        {
            UserId = IdentityUser.GetUserId();
            AuthenticatedUser = true;
        }
    }

    protected bool ValidExecution()
    {
        return !_notifier.HasNotifications();
    }

    protected ActionResult CustomResponse(object result = null)
    {
        if (ValidExecution())
            return Ok(new
            {
                success = true,
                data = result
            });

        return BadRequest(new
        {
            success = false,
            errors = _notifier.GetNotifications().Select(n => n.Message)
        });
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid) NotifyErrorInvalidModel(modelState);
        return CustomResponse();
    }

    protected void NotifyErrorInvalidModel(ModelStateDictionary modelState)
    {
        var errors = modelState.Values.SelectMany(e => e.Errors);

        foreach (var error in errors)
        {
            var errorMessage = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
            NotifyError(errorMessage);
        }
    }

    protected void NotifyError(string errorMessage)
    {
        _notifier.Handle(new Notification(errorMessage));
    }
}