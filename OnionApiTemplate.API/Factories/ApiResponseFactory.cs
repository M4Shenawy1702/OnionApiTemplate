using Microsoft.AspNetCore.Mvc;
using OnionApiTemplate.API.Models.Errors;
using System.Net;

namespace OnionApiTemplate.API.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrorResponse(ActionContext context)
        {
            var errors = context.ModelState.Where(error => error.Value!.Errors.Any()).Select(error => new ValidationError
            {
                Field = error.Key,
                Errors = error.Value!.Errors.Select(e => e.ErrorMessage)
            });
            var response = new ValidationErrorResponse()
            {
                ValidationErrors = errors,
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = "invalid errors occur "
            };
            return new BadRequestObjectResult(response);
        }
    }
}
