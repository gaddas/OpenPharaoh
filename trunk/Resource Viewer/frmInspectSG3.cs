using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenPharaoh;
using OpenPharaoh.Files;

namespace OpenPharaohResourceViewer
{
    public partial class frmInspectSG3 : Form
    {
        public SG3 File;

        public frmInspectSG3()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "SG3 Files|*.SG3|SG2 Files|*.SG2";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.LoadSG3(ofd.FileName);
                }
            }
        }

        private void LoadSG3(string filename)
        {
            this.File = new SG3(filename);

            var root = new TreeNode("SG3", 0, 0);
            var rootS = new TreeNode("Sprites", 0, 0);
            var rootSDescription = new TreeNode(string.Format("Total sprites: {0}", this.File.TotalImages), 3, 3);

            var rootB = new TreeNode("Bitmaps", 0, 0);
            var rootBDescription = new TreeNode(string.Format("Total bitmaps: {0}", this.File.TotalFiles1), 3, 3);

            root.Nodes.Add(rootS);
            rootS.Nodes.Add(rootSDescription);
            root.Nodes.Add(rootB);
            rootB.Nodes.Add(rootBDescription);

            root.Expand();
            rootB.Expand();
            rootS.Expand();

            this.treeViewMain.Nodes.Clear();
            this.treeViewMain.Nodes.Add(root);

            for (var s = 0; s < SG3.MAX_SPRITES; s++)
            {
                var name = this.File.SpriteNames[s];
                var value = this.File.SpriteImageIndex[s];

                if (value == 0 && s != 0) break;

                var nodeText = new TreeNode(name, 2, 2);
                var nodeIndex = new TreeNode(string.Format("0x{0:X4} [{0}]", value), 3, 3);
                rootS.Nodes.Add(nodeText);
                nodeText.Nodes.Add(nodeIndex);

                // load bitmaps
                var startIndex = this.File.SpriteImageIndex[s];
                var endIndex = (ushort)this.File.TotalImages + 1;

                for (ushort j = 0; j < SG3.MAX_SPRITES; j++)
                {
                    var index = this.File.SpriteImageIndex[j];

                    if (index >= startIndex && index <= endIndex && j != s && index != 0)
                    {
                        endIndex = index;
                    }
                }

                for (ushort j = startIndex; j < endIndex; j++)
                {
                    var bitmapNode = new TreeNode(j.ToString(), 4, 4);
                    bitmapNode.Tag = this.File.Images[j];

                    nodeText.Nodes.Add(bitmapNode);
                }
            }

            for (int f = 0; f < this.File.TotalFiles1; f++)
            {
                var fobject = this.File.Files[f];

                var nodeBitmap = new TreeNode(fobject.Filename, 5, 5);
                nodeBitmap.Tag = fobject;

                var nodeBitmapDescription = new TreeNode(string.Format("{0}", fobject.Description), 3, 3);
                var nodeBitmapImages = new TreeNode(string.Format("from {0} up to {1}, total {2}", fobject.FirstImage, fobject.LastImage, fobject.Count), 3, 3);
                rootB.Nodes.Add(nodeBitmap);
                nodeBitmap.Nodes.Add(nodeBitmapDescription);
                nodeBitmap.Nodes.Add(nodeBitmapImages);

                for (var j = fobject.FirstImage; j <= fobject.LastImage; j++)
                {
                    var bitmapNode = new TreeNode(j.ToString(), 4, 4);
                    bitmapNode.Tag = this.File.Images[(ushort)j];

                    nodeBitmap.Nodes.Add(bitmapNode);
                }
            }
        }

        private void treeViewMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.hexView.Clear();
            this.pictureBoxViewer.Image = null;
            this.labelView.Text = "";

            if (e.Node != null && e.Node.Tag is SG3.Sg3Image)
            {
                var b = ((SG3.Sg3Image)e.Node.Tag);

                if (b.Type == Bitmap555Type.Isometric)
                {
                    var nb = new Bitmap(b.Width, b.Height);
                    nb.MakeTransparent(Color.FromArgb(247, 0, 255));
                    var g = Graphics.FromImage(nb);

                    switch (b.TileSize)
                    {
                        case 1:
                            g.DrawImageUnscaled(b.Tiles[0], 0, nb.Height - Bitmap555.TILE_HEIGHT);
                            break;
                        case 2:
                            g.DrawImageUnscaled(b.Tiles[0], 30, (int)(nb.Height - 60));

                            g.DrawImageUnscaled(b.Tiles[1], 0, (int)(nb.Height - 45));
                            g.DrawImageUnscaled(b.Tiles[2], 60, (int)(nb.Height - 45));

                            g.DrawImageUnscaled(b.Tiles[3], 30, (int)(nb.Height - 30));
                            break;
                        case 3:
                            g.DrawImageUnscaled(b.Tiles[0], 60, (int)(nb.Height - 90));

                            g.DrawImageUnscaled(b.Tiles[1], 30, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[2], 90, (int)(nb.Height - 75));

                            g.DrawImageUnscaled(b.Tiles[3], 0, (int)(nb.Height - 60));
                            g.DrawImageUnscaled(b.Tiles[4], 60, (int)(nb.Height - 60));
                            g.DrawImageUnscaled(b.Tiles[5], 120, (int)(nb.Height - 60));

                            g.DrawImageUnscaled(b.Tiles[6], 30, (int)(nb.Height - 45));
                            g.DrawImageUnscaled(b.Tiles[7], 90, (int)(nb.Height - 45));

                            g.DrawImageUnscaled(b.Tiles[8], 60, (int)(nb.Height - 30));
                            break;
                        case 4:
                            g.DrawImageUnscaled(b.Tiles[0], 90, (int)(nb.Height - 120));

                            g.DrawImageUnscaled(b.Tiles[1], 60, (int)(nb.Height - 105));
                            g.DrawImageUnscaled(b.Tiles[2], 120, (int)(nb.Height - 105));

                            g.DrawImageUnscaled(b.Tiles[3], 30, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[4], 90, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[5], 150, (int)(nb.Height - 90));

                            g.DrawImageUnscaled(b.Tiles[6], 0, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[7], 60, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[8], 120, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[9], 180, (int)(nb.Height - 75));

                            g.DrawImageUnscaled(b.Tiles[10], 30, (int)(nb.Height - 60));
                            g.DrawImageUnscaled(b.Tiles[11], 90, (int)(nb.Height - 60));
                            g.DrawImageUnscaled(b.Tiles[12], 150, (int)(nb.Height - 60));

                            g.DrawImageUnscaled(b.Tiles[13], 60, (int)(nb.Height - 45));
                            g.DrawImageUnscaled(b.Tiles[14], 120, (int)(nb.Height - 45));

                            g.DrawImageUnscaled(b.Tiles[15], 90, (int)(nb.Height - 30));
                            break;

                        case 5:
                            g.DrawImageUnscaled(b.Tiles[0], 120, (int)(nb.Height - 150));

                            g.DrawImageUnscaled(b.Tiles[1], 90, (int)(nb.Height - 135));
                            g.DrawImageUnscaled(b.Tiles[2], 150, (int)(nb.Height - 135));

                            g.DrawImageUnscaled(b.Tiles[3], 60, (int)(nb.Height - 120));
                            g.DrawImageUnscaled(b.Tiles[4], 120, (int)(nb.Height - 120));
                            g.DrawImageUnscaled(b.Tiles[5], 180, (int)(nb.Height - 120));

                            g.DrawImageUnscaled(b.Tiles[6], 30, (int)(nb.Height - 105));
                            g.DrawImageUnscaled(b.Tiles[7], 90, (int)(nb.Height - 105));
                            g.DrawImageUnscaled(b.Tiles[8], 150, (int)(nb.Height - 105));
                            g.DrawImageUnscaled(b.Tiles[9], 210, (int)(nb.Height - 105));

                            g.DrawImageUnscaled(b.Tiles[10], 0, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[11], 60, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[12], 120, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[13], 180, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[14], 240, (int)(nb.Height - 90));

                            g.DrawImageUnscaled(b.Tiles[15], 30, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[16], 90, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[17], 150, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[18], 210, (int)(nb.Height - 75));

                            g.DrawImageUnscaled(b.Tiles[19], 60, (int)(nb.Height - 60));
                            g.DrawImageUnscaled(b.Tiles[20], 120, (int)(nb.Height - 60));
                            g.DrawImageUnscaled(b.Tiles[21], 180, (int)(nb.Height - 60));

                            g.DrawImageUnscaled(b.Tiles[22], 90, (int)(nb.Height - 45));
                            g.DrawImageUnscaled(b.Tiles[23], 150, (int)(nb.Height - 45));

                            g.DrawImageUnscaled(b.Tiles[24], 120, (int)(nb.Height - 30));
                            break;

                        case 6:
                            g.DrawImageUnscaled(b.Tiles[0], 150, (int)(nb.Height - 180));

                            g.DrawImageUnscaled(b.Tiles[1], 120, (int)(nb.Height - 165));
                            g.DrawImageUnscaled(b.Tiles[2], 180, (int)(nb.Height - 165));

                            g.DrawImageUnscaled(b.Tiles[3], 90, (int)(nb.Height - 150));
                            g.DrawImageUnscaled(b.Tiles[4], 150, (int)(nb.Height - 150));
                            g.DrawImageUnscaled(b.Tiles[5], 210, (int)(nb.Height - 150));

                            g.DrawImageUnscaled(b.Tiles[6], 60, (int)(nb.Height - 135));
                            g.DrawImageUnscaled(b.Tiles[7], 120, (int)(nb.Height - 135));
                            g.DrawImageUnscaled(b.Tiles[8], 180, (int)(nb.Height - 135));
                            g.DrawImageUnscaled(b.Tiles[9], 240, (int)(nb.Height - 135));

                            g.DrawImageUnscaled(b.Tiles[10], 30, (int)(nb.Height - 120));
                            g.DrawImageUnscaled(b.Tiles[11], 90, (int)(nb.Height - 120));
                            g.DrawImageUnscaled(b.Tiles[12], 150, (int)(nb.Height - 120));
                            g.DrawImageUnscaled(b.Tiles[13], 210, (int)(nb.Height - 120));
                            g.DrawImageUnscaled(b.Tiles[14], 270, (int)(nb.Height - 120));

                            g.DrawImageUnscaled(b.Tiles[15], 0, (int)(nb.Height - 105));
                            g.DrawImageUnscaled(b.Tiles[16], 60, (int)(nb.Height - 105));
                            g.DrawImageUnscaled(b.Tiles[17], 120, (int)(nb.Height - 105));
                            g.DrawImageUnscaled(b.Tiles[18], 180, (int)(nb.Height - 105));
                            g.DrawImageUnscaled(b.Tiles[19], 240, (int)(nb.Height - 105));
                            g.DrawImageUnscaled(b.Tiles[20], 300, (int)(nb.Height - 105));

                            g.DrawImageUnscaled(b.Tiles[21], 30, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[22], 90, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[23], 150, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[24], 210, (int)(nb.Height - 90));
                            g.DrawImageUnscaled(b.Tiles[25], 270, (int)(nb.Height - 90));

                            g.DrawImageUnscaled(b.Tiles[26], 60, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[27], 120, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[28], 180, (int)(nb.Height - 75));
                            g.DrawImageUnscaled(b.Tiles[29], 240, (int)(nb.Height - 75));

                            g.DrawImageUnscaled(b.Tiles[30], 90, (int)(nb.Height - 60));
                            g.DrawImageUnscaled(b.Tiles[31], 150, (int)(nb.Height - 60));
                            g.DrawImageUnscaled(b.Tiles[32], 210, (int)(nb.Height - 60));

                            g.DrawImageUnscaled(b.Tiles[33], 120, (int)(nb.Height - 45));
                            g.DrawImageUnscaled(b.Tiles[34], 180, (int)(nb.Height - 45));

                            g.DrawImageUnscaled(b.Tiles[35], 150, (int)(nb.Height - 30));
                            break;
                    }

                    if (b.Plain != null)
                    {
                        g.DrawImageUnscaled(b.Plain, 0, 0);
                    }

                    this.pictureBoxViewer.Image = nb;
                }
                else
                {
                    this.pictureBoxViewer.Image = b.Plain;
                }

                this.labelView.Text = string.Format(
                    "0000: Offset: {1:X8}{0}" +
                    "0004: Data Total: {7}{0}" +
                    "0008: Data Compressed: {8}{0}" +
                    "0014: Width:  0x{2:X4} [{2}]{0}" +
                    "0016: Height: 0x{3:X4} [{3}]{0}" +
                    "0032: Type:   0x{5:X2} [{4}]{0}" +
                    "0034: Is External: {6}{0}" +
                    "0037: Tile Size: {9}{0}",
                    System.Environment.NewLine,
                    b.Offset,
                    b.Width,
                    b.Height,
                    b.Type,
                    (int)b.Type,
                    b.ExternalFlag == 1,
                    b.DataLenghtTotal,
                    b.DataLengthTiles,
                    b.TileSize);

                this.hexView.Add(b.Data);
            }
        }

        private void treeViewMain_DoubleClick(object sender, EventArgs e)
        {
            ////if (this.treeViewMain.SelectedNode.Tag is SG3.Sg3Bitmap)
            ////{
            ////    var b = ((SG3.Sg3Bitmap)this.treeViewMain.SelectedNode.Tag);

            ////    var f = new frmInspect555();

            ////    f.Filename = this.File.Filename555;
            ////    f.ImageOffset = b.Offset;
            ////    f.ImageWidth = b.Width;
            ////    f.ImageHeight = b.Height;

            ////    f.Show();
            ////}
        }

        private void hexView_DataSelected(object sender, HexControl.DataSelectionEventArgs e)
        {

        }

        private void hexView_DataSelectionProgress(object sender, HexControl.DataSelectionEventArgs e)
        {

        }
    }
}
