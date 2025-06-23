using Minio;
using Minio.Exceptions;
using System.Threading.Tasks;
using Minio.DataModel.Args;

namespace AssetServerAPI.Utilities
{
    public class BlobStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;
        private readonly string _serviceUrl;
        private readonly string _accessKey;
        private readonly string _secretKey;

        public BlobStorageService(IConfiguration configuration)
        {
            _serviceUrl = configuration["MinIO:ServiceURL"]!;
            _accessKey = configuration["MinIO:AccessKey"]!;
            _secretKey = configuration["MinIO:SecretKey"]!;
            _bucketName = configuration["MinIO:BucketName"]!;

            
            var uri = new Uri(_serviceUrl);
            string endpoint = $"{uri.Host}:{uri.Port}"; // "minio:9000"

            _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(_accessKey, _secretKey)
                .WithSSL(false)
                .Build();
            
            CreateBucketIfNotExistsAsync().Wait();
        }

        public async Task CreateBucketIfNotExistsAsync()
        {
            bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!found)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
                await SetBucketPolicyAsync(_bucketName);
            }
            else
            {
                await SetBucketPolicyAsync(_bucketName);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            try
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                using var stream = file.OpenReadStream();
                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(fileName)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(file.ContentType));

                return $"{_serviceUrl}/{_bucketName}/{fileName}";
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error uploading file: {e.Message}");
                throw;
            }
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName));
        }
        
        public async Task SetBucketPolicyAsync(string bucketName)
        {
            string policyJson = $@"
            {{
                ""Version"": ""2012-10-17"",
                ""Statement"": [
                    {{
                        ""Effect"": ""Allow"",
                        ""Principal"": {{" + "\"AWS\": \"*\"" + @"}},
                        ""Action"": [
                            ""s3:GetObject""
                        ],
                        ""Resource"": [
                            ""arn:aws:s3:::{bucketName}/*""
                        ]
                    }}
                ]
            }}";

            await _minioClient.SetPolicyAsync(new SetPolicyArgs()
                .WithBucket(bucketName)
                .WithPolicy(policyJson));
        }
        
    }
}
