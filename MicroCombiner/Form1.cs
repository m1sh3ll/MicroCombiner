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

      string[] txtFiles = files.Where(f => f.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)).ToArray();

      if (txtFiles.Length == 0)
      {
        MessageBox.Show("No .txt files found.", "RtClickCombine");
        return;
      }

      string outputDir = Path.GetDirectoryName(txtFiles[0]);
      string outputFile = Path.Combine(outputDir, "Binder.xls");

      //try
      //{
      //  using (StreamWriter writer = new StreamWriter(outputFile))
      //  {
      //    foreach (string file in txtFiles)
      //    {
      //      //writer.WriteLine($"----- {Path.GetFileName(file)} -----");
      //      writer.Write(File.ReadAllText(file));
      //      //writer.WriteLine();
      //    }
      //  }

      // // MessageBox.Show($"CombinedText.txt created in:\n{outputDir}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
      //}
      //catch (Exception ex)
      //{
      //  MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //}
      try
      {
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
