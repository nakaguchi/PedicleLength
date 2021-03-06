# PedicleLength
[English Version](READMEen.md)
血管茎長測定プログラム

# 仕様
- Dicom入力（非圧縮データ，LosslessJPEG圧縮は非対応）
- 血管茎上の点をクリック入力
- 血管茎上の点を直線で結んだ距離を計測

# PedicleLengthCS プロジェクト
- C# Version

## 改善要望
- 血管形状の3D出力（3Dビューアで表示するため）
- DICOM読み込みの進捗表示
- 点の挿入（順序変更？）
- 点を立体の球で表示（3D出力向け）
- 設定保存にソフトウェアバージョンを表示する
## 実装済み機能
- DICOMフォルダを開く
- DICOM情報表示（voxel size）
- カーソル位置情報表示（x,y座標と値）
- Window設定(WL/WW)
- クリックでポイント設定
- 距離算出
- ポイント一覧表示
- ポイント削除
- クリックしたポイントのスライスを表示
- ポイント間を直線で描画
- ポイントと直線のサイズを変更可
- 設定を保存・読み込み

# PedicleLength プロジェクト
- C++ Version
- OpenCV + cvui
- RAW入力と表示まで開発
- Dicom入力を実装するには mist または dcmtk の導入が必要
- UIが貧弱（ファイル入力ダイアログ，血管茎上の点をリスト表示）なため開発中断
