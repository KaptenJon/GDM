using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace GDMPlugins.SQL
{
    /// <summary>
    /// A connection dialog that lets users build a SQL Server connection string
    /// by filling in server, authentication, and database fields — similar to
    /// the old Microsoft.Data.ConnectionUI dialog.
    /// </summary>
    internal sealed class SqlConnectionForm : Form
    {
        private TextBox _serverTextBox;
        private ComboBox _authCombo;
        private Label _userLabel;
        private TextBox _userTextBox;
        private Label _passLabel;
        private TextBox _passTextBox;
        private ComboBox _databaseCombo;
        private Button _refreshButton;
        private Button _testButton;
        private Button _okButton;
        private Button _cancelButton;
        private CheckBox _trustCertCheckBox;

        public string ConnectionString { get; private set; }

        public SqlConnectionForm(string existingConnectionString)
        {
            InitLayout(existingConnectionString);
        }

        private void InitLayout(string existingConnectionString)
        {
            Text = "Connect to SQL Server";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(420, 340);

            int labelX = 14, fieldX = 150, fieldW = 250, y = 14;

            // Server
            Controls.Add(new Label { Text = "Server name:", Location = new Point(labelX, y + 3), AutoSize = true });
            _serverTextBox = new TextBox { Location = new Point(fieldX, y), Width = fieldW };
            Controls.Add(_serverTextBox);

            // Authentication
            y += 32;
            Controls.Add(new Label { Text = "Authentication:", Location = new Point(labelX, y + 3), AutoSize = true });
            _authCombo = new ComboBox
            {
                Location = new Point(fieldX, y),
                Width = fieldW,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _authCombo.Items.AddRange(new[] { "Windows Authentication", "SQL Server Authentication" });
            _authCombo.SelectedIndex = 0;
            _authCombo.SelectedIndexChanged += (s, e) => UpdateAuthFields();
            Controls.Add(_authCombo);

            // Username
            y += 32;
            _userLabel = new Label { Text = "User name:", Location = new Point(labelX, y + 3), AutoSize = true };
            _userTextBox = new TextBox { Location = new Point(fieldX, y), Width = fieldW, Enabled = false };
            Controls.Add(_userLabel);
            Controls.Add(_userTextBox);

            // Password
            y += 32;
            _passLabel = new Label { Text = "Password:", Location = new Point(labelX, y + 3), AutoSize = true };
            _passTextBox = new TextBox { Location = new Point(fieldX, y), Width = fieldW, UseSystemPasswordChar = true, Enabled = false };
            Controls.Add(_passLabel);
            Controls.Add(_passTextBox);

            // Trust server certificate
            y += 32;
            _trustCertCheckBox = new CheckBox
            {
                Text = "Trust server certificate",
                Location = new Point(fieldX, y),
                AutoSize = true,
                Checked = true
            };
            Controls.Add(_trustCertCheckBox);

            // Database
            y += 32;
            Controls.Add(new Label { Text = "Database:", Location = new Point(labelX, y + 3), AutoSize = true });
            _databaseCombo = new ComboBox { Location = new Point(fieldX, y), Width = fieldW - 80 };
            Controls.Add(_databaseCombo);
            _refreshButton = new Button { Text = "Refresh", Location = new Point(fieldX + fieldW - 72, y - 1), Width = 72 };
            _refreshButton.Click += (s, e) => RefreshDatabases();
            Controls.Add(_refreshButton);

            // Buttons
            y += 48;
            _testButton = new Button { Text = "Test Connection", Location = new Point(labelX, y), Width = 110 };
            _testButton.Click += (s, e) => TestConnection();
            Controls.Add(_testButton);

            _okButton = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(fieldX + fieldW - 162, y), Width = 75 };
            _cancelButton = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(fieldX + fieldW - 78, y), Width = 78 };
            Controls.Add(_okButton);
            Controls.Add(_cancelButton);
            AcceptButton = _okButton;
            CancelButton = _cancelButton;

            // Pre-populate from existing connection string
            if (!string.IsNullOrWhiteSpace(existingConnectionString))
                PopulateFromConnectionString(existingConnectionString);
        }

        private void PopulateFromConnectionString(string connStr)
        {
            try
            {
                var b = new SqlConnectionStringBuilder(connStr);
                _serverTextBox.Text = b.DataSource;
                if (b.IntegratedSecurity)
                {
                    _authCombo.SelectedIndex = 0;
                }
                else
                {
                    _authCombo.SelectedIndex = 1;
                    _userTextBox.Text = b.UserID;
                    _passTextBox.Text = b.Password;
                }
                _trustCertCheckBox.Checked = b.TrustServerCertificate;
                if (!string.IsNullOrEmpty(b.InitialCatalog))
                {
                    _databaseCombo.Items.Add(b.InitialCatalog);
                    _databaseCombo.Text = b.InitialCatalog;
                }
            }
            catch
            {
                // If parsing fails, just leave fields empty
            }
            UpdateAuthFields();
        }

        private void UpdateAuthFields()
        {
            bool sqlAuth = _authCombo.SelectedIndex == 1;
            _userTextBox.Enabled = sqlAuth;
            _passTextBox.Enabled = sqlAuth;
            if (!sqlAuth)
            {
                _userTextBox.Text = "";
                _passTextBox.Text = "";
            }
        }

        private string BuildConnectionString()
        {
            var b = new SqlConnectionStringBuilder
            {
                DataSource = _serverTextBox.Text.Trim(),
                TrustServerCertificate = _trustCertCheckBox.Checked
            };

            if (_authCombo.SelectedIndex == 0)
            {
                b.IntegratedSecurity = true;
            }
            else
            {
                b.UserID = _userTextBox.Text.Trim();
                b.Password = _passTextBox.Text;
            }

            string db = _databaseCombo.Text.Trim();
            if (!string.IsNullOrEmpty(db))
                b.InitialCatalog = db;

            return b.ConnectionString;
        }

        private void RefreshDatabases()
        {
            _databaseCombo.Items.Clear();
            string saved = _databaseCombo.Text;

            try
            {
                Cursor = Cursors.WaitCursor;
                // Connect without a specific database to list all databases
                var b = new SqlConnectionStringBuilder(BuildConnectionString());
                b.InitialCatalog = "";
                b.ConnectTimeout = 5;

                using (var conn = new SqlConnection(b.ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(
                        "SELECT name FROM sys.databases WHERE name NOT IN ('master','tempdb','model','msdb') ORDER BY name", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            _databaseCombo.Items.Add(reader.GetString(0));
                    }
                }
                if (_databaseCombo.Items.Contains(saved))
                    _databaseCombo.Text = saved;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not retrieve databases:\n" + ex.Message,
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void TestConnection()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var b = new SqlConnectionStringBuilder(BuildConnectionString()) { ConnectTimeout = 5 };
                using (var conn = new SqlConnection(b.ConnectionString))
                {
                    conn.Open();
                }
                MessageBox.Show(this, "Connection successful!", "Test Connection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Connection failed:\n" + ex.Message, "Test Connection",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                string cs = BuildConnectionString();
                if (string.IsNullOrWhiteSpace(new SqlConnectionStringBuilder(cs).DataSource))
                {
                    MessageBox.Show(this, "Please enter a server name.", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                ConnectionString = cs;
            }
            base.OnFormClosing(e);
        }
    }
}
