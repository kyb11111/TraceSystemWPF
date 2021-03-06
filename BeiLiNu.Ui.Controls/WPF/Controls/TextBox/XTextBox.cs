﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeiLiNu.Ui.Controls.WPF.Controls
{
    public class XTextBox : TextBox
    {
        static XTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(XTextBox), new FrameworkPropertyMetadata(typeof(XTextBox)));
        }



        #region 属性


        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件背景图片")]
        public ImageSource NormalImgae
        {
            get { return (ImageSource)GetValue(NormalImgaeProperty); }
            set { SetValue(NormalImgaeProperty, value); }
        }

        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件背景图片")]
        public static readonly DependencyProperty NormalImgaeProperty =
            DependencyProperty.Register("NormalImgae", typeof(ImageSource), typeof(XTextBox));



        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件鼠标悬停时背景图片")]
        public ImageSource MouseOverImage
        {
            get { return (ImageSource)GetValue(MouseOverImageProperty); }
            set { SetValue(MouseOverImageProperty, value); }
        }
        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件鼠标悬停时背景图片")]
        public static readonly DependencyProperty MouseOverImageProperty =
            DependencyProperty.Register("MouseOverImage", typeof(ImageSource), typeof(XTextBox));



        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件按下时背景图片")]
        public ImageSource PressedImage
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件按下时背景图片")]
        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register("PressedImage", typeof(ImageSource), typeof(XTextBox));



        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件背景图片九宫格切图")]
        public Thickness NineGrid
        {
            get { return (Thickness)GetValue(NineGridProperty); }
            set { SetValue(NineGridProperty, value); }
        }

        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件背景图片九宫格切图")]
        public static readonly DependencyProperty NineGridProperty =
            DependencyProperty.Register("NineGrid", typeof(Thickness), typeof(XTextBox), new PropertyMetadata(new Thickness(0)));





        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件文字位置")]
        public Thickness TextMargin
        {
            get { return (Thickness)GetValue(TextMarginProperty); }
            set { SetValue(TextMarginProperty, value); }
        }

        [CategoryAttribute("自定义属性"), DescriptionAttribute("获取或设置控件文字位置")]
        public static readonly DependencyProperty TextMarginProperty =
            DependencyProperty.Register("TextMargin", typeof(Thickness), typeof(XTextBox), new PropertyMetadata(new Thickness(0)));


        
        #endregion


        #region 隐藏基类属性

        [Browsable(false)]
        public new Brush Background
        {
            get { return base.Background; }
            set { base.Background = value; }
        }

        [Browsable(false)]
        public new Brush BorderBrush
        {
            get { return base.BorderBrush; }
            set { base.BorderBrush = value; }
        }

        #endregion
    }
}
