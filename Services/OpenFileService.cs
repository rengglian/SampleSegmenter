using Microsoft.Win32;
using Prism.Mvvm;
using SampleSegmenter.Interfaces;
using System.IO;

namespace SampleSegmenter.Services
{
    public class OpenFileService : BindableBase, IOpenFileService
    {
        readonly OpenFileDialog _openFileDialog = new();
        string[] _selectedFileNames;

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        public bool? OpenFile()
        {
            _openFileDialog.Filter = "Image Files(*.JPEG;*.JPG;*.PNG;*.BMP)|*.JPEG;*.JPG;*.PNG;*.BMP|All files (*.*)|*.*";
            _openFileDialog.FilterIndex = 1;
            _openFileDialog.RestoreDirectory = true;

            var choosenFile = _openFileDialog.ShowDialog();
            if (choosenFile.HasValue && choosenFile.Value)
            {
                _selectedFileNames = _openFileDialog.FileNames;
                FileName = _selectedFileNames[0];
            }
            return choosenFile;
        }

        public string[] FileNames
        {
            get { return _selectedFileNames; }
        }

        public string FileNameOnly
        {
            get { return Path.GetFileNameWithoutExtension(_selectedFileNames[0]); }
        }
    }
}
