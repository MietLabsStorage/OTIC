using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotic.Coders
{
    public class Hamming: ICoder
    {
        public byte[] Decode(byte[] file, byte[] info, int oldSize)
        {
            List<byte> blob = new List<byte>();

            return blob.ToArray();
        }

        public (byte[] blob, byte[] info) Encode(byte[] file)
        {
            List<byte> blob = new List<byte>();
            List<byte> info = new List<byte>() { 0x00, 0x00 };

            var bitsArr = new BitArray(file);
            var inputBits = new List<bool>();
            foreach (var bit in bitsArr)
            {
                inputBits.Add((bool)bit);
            }

            var controlCount = (int)Math.Ceiling(Math.Log2(inputBits.Count + 1)) + 1;
            for (int i = 1; i <= controlCount; i++)
            {
                inputBits.Insert((int)(Math.Pow(2, i - 1) - 1), false);
            }

            var table = new bool[controlCount, inputBits.Count];

            for (int i = 0; i < controlCount; i++)
            {
                var id = 0;
                while (id < (int)Math.Pow(2, i) - 1)
                {
                    table[i, id] = false;
                    id++;
                }

                var doing = true;
                while (doing)
                {
                    for (int k = 0; k < (int)Math.Pow(2, i); k++)
                    {
                        if (id >= inputBits.Count)
                        {
                            doing = false;
                            break;
                        }
                        table[i, id] = true;
                        id++;
                    }
                    for (int k = 0; k < (int)Math.Pow(2, i); k++)
                    {
                        if (id >= inputBits.Count)
                        {
                            doing = false;
                            break;
                        }
                        table[i, id] = false;
                        id++;
                    }
                }
            }

            var controlLine = new List<bool>();

            for (int i = 0; i < controlCount; i++)
            {
                var res = 0;
                for (int j = 0; j < inputBits.Count; j++)
                {
                    res += (inputBits[j] ? 1 : 0) * (table[i, j] ? 1 : 0);
                }
                controlLine.Add(res % 2 == 1 ? true : false);
            }

            for (int i = 1; i <= controlCount; i++)
            {
                inputBits[(int)(Math.Pow(2, i - 1) - 1)] = controlLine[i - 1];
            }

            var listTable = new List<bool>();
            for(int i = 0; i < table.Length; i++)
            {
                listTable.Add(table[i / inputBits.Count, i % inputBits.Count]);
            }
            var bitTable = new BitArray(listTable.ToArray());
            byte[] byteTable = new byte[bitTable.Length / 8 + 1];
            bitTable.CopyTo(byteTable, 0);

            return (blob.ToArray(), info.ToArray());
        }
    }
}
