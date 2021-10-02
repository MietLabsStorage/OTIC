using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotic.Coders
{
    class DefaultCoder: ICoder
    {
        public byte[] Encode(byte[] file)
        {
            return file;
        }

        public byte[] Decode(byte[] archive)
        {
            return archive;
        }
    }
}
