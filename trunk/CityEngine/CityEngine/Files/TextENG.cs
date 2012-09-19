// -----------------------------------------------------------------------
// <copyright file="ENG.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace CityEngine.Files
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    public class TextENG
    {
        public TextENG()
        {
        }

        public TextENG(string filename)
        {
            using (var f = new FileStream(filename, FileMode.Open))
            {
                using (var b = new BinaryReader(f, Encoding.Default))
                {
                    //OFFSET  TYPE  DESCRIPTION
                    // 0    char*   Pharaoh textfile
                    //16    uint    Version number, always 0xd3 for C3's sg2 files

                    var header0x00 = new string(b.ReadChars("Pharaoh textfile".Length));
                    if (header0x00 != "Pharaoh textfile") throw new FormatException(string.Format("The file {0} is not in Pharaoh Textfile format.", filename));

                    var header0x10 = b.ReadUInt32();        // 0x0000: 28 9B 0A 00
                    var header0x14 = b.ReadUInt32();        // 0x0004: D5 00 00 00
                    var header0x18 = b.ReadUInt32();        // 0x0008: it's some number
                }
            }
        }
    }
}
