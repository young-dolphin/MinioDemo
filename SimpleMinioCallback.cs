using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minIODemo
{
    public class MinioCallback
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 详细路径携带文件名字
        /// </summary>
        public string Key { get; set; }
        public List<Records> Records { get; set; }
    }

    public class Records
    {
        //      "eventVersion": "2.0",
        //"eventSource": "minio:s3",
        //"awsRegion": "",
        //"eventTime": "2025-08-20T10:01:06.800Z",
        //"eventName": "s3:ObjectCreated:Put",
        //      "userIdentity": {
        //	"principalId": "root"
        //},
        //      "requestParameters": {
        //	"principalId": "root",
        //	"region": "",
        //	"sourceIPAddress": "127.0.0.1"
        //},
        //      "responseElements": {
        //	"x-amz-id-2": "d0af7ef13e9a79de08686e85c361d9af88c5ebdbbacf07a734ab2c11fd85e57f",
        //	"x-amz-request-id": "185D715B531D4A58",
        //	"x-minio-deployment-id": "1e5f66fc-c3c1-4dfb-8878-70e62fb899cf",
        //	"x-minio-origin-endpoint": "http://127.0.0.1:9005"
        //},
        public S3 s3 { get; set; }
        //      "source": {
        //	"host": "127.0.0.1",
        //	"port": "",
        //	"userAgent": "MinIO (windows; amd64) minio-go/v7.0.91 MinIO Console/(dev)"
        //}
    }

    public class S3
    {
        //      "s3SchemaVersion": "1.0",
        //"configurationId": "Config",
        public Bucket bucket { get; set; }
        public Object @object { get; set; }
    }

    public class Bucket
    {
        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string name { get; set; }

        //    		"ownerIdentity": {
        //	"principalId": "root"
        //},
        //"arn": "arn:aws:s3:::yangdafengceshi"
    }

    public class Object
    {
        /// <summary>
        /// key 对象的键（文件名）
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// size 对象大小（单位：字节）
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// eTag 实体标签（用于验证对象完整性）
        /// </summary>
        public string eTag { get; set; }

        /// <summary>
        /// contentType 内容类型（文件的MIME类型）
        /// </summary>
        public string contentType { get; set; }
        /// <summary>
        /// sequencer 序列号（用于标识对象操作的顺序）
        /// </summary>
        public string sequencer { get; set; }
    }
}
