using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace OpenPharaoh.Files
{
    public class SG3
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
            public Bitmap555Type Type;
            public byte ExternalFlag;

            public Bitmap Bitmap;

            public byte[] Data;
        }

        public SG3()
        {
        }

        public SG3(string filename)
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
                        //OFFSET  TYPE  DESCRIPTION
                        //   0    uint  Offset into the .555 file. Of note: if it's an external .555 file,
                        //              you have to subtract one from this offset!
                        //   4    uint  Length of the image data (total)
                        //   8    uint  Length of the compressed image data
                        //  12    uint  (unknown, zero bytes)
                        //  16    uint  Invert offset. For sprites (walkers), the image of the walker
                        //              moving to the left is the mirrored version of the image of the
                        //              walker going right. If this is set, copy the image at the index
                        //              (current - invert offset) and mirror it vertically.
                        //  20   ushort Width of the image
                        //  22   ushort Height of the image
                        //  24    byte* 6 unknown bytes. At least the first 4 bytes are 2 shorts
                        //  30   ushort Number of frames per animation (ex. 12 frames in the 'Walking North' animation)
                        //  32   ushort Number of animations (There is an animation of the walker walking north, south, etc...)
                        //  34   short  animation offset
                        //  36   short  animation offset
                        //  38    byte* 12 unknown bytes.
                        //  50   ushort Type of image. See below.
                        //  52    byte  Flag: 0 = use internal .555, 1 = use external .555 (defined in bitmap)
                        //  53    byte* 3 bytes, 3 unknown flags
                        //  56    byte  Bitmap ID (not always filled in correctly for SG3 files)
                        //  57    byte* 7 bytes, unknown

                        //-- For sg3 files with alpha masks: --
                        //  64    uint  Offset of alpha mask
                        //  68    uint  Length of alpha mask data

                        var offset = b.ReadUInt32();                        // 0
                        var imageDataTotalLength = b.ReadUInt32();          // 4
                        var imageDataCompressedLenght = b.ReadUInt32();     // 8
                        var unknown12 = b.ReadUInt32();                     // 12
                        var offsetInvert = b.ReadUInt32();                  // 16
                        var width = b.ReadUInt16();                         // 20
                        var height = b.ReadUInt16();                        // 22

                        var unknown24 = b.ReadBytes(6);                     // 24
                        var totalFramesPerAnimation = b.ReadUInt16();       // 30
                        var totalAnimations = b.ReadUInt16();               // 32
                        var unknown32 = b.ReadBytes(12 + 2 + 2);            // 34
                        var type = b.ReadUInt16();                          // 50
                        var externalFlag = b.ReadByte();                    // 52
                        var unknown53 = b.ReadBytes(3);                     // 53
                        var bitmapId = b.ReadByte();                        // 56
                        var unknown57 = b.ReadBytes(7);                     // 57

                        f.Seek(-(8 * 4 + 32), SeekOrigin.Current);
                        var data = b.ReadBytes(32 + 8 * 4);

                        if (i <= this.TotalBitmaps)
                        {
                            var sg2Bitmap = new Sg3Bitmap();
                            sg2Bitmap.Offset = offset;
                            sg2Bitmap.Width = width;
                            sg2Bitmap.Height = height;
                            sg2Bitmap.Type = (Bitmap555Type)type;
                            sg2Bitmap.ExternalFlag = externalFlag;
                            sg2Bitmap.Data = data;

                            // bitmap is valid
                            if (width != 0 && height != 0)
                            {
                                sg2Bitmap.Bitmap = Bitmap555.Load((Bitmap555Type)type, this.Filename555, width, height, offset);
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
