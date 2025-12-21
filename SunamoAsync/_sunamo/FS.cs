namespace SunamoAsync._sunamo;

//namespace SunamoAsync._sunamo.SunamoExceptions._AddedToAllCsproj;
internal class FS
{
    internal static void CreateUpfoldersPsysicallyUnlessThere(string path)
    {
        CreateFoldersPsysicallyUnlessThere(Path.GetDirectoryName(path));
    }
    internal static void CreateFoldersPsysicallyUnlessThere(string path)
    {
        ThrowEx.IsNullOrEmpty("path", path);
        //ThrowEx.IsNotWindowsPathFormat("path", path);
        if (Directory.Exists(path))
        {
            return;
        }
        List<string> foldersToCreate = new List<string>
{
path
};
        while (true)
        {
            path = Path.GetDirectoryName(path);

            if (Directory.Exists(path))
            {
                break;
            }
            foldersToCreate.Add(path);
        }
        foldersToCreate.Reverse();
        foreach (string item in foldersToCreate)
        {
            if (!Directory.Exists(item))
            {
                Directory.CreateDirectory(item);
            }
        }
    }
}