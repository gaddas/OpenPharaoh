// -----------------------------------------------------------------------
// <copyright file="ContainerSG3.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace CityEngine.Files
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ContainerSG3
    {
        public const uint OFFSET_COUNTS_DATA = 0x50;
        public const uint OFFSET_FILES_DATA = 0x2A8;
        public const uint OFFSET_BITMAP_DATA = 0x9F28;
        public const uint OFFSET_SPRITE_DATA = 0xA62E8;

        public static ushort MAX_SPRITES = 300;
        public static ushort MAX_FILES = 200;
        public static ushort MAX_IMAGES = 10000;

        public string FilenameSG3;
        public string Filename555;

        public uint MaxImages = 0;
        public uint TotalImages = 0;
        public uint TotalFiles1 = 0;
        public uint TotalFiles2 = 0;

        public List<Sg3File> Files = new List<Sg3File>();
        public Dictionary<ushort, Sg3Image> Images = new Dictionary<ushort, Sg3Image>();

        public ushort[] SpriteImageIndex = new ushort[MAX_SPRITES];
        public string[] SpriteNames = new string[MAX_SPRITES];

        public class Sg3File
        {
            public string Filename;
            public string Description;
            public uint FirstImage;
            public uint LastImage;
            public uint Count;
        }

        public class Sg3Image
        {
            public uint Offset;
            public uint DataLenghtTotal;
            public uint DataLengthTiles;
            public ushort Width;
            public ushort Height;
            public Bitmap555Type Type;
            public byte ExternalFlag;
            public byte TileSize;

            public System.Drawing.Bitmap Plain;
            public List<System.Drawing.Bitmap> Tiles = new List<System.Drawing.Bitmap>();

            public byte[] Data;
        }

        public ContainerSG3()
        {
        }

        public ContainerSG3(string filename)
        {
            this.FilenameSG3 = filename;
            this.Filename555 = Path.ChangeExtension(filename, ".555");

            if (Path.GetExtension(filename).ToUpper() == ".SG2")
            {
                MAX_FILES = 100;
            }

            using (var f = new FileStream(filename, FileMode.Open))
            {
                using (var b = new BinaryReader(f, Encoding.ASCII))
                {
                    //OFFSET  TYPE  DESCRIPTION
                    // 0    uint  For some SG's, the filesize, for others, it's a fixed value
                    // 4    uint  Version number, always 0xd3 for C3's sg2 files
                    // 8    uint  (unknown)
                    //12    int   Maximum number of image records in this file
                    //16    int   Number of image records in use
                    //20    int   Number of bitmap records in use (maximum is 100 for SG2, 200 for SG3
                    //24    int   Unknown: appears to be the number of bitmap records minus one
                    //28    uint  Total filesize of all loaded graphics (.555 files)
                    //32    uint  Filesize of the .555 file that "belongs" to this .sg2: the one with the same name
                    //36    uint  Filesize of any images pulled from external .555 files

                    var header0x00 = b.ReadUInt32();        // 0x0000: 28 9B 0A 00
                    var header0x04 = b.ReadUInt32();        // 0x0004: D5 00 00 00
                    var header0x08 = b.ReadUInt32();        // 0x0008: it's some number

                    this.MaxImages = b.ReadUInt32();        // 0x000C: max images count
                    this.TotalImages = b.ReadUInt32();      // 0x0010: total images count
                    this.TotalFiles1 = b.ReadUInt32();      // 0x0014: total files count
                    this.TotalFiles2 = b.ReadUInt32();      // 0x0018: total user files count

                    var fileSize555_All = b.ReadInt32();        // 0x001C: total filesize of all loaded 555 files
                    var fileSize555_This = b.ReadUInt32();      // 0x0020: *.555 file's size - 4
                    var fileSize555_External = b.ReadInt32();   // 0x0024: total filesize of all external 555 files

                    var header0x28 = b.ReadBytes(40);      // 0x0028: 40 bytes empty

                    for (int i = 0; i < MAX_SPRITES; i++)
                    {
                        this.SpriteImageIndex[i] = b.ReadUInt16();
                    }

                    for (int i = 0; i < MAX_FILES; i++)
                    {
                        //OFFSET  TYPE    DESCRIPTION
                        //  0    string  65 bytes, the filename of the .bmp file. This translates to the
                        //               name of the .555 file, if it's external.
                        // 65    string  51 bytes, comment for this bitmap record
                        //116    int     Width of some image in the bitmap (don't pay much attention to this)
                        //120    int     Height of some image in the bitmap (idem)
                        //124    uint    Number of images in this bitmap
                        //128    uint    Index of the first image of this bitmap
                        //132    uint    Index of the last image of this bitmap
                        //136    byte*   64 bytes, unkwnown

                        var cname = b.ReadChars(65);                // 0
                        var cdescription = b.ReadChars(51);         // 65
                        var width = b.ReadUInt32();                 // 116
                        var height = b.ReadUInt32();                // 120
                        var imagesCount = b.ReadUInt32();           // 124
                        var imagesFirst = b.ReadUInt32();           // 128
                        var imagesLast = b.ReadUInt32();            // 132
                        var data = b.ReadBytes(64);                 // 136

                        var name = new String(cname, 0, Array.IndexOf(cname, '\0'));
                        var description = new String(cdescription, 0, Array.IndexOf(cdescription, '\0'));

                        if (!string.IsNullOrEmpty(name))
                        {
                            var sg2File = new Sg3File();
                            sg2File.Description = description;
                            sg2File.Filename = name;
                            sg2File.LastImage = imagesLast;
                            sg2File.FirstImage = imagesFirst;
                            sg2File.Count = imagesCount;

                            this.Files.Add(sg2File);
                        }
                    }

                    for (ushort i = 0; i < this.MaxImages; i++)
                    {
                        //OFFSET  TYPE  DESCRIPTION
                        //   0    uint  Offset into the .555 file. Of note: if it's an external .555 file,
                        //              you have to subtract one from this offset!
                        //   4    uint  Length of the image data (total)
                        //   8    uint  Length of the image data (tiles)
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
                        //  53    byte* 2 bytes, 2 unknown flags
                        //  55    byte  Tile Size for isometric data
                        //  56    byte  Bitmap ID (not always filled in correctly for SG3 files)
                        //  57    byte* 7 bytes, unknown

                        //-- For sg3 files with alpha masks: --
                        //  64    uint  Offset of alpha mask
                        //  68    uint  Length of alpha mask data

                        var offset = b.ReadUInt32();                        // 0
                        var imageDataTotalLength = b.ReadUInt32();          // 4
                        var imageDataTilesLenght = b.ReadUInt32();          // 8
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
                        var unknown53 = b.ReadBytes(2);                     // 53
                        var tileSize = b.ReadByte();                        // 55
                        var bitmapId = b.ReadByte();                        // 56
                        var unknown57 = b.ReadBytes(7);                     // 57

                        f.Seek(-(8 * 4 + 32), SeekOrigin.Current);
                        var data = b.ReadBytes(32 + 8 * 4);

                        if (i <= this.TotalImages)
                        {
                            var sg2Bitmap = new Sg3Image();
                            sg2Bitmap.Offset = offset;
                            sg2Bitmap.DataLenghtTotal = imageDataTotalLength;
                            sg2Bitmap.DataLengthTiles = imageDataTilesLenght;
                            sg2Bitmap.Width = width;
                            sg2Bitmap.Height = height;
                            sg2Bitmap.Type = (Bitmap555Type)type;
                            sg2Bitmap.ExternalFlag = externalFlag;
                            sg2Bitmap.TileSize = tileSize;

                            sg2Bitmap.Data = data;

                            // bitmap is valid
                            if (width != 0 && height != 0)
                            {
                                if (sg2Bitmap.Type == Bitmap555Type.Isometric)
                                {
                                    if (externalFlag != 0) throw new NotImplementedException("Not able to read external tile images");

                                    for (uint tile = 0; tile < tileSize * tileSize; tile++)
                                    {
                                        var bitmap = Bitmap555.LoadIsometricBitmap(this.Filename555, width, height, offset + tile * Bitmap555.TILE_BYTES);
                                        sg2Bitmap.Tiles.Add(bitmap);
                                    }

                                    if (imageDataTotalLength - imageDataTilesLenght > 0)
                                    {
                                        var poffset = offset + imageDataTilesLenght;
                                        var plength = imageDataTotalLength - imageDataTilesLenght;
                                        sg2Bitmap.Plain = Bitmap555.LoadPlainCompressedBitmap(this.Filename555, width, height, poffset, plength);
                                    }
                                }
                                else
                                {
                                    if (externalFlag == 0)
                                    {
                                        sg2Bitmap.Plain = Bitmap555.Load((Bitmap555Type)type, this.Filename555, width, height, offset);
                                    }
                                    else
                                    {
                                        var file = this.Files.First(ff => ff.FirstImage <= i && ff.LastImage >= i);
                                        var externalFilename = Path.ChangeExtension(Path.GetDirectoryName(filename) + "\\" + file.Filename, ".555");

                                        if (!File.Exists(externalFilename))
                                        {
                                            externalFilename = Path.ChangeExtension(Path.GetDirectoryName(filename) + "\\555\\" + file.Filename, ".555");
                                        }

                                        sg2Bitmap.Plain = Bitmap555.Load((Bitmap555Type)type, externalFilename, width, height, offset - 1);
                                    }
                                }
                                //sg2Bitmap.Plain.Save(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Test\" + i.ToString() + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                            }

                            this.Images.Add(i, sg2Bitmap);
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

        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <param name="imageIndex">Index of the image.</param>
        public Microsoft.Xna.Framework.Graphics.Texture2D LoadTexture2d(Microsoft.Xna.Framework.Graphics.GraphicsDevice device, ushort imageIndex)
        {
            var image = this.Images[imageIndex];
            var bitmap = image.Plain;

            uint[] imgData = new uint[bitmap.Width * bitmap.Height];
            Microsoft.Xna.Framework.Graphics.Texture2D texture = new Microsoft.Xna.Framework.Graphics.Texture2D(device, bitmap.Width, bitmap.Height);

            unsafe
            {
                BitmapData origdata = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
                uint* byteData = (uint*)origdata.Scan0;
                for (int i = 0; i < imgData.Length; i++)
                {
                    imgData[i] = (byteData[i] & 0x000000ff) << 16 | (byteData[i] & 0x0000FF00) | (byteData[i] & 0x00FF0000) >> 16 | (byteData[i] & 0xFF000000);
                }
                bitmap.UnlockBits(origdata);
            }
            texture.SetData(imgData);

            return texture;
        }
    }
}
