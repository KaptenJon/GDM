using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using GDMCore;
using GDMInterfaces;

namespace GDMTool
{
    /// <summary>
    /// Controls the plug-in menu. The plug-in categories, which plug-in that is enabled depending on the selection of tables and columns.
    /// </summary>
    class PluginMenu
    {
        private MenuStrip _menu;
        private Controller _controller;
        private Bitmap _plus;
        private Bitmap _minus;
        private Bitmap _defaultPluginImage;
        private List<ToolStripMenuItem> _categories;

        public PluginMenu(Controller controller,MenuStrip menu)
        {
               
            _menu = menu;
            _controller = controller;
            _controller.TableSelection += controller_TableSelection;
            _controller.ColumnSelection += controller_ColumnSelection;
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            _plus = Images.Images.plus_icon;
            _minus = Images.Images.minus_icon;
            _defaultPluginImage = Images.Images.default_plugin; 
            _categories = new List<ToolStripMenuItem>();
            PopulateMenu();
            controller_TableSelection(null);
            controller_ColumnSelection(null, null);

        }

        private Type _latestColumnType;
        private bool _hideNonAvailible;

        void controller_ColumnSelection(string column, Type type)
        {
            _latestColumnType = type;
            foreach (ToolStripMenuItem ts in _categories)
            {
                List<ToolStripMenuItem> menuitems = (List<ToolStripMenuItem>)ts.Tag;
                foreach (ToolStripMenuItem t in menuitems)
                {
                    IPlugin tool = t.Tag as IPlugin;
                    t.Enabled = (ts.Image != _plus) && EvaluateVisible(tool);
                    t.Visible =  (ts.Image != _plus) && (!HideNonAvailible || EvaluateVisible(tool));

                }

            }
        }



        void controller_TableSelection(string table)
        {
            _latestColumnType = null;
            foreach (ToolStripMenuItem ts in _categories)
            {
                List<ToolStripMenuItem> menuitems = (List<ToolStripMenuItem>)ts.Tag;
                foreach (ToolStripMenuItem t in menuitems)
                {
                    IPlugin tool = t.Tag as IPlugin;
                    t.Enabled = (ts.Image != _plus) && EvaluateVisible(tool);
                    t.Visible =  (ts.Image != _plus) && (!HideNonAvailible || EvaluateVisible(tool));
                }
            }

        }

        private void PopulateMenu()
        {
            SetSection("Input", Controller.PluginManager.InputPlugins);
            Dictionary<string, List<ITool>> toolcategories = new Dictionary<string,List<ITool>>();

            foreach (ITool tool in Controller.PluginManager.ToolPlugins)
            {
                List<ITool> tools;

                if (toolcategories.TryGetValue(tool.ToolCategory,out tools))
                {
                    tools.Add(tool);
                }
                else
                {
                    tools = new List<ITool>();
                    tools.Add(tool);
                    toolcategories.Add(tool.ToolCategory, tools);
                }
            }
            
            foreach (string category in toolcategories.Keys)
            {
                List<ITool> tools;
                toolcategories.TryGetValue(category, out tools);
                SetSection(category, tools);
            }

            SetSection("Output", Controller.PluginManager.OutputPlugins);
            _menu.ItemClicked += _menu_ItemClicked;
        }

        private void SetSection<T>(string title, List<T> list)
        {
            ToolStripMenuItem toolStrip = new ToolStripMenuItem();
            toolStrip.CheckOnClick = true;
            toolStrip.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            toolStrip.Image = _minus;
            toolStrip.ImageAlign = ContentAlignment.TopLeft;
            toolStrip.Name = title;
            toolStrip.Size = new Size(106, 20);
            toolStrip.Text = title;
            toolStrip.TextAlign = ContentAlignment.TopLeft;
            //toolStrip.Click += new EventHandler(MainSection_Click);
            _menu.Items.Add(toolStrip);
            _categories.Add(toolStrip);

            List<ToolStripMenuItem> subItems = new List<ToolStripMenuItem>();

            foreach (T plugin in list)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.ImageAlign = ContentAlignment.MiddleLeft;
                item.TextAlign = ContentAlignment.MiddleLeft;

                try
                {
                    item.Image = ((IPlugin)plugin).Icon.GetThumbnailImage(20,20,null,IntPtr.Zero);
                    if(item.Image==null)
                        item.Image = _defaultPluginImage;  
                }
                catch
                {
                    item.Image = _defaultPluginImage;
                }
                
                item.ImageScaling = ToolStripItemImageScaling.None;
                item.Name = ((IPlugin)plugin).Name;
                item.Padding = new Padding(34, 2, 4, 2);
                item.Size = new Size(156, 24);
                item.Text = "  " + ((IPlugin)plugin).Name;
                item.ToolTipText = ((IPlugin)plugin).Description;
                item.Tag = plugin;

                item.Enabled = EvaluateVisible((IPlugin)plugin);
                item.Visible = (!HideNonAvailible || EvaluateVisible((IPlugin)plugin));

                _menu.Items.Add(item);
                subItems.Add(item);
            }
            toolStrip.Tag = subItems;
            
        }

        private void _menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (PluginSettings.LockPlugin + TimeSpan.FromSeconds(1)> DateTime.Now)
                return;
            
            ToolStripMenuItem menuItem = (ToolStripMenuItem)e.ClickedItem;

            if (menuItem.Selected && menuItem.Tag is IPlugin)
            {
                _controller.ActivePlugin = (IPlugin)menuItem.Tag;
            }
            else if (menuItem.Tag is List<ToolStripMenuItem>)
            {
                List<ToolStripMenuItem> subitems = (List<ToolStripMenuItem>) menuItem.Tag;
                menuItem.Image = menuItem.Image == _plus ? _minus : _plus;
                foreach (ToolStripItem item in subitems)
                {
                    IPlugin tool = item.Tag as IPlugin;
                    item.Enabled = (menuItem.Image != _plus) && EvaluateVisible(tool);
                    item.Visible = (menuItem.Image != _plus) && (!HideNonAvailible || EvaluateVisible(tool));
                }
            }

        }

      

        public bool HideNonAvailible
        {
            get { return _hideNonAvailible; }
            set
            {
                _hideNonAvailible = value;
                foreach (ToolStripItem menuItem in _menu.Items)
                {
                    var subitems = menuItem.Tag as List<ToolStripMenuItem>;
                    if (subitems != null)
                        foreach (var item in subitems)
                        {
                            IPlugin tool = item.Tag as IPlugin;
                            item.Enabled = (menuItem.Image != _plus) && EvaluateVisible(tool);
                            item.Visible = (menuItem.Image != _plus) && (!HideNonAvailible || EvaluateVisible(tool));
                        }
                }
            }
        }

        private bool EvaluateVisible(IPlugin tool)
        {
            if (tool != null)
            {
                if (tool is ITool)
                {
                    if (!((ITool) tool).NeedTableSelected)
                        return true;
                    else if (_controller.SelectedTable == null)
                        return false;
                    else
                    {
                        return ((ITool) tool).AcceptsDataType(_latestColumnType);
                    }
                }
                else if (tool is IOutput)
                    return _controller.SelectedTable != null;
            }
            return true;
        }
    }
}
