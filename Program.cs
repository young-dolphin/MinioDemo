
using Minio;
using Microsoft.AspNetCore.Builder;
using Minio.Exceptions;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using minIODemo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Net;
using Minio.DataModel.Args;
using Minio.DataModel.Notification;
using Models.SystemModel;
using Common;
using SqlSugar.Extensions;
using SqlSugar;

class Program
{
    static string CallBackApi = "";
    static string FilePath = "";
    static string Endpoint = "";
    static MinioSettings minioSettings = new MinioSettings();
    static IMinioClient _client = null;
    static async Task Main(string[] args)
    {
        Console.WriteLine("启动回调服务...");
        SnowFlakeSingle.WorkId = 2;
        try
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory()) // 设置配置文件所在目录（默认是程序运行目录）
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();
            //var minioSettings = new MinioSettings();
            configuration.GetSection("MinioSettings").Bind(minioSettings);
            CallBackApi = minioSettings.CallBackApi;
            FilePath = minioSettings.FilePath;
            Console.WriteLine($"Minio同步数据路径：{FilePath}\n");
            Console.WriteLine($"监测接口是否正常：{CallBackApi}/api/Test\n");
            Console.WriteLine($"回调接口：{CallBackApi}/api/CallBack\n");
            Console.WriteLine("Minio配置信息：加载成功");

            #region MinIo 7.0版本
            _client = new MinioClient()
                     .WithEndpoint(minioSettings.Endpoint)
                     .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
                     .Build();
            #endregion
            // 创建并启动Web主机
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();




        }
        catch (Exception ex)
        {
            LogHelper.Error($"有问题1：{ex.Message}");
        }

    }

    // 配置Web主机
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                try
                {
                    // 配置监听端口，可根据需要修改

                    webBuilder.UseUrls(CallBackApi);

                    webBuilder.Configure(app =>
                    {
                        // 配置中间件处理回调请求
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            // 注册回调接口
                            endpoints.MapPost("/api/CallBack", HandleCallbackAsync);
                            endpoints.MapGet("/api/Test", HandleHealthCheck);
                            endpoints.MapGet("/api/AddWebhookNotification", HandleAddWebhookNotification);
                            endpoints.MapGet("/api/RemoveWebhookNotification", HandleRemoveWebhookNotification);
                        });

                        // 处理404
                        app.Use(async (context, next) =>
                        {
                            await next();
                            if (context.Response.StatusCode == 404)
                            {
                                context.Response.ContentType = "text/plain; charset=utf-8";
                                await context.Response.WriteAsync("未找到请求的资源");
                            }
                        });
                    });
                }
                catch (Exception ex)
                {
                    LogHelper.Error($"有问题2：{ex.Message}");
                }

            });
    }


    private static async Task HandleCallbackAsync(HttpContext context)
    {
        var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
        var callbackData = JSONHelper.json_to_model<MinioCallback>(json);
        var result = ProcessCallbackDataAsync(callbackData);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 200;
        await context.Response.WriteAsync(result);
    }

    // 处理回调请求
    private static string ProcessCallbackDataAsync(MinioCallback callbackData)
    {
        try
        {
            string EventName = callbackData.EventName;
            string key = WebUtility.UrlDecode(callbackData.Records[0].s3.@object.key);
            string eventTime = callbackData.Records.First().eventTime;
            string bucket_name = callbackData.Records.First().s3.bucket.name;
            long size = callbackData.Records.First().s3.@object.size;
            string eTag = callbackData.Records.First().s3.@object.eTag;
            string contentType = callbackData.Records.First().s3.@object.contentType;
            string sequencer = callbackData.Records.First().s3.@object.sequencer;

            var message = new sys_base_minio
            {
                EventName = EventName,
                Key = key,
                EventTime = eventTime.ObjToDate(),
                Bucket_name = bucket_name,
                Size = size,
                Etag = eTag,
                ContentType = contentType,
                Sequencer = sequencer
            };

            SqlSugarHelper.Db.Insertable(message).ExecuteReturnSnowflakeId();
            Console.WriteLine("接收成功");
            return "接收成功";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"接收失败，{ex.Message}");
            return $"接收失败，{ex.Message}";
        }
    }

    private static async void GetMinioFileByKey()
    {

        MinioCallback callbackData = new MinioCallback();
        //存储桶
        string _bucketName = callbackData.Records.First().s3.bucket.name;

        //文件名称携带路径
        string _object_path_name = callbackData.Key.Replace(_bucketName + "/", "");

        string _file_name = callbackData.Key.Split("/").Last();

        string _curr_path = callbackData.Key.Substring(0, callbackData.Key.LastIndexOf('/') + 1).Replace("/", "\\");

        var str = callbackData.Records[0].s3.@object.key;
        //object里的中文会乱码，进行解码就可以了
        var zifuchuan = WebUtility.UrlDecode(str);

        LogHelper.Info($"回调成功：{_curr_path}\\{_file_name}");
        if (!callbackData.EventName.Contains("Delete"))
        {
            //Directory.Exists()

            try
            {
                Console.WriteLine("Running example for API: GetObjectAsync");
                //File.Delete(_file_name);
                var args = new GetObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(_object_path_name)
                    .WithFile(FilePath);
                //.WithServerSideEncryption(sse);
                _ = await _client.GetObjectAsync(args).ConfigureAwait(false);

                Console.WriteLine($"Downloaded the file {_file_name} from bucket {_bucketName}");
                Console.WriteLine();
                //
            }
            catch (Minio.Exceptions.ObjectNotFoundException)
            {
                // 明确捕获"对象不存在"异常
                LogHelper.Error($"对象不存在：Bucket={_bucketName}, Object={_object_path_name}");
            }
            catch (Minio.Exceptions.BucketNotFoundException)
            {
                // 捕获"存储桶不存在"异常
                LogHelper.Error($"存储桶不存在：{_bucketName}");
            }
            catch (Minio.Exceptions.AccessDeniedException)
            {
                // 捕获"权限不足"异常
                LogHelper.Error("没有访问该对象的权限，请检查 AccessKey 和 SecretKey");
            }
            catch (MinioException ex)
            {
                // 其他 MinIO 相关异常
                Console.WriteLine($"MinIO 操作失败：{ex.Message}");
            }
            catch (Exception ex)
            {
                // 非 MinIO 异常（如网络错误）
                LogHelper.Error($"获取对象时发生错误：{ex.Message}");
            }

            LogHelper.Info($"数据同步成功：{_curr_path}\\{_file_name}");
            Console.WriteLine($"数据同步成功：{_curr_path}\\{_file_name}");
        }
        else
        {
            System.IO.File.Delete(@$"{FilePath}\{_curr_path}\{_file_name}");
            Console.WriteLine($"删除成功：{_curr_path}\\{_file_name}");
            LogHelper.Info($"删除成功：{_curr_path}\\{_file_name}");
        }

    }





    // 健康检查接口
    private static async Task HandleHealthCheck(HttpContext context)
    {
        // 明确设置内容类型为text/plain并指定UTF-8编码
        context.Response.ContentType = "text/plain; charset=utf-8";
        // 确保使用UTF-8编码写入响应内容
        Console.WriteLine("回调服务测试成功，运行正常!!\n");
        await context.Response.WriteAsync("回调服务运行正常");
    }

    // 给Bucket添加通知事件
    private static async Task HandleAddWebhookNotification(HttpContext context)
    {
        await ConfigureWebhookNotification();
        // 明确设置内容类型为text/plain并指定UTF-8编码
        context.Response.ContentType = "text/plain; charset=utf-8";
        // 确保使用UTF-8编码写入响应内容
        Console.WriteLine("添加通知事件成功!!\n");
        await context.Response.WriteAsync("添加通知事件成功");
    }



    /// <summary>
    /// 配置Webhook事件通知
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static async Task ConfigureWebhookNotification()
    {
        try
        {
            Console.WriteLine("Running example for API: SetBucketNotificationAsync");
            var notification = new BucketNotification();
            var args = new SetBucketNotificationsArgs()
                .WithBucket("test")
                .WithBucketNotificationConfiguration(notification);

            // Uncomment the code below and change Arn and event types to configure.

            QueueConfig queueConfiguration = new QueueConfig("arn:minio:sqs::yanga:webhook");
            queueConfiguration.AddEvents(new List<EventType>() { EventType.BucketCreatedAll, EventType.ObjectRemovedDelete, EventType.ObjectCreatedPut });
            notification.AddQueue(queueConfiguration);

            await _client.SetBucketNotificationsAsync(args).ConfigureAwait(false);

            Console.WriteLine("Notifications set for the bucket {bucketName} were set successfully");
            Console.WriteLine();
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Bucket]  Exception: {e}");
        }
    }

    private static async Task HandleRemoveWebhookNotification(HttpContext context)
    {
        Console.WriteLine("Running example for API: RemoveAllBucketNotificationAsync");

        var args = new RemoveAllBucketNotificationsArgs()
            .WithBucket("移除哪个Buket?");
        await _client.RemoveAllBucketNotificationsAsync(args).ConfigureAwait(false);

        Console.WriteLine($"test 事件移除成功");
    }

    public class MinioSettings
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string CallBackApi { get; set; }
        public string FilePath { get; set; }
    }
}