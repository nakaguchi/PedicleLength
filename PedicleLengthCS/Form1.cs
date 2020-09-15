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

        private int _ImgW = 512;
        private int _ImgH = 512;
        private List<Mat> _Slices = new List<Mat>();
        private Mat _LineSheet;
        private List<Mat> _LineVolume = new List<Mat>();
        private List<Point3i> _Points = new List<Point3i>();
        private string _DicomDir = "";
        private double _Length = 0;
        private string _ConfigFile = "Pedicle.csv";
        private int _Index;
        private int _WL;
        private int _WW;
        private double _SliceThickness = -1.0;
        private double _PixelSpacingX = -1.0;
        private double _PixelSpacingY = -1.0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm() {
            InitializeComponent();
            pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            _Index = TbrSliceIdx.Value;
            _WL = TbrWindowLevel.Value;
            _WW = TbrWindowWidth.Value;
            this.SetTitle();
        }

        /// <summary>
        /// Dicomファイルの読み込み
        /// </summary>
        /// <returns></returns>
        private bool ReadDicom() {
            if (_DicomDir.Length < 1) return false;
            var dicomFiles = Directory.GetFiles(_DicomDir, @"*", SearchOption.TopDirectoryOnly);

            _Slices.Clear();
            foreach (var f in dicomFiles) {
                var dicomImg = new DicomImage(f);
                if (dicomImg == null) continue;
                if (_Slices.Count < 1) {
                    _SliceThickness = dicomImg.Dataset.GetValue<double>(new DicomTag(0x18, 0x50), 0);
                    _PixelSpacingX = dicomImg.Dataset.GetValue<double>(new DicomTag(0x28, 0x30), 0);
                    _PixelSpacingY = dicomImg.Dataset.GetValue<double>(new DicomTag(0x28, 0x30), 1);
                    _ImgW = dicomImg.Width;
                    _ImgH = dicomImg.Height;
                    _LineSheet = new Mat(new Size(_ImgW, _ImgH), MatType.CV_8UC3, new Scalar(0, 0, 255));
                }
                _Slices.Add(new Mat(_ImgH, _ImgW, MatType.CV_16SC1, dicomImg.PixelData.GetFrame(0).Data));
                _LineVolume.Add(new Mat(_ImgH, _ImgW, MatType.CV_8UC1, new Scalar(0)));
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
            double alpha = 255.0 / _WW;
            double beta = -alpha * (_WL - _WW / 2);

            // 画像を表示用に変換（Window処理含む）
            var frame8 = new Mat();
            _Slices[_Index].ConvertTo(frame8, MatType.CV_8UC1, alpha, beta);
            var frameC = new Mat();
            Cv2.CvtColor(frame8, frameC, ColorConversionCodes.GRAY2BGR);

            // 線の描画
            if (NumLineSize.Value > 0) {
                _LineSheet.CopyTo(frameC, _LineVolume[_Index]);
            }

            // 点の描画
            if (NumPointSize.Value > 0) {
                var PointSize = (int)NumPointSize.Value;
                for (int i = 0; i < _Points.Count; i++) {
                    if (_Points[i].Z == _Index) {
                        Cv2.Circle(frameC, new Point(_Points[i].X, _Points[i].Y), PointSize, new Scalar(255, 0, 0), Cv2.FILLED);
                        Cv2.PutText(frameC, $"{i + 1}", new Point(_Points[i].X + PointSize, _Points[i].Y + PointSize),
                            HersheyFonts.HersheySimplex, PointSize / 10.0, new Scalar(255, 0, 0));
                    }
                }

            }

            // 画像表示
            pictureBox1.Image = BitmapConverter.ToBitmap(frameC);

            // ラベル更新
            LblVoxel.Text = $"Voxel: {_PixelSpacingX:0.000} x {_PixelSpacingY:0.000} x {_SliceThickness:0.0} mm";
            LblSliceIdx.Text = $"Slice Index: {_Index}/{_Slices.Count - 1}";
            LblWindowLevel.Text = $"Window Level: {_WL}";
            LblWindowWidth.Text = $"Window Width: {_WW}";
        }


        /// <summary>
        /// スライダー移動時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderChanged(object sender, EventArgs e) {
            _Index = TbrSliceIdx.Value;
            _WL = TbrWindowLevel.Value;
            _WW = TbrWindowWidth.Value;
            this.Draw();
        }

        /// <summary>
        /// ウインドウタイトル更新
        /// </summary>
        private void SetTitle() {
            var ver = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            this.Text = $"Pedicle Length ver {ver.FileMajorPart}.{ver.FileMinorPart}";
#if DEBUG
            this.Text += " [DEBUG]";
#endif
            if (_DicomDir.Length > 0) this.Text += $" - {_DicomDir}";
        }

        /// <summary>
        /// Open Dicom Folderボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenDicom_Click(object sender, EventArgs e) {
#if DEBUG
            _DicomDir = @"D:\usr\prog\PedicleLength\SampleData\5mm early phase";
#else
            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel) return;
            _DicomDir = folderBrowserDialog1.SelectedPath;
#endif
            if (!ReadDicom()) return;
            this.SetTitle();
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
            if (pos.X >= 0) ctval = _Slices[_Index].At<short>(pos.Y, pos.X);
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
            _Points.Add(new Point3i(pos.X, pos.Y, _Index));
            this.UpdateList();
            this.Draw();
        }

        /// <summary>
        /// 選択点リストの更新
        /// </summary>
        private void UpdateList() {
            LbxPoints.Items.Clear();
            for(var i=0; i<_LineVolume.Count; i++) {
                _LineVolume[i].SetTo(new Scalar(0));
            }
            _Length = 0;
            for (var i = 0; i < _Points.Count; i++) {
                LbxPoints.Items.Add($"{i + 1,2} ({_Points[i].X,3}, {_Points[i].Y,3}, {_Points[i].Z,4})");
                if (i > 0) {
                    var dx = (_Points[i].X - _Points[i - 1].X) * _PixelSpacingX;
                    var dy = (_Points[i].Y - _Points[i - 1].Y) * _PixelSpacingY;
                    var dz = (_Points[i].Z - _Points[i - 1].Z) * _SliceThickness;
                    _Length += Math.Sqrt(dx * dx + dy * dy + dz * dz);
                    if (NumLineSize.Value > 0) this.DrawLine(_Points[i], _Points[i - 1]);
                }
            }
            LblLength.Text = $"{_Length,6:0.0}";
        }

        /// <summary>
        /// 点間に線を引く
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void DrawLine(Point3i p1, Point3i p2) {

            var dx = Math.Abs(p2.X - p1.X);
            var dy = Math.Abs(p2.Y - p1.Y);
            var dz = Math.Abs(p2.Z - p1.Z);
            var sx = (p2.X - p1.X >= 0) ? 1 : -1;
            var sy = (p2.Y - p1.Y >= 0) ? 1 : -1;
            var sz = (p2.Z - p1.Z >= 0) ? 1 : -1;
            var p = p1;
            int lineThickness = (int)NumLineSize.Value - 1;
            if (dx >= dy && dx >= dz) {
                var e1 = -dx;
                var e2 = -dx;
                for (var i = 0; i <= dx; i++) {
                    this.DrawBox(p, lineThickness);
                    p.X += sx;
                    e1 += 2 * dy;
                    e2 += 2 * dz;

                    if (e1 >= 0) {
                        p.Y += sy;
                        e1 -= 2 * dx;
                    }

                    if (e2 >= 0) {
                        p.Z += sz;
                        e2 -= 2 * dx;
                    }
                }
            } else if (dy >= dx && dy >= dz) {
                var e1 = -dy;
                var e2 = -dy;
                for (var i = 0; i <= dy; i++) {
                    this.DrawBox(p, lineThickness);
                    p.Y += sy;
                    e1 += 2 * dx;
                    e2 += 2 * dz;

                    if (e1 >= 0) {
                        p.X += sx;
                        e1 -= 2 * dy;
                    }

                    if (e2 >= 0) {
                        p.Z += sz;
                        e2 -= 2 * dy;
                    }
                }
            } else {
                var e1 = -dz;
                var e2 = -dz;
                for (var i = 0; i <= dz; i++) {
                    this.DrawBox(p, lineThickness);
                    p.Z += sz;
                    e1 += 2 * dx;
                    e2 += 2 * dy;

                    if (e1 >= 0) {
                        p.X += sx;
                        e1 -= 2 * dz;
                    }

                    if (e2 >= 0) {
                        p.Y += sy;
                        e2 -= 2 * dz;
                    }
                }
            }
        }

        /// <summary>
        /// 3次元矩形の描画
        /// </summary>
        /// <param name="pt"></param>
        private void DrawBox(Point3i pt, int lineThickness) {
            for (var z = pt.Z - lineThickness; z <= pt.Z + lineThickness; z++) {
                for (var y = pt.Y - lineThickness; y <= pt.Y + lineThickness; y++) {
                    for (var x = pt.X - lineThickness; x <= pt.X + lineThickness; x++) {
                        if (x >= 0 && x < _ImgW && y >= 0 && y < _ImgH && z >= 0 && z < _Slices.Count) {
                            _LineVolume[z].At<Byte>(y, x) = 255;
                        }
                    }
                }
            }
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

        /// <summary>
        /// 設定保存ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e) {
            if (_Slices.Count < 1) return;
            saveFileDialog1.FileName = _ConfigFile;
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            _ConfigFile = saveFileDialog1.FileName;

            var writer = new StreamWriter(saveFileDialog1.FileName, false);
            writer.WriteLine($"DicomDir,{_DicomDir}");
            writer.WriteLine($"Window Level,{_WL}");
            writer.WriteLine($"Window Width,{_WW}");
            for (var i = 0; i < _Points.Count; i++) {
                writer.WriteLine($"Point,{i},X,{_Points[i].X},Y,{_Points[i].Y},Z,{_Points[i].Z}");
            }
            writer.WriteLine($"Length,{_Length},mm");
            writer.Close();
        }

        /// <summary>
        /// 設定読み込みボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            _ConfigFile = openFileDialog1.FileName;

            _Points.Clear();
            _Slices.Clear();
            TbrSliceIdx.Value = 0;
            var reader = new StreamReader(openFileDialog1.FileName);
            while(reader.Peek() != -1) {
                var buf = reader.ReadLine();
                var elems = buf.Split(',');
                if (elems.Length > 0) {
                    switch (elems[0]) {
                    case "DicomDir":
                        if (elems.Length >= 2) _DicomDir = elems[1];
                        break;
                    case "Window Level":
                        if (elems.Length >= 2) {
                            int val = 0;
                            if (int.TryParse(elems[1], out val)) TbrWindowLevel.Value = val;
                        }
                        break;
                    case "Window Width":
                        if (elems.Length >= 2) {
                            int val = 0;
                            if (int.TryParse(elems[1], out val)) TbrWindowWidth.Value = val;
                        }
                        break;
                    case "Point":
                        if (elems.Length >= 8) {
                            var pt = new Point3i(-1, -1, -1);
                            int val = 0;
                            if (int.TryParse(elems[3], out val)) pt.X = val;
                            if (int.TryParse(elems[5], out val)) pt.Y = val;
                            if (int.TryParse(elems[7], out val)) pt.Z = val;
                            if (pt.X >= 0 && pt.Y >= 0 && pt.Z >= 0) {
                                _Points.Add(pt);
                            }
                        }
                        break;
                    }
                }
            }
            reader.Close();

            this.ReadDicom();
            this.UpdateList();
            this.SetTitle();
            this.Draw();
        }

        /// <summary>
        /// サイズ数値の変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SizeNumChanged(object sender, EventArgs e) {
            this.Draw();
        }

        /// <summary>
        /// 線幅数値の変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumLineSize_ValueChanged(object sender, EventArgs e) {
            this.UpdateList();
            this.Draw();
        }
    }
}
