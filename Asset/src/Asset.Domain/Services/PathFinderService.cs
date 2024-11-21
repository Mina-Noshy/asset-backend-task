using Asset.Domain.Interfaces.Common;
using Microsoft.AspNetCore.Http;

namespace Asset.Domain.Services;

public class PathFinderService(ICurrentUser _currentUser, IHttpContextAccessor _contextAccessor) : IPathFinderService
{
    private string ROOT_FOLDER_PATH
        => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");



    public string AssetsFolderPath
        => CombineCompanyInfo("Assets");
    public string ImagesFolderPath
        => CombineCompanyInfo("Images");
    public string ContentsFolderPath
        => CombineCompanyInfo("Contents");
    public string DocumentsFolderPath
        => CombineCompanyInfo("Documents");
    public string TemplatesFolderPath
        => CombineCompanyInfo("Templates");
    public string AttachmentsFolderPath
        => CombineCompanyInfo("Attachments");
    public string ReportsFolderPath
        => CombineCompanyInfo("Reports");
    public string UploadsFolderPath
        => CombineCompanyInfo("Uploads");

    public string GetFullURL(string folderPath, string fileName)
    {
        string fullPath = Path.Combine(folderPath, fileName);
        return GetFullURL(fullPath);
    }
    public string GetFullURL(string fullPath)
    {
        if (fullPath == null || fullPath.Length < 10)
        {
            throw new ArgumentException("The full path provided is invalid or too short.", nameof(fullPath));
        }

        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HTTP context is not available.");
        }

        string apiURL = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value.TrimEnd('/')}";

        Uri fileUri = new Uri(fullPath);
        string fileUrl = fileUri.AbsoluteUri;

        fileUrl = fileUrl
            .Substring(fileUrl.LastIndexOf("wwwroot"))
            .Replace("wwwroot", apiURL);

        return fileUrl;
    }







    private string CombineCompanyInfo(string folderPath)
    {
        string companyId = _currentUser.CompanyNo;

        if (string.IsNullOrWhiteSpace(companyId))
            companyId = "common";

        return Path.Combine(ROOT_FOLDER_PATH, folderPath, companyId);
    }
}
