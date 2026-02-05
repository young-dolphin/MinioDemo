
using Minio;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.AspNetCore.Builder;
using Minio.Exceptions;
using Minio.DataModel;
using System.Net.Mime;
using System.Security.AccessControl;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using minIODemo;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Minio;
using Minio.Exceptions;
using System.Reactive;
using System.Text;
using System.Reactive.Linq;
using Microsoft.AspNetCore.DataProtection;
using System.Net;
using Minio.DataModel.Args;
using System.Runtime.Intrinsics.X86;
class aaaaaaaaaaaa
{
    ////static MinioClient _client = null;
    //static string CallBackApi = "";
    //static string FilePath = "";
    //static string Endpoint = "";
    //static IMinioClient _client = null;
    //static async Task Main(string[] args)
    //{

    //    Console.WriteLine("启动回调服务...");

    //    try
    //    {
    //        var configuration = new ConfigurationBuilder()
    //           .SetBasePath(Directory.GetCurrentDirectory()) // 设置配置文件所在目录（默认是程序运行目录）
    //           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // 加载appsettings.json
    //           .Build();
    //        var minioSettings = new MinioSettings();
    //        configuration.GetSection("MinioSettings").Bind(minioSettings);
    //        CallBackApi = minioSettings.CallBackApi;
    //        FilePath = minioSettings.FilePath;
    //        Console.WriteLine($"Minio同步数据路径：{FilePath}\n");
    //        Console.WriteLine($"测试接口：{CallBackApi}/api/Test\n");
    //        Console.WriteLine($"回调接口：{CallBackApi}/api/CallBack\n");
    //        Console.WriteLine("Minio配置信息：加载成功");

    //        #region 7.0版本

    //        _client = new MinioClient()
    //                                .WithEndpoint(minioSettings.Endpoint)
    //                                .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
    //                                .Build();


    //        #endregion

    //        //下面是8.0的
    //        //_client = (MinioClient)new MinioClient()
    //        //            .WithEndpoint(minioSettings.Endpoint)
    //        //            .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
    //        //            //.WithSSL()
    //        //            .Build();
    //        //Endpoint = minioSettings.Endpoint;
    //        //_client = new MinioClient(
    //        //         endpoint: minioSettings.Endpoint,
    //        //         accessKey: minioSettings.AccessKey,
    //        //         secretKey: minioSettings.SecretKey
    //        //     );

    //        // 创建并启动Web主机
    //        var host = CreateHostBuilder(args).Build();
    //        await host.RunAsync();

    //    }
    //    catch (Exception ex)
    //    {
    //        LogHelper.Error($"有问题1：{ex.Message}");
    //    }


    //}

    //// 配置Web主机
    //public static IHostBuilder CreateHostBuilder(string[] args)
    //{
    //    return Host.CreateDefaultBuilder(args)
    //        .ConfigureWebHostDefaults(webBuilder =>
    //        {
    //            try
    //            {
    //                // 配置监听端口，可根据需要修改

    //                webBuilder.UseUrls(CallBackApi);

    //                webBuilder.Configure(app =>
    //                {
    //                    // 配置中间件处理回调请求
    //                    app.UseRouting();
    //                    app.UseEndpoints(endpoints =>
    //                    {
    //                        // 注册回调接口
    //                        endpoints.MapPost("/api/User/CallBack", HandleCallbackAsync);
    //                        endpoints.MapPost("/api/CallBack", HandleCallbackAsync);
    //                        endpoints.MapGet("/api/Test", HandleHealthCheck);
    //                        endpoints.MapGet("/api/AddWebhookNotification", HandleAddWebhookNotification);
    //                        endpoints.MapGet("/api/RemoveBN", RemoveBN);
    //                    });

    //                    // 处理404
    //                    app.Use(async (context, next) =>
    //                    {
    //                        await next();
    //                        if (context.Response.StatusCode == 404)
    //                        {
    //                            context.Response.ContentType = "text/plain; charset=utf-8";
    //                            await context.Response.WriteAsync("未找到请求的资源");
    //                        }
    //                    });
    //                });
    //            }
    //            catch (Exception ex)
    //            {
    //                LogHelper.Error($"有问题2：{ex.Message}");
    //            }

    //        });
    //}
    //private static async Task HandleCallbackAsync(HttpContext context)
    //{
    //    // 1. 从请求中读取并解析数据为 MinioCallback
    //    var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
    //    var callbackData = System.Text.Json.JsonSerializer.Deserialize<MinioCallback>(json);



    //    // 2. 调用你的业务逻辑处理方法
    //    string result = await ProcessCallbackDataAsync(callbackData);

    //    // 3. 设置响应
    //    context.Response.ContentType = "application/json";
    //    await context.Response.WriteAsync(result);
    //}

    //// 处理回调请求
    //private static async Task<string> ProcessCallbackDataAsync(MinioCallback callbackData)
    //{
    //    //存储桶
    //    string _bucketName = callbackData.Records.First().s3.bucket.name;

    //    //文件名称携带路径
    //    string _object_path_name = callbackData.Key.Replace(_bucketName + "/", "");

    //    string _file_name = callbackData.Key.Split("/").Last();

    //    string _curr_path = (callbackData.Key.Substring(0, callbackData.Key.LastIndexOf('/') + 1)).Replace("/", "\\");

    //    LogHelper.Info($"回调成功：{_curr_path}\\{_file_name}");
    //    if (!callbackData.EventName.Contains("Delete"))
    //    {
    //        var memoryStream = new MemoryStream();
    //        try
    //        {
    //            //try
    //            //{
    //            //    Console.WriteLine("Running example for API: GetObjectAsync");
    //            //    File.Delete(_file_name);
    //            //    var args = new GetObjectArgs()
    //            //        .WithBucket(_bucketName)
    //            //        .WithObject(_object_path_name)
    //            //        .WithFile(_file_name);
    //            //        //.WithServerSideEncryption(sse);
    //            //    _ = await _client.GetObjectAsync(args).ConfigureAwait(false);
    //            //    Console.WriteLine($"Downloaded the file {_file_name} from bucket {_bucketName}");
    //            //    Console.WriteLine();
    //            //}
    //            //catch (Exception e)
    //            //{
    //            //    Console.WriteLine($"[Bucket]  Exception: {e}");
    //            //}

    //            GetObjectArgs getObjectArgs = new GetObjectArgs()
    //                                              .WithBucket(_bucketName)
    //                                              .WithObject(_object_path_name)
    //                                              .WithCallbackStream((stream) =>
    //                                              {
    //                                                  stream.CopyTo(memoryStream);
    //                                              });
    //            await _client.GetObjectAsync(getObjectArgs);
    //        }
    //        catch (Minio.Exceptions.ObjectNotFoundException)
    //        {
    //            // 明确捕获"对象不存在"异常
    //            LogHelper.Error($"对象不存在：Bucket={_bucketName}, Object={_object_path_name}");
    //        }
    //        catch (Minio.Exceptions.BucketNotFoundException)
    //        {
    //            // 捕获"存储桶不存在"异常
    //            LogHelper.Error($"存储桶不存在：{_bucketName}");
    //        }
    //        catch (Minio.Exceptions.AccessDeniedException)
    //        {
    //            // 捕获"权限不足"异常
    //            LogHelper.Error("没有访问该对象的权限，请检查 AccessKey 和 SecretKey");
    //        }
    //        catch (MinioException ex)
    //        {
    //            // 其他 MinIO 相关异常
    //            Console.WriteLine($"MinIO 操作失败：{ex.Message}");
    //        }
    //        catch (Exception ex)
    //        {
    //            // 非 MinIO 异常（如网络错误）
    //            LogHelper.Error($"获取对象时发生错误：{ex.Message}");
    //        }


    //        DirectoryInfo directoryInfo = new DirectoryInfo(@$"{FilePath}\{_curr_path}");
    //        if (!directoryInfo.Exists)
    //        {
    //            directoryInfo.Create();
    //        }

    //        string pathsss = @$"{FilePath}\{_curr_path}\{_file_name}";

    //        using FileStream targetFileStream = new FileStream(pathsss, FileMode.Create);

    //        memoryStream.WriteTo(targetFileStream);
    //        LogHelper.Info($"数据同步成功：{_curr_path}\\{_file_name}");
    //        Console.WriteLine($"数据同步成功：{_curr_path}\\{_file_name}");
    //    }
    //    else
    //    {
    //        System.IO.File.Delete(@$"{FilePath}\{_curr_path}\{_file_name}");
    //        Console.WriteLine($"删除成功：{_curr_path}\\{_file_name}");
    //        LogHelper.Info($"删除成功：{_curr_path}\\{_file_name}");
    //    }

    //    return "成功";
    //}

    //// 健康检查接口
    //private static async Task HandleHealthCheck(HttpContext context)
    //{
    //    // 明确设置内容类型为text/plain并指定UTF-8编码
    //    context.Response.ContentType = "text/plain; charset=utf-8";
    //    // 确保使用UTF-8编码写入响应内容
    //    Console.WriteLine("回调服务测试成功，运行正常!!\n");
    //    await context.Response.WriteAsync("回调服务运行正常");
    //}

    //// 给Bucket添加通知事件
    //private static async Task HandleAddWebhookNotification(HttpContext context)
    //{
    //    await ConfigureWebhookNotification();
    //    // 明确设置内容类型为text/plain并指定UTF-8编码
    //    context.Response.ContentType = "text/plain; charset=utf-8";
    //    // 确保使用UTF-8编码写入响应内容
    //    Console.WriteLine("添加通知事件成功!!\n");
    //    await context.Response.WriteAsync("添加通知事件成功");
    //}

    ///// <summary>
    ///// 配置Webhook事件通知
    ///// </summary>
    //private static async Task ConfigureWebhookNotification()
    //{
    //    try
    //    {
    //        //// 检查当前的通知配置
    //        //var currentConfig = await _client.GetBucketNotificationsAsync("yangdafengceshi");
    //        //_client.RemoveAllBucketNotificationsAsync("yyyy");
    //        //Console.WriteLine($"当前配置: {currentConfig.ToXML()}");

    //        // 定义要监听的事件（例如：对象创建、删除）
    //        var events = new List<EventType>
    //        {
    //            EventType.ObjectCreatedAll,  // 所有对象创建事件
    //            EventType.ObjectRemovedAll   // 所有对象删除事件
    //        };

    //        // 创建Webhook通知配置（修正ARN格式）

    //        var queueConfig = new QueueConfig("arn:minio:sqs::yanga:webhook");

    //        //var queueConfig = new QueueConfig("arn:minio:sqs::1:webhook");
    //        //queueConfig.Queue = $"{CallBackApi}/api/CallBack";

    //        queueConfig.AddEvents(events);

    //        // 配置桶通知
    //        var bucketNotification = new BucketNotification();
    //        bucketNotification.AddQueue(queueConfig);

    //        // 应用到指定桶（注意替换为实际桶名）
    //        await _client.SetBucketNotificationsAsync("test", bucketNotification);
    //        Console.WriteLine($"配置Webhook事件通知成功 \n");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($" 配置Webhook事件通知  异常：{ex.Message}!!\n");
    //    }

    //}
    //private static async Task RemoveBN(HttpContext context)
    //{
    //    await _client.RemoveAllBucketNotificationsAsync("test");
    //    Console.WriteLine($"test 事件移除成功");
    //}

    //public class MinioSettings
    //{
    //    public string Endpoint { get; set; }
    //    public string AccessKey { get; set; }
    //    public string SecretKey { get; set; }
    //    public string CallBackApi { get; set; }
    //    public string FilePath { get; set; }
    //}
}