using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Watch_Face_Editor
{
    /// <summary>
    /// Drag and drop of image files from Explorer into the image lists.
    /// It only reuses the code that the "Add images" buttons already run,
    /// so a dropped file is handled exactly like a file picked in the dialog.
    /// </summary>
    public partial class Form1
    {
        // The editor only accepts PNG, the same filter the Add images dialog uses.
        private static readonly string[] DropImageExtensions = { ".png" };

        private void InitImageDragDrop()
        {
            AllowFileDropFromExplorer();

            // panel1 holds both grids: it catches the drop on the empty area around them.
            Control[] dropTargets = { dataGridView_ImagesList, dataGridView_AnimImagesList, panel1 };
            foreach (Control target in dropTargets)
            {
                if (target == null) continue;
                target.AllowDrop = true;
                target.DragEnter += ImageList_DragEnter;
                target.DragOver += ImageList_DragEnter;   // some controls reset the effect while the mouse moves
                target.DragDrop += ImageList_DragDrop;
            }
        }

        private void ImageList_DragEnter(object sender, DragEventArgs e)
        {
            bool accept = ProjectDir != null && GetDroppedImages(e.Data).Length > 0;
            e.Effect = accept ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void ImageList_DragDrop(object sender, DragEventArgs e)
        {
            if (ProjectDir == null) return;

            string[] files = GetDroppedImages(e.Data);
            if (files.Length == 0) return;

            Logger.WriteLine("* Images_DragDrop");

            // The editor does not get the focus from a drop, but it may show a message box.
            Activate();

            // Only one of the two grids is visible at a time, and it decides the target folder.
            if (dataGridView_AnimImagesList.Visible) AddAnimImagesToProject(files);
            else AddImagesToProject(files);

            Logger.WriteLine("* Images_DragDrop (end)");
        }

        /// <summary>
        /// Returns the image files in a drag operation, or an empty array when
        /// the dragged data is not a file list (text, a layer from a graphics editor, a URL).
        /// </summary>
        private string[] GetDroppedImages(IDataObject data)
        {
            if (data == null || !data.GetDataPresent(DataFormats.FileDrop)) return new string[0];

            string[] paths = data.GetData(DataFormats.FileDrop) as string[];
            if (paths == null) return new string[0];

            return paths
                .Where(path => File.Exists(path)
                            && DropImageExtensions.Contains(Path.GetExtension(path).ToLowerInvariant()))
                .ToArray();
        }

        // ===================== Drop while running as administrator =====================
        // Windows blocks messages sent from a process with lower privileges (UIPI).
        // Without this, a drop from Explorer is silently ignored when the editor runs elevated.

        private const uint WM_DROPFILES = 0x0233;
        private const uint WM_COPYGLOBALDATA = 0x0049;
        private const uint WM_COPYDATA = 0x004A;
        private const uint MSGFLT_ALLOW = 1;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeWindowMessageFilterEx(
            IntPtr hWnd, uint msg, uint action, IntPtr pChangeFilterStruct);

        private void AllowFileDropFromExplorer()
        {
            try
            {
                ChangeWindowMessageFilterEx(Handle, WM_DROPFILES, MSGFLT_ALLOW, IntPtr.Zero);
                ChangeWindowMessageFilterEx(Handle, WM_COPYGLOBALDATA, MSGFLT_ALLOW, IntPtr.Zero);
                ChangeWindowMessageFilterEx(Handle, WM_COPYDATA, MSGFLT_ALLOW, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                // Not available before Windows 7: drag and drop still works when not elevated.
                Logger.WriteLine("* ChangeWindowMessageFilterEx: " + ex.Message);
            }
        }
    }
}
