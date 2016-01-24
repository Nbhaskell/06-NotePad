using System;
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
using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;

namespace NotePad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string OriginalHash;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public void SaveFileFunc()
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "Text Files (.txt)|*.txt| All Files(*.*)|*.*";
            savefile.FilterIndex = 1;
            Nullable<bool> result = savefile.ShowDialog();
            if (result == true)
            {
                string filename = savefile.FileName;
                File.WriteAllText(filename, textBox.Text);
            }
        }
        private void New_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newwindow = new MainWindow();
            newwindow.Show();
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "Text Files (.txt)|*.txt";
            openfile.Multiselect = false;
            Nullable<bool> result = openfile.ShowDialog();
            if (result == true)
            {
                string filename = openfile.FileName;
                textBox.Text = File.ReadAllText(filename);
                OriginalHash = GetMD5HashData(textBox.Text);
            }

        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileFunc();
        }

       
        private string GetMD5HashData(string data)
        {
            
            MD5 md5 = MD5.Create();
            
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));
            
            StringBuilder returnValue = new StringBuilder();
           
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }
            
            return returnValue.ToString();
        }
        
        private bool ValidateMD5HashData(string inputData, string storedHashData)
        {
            
            string getHashInputData = GetMD5HashData(inputData);
            
            if (string.Compare(getHashInputData, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs cancelEvent)
        {
            
            string NewHash = GetMD5HashData(textBox.Text);
           
            if (NewHash != OriginalHash)
            {
                
                MessageBoxResult result = MessageBox.Show("Do you want to save changes before closing?", "Alert", MessageBoxButton.YesNoCancel);
                
                switch (result)
                {
                    
                    case MessageBoxResult.Yes:
                        SaveFileFunc();
                        break;
                   
                    case MessageBoxResult.No:
                        break;
                   
                    default:
                        cancelEvent.Cancel = true;
                        break;
                }
            }
        }
    }
}

