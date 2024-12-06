using Npgsql;
using SharedModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data;
using Employees.Services;

namespace Employees
{
    public partial class AddForm : Form, IConnectionStringConsumer
    {
        private string _connectionString;
        private int _currentUserId;
        public AddForm()
        {
            InitializeComponent();
        }
        public void SetOpenType(string openType, int? selectedId)
        {
            try
            {
                if (openType == "Edit" && selectedId != null)
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        connection.Open();
                        string query = "SELECT * FROM tbUsers WHERE id = @SelectedId";
                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("SelectedId", selectedId);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    UsernameTextBox.Text = reader.GetString(1);
                                    RoleTextBox.Text = reader.GetString(3);
                                    _currentUserId = reader.GetInt32(0);
                                    PasswordCheckTextBox.Enabled = false;
                                    PasswordTextBox.Enabled = false;
                                    PasswordCheckLabel.Enabled = false;
                                    PasswordLabel.Enabled = false;
                                }
                            }
                        }
                    }
                    var userRoles = GetUserRoles((long)selectedId);
                    MarkTreeViewNodes(TreeView, userRoles);
                    CreateBtn.Click += UpdateUser;
                }
                else if (openType == "Delete")
                {
                    if (MessageBox.Show("�� �������, ��� ������ ������� ����� ������������?", "�������� ������", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (var connection = new NpgsqlConnection(_connectionString))
                        {
                            connection.Open();
                            string query = "DELETE FROM tbUsers WHERE id = @Id;";
                            using (var command = new NpgsqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("Id",selectedId);
                                command.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("�������� ������������ �������!");
                        this.Close();
                    }
                    else
                    {
                        this.Close();
                    }
                    
                }
                else if (openType == "Add")
                {
                    CreateBtn.Click += CreateBtn_Click;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private List<Role> GetUserRoles(long userId)
        {
            var roles = new List<Role>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id_menuitem, allow_read, allow_write, allow_edit, allow_delete FROM tbRoles WHERE id_user = @UserId;";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("UserId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new Role
                            {
                                MenuItemId = reader.GetInt64(reader.GetOrdinal("id_menuitem")),
                                AllowRead = reader.GetBoolean(reader.GetOrdinal("allow_read")),
                                AllowWrite = reader.GetBoolean(reader.GetOrdinal("allow_write")),
                                AllowEdit = reader.GetBoolean(reader.GetOrdinal("allow_edit")),
                                AllowDelete = reader.GetBoolean(reader.GetOrdinal("allow_delete"))
                            });
                        }
                    }
                }
            }
            return roles;
        }
        private void MarkTreeViewNodes(TreeView treeView, List<Role> roles)
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                MarkNodeRecursive(node, roles);
            }
        }

        private void MarkNodeRecursive(TreeNode node, List<Role> roles)
        {
            if (node.Tag is Module module)
            {
                var role = roles.FirstOrDefault(r => r.MenuItemId == module.Id);
                if (role != null)
                {
                    foreach (TreeNode childNode in node.Nodes)
                    {
                        switch (childNode.Text)
                        {
                            case "������":
                                childNode.Checked = role.AllowRead;
                                break;
                            case "������":
                                childNode.Checked = role.AllowWrite;
                                break;
                            case "���������":
                                childNode.Checked = role.AllowEdit;
                                break;
                            case "��������":
                                childNode.Checked = role.AllowDelete;
                                break;
                        }
                    }
                }
            }

            foreach (TreeNode child in node.Nodes)
            {
                MarkNodeRecursive(child, roles);
            }
        }
        private void UpdateUser(object sender, EventArgs e)
        {
            if (MessageBox.Show("�� �������, ��� ������ �������� ������ ����� ������������?","���������� ������",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(UsernameTextBox.Text) || string.IsNullOrEmpty(RoleTextBox.Text))
                {
                    MessageBox.Show("����������, ��������� ��� ����.", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    using (var connection = new NpgsqlConnection(_connectionString))
                    {
                        connection.Open();
                        var query = "UPDATE tbUsers SET username = @Username, user_role = @Role WHERE id = @Id;";

                        using (var command = new NpgsqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("Username", UsernameTextBox.Text);
                            command.Parameters.AddWithValue("Role", RoleTextBox.Text);
                            command.Parameters.AddWithValue("Id", _currentUserId);
                            command.ExecuteNonQuery();
                        }
                        var rolesQuery = "DELETE from tbRoles WHERE id_user = @Id";
                        using (var command = new NpgsqlCommand(rolesQuery, connection))
                        {
                            command.Parameters.AddWithValue("Id", _currentUserId);
                            command.ExecuteNonQuery();
                        }
                    }
                    var roles = GetSelectedRoles();


                    SaveRoles(_currentUserId, roles);

                    MessageBox.Show("���������� ������ ������������ ������ �������!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                return;
            }
        }
        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            LoadModules();
        }
        private void LoadModules()
        {
            TreeView.Nodes.Clear();

            var modules = GetModulesFromDatabase();

            var moduleDict = modules.GroupBy(m => m.IdParent)
                                    .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var rootModule in moduleDict[0])
            {
                var rootNode = CreateTreeNode(rootModule, moduleDict);
                TreeView.Nodes.Add(rootNode);
            }
        }
        private List<Module> GetModulesFromDatabase()
        {
            var modules = new List<Module>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT id, id_parent, menuitem_name, dll_name, function_name, sequence_number,isnecessary FROM tbmodules WHERE isnecessary != true ORDER BY sequence_number";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(reader.GetOrdinal("menuitem_name")) == "������� ����")
                            {
                                continue;
                            }
                            var module = new Module
                            {
                                Id = reader.GetInt64(reader.GetOrdinal("id")),
                                IdParent = reader.GetInt32(reader.GetOrdinal("id_parent")),
                                MenuItemName = reader.GetString(reader.GetOrdinal("menuitem_name")),
                                DllName = reader.IsDBNull(reader.GetOrdinal("dll_name")) ? null : reader.GetString(reader.GetOrdinal("dll_name")),
                                FunctionName = reader.IsDBNull(reader.GetOrdinal("function_name")) ? null : reader.GetString(reader.GetOrdinal("function_name")),
                                SequenceNumber = reader.GetInt32(reader.GetOrdinal("sequence_number")),
                                IsNecessary = reader.GetBoolean(reader.GetOrdinal("isnecessary"))
                            };

                            modules.Add(module);
                        }
                    }
                }
            }

            return modules;
        }
        private TreeNode CreateTreeNode(Module module, Dictionary<long, List<Module>> moduleDict)
        {
            var node = new TreeNode(module.MenuItemName)
            {
                Tag = module // ����������� ������ Module � ����
            };

            // ���� � ������ ���� �������� ��������, �� ������� ��������
            if (!moduleDict.ContainsKey(module.Id) || moduleDict[module.Id].Count == 0)
            {
                // �������� ������ �� ����� ��� �������� ���������
                var readNode = new TreeNode("������") { Checked = false };
                var writeNode = new TreeNode("������") { Checked = false };
                var editNode = new TreeNode("���������") { Checked = false };
                var deleteNode = new TreeNode("��������") { Checked = false };

                node.Nodes.Add(readNode);
                node.Nodes.Add(writeNode);
                node.Nodes.Add(editNode);
                node.Nodes.Add(deleteNode);
            }
            else
            {
                // �� ��������� ��������, ���� � ���� ���� �������� ��������
                foreach (var childModule in moduleDict[module.Id])
                {
                    node.Nodes.Add(CreateTreeNode(childModule, moduleDict));
                }
            }

            return node;
        }
        private long SaveUser(string username, string password, string role)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO tbUsers (username, password_hash, user_role) VALUES (@username, @password_hash, @user_role) RETURNING id", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password_hash", User.HashPassword(password));
                    command.Parameters.AddWithValue("@user_role", role);

                    return (long)command.ExecuteScalar();
                }
            }
        }
        private void SaveRoles(long userId, List<Role> roles)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // ��������� ���� �� TreeView
                    foreach (var role in roles)
                    {
                        SaveRole(userId, role, connection, transaction);
                    }

                    // ��������� ������������ ����
                    var necessaryModules = GetModulesFromDatabase().Where(m => m.IsNecessary).ToList();
                    foreach (var necessaryModule in necessaryModules)
                    {
                        var role = new Role
                        {
                            MenuItemId = necessaryModule.Id,
                            AllowRead = true, // ������������ ������� ������ ��������
                            AllowWrite = false,
                            AllowEdit = false,
                            AllowDelete = false
                        };

                        SaveRole(userId, role, connection, transaction);
                    }

                    transaction.Commit();
                }
            }
        }
        private void SaveRole(long userId, Role role, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand(
                "INSERT INTO tbRoles (id_user, id_menuitem, allow_read, allow_write, allow_edit, allow_delete) " +
                "VALUES (@id_user, @id_menuitem, @allow_read, @allow_write, @allow_edit, @allow_delete)", connection, transaction))
            {
                command.Parameters.AddWithValue("@id_user", userId);
                command.Parameters.AddWithValue("@id_menuitem", role.MenuItemId);
                command.Parameters.AddWithValue("@allow_read", role.AllowRead);
                command.Parameters.AddWithValue("@allow_write", role.AllowWrite);
                command.Parameters.AddWithValue("@allow_edit", role.AllowEdit);
                command.Parameters.AddWithValue("@allow_delete", role.AllowDelete);

                command.ExecuteNonQuery();
            }
        }
        private List<Role> GetSelectedRoles()
        {
            var roles = new List<Role>();
            foreach (TreeNode node in TreeView.Nodes)
            {
                CollectParentRoles(node, roles);
            }
            return roles;
        }
        private void CollectParentRoles(TreeNode node, List<Role> roles)
        {
            var module = (Module)node.Tag;

            // ���������, ���� �� �������� ��������
            if (node.Nodes.Cast<TreeNode>().Any(child => child.Text == "������"))
            {
                var allowRead = node.Nodes.Cast<TreeNode>().First(n => n.Text == "������").Checked;
                var allowWrite = node.Nodes.Cast<TreeNode>().First(n => n.Text == "������").Checked;
                var allowEdit = node.Nodes.Cast<TreeNode>().First(n => n.Text == "���������").Checked;
                var allowDelete = node.Nodes.Cast<TreeNode>().First(n => n.Text == "��������").Checked;

                roles.Add(new Role
                {
                    MenuItemId = module.Id,
                    AllowRead = allowRead,
                    AllowWrite = allowWrite,
                    AllowEdit = allowEdit,
                    AllowDelete = allowDelete
                });
            }

            // ���������� ������� �������� ����
            foreach (TreeNode childNode in node.Nodes)
            {
                CollectParentRoles(childNode, roles);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string username = UsernameTextBox.Text;
                string password = PasswordTextBox.Text;
                string checkPass = PasswordCheckTextBox.Text;
                string role = RoleTextBox.Text;

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(checkPass))
                {
                    MessageBox.Show("����������, ��������� ��� ����.", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (password != checkPass)
                {
                    MessageBox.Show("�������� ������ �� ���������!", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                long userId = SaveUser(username, password, role);

                var roles = GetSelectedRoles();

                SaveRoles(userId, roles);

                MessageBox.Show("������������ ������� ��������!", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("������ ��� ���������� ������: " + ex.Message, "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
