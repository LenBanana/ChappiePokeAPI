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
using Models.DTOModels;
using ChappiePokeAPI.Extensions;

namespace ChappiePokeAPI.Controllers
{
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
        public async Task<IActionResult> UploadProductImages([FromForm] FileUploadRequest request)
        {
            try
            {
                var SessionKey = request.Param;
                User user = PokeDB.GetUserCopy(SessionKey, false);
                if (user != null && user.UserPrivileges != Models.Enums.UserPrivileges.Administrator)
                {
                    return StatusCode(401, "Unauthorized");
                }
                var files = request.Files;
                foreach (var file in files)
                {
                    //var fileType = Path.GetExtension(file.FileName);
                    //if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
                    //{
                    if (file != null && file.Length > 0)
                    {
                        var docExt = System.IO.Path.GetExtension(file.FileName).ToString();
                        var fileName = user.UserID + "/" + Path.GetRandomFileName() + docExt;
                        var userPath = Path.Combine(Paths.AssetUploadPath, user.UserID.ToString());
                        if (!Directory.Exists(userPath))
                            Directory.CreateDirectory(userPath);

                        string filePath = Path.Combine(Paths.AssetUploadPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }                        
                        PokeDB.Images.Add(new Image() { Fullsize = fileName, ImageID = 0, Thumbnail = fileName });
                    }
                    else
                    {
                        return BadRequest();
                    }
                    //}
                }
                await PokeDB.SaveChangesAsync();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
