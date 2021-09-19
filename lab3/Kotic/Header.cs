using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kotic
{
    public class Header
    {
        public const int SizeBytesCount = 2;
        public const int StartBytesCount = 2;
        public const int PositionFilesCount = 11;
        public static readonly int[] PositionArchiveSize = {12, 13, 14, 15};

        private long _offset;

        private readonly List<byte> _blob;

        public Header()
        {
            _blob = new List<byte>();
            this.AddSignature()
                .AddVersion()
                .AddSubversion()
                .Reserv(1)
                .AddCWC()
                .AddCC()
                .AddAIP()
                .AddFilesCount()
                .AddSize();

            _offset = 16;
        }

        public byte[] Blob => _blob.ToArray();

        private Header AddSignature()
        {
            _blob.AddRange(new byte[] {0x6b, 0x6f, 0x74, 0x69, 0x63});
            return this;
        }

        private Header AddVersion()
        {
            _blob.Add(0x01);
            return this;
        }

        private Header AddSubversion()
        {
            _blob.Add(0x03);
            return this;
        }

        private Header Reserv(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _blob.Add(0x00);
            }
            return this;
        }

        /// <summary>
        /// code without context
        /// </summary>
        /// <returns></returns>
        private Header AddCWC()
        {
            _blob.Add(0x00);
            return this;
        }

        /// <summary>
        /// code with context
        /// </summary>
        /// <returns></returns>
        private Header AddCC()
        {
            _blob.Add(0x00);
            return this;
        }

        /// <summary>
        /// anti-interference protection
        /// </summary>
        /// <returns></returns>
        private Header AddAIP()
        {
            _blob.Add(0x00);
            return this;
        }

        private Header AddFilesCount()
        {
            _blob.Add(0x00);
            return this;
        }

        private Header AddSize()
        {
            _blob.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            return this;
        }

        private void IncFilesCount()
        {
            _blob[PositionFilesCount] += 0x01;
        }

        public void AddFileInfo(FileInfo fileInfo)
        {
            List<byte> size = new List<byte>(new byte[] { 0x00, 0x00 });
            var fileSize = BitConverter.GetBytes(fileInfo.Length);
            Array.Reverse(fileSize);
            size.AddRange(fileSize);
            for (int i = 0; i < SizeBytesCount; i++)
            {
                _blob.Add(size[size.Count - 1 - i]);
            }

            _offset += SizeBytesCount + StartBytesCount + Encoding.ASCII.GetBytes(fileInfo.Name).Length;
            List<byte> start = new List<byte>(new byte[] { 0x00, 0x00 });
            var fileStart = BitConverter.GetBytes(_offset);
            Array.Reverse(fileStart);
            size.AddRange(fileStart);
            for (int i = 0; i < StartBytesCount; i++)
            {
                _blob.Add(size[size.Count - 1 - i]);
            }

            _offset += fileInfo.Length;
            IncFilesCount();
        }
    }
}
