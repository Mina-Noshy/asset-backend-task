namespace Asset.Domain.Interfaces.Common;

public interface IPathFinderService
{

    string GetFullURL(string fullPath);
    string GetFullURL(string folderPath, string fileName);



    string AssetsFolderPath { get; }
    string ImagesFolderPath { get; }
    string ContentsFolderPath { get; }
    string DocumentsFolderPath { get; }
    string TemplatesFolderPath { get; }
    string AttachmentsFolderPath { get; }
    string ReportsFolderPath { get; }
    string UploadsFolderPath { get; }



}
