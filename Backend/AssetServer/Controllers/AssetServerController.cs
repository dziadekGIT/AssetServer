using Microsoft.AspNetCore.Mvc;
using AssetServerAPI.Models;
using AssetServerAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace AssetServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetServerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageService _blobStorageService;
        
        public AssetServerController(ApplicationDbContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
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
                return BadRequest("Files are required");
            }
            
            var imageUrl = await _blobStorageService.UploadFileAsync(imagePath);
            var fbxUrl = await _blobStorageService.UploadFileAsync(fbxPath);
            
            var newAsset = new Asset
            {
                Name = asset.Name,
                ImageFileName = imagePath.FileName,
                FbxFileName = fbxPath.FileName,
                CreatedAt = DateTime.UtcNow,
                ImageFileUrl = imageUrl,
                FbxUrl = fbxUrl
            };
            
            _context.Assets.Add(newAsset);
            await _context.SaveChangesAsync();

            return Ok(newAsset);
        }

        
        [HttpGet("Read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                var assets = await _context.Assets.ToListAsync();
                return Ok(assets);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Read method: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error." });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var asset = await _context.Assets.FindAsync(id);
                if (asset == null)
                {
                    return NotFound(new { error = "Asset not found." });
                }
                return Ok(asset);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Get method: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error." });
            }
        }
        
        [HttpPatch("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Asset updatedAsset)
        {
            if (id != updatedAsset.Id)
            {
                return BadRequest(new { error = "ID mismatch." });
            }

            try
            {
                var existingAsset = await _context.Assets.FindAsync(id);
                if (existingAsset == null)
                {
                    return NotFound(new { error = "Asset not found." });
                }
                
                existingAsset.Name = updatedAsset.Name;
                
                _context.Assets.Update(existingAsset);
                await _context.SaveChangesAsync();

                return Ok(existingAsset);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update method: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error." });
            }
        }

        
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var asset = await _context.Assets.FindAsync(id);
                if (asset == null)
                {
                    return NotFound(new { error = "Asset not found." });
                }

                await _blobStorageService.DeleteFileAsync(asset.ImageFileName);
                await _blobStorageService.DeleteFileAsync(asset.FbxFileName);

                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Asset deleted successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete method: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error." });
            }
        }

    }
}
