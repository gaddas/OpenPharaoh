using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenPharaoh;

namespace OpenPharaohResourceViewer
{
    public partial class frmInspectSG3 : Form
    {
        public Sg3 File;

        public frmInspectSG3()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "SG3 Files|*.SG3";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.LoadSG3(ofd.FileName);
                }
            }
        }

        private void LoadSG3(string filename)
        {
            this.File = new Sg3(filename);

            var root = new TreeNode("SG3", 0, 0);

            this.treeViewMain.Nodes.Clear();
            this.treeViewMain.Nodes.Add(root);

            for (int i = 0; i < Sg3.MAX_SPRITES; i++)
            {
                var name = this.File.SpriteNames[i];
                var value = this.File.SpriteBitmapIndex[i];

                if (value == 0 && i != 0) break;

                var nodeText = new TreeNode(name, 2, 2);
                var nodeIndex = new TreeNode(string.Format("0x{0:X4} [{0}]", value), 3, 3);
                root.Nodes.Add(nodeText);
                nodeText.Nodes.Add(nodeIndex);

                // load bitmaps
                var startIndex = this.File.SpriteBitmapIndex[i];
                var endIndex = (ushort)this.File.TotalBitmaps + 1;

                for (ushort j = 0; j < Sg3.MAX_SPRITES; j++)
                {
                    var index = this.File.SpriteBitmapIndex[j];

                    if (index >= startIndex && index <= endIndex && j != i && index != 0)
                    {
                        endIndex = index;
                    }
                }

                for (ushort j = startIndex; j < endIndex; j++)
                {
                    var bitmapNode = new TreeNode(j.ToString(), 4, 4);
                    bitmapNode.Tag = this.File.Bitmaps[j];

                    nodeText.Nodes.Add(bitmapNode);
                }
            }

            root.ExpandAll();
        }

        private void treeViewMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.hexView.Clear();
            this.pictureBoxViewer.Image = null;
            this.labelView.Text = "";

            if (e.Node != null && e.Node.Tag is OpenPharaoh.Sg3.Sg3Bitmap)
            {
                var b = ((OpenPharaoh.Sg3.Sg3Bitmap)e.Node.Tag);

                this.pictureBoxViewer.Image = b.Bitmap;
                this.labelView.Text = string.Format(
                    "Offset: {1:X8}{0}" +
                    "Width:  0x{2:X4} [{2}]{0}" +
                    "Height: 0x{3:X4} [{3}]{0}",
                    System.Environment.NewLine,
                    b.Offset,
                    b.Width,
                    b.Height);

                this.hexView.Add(b.Unknown);
            }
        }

        private void treeViewMain_DoubleClick(object sender, EventArgs e)
        {
            if (this.treeViewMain.SelectedNode.Tag is OpenPharaoh.Sg3.Sg3Bitmap)
            {
                var b = ((OpenPharaoh.Sg3.Sg3Bitmap)this.treeViewMain.SelectedNode.Tag);

                var f = new frmInspect555();

                f.Filename = this.File.Filename555;
                f.ImageOffset = b.Offset;
                f.ImageWidth = b.Width;
                f.ImageHeight = b.Height;

                f.Show();
            }
        }

        private void hexView_DataSelected(object sender, HexControl.DataSelectionEventArgs e)
        {

        }

        private void hexView_DataSelectionProgress(object sender, HexControl.DataSelectionEventArgs e)
        {

        }
    }
}
