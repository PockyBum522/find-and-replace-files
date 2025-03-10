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
        
        // Clean bin and obj files
        cleanBinObjFoldersRecursively(topLevelDirectory);
        
        // Recursively walk all files and replace found matching content
        replaceContentRecursively(topLevelDirectory, fileContentNeedle, fileContentReplace);

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

    private static void replaceContentRecursively(string topLevelDirectory, string fileContentNeedle, string fileContentReplace)
    {
        foreach (var directoryPath in Directory.GetDirectories(topLevelDirectory))
        {
            replaceContentRecursively(directoryPath, fileContentNeedle, fileContentReplace);
        }

        foreach (var filePath in Directory.GetFiles(topLevelDirectory))
        {
            
        }
    }
}