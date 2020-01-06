namespace Seraf.XNA.FileMenu
{
    public class FileMenuArgs
    {
        public FileMenuArgs(int selectedFile, FileMenuChoice fileMenuChoice)
        {
            this.SelectedIndex = selectedFile;
            this.FileMenuChoice = fileMenuChoice;
        }

        public FileMenuChoice FileMenuChoice { get; }
        public int SelectedIndex { get; }
    }
}
