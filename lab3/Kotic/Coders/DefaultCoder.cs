using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotic.Coders
{
    class DefaultCoder: ICoder
    {
        public byte[] Decode(byte[] file, byte[] info, int oldSize)
        {
            if(info.Length != 0)
            {
                List<byte> newFile = new List<byte>(file);
                for (int i = 0; i < info.Length; i++)
                {
                    newFile.RemoveAt((int)info[i] - i);
                }

                return newFile.ToArray();
            }
            return file;
        }

        public (byte[] blob, byte[] info) Encode(byte[] file)
        {
            List<byte> blob = new List<byte>();
            List<byte> info = new List<byte>() {0x00, 0x00};
            
            int count = 0;
            int last = 0;
            byte index = 0x00;

            var rnd = new Random();
            for (int i = 0; i < file.Length; i++)
            {
                if (count == 0)
                {
                    count = rnd.Next(1, 16);
                }

                if (i - last == count)
                {
                    info.Add((byte)blob.Count);
                    info.Add((byte) (blob.Count + 1));
                    blob.Add(index);
                    blob.Add(BitConverter.GetBytes('q')[0]);
                    index += 0x01;
                    last = i;
                }

                blob.Add(file[i]);
            }

            var infoSize = info.Count - 2;
            var infoSizeBytes = BitConverter.GetBytes(infoSize);

            info[0] = infoSizeBytes[0];
            info[1] = infoSizeBytes[1];

            return (blob.ToArray(), info.ToArray());
        }
    }
}
