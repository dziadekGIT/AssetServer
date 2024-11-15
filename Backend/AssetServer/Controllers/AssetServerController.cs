using Microsoft.AspNetCore.Mvc;
using AssetServerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetServerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public AssetServerController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Stands for C from CRUD - CREATE. Adding asset to DB.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="imagePath"></param>
        /// <param name="fbxPath"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Asset asset, IFormFile imagePath, IFormFile fbxPath)
        {
         
            if (imagePath == null || fbxPath == null)
            {
                return BadRequest("Pliki są wymagane.");
            }


            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

   
            var imageFileName = Path.Combine(uploadsDirectory, imagePath.FileName);
            var fbxFileName = Path.Combine(uploadsDirectory, fbxPath.FileName);


            using (var stream = new FileStream(imageFileName, FileMode.Create))
            {
                await imagePath.CopyToAsync(stream);
            }


            using (var stream = new FileStream(fbxFileName, FileMode.Create))
            {
                await fbxPath.CopyToAsync(stream);
            }


            var newAsset = new Asset
            {
                Name = asset.Name,
                ImageFileName = imagePath.FileName,  
                FbxFileName = fbxPath.FileName,    
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Assets.Add(newAsset);
            await _context.SaveChangesAsync();
            
            return Ok(newAsset);   
        }
        
        [HttpGet("Read")]
        public IActionResult Read()
        {
            return Ok("cRud - Read - works");
        }
        
        [HttpPatch("Update")]
        public IActionResult Update()
        {
            return Ok("crUd - Update - works");
        }
        
        [HttpDelete("Delete")]
        public IActionResult Delete()
        {
            return Ok("cruD - Delete - works");
        }
    }
}
