# PedicleLength
血管茎長測定プログラム

# 仕様
- Dicom入力（非圧縮データ，LosslessJPEG圧縮は非対応）
- 血管茎上の点をクリック入力
- 血管茎上の点を直線で結んだ距離を計測

# PedicleLength プロジェクト
- C++ Version
- OpenCV + cvui
- RAW入力と表示まで開発
- Dicom入力を実装するには mist または dcmtk の導入が必要
- UIが貧弱（ファイル入力ダイアログ，血管茎上の点をリスト表示）なため開発中断

# PedicleLengthCS プロジェクト
- C# Version
