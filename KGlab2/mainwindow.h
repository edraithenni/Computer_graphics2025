#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QImage>
#include <QPixmap>
#include <QFileDialog>
#include <QMessageBox>
#include <opencv2/opencv.hpp>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void on_btnOpen_clicked();
    void on_btnSave_clicked();
    void on_btnReset_clicked();
    void on_btnErode_clicked();
    void on_btnDilate_clicked();
    void on_btnBlur_clicked();
    void on_sliderKernel_valueChanged(int value);
    void on_btnOpenMorph_clicked();
    void on_btnCloseMorph_clicked();
    void on_btnGaussianBlur_clicked();
    void on_spinKernel_valueChanged(int value);

private:
    Ui::MainWindow *ui;
    cv::Mat image;
    cv::Mat processedImage;
    int blurKernelSize = 5;

    void showImage(const cv::Mat &img);
    cv::Mat getKernel();
};

#endif // MAINWINDOW_H
