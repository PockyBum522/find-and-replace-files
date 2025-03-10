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
        renameFilesRecursively(topLevelDirectory, filenameNeedle, filenameReplace);

        // Depth-first recursively walk all files and replace needle in directories
        replaceInDirectoryNamesRecursively(topLevelDirectory, filenameNeedle, filenameReplace);
    }

    private static void replaceInDirectoryNamesRecursively(string topLevelDirectory, string filenameNeedle, string filenameReplace)
    {
        foreach (var directoryPath in Directory.GetDirectories(topLevelDirectory))
        {
            var directoryName = Path.GetFileName(directoryPath);
            
            if (directoryName.Contains(filenameNeedle, StringComparison.OrdinalIgnoreCase))
            {
                
                var fileParentDirectory = Path.GetDirectoryName(directoryPath);
            
                var newFullPath = Path.Join(fileParentDirectory, directoryName.Replace(filenameNeedle, filenameReplace, StringComparison.InvariantCultureIgnoreCase));
                
                Console.WriteLine();
                Console.WriteLine("Renaming:");
                Console.WriteLine($"{directoryPath} to:");
                Console.WriteLine(newFullPath);
                
                Directory.Move(directoryPath, newFullPath);
            }
        }

        // Now that we've renamed any matching, run recursively so it can act on the new names/paths
        foreach (var directoryPath in Directory.GetDirectories(topLevelDirectory))
        {
            replaceInDirectoryNamesRecursively(directoryPath, filenameNeedle, filenameReplace);
        }
    }

    private static void renameFilesRecursively(string topLevelDirectory, string filenameNeedle, string filenameReplace)
    {
        foreach (var directoryPath in Directory.GetDirectories(topLevelDirectory))
        {
            renameFilesRecursively(directoryPath, filenameNeedle, filenameReplace);
        }

        foreach (var filePath in Directory.GetFiles(topLevelDirectory))
        {
            var fileName = Path.GetFileName(filePath);
            
            if (!fileName.Contains(filenameNeedle, StringComparison.InvariantCultureIgnoreCase)) continue;
            
            // Otherwise, rename file
            
            var fileParentDirectory = Path.GetDirectoryName(filePath);
            
            var newFullPath = Path.Join(fileParentDirectory, fileName.Replace(filenameNeedle, filenameReplace, StringComparison.InvariantCultureIgnoreCase));

            Console.WriteLine();
            Console.WriteLine("Renaming:");
            Console.WriteLine($"{filePath} to:");
            Console.WriteLine(newFullPath);
            
            File.Move(filePath, newFullPath);
        }
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