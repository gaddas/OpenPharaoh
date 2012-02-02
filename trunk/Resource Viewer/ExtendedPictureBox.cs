using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OpenPharaohResourceViewer
{
    public class ExtendedPictureBox : PictureBox
    {
        private InterpolationMode m_InterpolationMode;

        public ExtendedPictureBox()
        {
            this.InterpolationMode = InterpolationMode.Default;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private IContainer components;

        private void InitializeComponent()
        {
            components = new Container();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = this.InterpolationMode;
            base.OnPaint(e);
        }

        [Description("InterpolationMode"), Category("Behavior")]
        public InterpolationMode InterpolationMode
        {
            get
            {
                return this.m_InterpolationMode;
            }

            set
            {
                this.m_InterpolationMode = value; this.Invalidate();
            }
        }
    }
}
