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
        public int _currentUserId;
        public AddForm(float fontSize)
        {
            InitializeComponent();
            ChangeFontSizeInForm(this, fontSize);
        }
        public void ChangeFontSizeInForm(Control? parent, float newSize)
        {
            if (parent == null)
            {
                parent = this;
            }
            foreach (Control control in parent.Controls)
            {
                control.Font = new Font(control.Font.FontFamily, newSize, control.Font.Style);
                if (control is Label || control is Button || control is CheckBox || control is ToolStripMenuItem || control is Panel)
                {
                    control.AutoSize = false;
                }
                if (control.HasChildren)
                {
                    ChangeFontSizeInForm(control, newSize);
                }
            }
            this.Refresh();
        }
        public void Message(string message)
        {
            MessageBox.Show(message);
        }
        public void SetOpenType(string openType, int? selectedId, bool isAdmin)
        {
            try
            {
                if (openType == "Edit" && selectedId != null)
                {
                    if (isAdmin)
                    {
                        AdminCheck.Visible = true;
                        ChangePasswordBtn.Visible = true;
                    }
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
                                    AdminCheck.Checked = reader.GetBoolean(4);
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
                    if (MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Удаление данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (var connection = new NpgsqlConnection(_connectionString))
                        {
                            connection.Open();
                            string query = "SELECT isadmin FROM tbUsers WHERE id = @Id;";
                            using (var command = new NpgsqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("Id", selectedId);
                                using (var reader = command.ExecuteReader())
                                {
                                    bool isSelectedAdmin = false;
                                    if (reader.Read())
                                    {
                                        isSelectedAdmin = reader.GetBoolean(0);
                                    }
                                    else
                                    {
                                        throw new Exception("Выбранный пользователь не найден в базе данных! Попробуйте обновить страницу пользователей.");
                                    }

                                    if (isSelectedAdmin == true && isAdmin == false)
                                    {
                                        throw new Exception("Выбранный пользователь является администратором. У Вас не хватает прав для удаления этого пользователя.");
                                    }
                                    else if (isSelectedAdmin == true && isAdmin == true)
                                    {
                                        throw new Exception("Вы не можете удалить другого администратора.");
                                    }
                                }
                            }
                        }
                        using (var connection = new NpgsqlConnection(_connectionString))
                        {
                            connection.Open();
                            string query = "DELETE FROM tbUsers WHERE id = @Id;";
                            using (var command = new NpgsqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("Id", selectedId);
                                command.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Удаление пользователя успешно!");
                        this.Close();
                    }
                    else
                    {
                        this.Close();
                    }

                }
                else if (openType == "Add")
                {
                    if (isAdmin)
                    {
                        AdminCheck.Visible = true;
                    }
                    CreateBtn.Click += CreateBtn_Click;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public List<Role> GetUserRoles(long userId)
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
        public void MarkTreeViewNodes(TreeView treeView, List<Role> roles)
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
                            case "Чтение":
                                childNode.Checked = role.AllowRead;
                                break;
                            case "Запись":
                                childNode.Checked = role.AllowWrite;
                                break;
                            case "Изменение":
                                childNode.Checked = role.AllowEdit;
                                break;
                            case "Удаление":
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
        public void UpdateUser(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите обновить данные этого пользователя?", "Обновление данных", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (string.IsNullOrEmpty(UsernameTextBox.Text) || string.IsNullOrEmpty(RoleTextBox.Text))
                    {
                        MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        using (var connection = new NpgsqlConnection(_connectionString))
                        {
                            connection.Open();
                            var query = "UPDATE tbUsers SET username = @Username, user_role = @Role, isadmin = @IsAdmin WHERE id = @Id;";

                            using (var command = new NpgsqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("Username", UsernameTextBox.Text);
                                command.Parameters.AddWithValue("Role", RoleTextBox.Text);
                                command.Parameters.AddWithValue("Id", _currentUserId);
                                command.Parameters.AddWithValue("IsAdmin", AdminCheck.Checked);
                                command.ExecuteNonQuery();
                            }
                            var rolesQuery = "DELETE from tbRoles WHERE id_user = @Id";
                            using (var command = new NpgsqlCommand(rolesQuery, connection))
                            {
                                command.Parameters.AddWithValue("Id", _currentUserId);
                                command.ExecuteNonQuery();
                            }
                        }
                        if (!AdminCheck.Checked)
                        {
                            var roles = GetSelectedRoles();
                            SaveRoles(_currentUserId, roles);
                        }

                        MessageBox.Show("Обновление данных пользователя прошло успешно!");
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            if (moduleDict.Count > 0)
            {
                foreach(var module in moduleDict)
                {
                    foreach (var rootModule in moduleDict[module.Key])
                    {
                        var rootNode = CreateTreeNode(rootModule, moduleDict);
                        TreeView.Nodes.Add(rootNode);
                    }
                }
            }
            else
            {
                TreeView.Nodes.Clear();
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
                            if (reader.GetString(reader.GetOrdinal("menuitem_name")) == "Главное меню")
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
                Tag = module // Привязываем объект Module к узлу
            };

            bool isRootNode = module.IdParent == 0;

            if (isRootNode)
            {
                // Добавляем чекбоксы только для узлов верхнего уровня
                var readNode = new TreeNode("Чтение") { Checked = false };
                var writeNode = new TreeNode("Запись") { Checked = false };
                var editNode = new TreeNode("Изменение") { Checked = false };
                var deleteNode = new TreeNode("Удаление") { Checked = false };

                node.Nodes.Add(readNode);
                node.Nodes.Add(writeNode);
                node.Nodes.Add(editNode);
                node.Nodes.Add(deleteNode);
            }

            return node;
        }
        private long SaveUser(string username, string password, string role)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO tbUsers (username, password_hash, user_role, isadmin) VALUES (@username, @password_hash, @user_role, @IsAdmin) RETURNING id", connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password_hash", User.HashPassword(password));
                    command.Parameters.AddWithValue("@user_role", role);
                    command.Parameters.AddWithValue("@IsAdmin", AdminCheck.Checked);

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
                    // Сохраняем роли из TreeView
                    foreach (var role in roles)
                    {
                        SaveRole(userId, role, connection, transaction);
                    }

                    // Сохраняем обязательные роли
                    var necessaryModules = GetModulesFromDatabase().Where(m => m.IsNecessary).ToList();
                    foreach (var necessaryModule in necessaryModules)
                    {
                        var role = new Role
                        {
                            MenuItemId = necessaryModule.Id,
                            AllowRead = true, // Обязательные вкладки всегда доступны
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

            // Проверяем, есть ли дочерние элементы
            if (node.Nodes.Cast<TreeNode>().Any(child => child.Text == "Чтение"))
            {
                var allowRead = node.Nodes.Cast<TreeNode>().First(n => n.Text == "Чтение").Checked;
                var allowWrite = node.Nodes.Cast<TreeNode>().First(n => n.Text == "Запись").Checked;
                var allowEdit = node.Nodes.Cast<TreeNode>().First(n => n.Text == "Изменение").Checked;
                var allowDelete = node.Nodes.Cast<TreeNode>().First(n => n.Text == "Удаление").Checked;

                roles.Add(new Role
                {
                    MenuItemId = module.Id,
                    AllowRead = allowRead,
                    AllowWrite = allowWrite,
                    AllowEdit = allowEdit,
                    AllowDelete = allowDelete
                });
            }

            // Рекурсивно обходим дочерние узлы
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
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (password != checkPass)
                {
                    MessageBox.Show("Введённые пароли не совпадают!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                long userId = SaveUser(username, password, role);

                if (!AdminCheck.Checked)
                {
                    var roles = GetSelectedRoles();

                    SaveRoles(userId, roles);
                }

                MessageBox.Show("Пользователь успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangePasswordBtn_Click(object sender, EventArgs e)
        {
            ChangePasswordForm changeForm = new ChangePasswordForm();
            string newPassword;
            if (changeForm.ShowDialog() == DialogResult.OK)
            {
                newPassword = changeForm.PasswordTextBox.Text;
                changeForm.Close();
            }
            else
            {
                MessageBox.Show("Операция отменена пользователем");
                return;
            }
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "UPDATE tbUsers SET password_hash = @PasswordHash WHERE id = @Id;";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("PasswordHash", User.HashPassword(newPassword));
                        command.Parameters.AddWithValue("Id", _currentUserId);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Смена пароля прошла успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateBtn_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
