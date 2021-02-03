using System;
using System.Collections.Generic;
using Gtk;
using Pango;

namespace YoonFactory.Mono
{
    public static class GtkFactory
    {
        public static void ChangeWidgetFont(Gtk.Widget widget, FontDescription font, Gdk.Color fgColor, StateType nState=StateType.Normal)
        {
            widget.ModifyFont(font);
            widget.ModifyFg(nState, fgColor);
        }

        public static void ChangeWidgetColor(Gtk.Widget widget, Gdk.Color bgColor, StateType nState=StateType.Normal)
        {
            widget.ModifyBg(nState, bgColor);
        }

        public static void ChangeContainerFont(Gtk.Container container, FontDescription font, Gdk.Color fgColor, StateType nState=StateType.Normal)
        {
            // Modify Button Fonts  (Parents : Button >> Child : Label)
            ChangeWidgetFont(container, font, fgColor, nState);
            foreach (Widget subWidget in container.Children)
            {
                if (subWidget is Gtk.Label subLabel)
                {
                    ChangeWidgetFont(subLabel, font, fgColor, nState);
                }
                else if (subWidget is Gtk.Container subContainer)
                {
                    ChangeContainerFont(subContainer, font, fgColor, nState);
                }
                else if (subWidget is Gtk.MenuItem subMenuItem)
                {
                    var subMenu = subMenuItem.Submenu;
                    ChangeContainerFont(subMenuItem, font, fgColor, nState);
                    if (subMenu is Gtk.Container subMenuContainer)
                    {
                        ChangeContainerFont(subMenuContainer, font, fgColor, nState);
                    }
                    else if (subMenu != null)
                    {
                        ChangeWidgetFont(subMenu, font, fgColor, nState);
                    }
                }
            }
        }

        public static void ChangeContainerColor(Gtk.Container container, Gdk.Color bgColor, StateType nState=StateType.Normal)
        {
            // Modify Button Color  (Parents : Button >> Child : Label)
            ChangeWidgetColor(container, bgColor, nState);
            foreach (Widget subWidget in container.Children)
            {
                if (subWidget is Gtk.Label subLabel)
                {
                    ChangeWidgetColor(subLabel, bgColor, nState);
                }
                else if (subWidget is Gtk.Container subContainer)
                {
                    ChangeContainerColor(subContainer, bgColor, nState);
                }
                else if (subWidget is Gtk.MenuItem subMenuItem)
                {
                    var subMenu = subMenuItem.Submenu;
                    ChangeContainerColor(subMenuItem, bgColor, nState);
                    if (subMenu is Gtk.Container subMenuContainer)
                    {
                        ChangeContainerColor(subMenuContainer, bgColor, nState);
                    }
                    else if (subMenu != null)
                    {
                        ChangeWidgetColor(subMenu, bgColor, nState);
                    }
                }
            }
        }

        public static void SetWidgetInBoxLayout(ref Box box, Widget widget, int iOrder, uint padding=0)
        {
            if (box == null || widget == null) return;

            box.Add(widget);
            // Do not use PackStart or PackEnd in GTKSharp (Error: Child => Parants == null)
            // box.PackStart(widget, false, false, padding); // Expand=false, Fill=false, Padding(margin)=0
            try
            {
                Box.BoxChild boxChild = box[widget] as Box.BoxChild;
                boxChild.Position = iOrder;
                boxChild.Expand = false;
                boxChild.Fill = false;
                boxChild.Padding = padding;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            widget.Show();
        }

        public static void SetWidgetInTableLayout(ref Table table, Widget widget, uint iRow, uint iCol, uint rowSize=1, uint colSize=1, uint padding=0, AttachOptions option=AttachOptions.Fill) // iRow : 0, 1, 2 ...,  iCol : 0, 1, 2 ...
        {
            if (table == null || widget == null) return;
            if (iRow > table.NRows - 1 || iCol > table.NColumns - 1 || iRow + rowSize > table.NRows || iCol + colSize > table.NColumns) return;

            table.Add(widget);

            try
            {
                Table.TableChild tableChild = table[widget] as Table.TableChild;
                tableChild.TopAttach = iRow;
                tableChild.BottomAttach = iRow + rowSize;
                tableChild.LeftAttach = iCol;
                tableChild.RightAttach = iCol + colSize;
                tableChild.XOptions = option;
                tableChild.YOptions = option;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            widget.Show();
        }

        public static bool IsWidgetInContainter(Container container, Widget widget)
        {
            if (container == widget) return true;

            foreach(Widget subWidget in container.Children)
            {
                if (subWidget == widget) return true;
                else if (subWidget is Gtk.Container subContainer)
                {
                    if (IsWidgetInContainter(subContainer, widget))
                        return true;
                }
            }
            return false;
        }

        public static void SetComboboxItems(ref ComboBox comboBox, string[] arrayItems, int nID=0)
        {
            try
            {
                ListStore listStore = new ListStore(typeof(string)); // ListStore는 ComboBox의 열(Column)이다 { 0 : string } (갯수가 1개뿐임)
                TreeIter initIter = new TreeIter();
                if (nID >= arrayItems.Length || arrayItems.Length == 0)
                {
                    comboBox.Model = listStore;
                    return;
                }
                for (int iText = 0; iText < arrayItems.Length; iText++)
                {
                    TreeIter iter = listStore.AppendValues(arrayItems[iText]);
                    if (iText == nID)
                        initIter = iter;
                }
                comboBox.Model = listStore;
                comboBox.SetActiveIter(initIter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void SetComboboxItems(ref ComboBox comboBox, List<string> listItems, int nID = 0)
        {
            try
            {
                ListStore listStore = new ListStore(typeof(string)); // ListStore는 ComboBox의 열(Column)이다 { 0 : string } (갯수가 1개뿐임)
                TreeIter initIter = new TreeIter();
                if (nID >= listItems.Count || listItems.Count == 0)
                {
                    comboBox.Model = listStore;
                    return;
                }
                foreach (string strItem in listItems)
                {
                    TreeIter iter = listStore.AppendValues(strItem);
                    if (listItems.IndexOf(strItem) == nID)
                        initIter = iter;
                }
                comboBox.Model = listStore;
                comboBox.SetActiveIter(initIter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void SetComboboxItems(ref ComboBoxEntry comboBox, string[] arrayItems, int nID = 0)
        {
            try
            {
                ListStore listStore = new ListStore(typeof(string)); // ListStore는 ComboBox의 열(Column)이다 { 0 : string } (갯수가 1개뿐임)
                TreeIter initIter = new TreeIter();
                if (nID >= arrayItems.Length || arrayItems.Length == 0)
                {
                    comboBox.Model = listStore;
                    comboBox.Entry.Text = "";
                    return;
                }
                for (int iText = 0; iText < arrayItems.Length; iText++)
                {
                    TreeIter iter = listStore.AppendValues(arrayItems[iText]);
                    if (iText == nID)
                        initIter = iter;
                }
                comboBox.Model = listStore;
                comboBox.SetActiveIter(initIter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void SetComboboxItems(ref ComboBoxEntry comboBox, List<string> listItems, int nID = 0)
        {
            try
            {
                ListStore listStore = new ListStore(typeof(string)); // ListStore는 ComboBox의 열(Column)이다 { 0 : string } (갯수가 1개뿐임)
                TreeIter initIter = new TreeIter();
                if (nID >= listItems.Count || listItems.Count == 0)
                {
                    comboBox.Model = listStore;
                    comboBox.Entry.Text = "";
                    return;
                }
                foreach (string strItem in listItems)
                {
                    TreeIter iter = listStore.AppendValues(strItem);
                    if (listItems.IndexOf(strItem) == nID)
                        initIter = iter;
                }
                comboBox.Model = listStore;
                comboBox.SetActiveIter(initIter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static Gdk.Image GetImageData(string strImageFilePath)
        {
            Gtk.Image iSource = new Gtk.Image(strImageFilePath);
            Gdk.Image iTargetData;
            iSource.GetImage(out iTargetData, out _);

            return iTargetData;
        }

        public static Gdk.Pixmap GetImagePixelMap(string strImageFilePath)
        {
            Gtk.Image iSource = new Gtk.Image(strImageFilePath);
            Gdk.Pixmap iTargetPixMap;
            iSource.GetImage(out _, out iTargetPixMap);

            return iTargetPixMap;
        }

        public static Gdk.Image GetImageData(Gtk.Image iSource)
        {
            Gdk.Image iTargetData;
            iSource.GetImage(out iTargetData, out _);

            return iTargetData;
        }

        public static Gdk.Pixmap GetImagePixelMap(Gtk.Image iSource)
        {
            Gdk.Pixmap iTargetPixMap;
            iSource.GetImage(out _, out iTargetPixMap);

            return iTargetPixMap;
        }

        public static void ResizeImage(ref Gtk.Image image, int nWidth, int nHeight)
        {
            if (image.StorageType != ImageType.Pixbuf) return;

            Gdk.Pixbuf iPixBuf;
            int nWidthOrg = image.Pixbuf.Width;
            int nHeightOrg = image.Pixbuf.Height;
            float dScaleWidthPerHeight = (float)nWidthOrg / (float)nHeightOrg;
            if(nWidth > nHeight)
            {
                if (nWidth > nHeight * dScaleWidthPerHeight)
                    nWidth = (int)(nHeight * dScaleWidthPerHeight);
                else
                    nHeight = (int)(nWidth / dScaleWidthPerHeight);
            }
            else
            {
                if (nHeight > nWidth / dScaleWidthPerHeight)
                    nHeight = (int)(nWidth / dScaleWidthPerHeight);
                else
                    nWidth = (int)(nHeight * dScaleWidthPerHeight);
            }
            try
            {
                iPixBuf = image.Pixbuf.ScaleSimple(nWidth, nHeight, Gdk.InterpType.Bilinear);
                image.Pixbuf = iPixBuf;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
    }
}