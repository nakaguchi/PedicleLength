using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dicom;
using Dicom.Imaging;
using System.Diagnostics;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PedicleLengthCS {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {

            //var file = DicomFile.Open(@"D:\usr\prog\PedicleLength\SampleData\5mm early phase\pedicle length 5mm early001");
            //var img = new DicomImage(file.Dataset);
            //var imgd = img.RenderImage(0).Pixels;

            var image = new DicomImage(@"D:\usr\prog\PedicleLength\SampleData\5mm early phase\pedicle length 5mm early001");
            var data = image.PixelData;
            Debug.WriteLine(data.NumberOfFrames);
            var buf = data.GetFrame(0);
            Debug.WriteLine(buf.Size);

            Mat frame = new Mat(512, 512, MatType.CV_16SC1, buf.Data);
            double minV, maxV;
            frame.MinMaxIdx(out minV, out maxV);
            Debug.WriteLine($"min {minV} max {maxV}");
            var frame8 = new Mat();
            frame.ConvertTo(frame8, MatType.CV_8UC1, 1, 0);
            pictureBox1.Image = BitmapConverter.ToBitmap(frame8);

        }
    }
}
