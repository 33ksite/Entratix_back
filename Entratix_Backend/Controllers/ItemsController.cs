using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Entratix_Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles = "Admin, HR")] // Only Admin or HR roles can access this controller
    //[Authorize(Roles = "SuperAdmin")]  Admin and SuperAdmin role can access this controller
    [ApiController]
    public class ItemsController : Controller
    {
        public List<string> colorList = new List<string> { "Red", "Blue", "Green", "Yellow", "Black", "White" };

        [HttpGet("GetColorList")]
        public List<string> GetColorList()
        {
            try
            {
                return colorList;
            } catch (Exception e)
            {
              throw new Exception(e.Message);
            }
           
        }
    }
}
