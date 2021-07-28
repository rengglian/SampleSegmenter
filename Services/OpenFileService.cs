using Microsoft.Win32;
using SampleSegmenter.Interfaces;

namespace SampleSegmenter.Services
{
    public class OpenFileService : IOpenFileService
    {
        OpenFileDialog _openFileDialog = new OpenFileDialog();
        string[] _selectedFileNames;

        public bool? OpenFile()
        {
            _openFileDialog.InitialDirectory = "c:\\";
            _openFileDialog.Filter = "Image Files(*.JPEG;*.JPG;*.PNG;*.BMP)|*.JPEG;*.JPG;*.PNG;*.BMP|All files (*.*)|*.*";
            _openFileDialog.FilterIndex = 1;
            _openFileDialog.RestoreDirectory = true;

            var choosenFile = _openFileDialog.ShowDialog();
            if (choosenFile.HasValue && choosenFile.Value)
            {
                _selectedFileNames = _openFileDialog.FileNames;
            }
            return choosenFile;
        }

        public string[] FileNames
        {
            get { return _selectedFileNames; }
        }
    }
}
