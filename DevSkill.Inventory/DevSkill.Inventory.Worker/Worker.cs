using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SQS;
using Amazon.SQS.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DevSkill.Inventory.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IAmazonSQS _sqsClient;
        private readonly IAmazonS3 _s3Client;
        private const string QueueUrl = "https://sqs.us-east-1.amazonaws.com/847888492411/Nazmul-Queue";
        private const string S3BucketName  = "aspnetb11";

        public Worker(ILogger<Worker> logger,
            IServiceScopeFactory scopeFactory,
            IAmazonSQS sqsClient,
            IAmazonS3 s3Client,
            IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _sqsClient = sqsClient;
            _s3Client = s3Client;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string imagePath = Path.GetFullPath(_configuration["ImageFolderPath"], AppContext.BaseDirectory);

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
                {
                    QueueUrl = QueueUrl,
                    MaxNumberOfMessages = 1,
                    WaitTimeSeconds = 10
                });

                foreach (var message in response.Messages)
                {
                    try
                    {
                        var fileName = message.Body.Trim();
                        var inputPath = Path.Combine(imagePath, fileName);

                        if (!File.Exists(inputPath))
                        {
                            _logger.LogWarning("File not found: {0}", inputPath);
                            continue;
                        }
                        // Ensure file has a valid extension
                        var extension = Path.GetExtension(fileName);
                        if (string.IsNullOrWhiteSpace(extension))
                        {
                            extension = ".jpg";
                            fileName += extension;
                            inputPath += extension;
                        }

                        

                        var resizedName = "resizedImage-" + Path.GetFileNameWithoutExtension(fileName) + extension;
                        var resizedPath = Path.Combine(imagePath, resizedName);

                        using var image = await Image.LoadAsync(inputPath);
                        image.Mutate(x => x.Resize(300, 300));
                        await image.SaveAsync(resizedPath); 

                        await using var fileStream = File.OpenRead(resizedPath);
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = fileStream,
                            Key = "Nazmul/images/" + fileName,
                            BucketName = S3BucketName,
                            ContentType = "image/jpeg"
                        };

                        var transferUtility = new TransferUtility(_s3Client);
                        await transferUtility.UploadAsync(uploadRequest);

                        await _sqsClient.DeleteMessageAsync(QueueUrl, message.ReceiptHandle);
                        _logger.LogInformation("Successfully processed and uploaded {0}", fileName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process message: {0}", message.Body);
                    }
                }

                await Task.Delay(2000, stoppingToken);
            }
        }

    }
}
