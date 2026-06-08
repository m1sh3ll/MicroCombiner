using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MicroCombiner
{
      public partial class Form1 : Form
      {
            public Form1()
            {
                  InitializeComponent();
                  this.AllowDrop = true;
                  this.DragEnter += Form1_DragEnter;
                  this.DragDrop += Form1_DragDrop;
            }

            private void Form1_DragEnter(object sender, DragEventArgs e)
            {
                  if (e.Data.GetDataPresent(DataFormats.FileDrop))
                        e.Effect = DragDropEffects.Copy;
            }

            private void Form1_DragDrop(object sender, DragEventArgs e)
            {
                  string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                  // Only process text files dropped onto the form.                 
                  string[] txtFiles = files.Where(f => f.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)).ToArray();

                  if (txtFiles.Length == 0)
                  {
                        MessageBox.Show("No .txt files found.", "RtClickCombine");
                        return;
                  }
                  string binderFile = "Binder.xls";
                  string outputDir = Path.GetDirectoryName(txtFiles[0]);

                  // Choose the binder filename based on the marketplace name in the first dropped file.
                  if (txtFiles[0].Contains("AMAZON"))
                  {
                        binderFile = "Binder_AMAZON.xls";
                  }
                  else if (txtFiles[0].Contains("SHOPIFY"))
                  {
                        binderFile = "Binder_SHOPIFY.xls";
                  }
                  else if (txtFiles[0].Contains("EBAY"))
                  {
                        binderFile = "Binder_EBAY.xls";
                  }

                  //string outputFile = Path.Combine(outputDir, binderFile);


                  string baseName = Path.GetFileNameWithoutExtension(txtFiles[0]);
                  string[] parts = baseName.Split('-');

                  string outputName;

                  if (parts.Length >= 2)
                        outputName = $"{parts[0]}-{parts[1]}-{binderFile}";
                  else
                        outputName = $"{baseName}-{binderFile}";

                  string outputFile = Path.Combine(outputDir, outputName);


                  try
                  {
                        // Prevent duplicate lines from being written to the output file.
                        HashSet<string> uniqueLines = new HashSet<string>();

                        using (StreamWriter writer = new StreamWriter(outputFile))
                        {
                              foreach (string file in txtFiles)
                              {
                                    foreach (string line in File.ReadLines(file))
                                    {
                                          if (uniqueLines.Add(line)) // Only write unique lines
                                                writer.WriteLine(line);
                                    }
                              }
                        }

                        //MessageBox.Show($"CombinedText.txt created in:\n{outputDir}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
                  catch (Exception ex)
                  {
                        MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
            }
      }
}
