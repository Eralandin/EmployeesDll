using Npgsql;
using SharedModels;

namespace Employees
{
    public partial class AddForm : Form, IConnectionStringConsumer
    {
        private string _connectionString;
        public AddForm()
        {
            InitializeComponent();
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

                string query = "SELECT id, id_parent, menuitem_name, dll_name, function_name, sequence_number FROM tbmodules ORDER BY sequence_number";
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
                                SequenceNumber = reader.GetInt32(reader.GetOrdinal("sequence_number"))
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

            // Если у модуля есть дочерние элементы, не создаем чекбоксы
            if (!moduleDict.ContainsKey(module.Id) || moduleDict[module.Id].Count == 0)
            {
                // Создаем дочерние узлы для типов прав (если нет дочерних элементов)
                var readNode = new TreeNode("Чтение") { Checked = false }; // Можно установить Checked в зависимости от состояния
                var writeNode = new TreeNode("Запись") { Checked = false };
                var editNode = new TreeNode("Изменение") { Checked = false };
                var deleteNode = new TreeNode("Удаление") { Checked = false };

                node.Nodes.Add(readNode);
                node.Nodes.Add(writeNode);
                node.Nodes.Add(editNode);
                node.Nodes.Add(deleteNode);
            }

            // Рекурсивно добавляем дочерние элементы (если они есть)
            if (moduleDict.ContainsKey(module.Id))
            {
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
                    foreach (var role in roles)
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
                    transaction.Commit();
                }
            }
        }
        private List<Role> GetSelectedRoles()
        {
            var roles = new List<Role>();
            foreach (TreeNode node in TreeView.Nodes)
            {
                CollectRoles(node, roles);
            }
            return roles;
        }
        private void CollectRoles(TreeNode node, List<Role> roles)
        {
            var module = (Module)node.Tag;

            // Для модуля, всегда сохраняем значения прав с default значением false
            var role = new Role
            {
                MenuItemId = module.Id,
                AllowRead = false,   // По умолчанию все права false
                AllowWrite = false,
                AllowEdit = false,
                AllowDelete = false
            };

            // Проверяем чекбоксы на наличие значений и изменяем их, если они отмечены
            var allowReadNode = node.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == "Чтение");
            var allowWriteNode = node.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == "Запись");
            var allowEditNode = node.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == "Изменение");
            var allowDeleteNode = node.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == "Удаление");

            if (allowReadNode != null && allowReadNode.Checked)
            {
                role.AllowRead = true;
            }
            if (allowWriteNode != null && allowWriteNode.Checked)
            {
                role.AllowWrite = true;
            }
            if (allowEditNode != null && allowEditNode.Checked)
            {
                role.AllowEdit = true;
            }
            if (allowDeleteNode != null && allowDeleteNode.Checked)
            {
                role.AllowDelete = true;
            }

            // Добавляем роль в список
            roles.Add(role);

            // Рекурсивно обрабатываем дочерние узлы
            foreach (TreeNode childNode in node.Nodes)
            {
                // Игнорируем узлы типа прав (Чтение, Запись, Изменение, Удаление)
                if (childNode.Text == "Чтение" || childNode.Text == "Запись" || childNode.Text == "Изменение" || childNode.Text == "Удаление")
                    continue;

                CollectRoles(childNode, roles);
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

                var roles = GetSelectedRoles();

                SaveRoles(userId, roles);

                MessageBox.Show("Пользователь успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
