using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChappiePokeAPI.DataAccess;
using EntityModels.EntityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChappiePokeAPI.Controllers
{
    [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {

        PokeDBContext PokeDB;

        public MainController(PokeDBContext PokeDBContext)
        {
            PokeDB = PokeDBContext;
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadProductImages()
        {
            var files = Request.Form.Files;
            return Ok();
        }
    }
}
