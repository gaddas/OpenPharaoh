using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace OpenPharaoh
{
    public class Sg3
    {
        public const uint OFFSET_COUNTS_DATA = 0x50;
        public const uint OFFSET_FILES_DATA = 0x2A8;
        public const uint OFFSET_BITMAP_DATA = 0x9F28;
        public const uint OFFSET_SPRITE_DATA = 0xA62E8;

        public const ushort MAX_SPRITES = 300;
        public const ushort MAX_FILES = 200;
        public const ushort MAX_BITMAPS = 10000;

        public string FilenameSG3;
        public string Filename555;

        public uint TotalBitmaps = 0;
        public uint TotalFiles1 = 0;
        public uint TotalFiles2 = 0;
        public uint FileSize555 = 0;

        public Dictionary<string, Sg3File> Files = new Dictionary<string, Sg3File>();
        public Dictionary<ushort, Sg3Bitmap> Bitmaps = new Dictionary<ushort, Sg3Bitmap>();

        public ushort[] SpriteBitmapIndex = new ushort[MAX_SPRITES];
        public string[] SpriteNames = new string[MAX_SPRITES];

        public class Sg3File
        {
            public string Filename;
            public string Description;
        }

        public class Sg3Bitmap
        {
            public uint Offset;
            public ushort Width;
            public ushort Height;

            public Bitmap Bitmap;

            public byte[] Unknown;
        }

        public Sg3()
        {

        }

        public Sg3(string filename)
        {
            this.FilenameSG3 = filename;
            this.Filename555 = filename.Replace(".sg3", ".555");

            using (var f = new FileStream(filename, FileMode.Open))
            {
                using (var b = new BinaryReader(f))
                {
                    var header0x00 = b.ReadUInt32();        // 0x0000: 28 9B 0A 00
                    var header0x04 = b.ReadUInt32();        // 0x0004: D5 00 00 00
                    var header0x08 = b.ReadUInt32();        // 0x0008: it's some number
                    var header0x0C = b.ReadUInt32();        // 0x000C: 10 27 00 00

                    this.TotalBitmaps = b.ReadUInt32();     // 0x0010: total bitmaps count
                    this.TotalFiles1 = b.ReadUInt32();      // 0x0014: total files count
                    this.TotalFiles2 = b.ReadUInt32();      // 0x0018: total user files count

                    var header0x1C = b.ReadInt32();        // 0x001C: 

                    this.FileSize555 = b.ReadUInt32();      // 0x0020: *.555 file's size - 4

                    var header0x24 = b.ReadInt32();        // 0x0024: 
                    var header0x28 = b.ReadBytes(40);      // 0x0028: 40 bytes empty

                    for (int i = 0; i < MAX_SPRITES; i++)
                    {
                        this.SpriteBitmapIndex[i] = b.ReadUInt16();
                    }

                    for (int i = 0; i < MAX_FILES; i++)
                    {
                        var cname = b.ReadChars(65);
                        var cdescription = b.ReadChars(51);
                        var data = b.ReadBytes(84);

                        var name = new String(cname, 0, Array.IndexOf(cname, '\0'));
                        var description = new String(cdescription, 0, Array.IndexOf(cdescription, '\0'));

                        if (!string.IsNullOrEmpty(name))
                        {
                            var sg2File = new Sg3File();
                            sg2File.Description = description;
                            sg2File.Filename = name;

                            this.Files.Add(name.Trim(), sg2File);
                        }
                    }

                    for (ushort i = 0; i < MAX_BITMAPS; i++)
                    {
                        var offset = b.ReadUInt32();
                        var value1 = b.ReadUInt32();
                        var value2 = b.ReadUInt32();
                        var value3 = b.ReadUInt32();
                        var value4 = b.ReadUInt32();

                        var width = b.ReadUInt16();
                        var height = b.ReadUInt16();

                        var value5 = b.ReadUInt16();
                        var value6 = b.ReadUInt16();
                        var value7 = b.ReadUInt32();
                        var rest = b.ReadBytes(32);

                        f.Seek(-(8 * 4 + 32), SeekOrigin.Current);
                        var unknown = b.ReadBytes(32 + 8 * 4);

                        if (i <= this.TotalBitmaps)
                        {
                            var sg2Bitmap = new Sg3Bitmap();
                            sg2Bitmap.Offset = offset;
                            sg2Bitmap.Width = width;
                            sg2Bitmap.Height = height;
                            sg2Bitmap.Unknown = unknown;

                            // bitmap is valid
                            if (width != 0 && height != 0)
                            {
                                sg2Bitmap.Bitmap = FileHelper.LoadBitmapFrom555(this.Filename555, (int)width, (int)height, (int)offset);
                                //sg2Bitmap.Bitmap.Save(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Test\" + i.ToString() + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                            }

                            this.Bitmaps.Add(i, sg2Bitmap);
                        }
                    }

                    for (int i = 0; i < MAX_SPRITES; i++)
                    {
                        var cname = b.ReadChars(48);
                        this.SpriteNames[i] = new String(cname, 0, Array.IndexOf(cname, '\0'));
                    }
                }
            }
        }
    }
}
