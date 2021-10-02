using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kotic
{
    class Body
    {
        private readonly List<byte> _blob;

        public Body()
        {
            _blob = new List<byte>();
        }

        public byte[] Blob => _blob.ToArray();

        public void AddBodyFile(FileInfo fileInfo, string path)
        {
            _blob.AddRange(new BodyFile(fileInfo, path).Blob);
        }
    }
}
