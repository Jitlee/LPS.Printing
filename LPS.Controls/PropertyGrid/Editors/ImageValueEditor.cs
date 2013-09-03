using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LPS.Controls.PropertyGrid.Parts
{
    public class ImageValueEditor : ValueEditorBase
    {
        private readonly Image _image = new Image()
        {
            Width = 16d,
            Height = 16d,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(3),
        };

        private readonly Button _removeButton = new Button()
        {
            VerticalAlignment = VerticalAlignment.Center,
            Content = "×",
            Foreground = Brushes.Red,
            Visibility = Visibility.Hidden,
        };

        private readonly Button _browseButton = new Button()
        {
            VerticalAlignment = VerticalAlignment.Center,
            Content = "...",
            Visibility = Visibility.Hidden,
        };

        private readonly TextBox _textBox = new TextBox()
        {
            IsReadOnly = true,
            Background = null,
            BorderBrush = null,
            VerticalAlignment = VerticalAlignment.Center,
        };            

        private readonly ImageAttribute _imageAttribute;
        private readonly TypeEnum _typeEnum;

        public ImageValueEditor(PropertyItem item, ImageAttribute imageAttribute)
            : base(item)
        {
            _typeEnum = TypeEnum.Other;
            _imageAttribute = imageAttribute;
            if (null != _imageAttribute)
            {
                if (typeof(string).IsAssignableFrom(item.PropertyInfo.PropertyType))
                {
                    _typeEnum = TypeEnum.String;
                }
                else if (typeof(byte[]).IsAssignableFrom(item.PropertyInfo.PropertyType))
                {
                    _typeEnum = TypeEnum.ByteArray;
                }
                Layout();
            }
            UpdateValue(item.Value);
        }

        private void Layout()
        {
            switch (_typeEnum)
            {
                case TypeEnum.ByteArray:
                case TypeEnum.String:
                    Grid.SetColumn(_removeButton, 1);
                    Grid.SetColumn(_browseButton, 2);

                    Grid grid = new Grid() { Background = Brushes.Transparent };
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

                    grid.Children.Add(_image);
                    grid.Children.Add(_removeButton);
                    grid.Children.Add(_browseButton);

                    if (base.Item.PropertyInfo.CanWrite)
                    {
                        this.LostFocus += ImageValueEditor_LostFocus;
                        this.MouseEnter += ImageValueEditor_MouseEnter;
                        this.MouseLeave += ImageValueEditor_MouseLeave;
                        this._removeButton.Click += RemoveButton_Click;
                        this._browseButton.Click += BrowseButton_Click;
                    }
                    this.Content = grid;
                    break;
                default:
                    this.Content = _textBox;
                    break;
            }
        }

        private void UpdateValue(object value)
        {
            switch (_typeEnum)
            {
                case TypeEnum.ByteArray:
                    _image.Source = ConvertByteArraryToImage(value);
                    break;
                case TypeEnum.String:
                    _image.Source = ConvertPathToImage(value);
                    break;
                default:
                    _textBox.Text = null != base.Item.Value ? base.Item.Value.ToString() : string.Empty;
                    break;
            }
            if (_image.Source == null)
            {
                _removeButton.Visibility = Visibility.Hidden;
            }
        }

        private ImageSource ConvertByteArraryToImage(object value)
        {
            if (value is byte[])
            {
                if ((value as byte[]).Length > 0)
                {
                    try
                    {
                        var img = new BitmapImage();
                        img.BeginInit();

                        img.StreamSource = new MemoryStream((byte[])value);

                        img.EndInit();
                        return img;
                    }
                    catch { }
                }
            }
            return null;
        }

        private ImageSource ConvertPathToImage(object value)
        {
            if (value is string)
            {
                var img = new BitmapImage();
                string path = value.ToString();
                if (!string.IsNullOrEmpty(_imageAttribute.RelativePath))
                {
                    if (_imageAttribute.RelativePath == "./")
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                    }
                    else
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                            _imageAttribute.RelativePath, path);
                    }
                }
                try
                {
                    if (File.Exists(path) || Uri.CheckSchemeName(path))
                    {
                        return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                    }
                }
                catch
                {

                }
            }
            return null;
        }

        void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            _image.Source = null;
            if (base.Item.PropertyInfo.CanWrite)
            {
                base.Item.Value = null;
            }
            _removeButton.Visibility = Visibility.Hidden;
        }

        void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "图片(*.jpg;*.bmp;*.png)|*.jpg;*.bmp;*.png";
            if(!string.IsNullOrEmpty(_imageAttribute.InitialDirectory))
            {
                string initialDirectory = _imageAttribute.InitialDirectory;
                if (File.Exists(initialDirectory))
                {
                    dlg.InitialDirectory = initialDirectory;
                }
                else if (File.Exists(initialDirectory = 
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        _imageAttribute.InitialDirectory)))
                {
                    dlg.InitialDirectory = initialDirectory;
                }
            }
            if (dlg.ShowDialog() == true)
            {
                string fileName = dlg.FileName;
                BrowseCompleted(fileName);
                UpdateValue(base.Item.Value);
            }
        }

        private void BrowseCompleted(string fileName)
        {
            switch (_typeEnum)
            {
                case TypeEnum.String:
                    string name = new FileInfo(fileName).Name;
                    if (_imageAttribute.RelativePath == "./")
                    {
                        base.Item.Value = CopyFile(fileName,
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name));
                    }
                    else if (!string.IsNullOrEmpty(_imageAttribute.RelativePath))
                    {
                        base.Item.Value = CopyFile(fileName,
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                _imageAttribute.RelativePath,
                                name));
                    }
                    else
                    {
                        base.Item.Value = fileName;
                    }
                    break;
                case TypeEnum.ByteArray:
                    base.Item.Value = File.ReadAllBytes(fileName);
                    break;
            }
        }

        public string CopyFile(string from, string to)
        {
            FileInfo info = new FileInfo(to);
            if (info.Exists)
            {
                if (IsValidFileContent(from, to))
                {
                    return info.Name;
                }
                string patten = @"(?<=\()[0-9]+(?=\)\.\w+$)";
                Match match = Regex.Match(info.Name, patten);
                if (match.Success)
                {
                    to = Regex.Replace(to, patten, (Convert.ToInt32(match.Value) + 1).ToString());
                }
                else
                {
                    to = to.Insert(to.Length - info.Extension.Length, "(1)");
                }
                return CopyFile(from, to);
            }
            try
            {
                if (!Directory.Exists(info.DirectoryName))
                {
                    info.Directory.Create();
                }
                File.Copy(from, to);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "设置图片出错", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return info.Name;
        }

        public bool IsValidFileContent(string filePath1, string filePath2)
        {
            if (string.Compare(filePath1, filePath2, true) == 0)
            {
                return true;
            }

            try
            {
                //创建一个哈希算法对象 
                using (HashAlgorithm hash = HashAlgorithm.Create())
                {
                    using (FileStream file1 = new FileStream(filePath1, FileMode.Open), file2 = new FileStream(filePath2, FileMode.Open))
                    {
                        byte[] hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组 
                        byte[] hashByte2 = hash.ComputeHash(file2);
                        string str1 = BitConverter.ToString(hashByte1);//将字节数组装换为字符串 
                        string str2 = BitConverter.ToString(hashByte2);
                        return (str1 == str2);//比较哈希码 
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void ImageValueEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            //base.OnGotFocus(e);
            //_removeButton.Visibility = _browseButton.Visibility = Visibility.Hidden;
        }

        private void ImageValueEditor_MouseEnter(object sender, MouseEventArgs e)
        {
            //base.OnMouseEnter(e);
            _browseButton.Visibility = Visibility.Visible;
            if (null != base.Item.Value)
            {
                _removeButton.Visibility = Visibility.Visible;
            }
        }

        private void ImageValueEditor_MouseLeave(object sender, MouseEventArgs e)
        {
            //base.OnMouseLeave(e);
            _removeButton.Visibility = _browseButton.Visibility = Visibility.Hidden;
        }

        public override void OnValueChanged(object newValue)
        {
            UpdateValue(newValue);
        }

        private enum TypeEnum
        {
            String,
            ByteArray,
            Other,
        }
    }
}
