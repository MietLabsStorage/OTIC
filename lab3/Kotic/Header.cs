using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kotic
{
    public class Header
    {
        public static readonly int[] PositionSignature = {0, 1, 2, 3, 4};
        public static readonly int PositionVersion = 5;
        public static readonly int PositionSubversion = 6;
        public static readonly int PositionFilesCount = 11;
        public static readonly int[] PositionArchiveSize = {12, 13, 14, 15};

        private readonly List<byte> _blob;

        public Header()
        {
            _blob = new List<byte>();
            this.AddSignature()
                .AddVersion()
                .AddSubversion()
                .AddReserve(1)
                .AddCWC()
                .AddCC()
                .AddAIP()
                .AddFilesCount()
                .AddArchiveSize();
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

        private Header AddReserve(int count)
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

        private Header AddArchiveSize()
        {
            _blob.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
            return this;
        }

        public void IncFilesCount()
        {
            _blob[PositionFilesCount] += 0x01;
        }
    }
}
