using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;


namespace Zenref.Ava.Views
{
    public partial class DragAndDropWindow : Window
    {
        Button? ImportButton;
        Button? CancelButton;
        Button? NextButton;
        private TextBlock FilesTextBlock;

        public DragAndDropWindow()
        {
            InitializeComponent();
            InitializeWindow();

            AddHandler(DragDrop.DropEvent, Drop);
            AddHandler(DragDrop.DragOverEvent, DragOver);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InitializeWindow()
        {
            FilesTextBlock = this.FindControl<TextBlock>("fileNameTextBlock");
            ImportButton = this.FindControl<Button>("importButton");
            ImportButton.Click += async (s, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = @"C:\",
                    Title = "Browse for excel file",
                    AllowMultiple = true,
                };
                openFileDialog.Filters.Add(new FileDialogFilter() { Name = "Excel", Extensions = { "xlsx", "xlsm" } });
                string[] result = await openFileDialog.ShowAsync(this);
                if (result != null)
                {
                    string filePath = string.Join(Environment.NewLine, result);
                    FilesTextBlock.Text = filePath;
                }
            };
            CancelButton = this.FindControl<Button>("cancelButton");
            CancelButton.Click += (s, e) => 
            {
                this.Close();
            };
            NextButton = this.FindControl<Button>("nextButton");
            NextButton.Click += (s, e) =>
            {
                
            };
        }

        private void DragOver(object sender, DragEventArgs e)
        {
            e.DragEffects = e.DragEffects & DragDropEffects.Copy;
            if (!e.Data.Contains(DataFormats.FileNames))
            {
                e.DragEffects = DragDropEffects.None;
            }
        }
        private void Drop(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                string filePath = string.Join(Environment.NewLine, e.Data.GetFileNames());
                FilesTextBlock.Text = filePath;

                //_TextBlock.Text = "";
                //List<string> fileList = new List<string>(e.Data.GetFileNames());
                //foreach (string file in fileList)
                //{
                //    FileInfo fileInfo = new FileInfo(file);
                //    long fileSize = fileInfo.Length / 1024;
                //    _TextBlock.Text = string.Concat(_TextBlock.Text, $"{file} ({fileSize} KB)\n");
                //}
            }
        }
    }
}
