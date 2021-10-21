using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kotic;

namespace KoticGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> filenames;
        public MainWindow()
        {
            filenames = new List<string>();
            InitializeComponent();
        }

        public void ClickDirArchiving(object sender, RoutedEventArgs e)
        {

            l_error.Content = "";
            l_inf.Content = "";
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = "D:";
                dialog.Title = "Выбор папки для архивирования";
                dialog.IsFolderPicker = true;
                dialog.Multiselect = true;
                /*dialog.EnsurePathExists = true;
                dialog.EnsureFileExists = false;
                dialog.AllowNonFileSystemItems = false;*/
                CommonFileDialogResult result = dialog.ShowDialog();

                foreach (string file in dialog.FileNames)
                {
                    filenames.Add(file);
                }

                dialog.Title = "Выбор пути сохранения архива";
                result = dialog.ShowDialog();
                if (filenames.Count == 0)
                {
                    throw new Exception("Не выбраны директории");
                }
                if (dialog.FileName == null)
                {
                    throw new Exception("Не выбран путь сохранения");
                }
                l_inf.Content = "Архивация началась";
                KoticArchivator koticArchivator = new KoticArchivator(filenames);
                koticArchivator.GenerateArchive(dialog.FileName);
                l_inf.Content = "Архивация прошла успешно";
            }
            catch (Exception ex)
            {
                l_inf.Content = "Архивация не прошла";
                l_error.Content = ex.Message;
                if (ex is InvalidOperationException)
                {
                    l_error.Content = "Не выбран путь сохранения";
                }
            }
               
        }
        public void ClickFileArchiving(object sender, RoutedEventArgs e)
        {
            l_error.Content = "";
            l_inf.Content = "";
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = "D:";
                dialog.Title = "Выбор файла для архивирования";             
                dialog.Multiselect = true;
                /*dialog.EnsurePathExists = true;
                dialog.EnsureFileExists = false;
                dialog.AllowNonFileSystemItems = false;*/
                CommonFileDialogResult result = dialog.ShowDialog();

                foreach (string file in dialog.FileNames)
                {
                    filenames.Add(file);
                }

                dialog.Title = "Выбор пути сохранения архива";
                dialog.IsFolderPicker = true;
                result = dialog.ShowDialog();
                if (filenames.Count == 0)
                {
                    throw new Exception("Не выбраны файлы");
                }
                if (dialog.FileName == null)
                {
                    throw new Exception("Не выбран путь сохранения");
                }
              
                l_inf.Content = "Архивация началась";
                KoticArchivator koticArchivator = new KoticArchivator(filenames);
                koticArchivator.GenerateArchive(dialog.FileName);
                l_inf.Content = "Архивация прошла успешно";
                filenames = new List<string>();
            }
            catch (Exception ex)
            {
                l_inf.Content = "Архивация не прошла";
                l_error.Content = ex.Message;
                if (ex is InvalidOperationException)
                {
                    l_error.Content = "Не выбран путь сохранения";
                }
            }

        }


        public void ClickDearchiving(object sender, RoutedEventArgs e)
        {
            l_error.Content = "";
            l_inf.Content = "";
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = "D:";
                dialog.Title = "Выбор архива";
                CommonFileDialogResult result = dialog.ShowDialog();
                KoticDearchivator koticDearchivator = new KoticDearchivator(dialog.FileName);
                dialog.Title = "Путь разархивирования"; dialog.IsFolderPicker = true;
                result = dialog.ShowDialog();
                koticDearchivator.GenerateFilesFromArchive(dialog.FileName);
                l_inf.Content = "Разархивация прошла успешно";


            }
            catch (Exception ex)
            {
                l_error.Content = ex.Message;
                l_inf.Content = "Разархивация не прошла";
            }
        }
    }
}
