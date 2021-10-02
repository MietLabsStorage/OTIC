using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotic.Coders
{
    interface ICoder
    {
        byte[] Encode(byte[] file);

        byte[] Decode(byte[] file, int oldSize);
    }
}
