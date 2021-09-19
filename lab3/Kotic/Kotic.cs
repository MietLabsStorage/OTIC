using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Kotic
{
    public class Kotic
    {
        private readonly Header _header;
        private readonly Body _body;

        public Kotic(string filename)
        {
            _header = new Header();
            _body = new Body();

            FileInfo fileInfo = new FileInfo(filename);
            _header.AddFileInfo(fileInfo);
            _body.AddBodyFile(fileInfo);

            _header.AddFileInfo(fileInfo);
            _body.AddBodyFile(fileInfo);

            _header.AddFileInfo(fileInfo);
            _body.AddBodyFile(fileInfo);
        }

        public string Extension => "kotic";

        public byte[] Blob()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(_header.Blob);
            bytes.AddRange(_body.Blob);
            return bytes.ToArray();
        } 

        public void GenerateArchive(string generatePath)
        {
            string filename = $"{generatePath}\\{"kotic"}.{Extension}";

            var blob = Blob();
            UpdateArchiveSize(ref blob);

            File.WriteAllBytes(filename, blob);
        }

        private void UpdateArchiveSize(ref byte[] blob)
        {
            var blobSize = blob.Length;

            List<byte> size = new List<byte>(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            var archiveSize = BitConverter.GetBytes(blobSize);
            Array.Reverse(archiveSize);
            size.AddRange(archiveSize);
            for (int i = 0; i < Header.PositionArchiveSize.Length; i++)
            {
                blob[Header.PositionArchiveSize[i]] = archiveSize[i];
            }
        }
    }
}
