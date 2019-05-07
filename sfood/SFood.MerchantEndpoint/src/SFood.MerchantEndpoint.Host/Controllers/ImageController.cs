using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFood.MerchantEndpoint.Application;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Host.Models;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Host.Controllers
{
    [Authorize]
    [ApiController, Route("api/[controller]")]
    public class ImageController : BaseController
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// 只支持单个图片文件上传
        /// </summary>
        /// <param name="imageFiles"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse Upload(List<IFormFile> imageFiles)
        {
            var response = new ApiResponse() {
                StatusCode = BusinessStatusCode.Success
            };
            var img = HttpContext.Request.Form.Files.GetFile("image");
            if (img == null)
            {
                throw new BadRequestException("未发现提交的文件");
            }

            using (var stream = img.OpenReadStream())
            {
                response.Data = _imageService.Upload(stream);
            }
            return response;
        }
    }
}
