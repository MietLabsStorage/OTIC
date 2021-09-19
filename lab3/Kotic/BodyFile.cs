using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kotic
{
    class BodyFile
    {
        private readonly List<byte> _blob;
        private readonly FileInfo _fileInfo;

        public BodyFile(FileInfo fileInfo)
        {
            _blob = new List<byte>();
            _fileInfo = fileInfo;

            this.AddFilename()
                .AddFileBlob();
        }

        public byte[] FileNameByBytes => Encoding.ASCII.GetBytes(_fileInfo.Name);
        public byte[] Blob => _blob.ToArray();

        private BodyFile AddFilename()
        {
            _blob.AddRange(FileNameByBytes);
            return this;
        }

        private BodyFile AddFileBlob()
        {
            using (var stream = _fileInfo.OpenRead())
            {
                byte[] file = new byte[_fileInfo.Length];
                stream.Read(file, 0, file.Length);
                _blob.AddRange(file);
            }
            return this;
        }
    }
}
