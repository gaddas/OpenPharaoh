using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPharaoh.Files
{
    public enum Bitmap555Type : uint
    {
        Plain = 0,
        Plain1 = 1,
        Plain10 = 10,
        Plain12 = 12,
        Plain13 = 13,
        PlainCompressed256 = 256,
        PlainCompressed257 = 257,
        PlainCompressed276 = 276,
        Isometric = 30,
    }
}
