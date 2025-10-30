#include "mainwindow.h"
#include "ui_mainwindow.h"

using namespace cv;

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    ui->sliderKernel->setMinimum(1);
    ui->sliderKernel->setMaximum(25);
    ui->sliderKernel->setValue(5);
    ui->spinKernel->setValue(5);

    ui->comboShape->addItem("Rectangle", cv::MORPH_RECT);
    ui->comboShape->addItem("Ellipse", cv::MORPH_ELLIPSE);
    ui->comboShape->addItem("Cross", cv::MORPH_CROSS);

}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::showImage(const cv::Mat &img)
{
    if (img.empty()) return;

    cv::Mat rgb;
    if (img.channels() == 1)
        cvtColor(img, rgb, cv::COLOR_GRAY2RGB);
    else
        cvtColor(img, rgb, cv::COLOR_BGR2RGB);

    QImage qimg(rgb.data, rgb.cols, rgb.rows, rgb.step, QImage::Format_RGB888);
    ui->imageLabel->setPixmap(QPixmap::fromImage(qimg).scaled(
        ui->imageLabel->size(), Qt::KeepAspectRatio, Qt::SmoothTransformation));
}

void MainWindow::on_btnOpen_clicked()
{
    QString filename = QFileDialog::getOpenFileName(this, "Open image", "", "Images (*.png *.jpg *.jpeg *.bmp)");
    if (filename.isEmpty()) return;

    image = imread(filename.toStdString());
    if (image.empty()) {
        QMessageBox::warning(this, "Error", "Cannot load image");
        return;
    }
    processedImage = image.clone();
    showImage(processedImage);
}

void MainWindow::on_btnSave_clicked()
{
    if (processedImage.empty()) return;
    QString filename = QFileDialog::getSaveFileName(this, "Save as", "", "Images (*.png *.jpg *.bmp)");
    if (!filename.isEmpty()) {
        imwrite(filename.toStdString(), processedImage);
    }
}

void MainWindow::on_btnReset_clicked()
{
    if (image.empty()) return;
    processedImage = image.clone();
    showImage(processedImage);
}

void MainWindow::on_btnErode_clicked()
{
    if (processedImage.empty()) return;
    erode(processedImage, processedImage, Mat());
    showImage(processedImage);
}

void MainWindow::on_btnDilate_clicked()
{
    if (processedImage.empty()) return;
    dilate(processedImage, processedImage, Mat());
    showImage(processedImage);
}

void MainWindow::on_btnBlur_clicked()
{
    if (processedImage.empty()) return;
    blur(processedImage, processedImage, Size(blurKernelSize, blurKernelSize));
    showImage(processedImage);
}

void MainWindow::on_sliderKernel_valueChanged(int value)
{
    if (value % 2 == 0) value++;
    blurKernelSize = value;
    ui->spinKernel->setValue(value);
}

void MainWindow::on_spinKernel_valueChanged(int value)
{
    if (value % 2 == 0) value++;
    blurKernelSize = value;
    ui->sliderKernel->setValue(value);
}

void MainWindow::on_btnOpenMorph_clicked()
{
    if (processedImage.empty()) return;
    morphologyEx(processedImage, processedImage, MORPH_OPEN, getKernel());
    showImage(processedImage);
}

void MainWindow::on_btnCloseMorph_clicked()
{
    if (processedImage.empty()) return;
    morphologyEx(processedImage, processedImage, MORPH_CLOSE, getKernel());
    showImage(processedImage);
}

void MainWindow::on_btnGaussianBlur_clicked()
{
    if (processedImage.empty()) return;
    cv::GaussianBlur(processedImage, processedImage, cv::Size(blurKernelSize, blurKernelSize), 0);
    showImage(processedImage);
}

cv::Mat MainWindow::getKernel()
{
    int k = blurKernelSize;
    if (k % 2 == 0) k++;

    int shape = ui->comboShape->currentData().toInt();

    return cv::getStructuringElement(shape, cv::Size(k, k));
}


