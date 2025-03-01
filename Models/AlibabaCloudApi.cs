using System;
using System.Text.Json;
using System.Threading.Tasks;
using AlibabaCloud.SDK.Cloudauth20190307.Models;

using Tea;

namespace AliyunIdVerification.Models;

// https://help.aliyun.com/zh/id-verification/financial-grade-id-verification/server-side-integration-2?spm=a2c4g.11186623.0.0.690c2d475huMVj#reference-2561225
public class AlibabaCloudApi
{
    /// <term><b>Description:</b></term>
    /// <description>
    /// <para>使用AK&amp;SK初始化账号Client</para>
    /// </description>
    /// 
    /// <returns>
    /// Client
    /// </returns>
    /// 
    /// <term><b>Exception:</b></term>
    /// Exception
    public static AlibabaCloud.SDK.Cloudauth20190307.Client CreateClient()
    {
        // 工程代码泄露可能会导致 AccessKey 泄露，并威胁账号下所有资源的安全性。以下代码示例仅供参考。
        // 建议使用更安全的 STS 方式，更多鉴权访问方式请参见：https://help.aliyun.com/document_detail/378671.html。
        AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config
        {
            // 必填，请确保代码运行环境设置了环境变量 ALIBABA_CLOUD_ACCESS_KEY_ID。
            AccessKeyId = "you aliyun  accesskeyId",
            // 必填，请确保代码运行环境设置了环境变量 ALIBABA_CLOUD_ACCESS_KEY_SECRET。
            AccessKeySecret = "you aliyun accesskeySecret",
            // Endpoint 请参考 https://api.aliyun.com/product/Cloudauth
            Endpoint = "cloudauth.cn-beijing.aliyuncs.com"
        };
        return new AlibabaCloud.SDK.Cloudauth20190307.Client(config);
    }

    public static async Task<string> GetVerifyResult()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "result.txt");
        var certifyId = File.ReadAllText(path);
        var sceneId = 1000012746;
        AlibabaCloud.SDK.Cloudauth20190307.Client client = CreateClient();
        AlibabaCloud.SDK.Cloudauth20190307.Models.DescribeFaceVerifyRequest describeFaceVerifyRequest = new AlibabaCloud.SDK.Cloudauth20190307.Models.DescribeFaceVerifyRequest()
        {
            CertifyId = certifyId,
            SceneId = sceneId
        };
        AlibabaCloud.TeaUtil.Models.RuntimeOptions runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
        try
        {
            // 复制代码运行请自行打印 API 的返回值
            DescribeFaceVerifyResponse response = await client.DescribeFaceVerifyWithOptionsAsync(describeFaceVerifyRequest, runtime);
            return JsonSerializer.Serialize(response.Body);
        }
        catch (TeaException error)
        {
            // 此处仅做打印展示，请谨慎对待异常处理，在工程项目中切勿直接忽略异常。
            // 错误 message
            Console.WriteLine(error.Message);
            // 诊断地址
            Console.WriteLine(error.Data["Recommend"]);
            AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
            return error.Message;
        }
        catch (Exception _error)
        {
            TeaException error = new TeaException(new Dictionary<string, object>
                {
                    { "message", _error.Message }
                });
            // 此处仅做打印展示，请谨慎对待异常处理，在工程项目中切勿直接忽略异常。
            // 错误 message
            Console.WriteLine(error.Message);
            // 诊断地址
            Console.WriteLine(error.Data["Recommend"]);
            AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
            return error.Message;
        }
    }

    public static async Task<string> CompareFaceVerifyWithOptions(VerifyModel model)
    {
        AlibabaCloud.SDK.Cloudauth20190307.Client client = CreateClient();
        InitFaceVerifyRequest initFaceVerifyRequest = new InitFaceVerifyRequest()
        {
            SceneId = 1000012746,
            OuterOrderNo = model.OuterOrderNo,
            ProductCode = "ID_PRO",
            Model = "LIVENESS",
            CertType = "IDENTITY_CARD",
            CertNo = model.CertNo,
            CertName = model.CertName,
            ReturnUrl = "http://120.26.76.224:5040/Verification/result",
            MetaInfo = model.MetaInfo,
            //Mobile = "18137471138",
            //Ip = model.IP,
            //UserId = "18137471138",
            //CallbackUrl = "https://help.aliyun.com",
            //CallbackToken = Guid.NewGuid().ToString("N"),
            //CertifyUrlType = "h5",
            //CertifyUrlStyle = "L",
            //AuthId = "92d46b9e9e2d703f2897f350d5bd4149",
            //EncryptType = "",
            //ProcedurePriority = "url",
            //FaceGuardOutput = string.Empty,
            //RarelyCharacters = "N",
            //VideoEvidence = "false",
            //CameraSelection = "N"
        };
        AlibabaCloud.TeaUtil.Models.RuntimeOptions runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();
        try
        {
            // 复制代码运行请自行打印 API 的返回值
            InitFaceVerifyResponse response = await client.InitFaceVerifyWithOptionsAsync(initFaceVerifyRequest, runtime);
            var code = response.Body.Code;
            if (code == "200")
            {
                var certifyUrl = response.Body.ResultObject.CertifyUrl;
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "result.txt"), response.Body.ResultObject.CertifyId);
                return certifyUrl;
            }
            else
            {
                return response.Body.Message;
            }
        }
        catch (TeaException error)
        {
            // 此处仅做打印展示，请谨慎对待异常处理，在工程项目中切勿直接忽略异常。
            // 错误 message
            Console.WriteLine(error.Message);
            // 诊断地址
            Console.WriteLine(error.Data["Recommend"]);
            AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
            return error.Message;
        }
        catch (Exception _error)
        {
            TeaException error = new TeaException(new Dictionary<string, object>
                {
                    { "message", _error.Message }
                });
            // 此处仅做打印展示，请谨慎对待异常处理，在工程项目中切勿直接忽略异常。
            // 错误 message
            Console.WriteLine(error.Message);
            // 诊断地址
            Console.WriteLine(error.Data["Recommend"]);
            AlibabaCloud.TeaUtil.Common.AssertAsString(error.Message);
            return error.Message;
        }
    }
}
