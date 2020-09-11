using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Dicom;
using Dicom.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PedicleLengthCS {
    public partial class MainForm : Form {

        public const int ImgW = 512;
        public const int ImgH = 512;
        public const int PointSize = 4;
        private List<Mat> _Slices = new List<Mat>();
        private List<Point3i> _Points = new List<Point3i>();
        int _idx;
        int _wl;
        int _ww;
        double _SliceThickness = -1.0;
        double _PixelSpacingX = -1.0;
        double _PixelSpacingY = -1.0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm() {
            InitializeComponent();
            pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            _idx = TbrSliceIdx.Value;
            _wl = TbrWindowLevel.Value;
            _ww = TbrWindowWidth.Value;
        }

        /// <summary>
        /// Dicomファイルの読み込み
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private bool ReadDicom(string dir) {
            var dicomFiles = Directory.GetFiles(dir, @"*", SearchOption.TopDirectoryOnly);

            _Slices.Clear();
            foreach (var f in dicomFiles) {
                var dicomImg = new DicomImage(f);
                if (dicomImg == null) continue;
                if (_Slices.Count < 1) {
                    _SliceThickness = dicomImg.Dataset.GetValue<double>(new DicomTag(0x18, 0x50), 0);
                    _PixelSpacingX = dicomImg.Dataset.GetValue<double>(new DicomTag(0x28, 0x30), 0);
                    _PixelSpacingY = dicomImg.Dataset.GetValue<double>(new DicomTag(0x28, 0x30), 1);
                }
                _Slices.Add(new Mat(ImgH, ImgW, MatType.CV_16SC1, dicomImg.PixelData.GetFrame(0).Data));
            }
            if (_Slices.Count < 1) {
                MessageBox.Show("Can't find Dicom image");
                return false;
            }
            TbrSliceIdx.Minimum = 0;
            TbrSliceIdx.Maximum = _Slices.Count - 1;
            TbrSliceIdx.Value = 0;

            return true;
        }

        /// <summary>
        /// 描画
        /// </summary>
        private void Draw() {
            if (_Slices.Count < 1) return;  // データ読み込み前は表示しない

            // パラメータ設定
            double alpha = 255.0 / _ww;
            double beta = -alpha * (_wl - _ww / 2);

            // 画像を表示用に変換（Window処理含む）
            var frame8 = new Mat();
            _Slices[_idx].ConvertTo(frame8, MatType.CV_8UC1, alpha, beta);
            var frameC = new Mat();
            Cv2.CvtColor(frame8, frameC, ColorConversionCodes.GRAY2BGR);

            // 点の描画
            for (int i = 0; i < _Points.Count; i++) {
                if (_Points[i].Z == _idx) {
                    Cv2.Circle(frameC, new Point(_Points[i].X, _Points[i].Y), PointSize, new Scalar(255, 0, 0), Cv2.FILLED);
                    Cv2.PutText(frameC, $"{i + 1}", new Point(_Points[i].X + PointSize, _Points[i].Y + PointSize),
                        HersheyFonts.HersheySimplex, 0.4, new Scalar(255, 0, 0));
                }
            }
            // 画像表示
            pictureBox1.Image = BitmapConverter.ToBitmap(frameC);

            // ラベル更新
            LblVoxel.Text = $"Voxel: {_PixelSpacingX:0.000} x {_PixelSpacingY:0.000} x {_SliceThickness:0.0} mm";
            LblSliceIdx.Text = $"Slice Index: {_idx}/{_Slices.Count - 1}";
            LblWindowLevel.Text = $"Window Level: {_wl}";
            LblWindowWidth.Text = $"Window Width: {_ww}";
        }


        /// <summary>
        /// スライダー移動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderChanged(object sender, EventArgs e) {
            _idx = TbrSliceIdx.Value;
            _wl = TbrWindowLevel.Value;
            _ww = TbrWindowWidth.Value;
            this.Draw();
        }

        /// <summary>
        /// Open Dicom Folderボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenDicom_Click(object sender, EventArgs e) {
            if (!ReadDicom(@"D:\usr\prog\PedicleLength\SampleData\5mm early phase")) return;
            this.Draw();
        }

        /// <summary>
        /// Zoomモードでピクチャボックスに表示した画像の座標を取得する
        /// </summary>
        /// <param name="pb"></param>
        /// <param name="px"></param>
        /// <param name="py"></param>
        /// <returns></returns>
        private Point ImagePosOnPbox(PictureBox pb, int px, int py) {
            var pos = new Point(-1, -1);
            int pw = pb.Width;
            int ph = pb.Height;
            if (pb.Image == null) return pos;
            int iw = pb.Image.Width;
            int ih = pb.Image.Height;

            if (pw * ih > ph * iw) { // PBox横長
                var piw = ph * iw / ih;
                var margin = (pw - piw) / 2;
                if (px < margin || px >= pw - margin) return pos;
                pos.Y = ih * py / ph;
                pos.X = iw * (px - margin) / piw;
            } else {    // PBox縦長
                var pih = pw * ih / iw;
                var margin = (ph - pih) / 2;
                if (py < margin || py >= ph - margin) return pos;
                pos.X = iw * px / pw;
                pos.Y = ih * (py - margin) / pih;
            }

            return pos;
        }

        /// <summary>
        /// ピクチャボックス上のマウス移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            var pos = ImagePosOnPbox((PictureBox)sender, e.X, e.Y);
            short ctval = -1;
            if (pos.X >= 0) ctval = _Slices[_idx].At<short>(pos.Y, pos.X);
            LblCursor.Text = $"Cursor ({pos.X}, {pos.Y}) : {ctval}";
        }

        /// <summary>
        /// ピクチャボックス上のホイール操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e) {
            if (e.Delta < 0 && TbrSliceIdx.Value < TbrSliceIdx.Maximum) {
                TbrSliceIdx.Value++;
            }
            if (e.Delta > 0 && TbrSliceIdx.Value > TbrSliceIdx.Minimum) {
                TbrSliceIdx.Value--;
            }
        }

        /// <summary>
        /// ピクチャボックス上のマウスクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e) {
            var pos = ImagePosOnPbox((PictureBox)sender, e.X, e.Y);
            if (pos.X < 0) return;  // 画像外なら終了
            _Points.Add(new Point3i(pos.X, pos.Y, _idx));
            this.UpdateList();
            this.Draw();
        }

        /// <summary>
        /// 選択点リストの更新
        /// </summary>
        private void UpdateList() {
            LbxPoints.Items.Clear();
            double length = 0;
            for (var i = 0; i < _Points.Count; i++) {
                LbxPoints.Items.Add($"{i + 1,2} ({_Points[i].X,3}, {_Points[i].Y,3}, {_Points[i].Z,4})");
                if (i > 0) {
                    var dx = (_Points[i].X - _Points[i - 1].X) * _PixelSpacingX;
                    var dy = (_Points[i].Y - _Points[i - 1].Y) * _PixelSpacingY;
                    var dz = (_Points[i].Z - _Points[i - 1].Z) * _SliceThickness;
                    length += Math.Sqrt(dx * dx + dy * dy + dz * dz);
                }
            }
            LblLength.Text = $"{length,6:0.0}";
        }

        /// <summary>
        /// リスト項目クリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LbxPoints_Click(object sender, EventArgs e) {
            if (((ListBox)sender).Items.Count < 1) return;
            var txt = ((ListBox)sender).SelectedItem.ToString();
            var idx = Convert.ToInt32(txt.Substring(0,3)) - 1;
            if (idx >= 0 && idx < _Points.Count) {
                TbrSliceIdx.Value = _Points[idx].Z;
            }
        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, EventArgs e) {
            if (LbxPoints.SelectedIndex < 0) return;
            _Points.RemoveAt(LbxPoints.SelectedIndex);
            this.UpdateList();
            this.Draw();
        }
    }
}
