﻿using System;
using System.Collections.Generic;
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

namespace Radical
{
    /// <summary>
    /// Interaction logic for DesignVariableControl.xaml
    /// </summary>
    /// 

    //STYLES
    //Int and Float inpute parsing styles
    public static class Styles
    {
        public const System.Globalization.NumberStyles STYLEFLOAT = System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.AllowLeadingSign;
        public const System.Globalization.NumberStyles STYLEINT = System.Globalization.NumberStyles.Integer;
    }

    public partial class VariableControl : UserControl
    {
        public VariableControl()
        {
            this.DataContext = ControlVM;
            InitializeComponent();
        }

        //CONSTRUCTOR
        public VariableControl(VarVM varvm)
        {
            this.ControlVM = varvm;
            this.DataContext = ControlVM;
            InitializeComponent();
        }

        public VarVM ControlVM { get; set; }

        //MEASURE STRING
        //Obtain the size of the current user input
        private Size MeasureString(string candidate, TextBox box)
        {
            var formattedText = new FormattedText(candidate, System.Globalization.CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(box.FontFamily, box.FontStyle, box.FontWeight, box.FontStretch), box.FontSize, Brushes.Black);
            return new Size(formattedText.Width, formattedText.Height);
        }

        //TEXT CHANGED
        //Resize textbox to fit user input
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            if(box.Text!="")
                box.Width = MeasureString(box.Text, box).Width;
        }

        //GOT FOCUS
        //Clear box contents when it's active
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            box.Clear();
        }

        //LOST FOCUS
        //If TextBox is left empty, set value to 0
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;

            if(box.Text=="")
                box.Text = "0";
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        //PREVIEW KEY DOWN
        //Allow pressing enter to save textbox content
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //Exit the Text Box
                Keyboard.ClearFocus();
                TextBox_LostFocus(sender, e);

                //Update the value of the Text Box after exiting
                TextBox box = (TextBox)sender;
                DependencyProperty prop = TextBox.TextProperty;
                BindingExpression binding = BindingOperations.GetBindingExpression(box, prop);
                if (binding != null) { binding.UpdateSource(); }
            }
        }

        //PREVIVEW TEXT INPUT
        //Only allow user to input parseable float text
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox box = (TextBox)sender;

            //Accept negative sign only as first character
            if (box.Text=="" && e.Text=="-"){return;}

            e.Handled = !(IsTextAllowed(box.Text + e.Text));
        }

        //IS TEXT ALLOWED
        //Determine if user input is parseable
        private static bool IsTextAllowed(string text)
        {
            double val = 0;
            return double.TryParse(text, Styles.STYLEFLOAT, System.Globalization.CultureInfo.InvariantCulture, out val);
        }
    }
}
