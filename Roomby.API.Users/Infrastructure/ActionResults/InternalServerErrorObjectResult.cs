using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Roomby.API.Users.Infrastructure.ActionResults {
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}