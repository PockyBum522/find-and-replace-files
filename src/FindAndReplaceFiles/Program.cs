namespace FindAndReplaceFiles;

public static class Program
{
    internal static void Main(string[] args)
    {
        var topLevelDirectory = "/media/secondary/repos/kit-contracting-work/non-contracted work/RunDnc";
        
        var filenameNeedle = "RotovapCSharpPiController";
        var filenameReplace = "RunDnc";
        
        var fileContentNeedle = "RotovapCSharpPiController";
        var fileContentReplace = "RunDnc";    
        
        List<string> validFileExtensions = [".cs", ".sln", ".csproj", ".axaml", ".xml", ".json", ".html", ".manifest", ".plist", ".xib"];
        
        // Clean bin and obj files
        cleanBinObjFoldersRecursively(topLevelDirectory);
        
        // Recursively walk all files and replace found matching content
        replaceContentRecursively(topLevelDirectory, fileContentNeedle, fileContentReplace, validFileExtensions);

        // Recursively walk all files and replace filenames in files only


        // Depth-first recursively walk all files and replace needle in directories
    }

    private static void cleanBinObjFoldersRecursively(string topLevelDirectory)
    {
        foreach (var directoryPath in Directory.GetDirectories(topLevelDirectory))
        {
            cleanBinObjFoldersRecursively(directoryPath);
            
            var directoryName = Path.GetFileName(directoryPath);

            if (directoryName is not ("bin" or "obj")) continue;
            
            Console.WriteLine($"Removing directory {directoryPath}");
                
            Directory.Delete(directoryPath, true);
        }
    }

    private static void replaceContentRecursively(string topLevelDirectory, string fileContentNeedle, string fileContentReplace, List<string> validFileExtensions)
    {
        foreach (var directoryPath in Directory.GetDirectories(topLevelDirectory))
        {
            replaceContentRecursively(directoryPath, fileContentNeedle, fileContentReplace, validFileExtensions);
        }

        foreach (var filePath in Directory.GetFiles(topLevelDirectory))
        {
            if (!fileExtensionIsValid(filePath, validFileExtensions)) continue; 
            
            var fileContent = File.ReadAllText(filePath);
            
            fileContent = fileContent.Replace(fileContentNeedle, fileContentReplace);
            
            File.WriteAllText(filePath, fileContent);
        }
    }

    private static bool fileExtensionIsValid(string filePath, List<string> validFileExtensions)
    {
        var isValid = false;

        foreach (var fileExtension in validFileExtensions)
        {
            if (filePath.EndsWith(fileExtension, StringComparison.InvariantCultureIgnoreCase)) isValid = true;
        }
        
        return isValid;
    }
}