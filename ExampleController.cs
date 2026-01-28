using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel.Args;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minIODemo
{
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly IMinioClient minioClient;

        public ExampleController(IMinioClient minioClient)
        {
            this.minioClient = minioClient;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUrl(string bucketID)
        {
            return Ok(await minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                    .WithBucket(bucketID))
                .ConfigureAwait(false));
        }
    }

    [ApiController]
    public class ExampleFactoryController : ControllerBase
    {
        private readonly IMinioClientFactory minioClientFactory;

        public ExampleFactoryController(IMinioClientFactory minioClientFactory)
        {
            this.minioClientFactory = minioClientFactory;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUrl(string bucketID)
        {
            var minioClient = minioClientFactory.CreateClient(); //Has optional argument to configure specifics

            return Ok(await minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                    .WithBucket(bucketID))
                .ConfigureAwait(false));
        }
    }
}
