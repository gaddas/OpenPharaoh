using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

using OpenPharaoh;
using OpenPharaoh.Files;

namespace OpenPharaohResourceViewer
{
    public partial class frmInspect555 : Form
    {
        public string Filename = "";

        public uint ImageOffset
        {
            set
            {
                if (this.scrollOffset.Maximum < value)
                {
                    this.scrollOffset.Maximum = (int)new FileInfo(this.Filename).Length;
                }
                this.scrollOffset.Value = (int)value;
            }
        }

        public ushort ImageWidth
        {
            set
            {
                if (this.tbWidth.Maximum < value)
                {
                    this.tbWidth.Maximum = value;
                }
                this.tbWidth.Value = value;
            }
        }

        public ushort ImageHeight
        {
            set
            {
                if (this.tbHeight.Maximum < value)
                {
                    this.tbHeight.Maximum = value;
                }
                this.tbHeight.Value = value;
            }
        }

        public frmInspect555()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.TestSG2();
            //this.LoadAndDraw();
        }

        private void TestSG2()
        {
            var sg2 = new SG3(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Data\Pharaoh_General.sg3");
            //var sg2 = new SG3(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Data\Expansion.sg3");
        }

        private void Test555()
        {
            ////var b = FileHelper.LoadBitmapFrom555(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Data\Demo2.555", 640, 480, 0);
            ////this.BackgroundImage = b;
            ////this.Width = 640;
            ////this.Height = 480;

            ////var b = FileHelper.LoadBitmapFrom555(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Data\Expansion.555", 24, 128, 4 + 1024 * 2 + 512 * 15 + 108);
            ////this.BackgroundImage = b;
            ////this.Width = 512;
            ////this.Height = 512;

            ////var b = FileHelper.LoadBitmapFrom555(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Data\mudbrick_pyramid.555", 32, 512, 39218);
            ////this.BackgroundImage = b;
            ////this.Width = 512;
            ////this.Height = 512;

            var b = Bitmap555.LoadPlainBitmap_Debug(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Data\Expansion.555", 39, 24 * 5, 39218 - 39 * 2);
            this.pictureBox1.Image = b;
            this.Width = 512;
            this.Height = 512;
        }

        public void LoadAndDraw()
        {
            var fileSize = (int)new FileInfo(this.Filename).Length;

            this.scrollOffset.Maximum = fileSize;
            this.scrollOffset.LargeChange = this.tbWidth.Value * 2;

            this.lblHeight.Text = this.tbHeight.Value.ToString() + "px";
            this.lblWidth.Text = this.tbWidth.Value.ToString() + "px";
            this.lblOffset.Text = string.Format("Offset: 0x{0:X8} [{0}]", this.scrollOffset.Value);

            var width = (ushort)this.tbWidth.Value;
            var height = (ushort)(this.tbHeight.Value * (int)this.nudItemsCount.Value);
            var offset = (uint)this.scrollOffset.Value;

            var b = Bitmap555.LoadPlainBitmap_Debug(this.Filename, width, height, offset);
            this.pictureBox1.Image = b;
        }

        private void tbHeight_Scroll(object sender, EventArgs e)
        {
            this.LoadAndDraw();
        }

        private void tbWidth_Scroll(object sender, EventArgs e)
        {
            this.LoadAndDraw();
        }

        private void nudItemsCount_ValueChanged(object sender, EventArgs e)
        {
            this.LoadAndDraw();
        }

        private void scrollOffset_ValueChanged(object sender, EventArgs e)
        {
            this.LoadAndDraw();
        }

        private void frmInspect555_Shown(object sender, EventArgs e)
        {
            this.LoadAndDraw();
        }
    }
}
