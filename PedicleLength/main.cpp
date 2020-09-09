#include <stdio.h>
#include <string>
#include "nkcOpenCV.h"
#define CVUI_IMPLEMENTATION
#include "cvui.h"

const std::string fname = "early5mm.raw";
const cv::String WINDOW = "Pedicle Length";
const cv::Size SliceSize = cv::Size(512, 512);
const int SliceNum = 175;
const int KEY_ESC = 27;

int main() {

	// ファイル読み込み
	FILE* fp;
	if (::fopen_s(&fp, fname.c_str(), "rb") != 0) {
		::fprintf(stderr, "Can't open raw file: %s\n", fname.c_str());
		::getchar();
		return 1;
	}
	const int DataSize = SliceSize.area() * SliceNum;
	short* data = new short[DataSize];
	::fread_s(data, sizeof(short) * DataSize, sizeof(short), DataSize, fp);
	::fclose(fp);

	// ウインドウ準備
	cvui::init(WINDOW);
	cv::Mat mainFrame = cv::Mat(cv::Size(800, 600), CV_8UC3);

	int sliceIdx = 0;
	int wl = 0, ww = 300;
	while (true) {
		// 表示画像処理
		cv::Mat sliceImg(SliceSize, CV_16SC1, data + sliceIdx * SliceSize.area());
		cv::Mat sliceD, sliceC;
		double alpha = 255.0 / ww;
		double beta = -alpha * (wl - ww / 2);
		sliceImg.convertTo(sliceD, CV_8UC1, alpha, beta);
		cv::cvtColor(sliceD, sliceC, cv::COLOR_GRAY2BGR);

		// GUI描画
		mainFrame = cv::Scalar(49, 52, 49);
		const cv::Point ImagePos(10, 10);
		cvui::beginRow(mainFrame, ImagePos.x, ImagePos.y, -1, -1, 10);

		cvui::beginColumn(512);
		cvui::image(sliceC);
		cvui::trackbar(SliceSize.width, &sliceIdx, 0, SliceNum - 1, 1, "%.0Lf");
		cvui::endColumn();

		cvui::beginColumn(250, -1, 5);
		cvui::text("Window Level");
		cvui::trackbar(250, &wl, -2000, 2000, 1, "%.0Lf");
		cvui::text("Window Width");
		cvui::trackbar(250, &ww, 1, 3000, 1, "%.0Lf");
		cv::Point mpos = cvui::mouse(WINDOW) - ImagePos;
		cvui::text(" ");
		if (mpos.x >= 0 && mpos.y >= 0 && mpos.x < SliceSize.width && mpos.y < SliceSize.height) {
			cvui::printf("Cursor (%3d, %3d) =%5d", mpos.x, mpos.y, sliceImg.at<short>(mpos));
		} else {
			cvui::printf("Cursor (___, ___) =  ___");
		}
		cvui::endColumn();

		cvui::endRow();
		cvui::imshow(WINDOW, mainFrame);
		if (cv::waitKey(1) == KEY_ESC) break;
	}

	return 0;
}
