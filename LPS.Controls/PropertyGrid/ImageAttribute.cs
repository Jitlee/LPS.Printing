using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LPS.Controls
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ImageAttribute : Attribute
    {
        /// <summary>
        /// 选择目录的时候默认路径
        /// </summary>
        public string InitialDirectory = string.Empty;

        /// <summary>
        /// 图片存放相对路径 不包含网站的更目录， 如 : Upload\\ImageMap
        /// 默认保存在当前程序的目录下
        /// 如果置空，则可以添加绝对路径
        /// </summary>
        public string RelativePath = "./";


        // Methods
        public ImageAttribute(string initialDirectory = "")
        {
            InitialDirectory = initialDirectory;
        }

        public ImageAttribute(string initialDirectory, string relativePath = "")
        {
            InitialDirectory = initialDirectory;
            RelativePath = relativePath;
        }
    }
}
