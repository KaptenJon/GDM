using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using GDMCore;
using GDMInterfaces;

namespace GDMTool
{
    /// <summary>
    /// Controls the treeview component and the configuration menu that is used to display, add, remove and load configurations.
    /// </summary>
    public class ConfigViewer
    {
        private Controller _controller;
        private TreeView _treeView;
        private TreeNode _saved;
        private TreeNode _current;
        private ImageList _treeImages;
        private ToolStrip _menu;

        public ConfigViewer(Controller controller, TreeView treeView, ToolStrip menu)
        {
            _menu = menu;
            _controller = controller;
            _treeView = treeView;
            _treeImages = new ImageList();
            _treeImages.ColorDepth = ColorDepth.Depth32Bit;
            FillImageList();
            _treeImages.ImageSize = new Size(16, 16);
            _treeView.ImageList = _treeImages;
           
            _saved = _treeView.Nodes.Add("Saved Data Configurations");
            _saved.ImageKey = "config_folder";
            _saved.SelectedImageKey = _saved.ImageKey;
            _treeView.AfterSelect += treeView_AfterSelect;

            _controller.CurrentConfigUpdated += UpdateCurrentConfiguration;

            if (controller.ConfigManager.Configurations != null)
            {
                PreviousConfigurations();
            }
            _menu.Items["saveConfig"].Click += SaveConfig_Click;
            _menu.Items["newConfig"].Click += NewConfig_Click;
            _menu.Items["openConfig"].Click += OpenConfig_Click;
            _menu.Items["deleteConfig"].Click += DeleteConfig_Click;
            _menu.Items["exportConfig"].Click += ExportConfig_Click;
            _menu.Items["importConfig"].Click += ImportConfig_Click;
            _menu.Items["mergeConfig"].Click += Merge_Click;
            _current = new TreeNode("Current Data Configuration");
            _current.ImageKey = "config";
            _current.SelectedImageKey = _current.ImageKey;
            _treeView.Nodes.Add(_current);

        }


        void ImportConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML Files|*.xml|All Files|*.*";
            dialog.Title = "Import Configuration";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _controller.ConfigManager.Import(dialog.FileName);
                    PreviousConfigurations();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void ExportConfig_Click(object sender, EventArgs e)
        {
            Config config = (Config)_treeView.SelectedNode.Tag;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML Files|*.xml|All Files|*.*";
            dialog.Title = "Export Configuration";
            dialog.DefaultExt = "xml";
            dialog.AddExtension = true;
            dialog.FileName = config.Name + ".xml";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    _controller.ConfigManager.Export(config, dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void FillImageList()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Bitmap a = Images.Images.default_plugin;
            Bitmap b = Images.Images.bullet;
            Bitmap c = Images.Images.folder;
            Bitmap d = Images.Images.cog;

            _treeImages.Images.Add("default_plugin", a);
            _treeImages.Images.Add("plugin_property", b);
            _treeImages.Images.Add("config_folder", c);
            _treeImages.Images.Add("config", d);
        }

        void OpenConfig_Click(object sender, EventArgs e)
        {
            _controller.ResetData();
            _current.Nodes.Clear();

            Config config = _treeView.SelectedNode.Tag as Config;
            if(config != null)
            _controller.BatchRun(config);
        }

        private void Merge_Click(object sender, EventArgs e)
        {
            Config config = _treeView.SelectedNode.Tag as Config;
            if (config != null)
                _controller.BatchRun(config);
        }


        void NewConfig_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Starting a new configuration session resets the current configuration.\n\nProceed?",
                "New Data Configuration", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _controller.ResetData();
                _current.Nodes.Clear();
            }
        }

        void SaveConfig_Click(object sender, EventArgs e)
        {
            NewConfig win = new NewConfig();
            if (win.ShowDialog() == DialogResult.OK)
            {
                string name = win.ConfigName;
                if (_controller.ConfigManager.IsValidNewName(name))
                {
                    _controller.ConfigManager.SaveCurrent(name);
                    PreviousConfigurations();
                }
                else
                {
                    MessageBox.Show("Not a valid name");
                }
            }
        }

        void DeleteConfig_Click(object sender, EventArgs e)
        {
            Config n = _treeView.SelectedNode?.Tag as Config;
            if (n == null)
                return;
            
            
            if (MessageBox.Show("Are you sure you want to delete the selected configuration?",
                "Delete Data Configuration", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DeleteConfig(n);
            }
        }

        public void DeleteConfig(Config n)
        {
            _controller.ConfigManager.DeleteConfig(n);
            _treeView.Nodes.Remove(_treeView.SelectedNode);
        }

        void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            bool configSelected = e.Node.Tag == null;
            _menu.Items["exportConfig"].Enabled = !configSelected;
            _menu.Items["openConfig"].Enabled = !configSelected;
            _menu.Items["deleteConfig"].Enabled = !configSelected;
        }

        // Updates the config view with configuration files, if any, saved on disk
        private void PreviousConfigurations()
        {
            if (_saved.Nodes.Count > 0) _saved.Nodes.Clear();
            for(int i=0; i<_controller.ConfigManager.Configurations.Count; i++)
            {
                string nodeName = _controller.ConfigManager.Configurations[i].Name;
                if (nodeName.ToLower().Contains(".xml"))
                    nodeName = nodeName.Replace(".xml", "");
                TreeNode configNode = new TreeNode(nodeName);
                configNode.Tag = _controller.ConfigManager.Configurations[i];
                configNode.ImageKey = "config";
                configNode.SelectedImageKey = configNode.ImageKey;
                _saved.Nodes.Add(configNode);

                for (int j = 0; j < _controller.ConfigManager.Configurations[i].Operations.Count; j++)
                {
                    UpdateCurrentConfiguration(_saved.Nodes[i], _controller.ConfigManager.Configurations[i].Operations[j]);
                }
            }
            _saved.Expand();
            _treeView.Refresh();
        }

        private void UpdateCurrentConfiguration()
        {
            _menu.Items["saveConfig"].Enabled = true;
            _menu.Invoke((MethodInvoker)(() =>
            {
                _current.Nodes.Clear();

                foreach (Operation op in _controller.ConfigManager.CurrentConfig.Operations)
                {
                    UpdateCurrentConfiguration(_current, op);
                }
                _current.Expand();
            }));
        }

        // Updates the config view with the settings used at the latest plugin execution
        private void UpdateCurrentConfiguration(TreeNode node, Operation operation)
        {
            string name = operation.Plugin;
            TreeNode operationNode = new TreeNode(name);
            operationNode.Tag = operation;
            node.Nodes.Add(operationNode);
            IPlugin plugin = Controller.PluginManager.GetPlugin(name);

            //Name of plugin updated Todo make it visible
            if (plugin == null)
                return;

            if (plugin.Icon != null)
            {
                _treeImages.Images.Add(name, plugin.Icon.GetThumbnailImage(20, 20, () => true, IntPtr.Zero));
                operationNode.ImageKey = name;  
            }
            else
            {
                operationNode.ImageKey = "plugin_default";
            }
            operationNode.SelectedImageKey = operationNode.ImageKey;
            PropertyInfo[] info = operation.Settings.GetType().GetProperties();
            foreach (PropertyInfo k in info)
            {
                if (k.CanRead)
                {
                    TreeNode n = operationNode.Nodes.Add(k.Name + ": " + k.GetValue(operation.Settings, null));
                    n.ImageKey = "plugin_property";
                    n.SelectedImageKey = n.ImageKey;
                }
            }
            BatchPluginSettings batch = operation.Settings as BatchPluginSettings;

            if (batch == null)
                return;
            foreach (var child in batch.ChildPlugins)
            {
                AddChildPlugins(child, operationNode);   
            }
            operationNode.Expand();

        }

        private void AddChildPlugins(Operation operation, TreeNode node)
        {
            string name = operation.Plugin;
            TreeNode operationNode = new TreeNode(name);
            operationNode.Tag = operation;
            node.Nodes.Add(operationNode);
            IPlugin plugin = Controller.PluginManager.GetPlugin(name);

            //Name of plugin updated Todo make it visible
            if (plugin == null)
                return;

            if (plugin.Icon != null)
            {
                _treeImages.Images.Add(name, plugin.Icon.GetThumbnailImage(20, 20, () => true, IntPtr.Zero));
                operationNode.ImageKey = name;
            }
            else
            {
                operationNode.ImageKey = "plugin_default";
            }
            operationNode.SelectedImageKey = operationNode.ImageKey;
            PropertyInfo[] info = operation.Settings.GetType().GetProperties();
            foreach (PropertyInfo k in info)
            {
                if (k.CanRead)
                {
                    TreeNode n = operationNode.Nodes.Add(k.Name + ": " + k.GetValue(operation.Settings, null));
                    n.ImageKey = "plugin_property";
                    n.SelectedImageKey = n.ImageKey;
                }
            }
        }


    }
}