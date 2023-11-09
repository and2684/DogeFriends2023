using DogeFriendsSharedClassLibrary.Dto;
using ImageService.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ImageService.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IMongoCollection<ImageModel> _imageCollection;

        public ImageController(IMongoDatabase database)
        {
            _imageCollection = database.GetCollection<ImageModel>("Images");
        }

        /// <summary>
        /// Добавляет изображение в коллекцию.
        /// </summary>
        [HttpPost("add")]
        public async Task<IActionResult> AddImage([FromBody] AddImageDto addImageDto)
        {
            var newImage = new ImageModel
            {
                UID = addImageDto.UID,
                EntityName = addImageDto.EntityName,
                Base64Data = addImageDto.Base64Data,
                IsMain = addImageDto.IsMain
            };

            if (addImageDto.IsMain)
            {
                var filter = Builders<ImageModel>.Filter.Eq("UID", addImageDto.UID) & Builders<ImageModel>.Filter.Eq("EntityName", addImageDto.EntityName);
                var update = Builders<ImageModel>.Update.Set("IsMain", false);
                await _imageCollection.UpdateManyAsync(filter, update);
            }

            await _imageCollection.InsertOneAsync(newImage);

            // Вернуть ID
            return Ok(newImage.Id);
        }

        /// <summary>
        /// Удаляет изображение из коллекции.
        /// </summary>
        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveImage(string uid, string entityname, string imageId)
        {
            var result = await _imageCollection.DeleteOneAsync(img => img.UID == uid && img.EntityName == entityname && img.Id == imageId);
            return Ok(result.DeletedCount > 0);
        }

        /// <summary>
        /// Устанавливает изображение как основное.
        /// </summary>
        [HttpPost("setmain")]
        public async Task<IActionResult> SetMainImage(string uid, string entityname, int imageId)
        {
            var filter = Builders<ImageModel>.Filter.Eq("UID", uid) & Builders<ImageModel>.Filter.Eq("EntityName", entityname) & Builders<ImageModel>.Filter.Eq("Id", imageId);
            var update = Builders<ImageModel>.Update.Set("IsMain", true);
            await _imageCollection.UpdateOneAsync(filter, update);

            return Ok(true);
        }

        /// <summary>
        /// Получает все изображения по указанным UID и EntityName.
        /// </summary>
        [HttpGet("getall")]
        public async Task<IActionResult> GetImages([FromQuery] GetImageDto getImageDto)
        {
            var images = await _imageCollection.Find(img => img.UID == getImageDto.UID && img.EntityName == getImageDto.EntityName).ToListAsync();
            return Ok(images);
        }

        /// <summary>
        /// Получает основное изображение по указанным UID и EntityName.
        /// </summary>
        [HttpGet("getmain")]
        public async Task<IActionResult> GetMainImage([FromQuery] GetImageDto getImageDto)
        {
            var mainImage = await _imageCollection.Find(img => img.UID == getImageDto.UID && img.EntityName == getImageDto.EntityName && img.IsMain).FirstOrDefaultAsync();
            return Ok(mainImage);
        }
    }
}
