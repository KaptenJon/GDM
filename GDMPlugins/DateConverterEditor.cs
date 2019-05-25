using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GDMPlugins
{
    public partial class DateConverterEditor : Form
    {
        private DateConverterSettings _settings;
        private List<DateConverter.TimeAtomBlock> _atomBlockList;
        private List<DateConverter.TimeAtomBlock> _selectedBlocks;
        private Hashtable _blockNodes;
        private const string Wildcard = "#";
        private TreeNode _wildcardNode = new TreeNode("Wildcard");
        //Says if the mouse is situated in the specifiedPatternsListBox
        private bool _focus = false;

        public DateConverterEditor()
        {
            InitializeComponent();
        }

        public DateConverterEditor(DateConverterSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            _atomBlockList = settings.TimeAtoms;
            _selectedBlocks = new List<DateConverter.TimeAtomBlock>();
        }

        private void GenerateTreeView()
        {
            _blockNodes = new Hashtable();
            treeView.Nodes.Clear();

            foreach (DateConverter.TimeAtomBlock block in _atomBlockList)
            {
                TreeNode categoryNode;

                if (!_blockNodes.Contains(block.Category))
                {
                    categoryNode = new TreeNode(block.Category);
                    treeView.Nodes.Add(categoryNode);
                    _blockNodes.Add(block.Category, categoryNode);
                }

                categoryNode = (TreeNode)_blockNodes[block.Category];

                foreach (string atom in block.Atoms)
                {
                    categoryNode.Nodes.Add(atom);
                }
            }

            treeView.Nodes.Add(_wildcardNode);
        }

        private void DateConverterEditor_Load(object sender, EventArgs e)
        {
            GenerateTreeView();

            treeView.NodeMouseDoubleClick += delegate(Object obj, TreeNodeMouseClickEventArgs m)
            {
                TreeNode clickedNode = treeView.SelectedNode;

                try
                {
                    if (clickedNode == _wildcardNode) textBox.Text += Wildcard;
                    else if (clickedNode.Parent != null)
                    {
                        textBox.Text += clickedNode.Text;
                        // Remove choosen...
                        treeView.Nodes.Remove(clickedNode.Parent);

                        // ...and all those that cannot occur together
                        foreach (DateConverter.TimeAtomBlock block in _atomBlockList)
                        {
                            if (block.Category == clickedNode.Parent.Text)
                            {
                                _selectedBlocks.Add(block);
                                foreach (DateConverter.TimeAtomBlock exclude in block.Excludes)
                                {
                                    treeView.Nodes.Remove((TreeNode)_blockNodes[exclude.Category]);
                                }
                                break;
                            }
                        }
                    }
                }
                catch { }
            };

            listBox.MouseEnter += delegate(Object obj, EventArgs m)
            {
                _focus = true;
            };

            listBox.MouseLeave += delegate(Object obj, EventArgs m)
            {
                _focus = false;
            };

            MouseClick += delegate(Object obj, MouseEventArgs m)
            {
                if (!_focus && obj.GetType()!=typeof(Button))
                    listBox.ClearSelected();
            };
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string errorMsg = "";
            if (textBox.Text != "" && IsValid(out errorMsg))
            {
                listBox.Items.Add(textBox.Text);
                textBox.Text = "";
            }
            else MessageBox.Show(errorMsg, "Erroneous input", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GenerateTreeView();
        }

        private bool IsValid(out string errorMsg)
        {
            foreach (DateConverter.TimeAtomBlock block in _selectedBlocks)
            {
                foreach (DateConverter.TimeAtomBlock required in block.Requires)
                {
                    if (!_selectedBlocks.Contains(required))
                    {
                        errorMsg = "Invalid combination of time atoms. ";
                        return false;
                    }
                }
            }
            if (textBox.Text == "")
                errorMsg = "No pattern specified. ";
            else errorMsg = "";
            return true;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
                listBox.Items.Remove(listBox.SelectedItem);
            else
            {
                textBox.Text = "";
                listBox.Items.Clear();
                treeView.Nodes.Clear();
                _selectedBlocks= new List<DateConverter.TimeAtomBlock>();
                GenerateTreeView();
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string[] patterns = new string[listBox.Items.Count];
            for(int i=0; i<listBox.Items.Count; i++)
            {
                patterns[i] = listBox.Items[i].ToString();
            }

            _settings.Patterns = patterns;
            Close();
        }
    }
}
