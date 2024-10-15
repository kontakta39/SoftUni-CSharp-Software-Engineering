using Microsoft.AspNetCore.Mvc;

namespace CinemaWebApp.Controllers;

public class BaseController : Controller
{
    protected bool isGuidValid(string? id, ref Guid guidId)
    {
        //Non-existing parameter in the URL
        if (String.IsNullOrWhiteSpace(id))
        {
            return false;
        }

        //Invalid parameter in the URL
        bool isGuidValid = Guid.TryParse(id, out guidId);

        if (!isGuidValid)
        {
            return false;
        }

        return true;
    }
}