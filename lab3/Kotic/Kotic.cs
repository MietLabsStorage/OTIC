using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotic
{
    public static class Kotic
    {
        public static readonly byte CurrentVersion = 0x01;
        public static readonly byte CurrentSubversion = 0x06;
        public static readonly byte[] Signature = new byte[] { 0x6b, 0x6f, 0x74, 0x69, 0x63 };

        public static void CheckSignature(byte[] signature)
        {
            bool check = signature.Length == Signature.Length;
            if (check)
            {
                for (int i = 0; i < Signature.Length; i++)
                {
                    check &= signature[i] == Signature[i];
                }
            }

            if (!check)
            {
                throw new Exception("Not kotic file");
            }
        }

        public static void CheckVersion(byte version, byte subversion)
        {
            if (version != CurrentVersion || subversion != CurrentSubversion)
            {
                throw new Exception("Bad version");
            }
        }
    }
}
