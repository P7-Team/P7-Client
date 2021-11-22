using System.IO;

public class PathHelper
{
    private const char EXTENSION_SEPERATOR = '.';
    
    public static string ExtractFileNameFromPath(string absolutePath, bool removeFileExtension=false)
    {
        char seperator = Path.DirectorySeparatorChar;
        int filenameBeginIndex = absolutePath.LastIndexOf(seperator) + 1;
        int filenameEndIndex = absolutePath.Length;

        int extLastIdx = absolutePath.LastIndexOf(EXTENSION_SEPERATOR);
        if (removeFileExtension &&
            filenameBeginIndex < extLastIdx) // Only if last '.' is after index where filename begins
        {
            filenameEndIndex = extLastIdx;
            
        }
            
        return absolutePath[filenameBeginIndex..filenameEndIndex];
    }
}