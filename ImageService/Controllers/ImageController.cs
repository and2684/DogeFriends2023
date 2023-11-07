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

        [HttpPost("add")]
        public IActionResult AddImage([FromBody] AddImageDto addImageDto)
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
                _imageCollection.UpdateMany(filter, update);
            }

            _imageCollection.InsertOne(newImage);

            // Вернуть ID только если изображение установлено как основное
            if (addImageDto.IsMain)
            {
                var insertedImage = _imageCollection.Find(img => img.UID == addImageDto.UID && img.EntityName == addImageDto.EntityName && img.IsMain).FirstOrDefault();
                return Ok(insertedImage.Id);
            }

            return Ok();
        }

        [HttpDelete("remove")]
        public IActionResult RemoveImage(string uid, string entityname, string imageId)
        {
            var result = _imageCollection.DeleteOne(img => img.UID == uid && img.EntityName == entityname && img.Id == imageId);
            return Ok(result.DeletedCount > 0);
        }

        [HttpPost("setmain")]
        public IActionResult SetMainImage(string uid, string entityname, int imageId)
        {
            var filter = Builders<ImageModel>.Filter.Eq("UID", uid) & Builders<ImageModel>.Filter.Eq("EntityName", entityname) & Builders<ImageModel>.Filter.Eq("Id", imageId);
            var update = Builders<ImageModel>.Update.Set("IsMain", true);
            _imageCollection.UpdateOne(filter, update);

            return Ok(true);
        }
    }
}
