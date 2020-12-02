using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChappiePokeAPI.DataAccess;
using EntityModels.EntityModels;
using HelperVariables.Globals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChappiePokeAPI.Controllers
{
    [Microsoft.AspNetCore.Cors.EnableCors("AllowAll")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : Controller
    {

        PokeDBContext PokeDB;

        public MainController(PokeDBContext PokeDBContext)
        {
            PokeDB = PokeDBContext;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> UploadProductImages([FromForm] IFormFileCollection collection)
        {
            var files = Request.Form.Files;
            try
            {
                foreach (var file in files)
                {
                    //var fileType = Path.GetExtension(file.FileName);
                    //if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
                    //{
                    var docExt = System.IO.Path.GetExtension(file.FileName).ToString();
                    var fileName = Path.GetRandomFileName() + docExt;
                    if (file != null && file.Length > 0)
                    {
                        string filePath = Path.Combine(Paths.AssetUploadPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                    //}
                }
            } catch
            {
                return StatusCode(501, "F");
            }
            return Ok();
        }



        [HttpGet]
        public IActionResult TestGet()
        {
            return Ok();
        }
    }
}
