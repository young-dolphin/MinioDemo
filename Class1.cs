using Minio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minIODemo
{
    public class Class1
    {

//        #region MyRegion
//        //IMinioClient minioClient = new MinioClient()
//        //                                    .WithEndpoint("play.min.io")
//        //                                    .WithCredentials("Q3AM3UQ867SPQQA43P2F", "zuf+tfteSlswRu7BJ86wekitnifILbZam1KYY3TG")
//        //                                    .WithSSL()
//        //                                    .Build();
//        IMinioClient minioClient = new MinioClient()
//                                            .WithEndpoint("127.0.0.1:9005")
//                                            .WithCredentials("4TG1F2UV131LB8RMETLC", "D21QN7IZf7IUNuTJvRe07GsFMnCgYPt5BSFFTNQA")
//                                            //.WithSSL()
//                                            .Build();
//try
//{
//    string bucketName = "yangdafengceshi";
//        bool found = await minioClient.BucketExistsAsync(new BucketExistsArgs()
//           .WithBucket(bucketName));
//    if (found)
//    {

//        Console.WriteLine("mybucket already exists");


//        var list = await minioClient.ListBucketsAsync();
//        foreach (Bucket bucket in list.Buckets)
//        {
//            Console.WriteLine(bucket.Name + " " + bucket.CreationDateDateTime);
//        }



//    #region 下载

//    //StatObjectArgs statObjectArgs = new StatObjectArgs()
//    //                              .WithBucket(bucketName)
//    //                              .WithObject("yangdafeng.txt");
//    //await minioClient.StatObjectAsync(statObjectArgs);

//    // Get input stream to have content of 'my-objectname' from 'my-bucketname'

//    bucketName = "yangdafengceshi";

//        var memoryStream = new MemoryStream();
//    var fileName = "Buyo/202411161334PM20357.txt";
//    GetObjectArgs getObjectArgs = new GetObjectArgs()
//                                      .WithBucket(bucketName)
//                                      .WithObject(fileName)
//                                      .WithCallbackStream((stream) =>
//                                      {
//                                          stream.CopyTo(memoryStream);
//                                      });
//    await minioClient.GetObjectAsync(getObjectArgs);

//    DirectoryInfo directoryInfo = new DirectoryInfo(@$"C:\Users\Rick\Desktop\杨大枫测试文件\下载的文件\Buyo\nihao\ss");
//        if (!directoryInfo.Exists)
//        {
//            directoryInfo.Create();
//        }

//using FileStream targetFileStream = new FileStream(@$"C:\Users\Rick\Desktop\杨大枫测试文件\下载的文件\{fileName}", FileMode.Create);

//memoryStream.WriteTo(targetFileStream);
//        //System.IO.File.Delete(path);
//        #endregion



//        //var location = "us-east-1";
//        //var objectName = "333333.txt";//上传的文件名字
//        //var filePath = "F:\\Minio\\新文件 1.txt";
//        //var contentType = "application/zip";

//        //var putObjectArgs = new PutObjectArgs()
//        //           .WithBucket(bucketName)
//        //           .WithObject(objectName)
//        //           .WithFileName(filePath);
//        ////.WithContentType(contentType);
//        //await minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
//        //Console.WriteLine($"上传成功,上传路径{filePath}");
//    }
//    else
//{
//    // Create bucket 'my-bucketname'.
//    await minioClient.MakeBucketAsync(new MakeBucketArgs()
//    .WithBucket(bucketName));
//    Console.WriteLine("mybucket is created successfully");
//    Console.WriteLine($"创建成功,木桶：{bucketName}");

//}
//}
//catch (Exception ex)
//{

//    throw ex;
//}
//#endregion

    }
}
