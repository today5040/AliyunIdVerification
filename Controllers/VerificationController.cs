using Microsoft.AspNetCore.Mvc;

using AliyunIdVerification.Models;

namespace AliyunIdVerification.Controllers;

public class VerificationController : Controller
{
    public IActionResult Index()
    {
        var model = new VerifyModel()
        {
            OuterOrderNo = Guid.NewGuid().ToString("N"),
            CertName = "胡晓凯",
            CertNo = "411082199005092411"
        };
        return View(model);
    }

    public async Task<IActionResult> Result()
    {
        ViewData["result"] = await AlibabaCloudApi.GetVerifyResult();
        return View();
    }

    public async Task<IActionResult> Create(VerifyModel model)
    {
        model.IP = GetClientIpAddress();
        var result = await AlibabaCloudApi.CompareFaceVerifyWithOptions(model);
        if (result.Contains("https"))
        {
            return Redirect(result);
        }
        else
        {
            return Content(result);
        }
    }

    private string GetClientIpAddress()
    {
        var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;

        // 可能通过代理（如负载均衡、反向代理）来访问
        var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim(); // 获取第一个代理的IP地址
        }

        return remoteIpAddress?.ToString() ?? "Unknown";
    }
}
