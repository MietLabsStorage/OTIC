using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kotic.Coders;

namespace Kotic
{
    public class BodyFileHeader
    {
        public static readonly int PositionFileNameSize = 6;
        public static readonly int PositionFileName = 7;

        public const int OldSizeBytesCount = 3;
        public const int NewSizeBytesCount = 3;

        private readonly List<byte> _blob;

        public BodyFileHeader(int oldSize, int newSize, string fileName)
        {
            _blob = new List<byte>();
            this.AddOldSize(oldSize)
                .AddNewSize(newSize)
                .AddFileNameSize(fileName)
                .AddFileName(fileName);
        }

        public byte[] Blob => _blob.ToArray();

        private BodyFileHeader AddOldSize(int oldSize)
        {
            var oldSizeBytes = BitConverter.GetBytes(oldSize);
            for (int i = OldSizeBytesCount - 1; i >= 0;  i--)
            {
                _blob.Add(oldSizeBytes[i]);
            }
            return this;
        }

        private BodyFileHeader AddNewSize(int newSize)
        {
            var newSizeBytes = BitConverter.GetBytes(newSize);
            for (int i = NewSizeBytesCount - 1; i >= 0; i--)
            {
                _blob.Add(newSizeBytes[i]);
            }
            return this;
        }

        private BodyFileHeader AddFileNameSize(string fileName)
        {
            var fileNameSize = BitConverter.GetBytes(fileName.Length);
            _blob.Add(fileNameSize.First());
            return this;
        }

        private BodyFileHeader AddFileName(string fileName)
        {
            _blob.AddRange(Encoding.ASCII.GetBytes(fileName));
            return this;
        }
    }

    class BodyFile
    {
        private readonly List<byte> _blob;

        public BodyFile(FileInfo fileInfo, string path)
        {
            _blob = new List<byte>();

            ICoder coder = new DefaultCoder();

            var file = File.ReadAllBytes(fileInfo.FullName);
            var encodedFile = coder.Encode(file);
            var fileName = Path.Combine(path, fileInfo.Name);

            this.AddFileHeader(file.Length, encodedFile.Length, fileName)
                .AddFileBlob(file);
        }

        public byte[] Blob => _blob.ToArray();

        private BodyFile AddFileHeader(int oldSize, int newSize, string fileName)
        {
            _blob.AddRange(new BodyFileHeader(oldSize, newSize, fileName).Blob);
            return this;
        }

        private BodyFile AddFileBlob(byte[] fileBlob)
        {
            _blob.AddRange(fileBlob);
            return this;
        }
    }
}
