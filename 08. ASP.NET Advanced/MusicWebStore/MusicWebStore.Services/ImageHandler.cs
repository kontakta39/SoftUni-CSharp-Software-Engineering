using Microsoft.AspNetCore.Http;

namespace MusicWebStore.Services;

public class ImageHandler
{
    private readonly string _finalFolderPath;

    public ImageHandler(string finalFolderPath)
    {
        _finalFolderPath = finalFolderPath;
    }

    public static string ValidateImage(IFormFile imageFile, string[] allowedContentTypes, string[] allowedExtensions, long maxSizeInBytes = 5 * 1024 * 1024)
    {
        // Check if the file is null
        if (imageFile == null)
        {
            return "No file was uploaded.";
        }

        // Validate content type
        if (!allowedContentTypes.Contains(imageFile.ContentType))
        {
            return "Invalid file format. Allowed formats are JPG, JPEG, PNG, GIF, and WEBP.";
        }

        // Validate file extension
        string fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return "Invalid file extension. Allowed extensions are .jpg, .jpeg, .png, .gif, and .webp.";
        }

        // Validate file size
        if (imageFile.Length > maxSizeInBytes)
        {
            return "The file size exceeds the maximum allowed limit of 5MB.";
        }

        // If all validations pass, return null (no errors)
        return null;
    }

    public static async Task<string> SaveTempImageAsync(IFormFile imageFile, string tempFolderPath)
    {
        string fileName = Path.GetFileName(imageFile.FileName);
        string savePath = Path.Combine(tempFolderPath, fileName);

        Directory.CreateDirectory(tempFolderPath); // Ensure the directory exists
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return fileName;
    }

    public static string MoveImageToFinalFolder(string fileName, string tempFolderPath, string finalFolderPath)
    {
        // Combine paths for the source (temp) and destination (final)
        string tempPath = Path.Combine(tempFolderPath, fileName);
        string finalPath = Path.Combine(finalFolderPath, fileName); // Correct destination file path

        // Ensure the final folder exists
        Directory.CreateDirectory(finalFolderPath);

        // Move the file
        if (File.Exists(tempPath)) // Check if the source file exists
        {
            File.Move(tempPath, finalPath);
            return fileName; // Return the final file name
        }
        else
        {
            throw new FileNotFoundException($"The source file does not exist: {tempPath}");
        }
    }

    public void DeleteImage(string fileName, string folderPath)
    {
        string filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public async Task<string> SaveFinalImageAsync(IFormFile imageFile)
    {
        // Ensure the final directory exists
        Directory.CreateDirectory(_finalFolderPath);

        // Get the file name and create the save path
        string fileName = Path.GetFileName(imageFile.FileName);
        string savePath = Path.Combine(_finalFolderPath, fileName);

        // Save the file to the final folder
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return fileName; // Return the file name to store in the database
    }

    //public bool IsValidImage(IFormFile imageFile, string[] allowedContentTypes, string[] allowedExtensions)
    //{
    //    return allowedContentTypes.Contains(imageFile.ContentType) &&
    //           allowedExtensions.Contains(Path.GetExtension(imageFile.FileName).ToLower());
    //}
}