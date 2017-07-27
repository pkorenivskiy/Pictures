using Pictures.Domain.DataAccess;
using Pictures.Dto;
using StructureMap.Attributes;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace Pictures.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [SetterProperty]
        public IGenericRepository<Picture> PictureRepository { get; set; }

        [SetterProperty]
        public GenericService<Picture> PictureService { get; set; }

        public static RoutedCommand EscKeyCommand = new RoutedCommand();
        public static RoutedCommand UpKeyCommand = new RoutedCommand();
        public static RoutedCommand DownKeyCommand = new RoutedCommand();
        public static RoutedCommand BlurBtnCommand = new RoutedCommand();
        public static RoutedCommand MenuViewCommand = new RoutedCommand();
        public static RoutedCommand MenuDelCommand = new RoutedCommand();

        private int _imageIndex = -1;

        public MainWindow()
        {
            InitializeComponent();

            EscKeyCommand.InputGestures.Add(new KeyGesture(Key.Escape));
            UpKeyCommand.InputGestures.Add(new KeyGesture(Key.Up));
            DownKeyCommand.InputGestures.Add(new KeyGesture(Key.Down));
            BlurBtnCommand.InputGestures.Add(new KeyGesture(Key.B, ModifierKeys.Control));

            MenuViewCommand.InputGestures.Add(new KeyGesture(Key.Enter));
            MenuDelCommand.InputGestures.Add(new KeyGesture(Key.Delete));
        }

        private void PictureGroupBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var f in files)
                {
                    try
                    {
                        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(f);
                        var thisFormat = imgInput.RawFormat;

                        BitmapFrame image = BitmapFrame.Create(new Uri(f));
                        BitmapFrame thumbnail = fastResize(image, 128, 128);

                        var picture = new Picture
                        {
                            Width = imgInput.Width,
                            Height = imgInput.Height,
                            Id = 0,
                            Name = f,
                            Image = image,
                            Thumbnail = thumbnail
                        };

                        PictureService.Add(picture);
                        PicturesListBox.Items.Add(picture);
                    }
                    catch (Exception /*ex*/)
                    {

                    }
                }
            }
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (item == null || !item.IsSelected)
                return;

            var picture = PicturesListBox.SelectedItem as Picture;
            if (picture == null)
                return;

            _imageIndex = PicturesListBox.Items.IndexOf(picture);

            FullPictureImage.Source = picture.Image;
            PictureGroupBox.Visibility = Visibility.Collapsed;
            FullPictureView.Visibility = Visibility.Visible;

            FullPictureView.Effect = null;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            closeFullView();
        }

        private void closeFullView()
        {
            if (FullPictureView.Visibility == Visibility.Visible)
            {
                PictureGroupBox.Visibility = Visibility.Visible;
                FullPictureView.Visibility = Visibility.Collapsed;
            }
        }

        private void FullPicture_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
                closeFullView();
        }

        #region COMMANDS
        private void EscKeyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            closeFullView();
        }

        private void UpKeyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (FullPictureView.Visibility == Visibility.Visible && _imageIndex >= 0 && _imageIndex < PicturesListBox.Items.Count - 1)
            {
                var picture = (PicturesListBox.Items.GetItemAt(++_imageIndex) as Picture);
                FullPictureImage.Source = picture.Image;

                FullPictureImage.Effect = null;
            }
        }

        private void DownKeyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (FullPictureView.Visibility == Visibility.Visible && _imageIndex > 0 && _imageIndex < PicturesListBox.Items.Count)
            {
                var picture = (PicturesListBox.Items.GetItemAt(--_imageIndex) as Picture);
                FullPictureImage.Source = picture.Image;

                FullPictureImage.Effect = null;
            }
        }

        private void BlurBtnCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (FullPictureView.Visibility == Visibility.Visible && _imageIndex > 0 && _imageIndex < PicturesListBox.Items.Count)
            {
                BlurEffect blurEffect = new BlurEffect();
                blurEffect.KernelType = KernelType.Gaussian;
                FullPictureImage.Effect =  blurEffect;
            }
        }

        private void MenuViewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (FullPictureView.Visibility != Visibility.Visible && PicturesListBox.SelectedIndex != -1)
            {
                var picture = PicturesListBox.Items.GetItemAt(PicturesListBox.SelectedIndex) as Picture;
                if (picture == null)
                    return;

                _imageIndex = PicturesListBox.Items.IndexOf(picture);

                FullPictureImage.Source = picture.Image;
                PictureGroupBox.Visibility = Visibility.Collapsed;
                FullPictureView.Visibility = Visibility.Visible;

                FullPictureView.Effect = null;
            }
        }

        private void MenuDelCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (FullPictureView.Visibility != Visibility.Visible && PicturesListBox.SelectedIndex != -1)
            {
                if (MessageBox.Show("Are You Sure to delete this picture", 
                    "Question", 
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    PicturesListBox.Items.RemoveAt(PicturesListBox.SelectedIndex);
                    //TODO: need to remove from repository, but Id isn't implemented
                }
            }
        }

        #endregion //COMMANDS

        #region PRIVATE_METHODS
        private static BitmapFrame fastResize(BitmapFrame bfPhoto, int nWidth, int nHeight)
        {
            var scale = Math.Max(nWidth / bfPhoto.Width, nHeight / bfPhoto.Height);

            TransformedBitmap tbBitmap = new TransformedBitmap(bfPhoto, new ScaleTransform(scale, scale, 0, 0));
            return BitmapFrame.Create(tbBitmap);
        }

        public void initRepo()
        {
            if (Directory.Exists("images"))
            {
                var directoryInfo = new DirectoryInfo("images");

                foreach (var fi in directoryInfo.GetFiles())
                {
                    try
                    {
                        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(fi.FullName);
                        var thisFormat = imgInput.RawFormat;

                        BitmapFrame image = BitmapFrame.Create(new Uri(fi.FullName));
                        BitmapFrame thumbnail = fastResize(image, 128, 128);

                        var picture = new Picture
                        {
                            Width = imgInput.Width,
                            Height = imgInput.Height,
                            Id = 0,
                            Name = fi.FullName,
                            Image = image,
                            Thumbnail = thumbnail
                        };

                        PictureService.Add(picture);
                    }
                    catch (Exception /*ex*/)
                    {

                    }
                }
            }

            var pictures = PictureRepository.GetAll();
            foreach (var p in pictures)
                PicturesListBox.Items.Add(p);
        }
        #endregion //PRIVATE_METHODS
    }
}
