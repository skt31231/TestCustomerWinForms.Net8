using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TestCustomerWinForms.Net8
{
    public partial class CategoryForm : Form
    {
        private readonly CategoryRepository _repo = new CategoryRepository();

        public CategoryForm()
        {
            InitializeComponent();
        }

        private void LoadGrid()
        {
            try
            {
                var data = _repo.GetAll();
                dgv.DataSource = data;
                dgv.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load data.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputs()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            picPicture.Image = null;
            txtName.Focus();
        }

        private void btnLoad_Click(object? sender, EventArgs e)
        {
            LoadGrid();
            Testing.ShowMessage();
        }

        private byte[]? GetImageBytes()
        {
            if (picPicture.Image == null) return null;
            using var ms = new MemoryStream();
            picPicture.Image.Save(ms, picPicture.Image.RawFormat);
            return ms.ToArray();
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            try
            {
                var name = (txtName.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Category Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var desc = txtDescription.Text;
                var pic = GetImageBytes();
                var id = _repo.Add(new Category { CategoryName = name, Description = desc, Picture = pic });
                txtId.Text = id.ToString();
                LoadGrid();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.DataBoundItem is Category c && c.CategoryID == id)
                    {
                        row.Selected = true;
                        dgv.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add category.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("Select a category to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var name = (txtName.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Category Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var desc = txtDescription.Text;
                var pic = GetImageBytes();
                _repo.Update(new Category { CategoryID = id, CategoryName = name, Description = desc, Picture = pic });
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update category.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("Select a category to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var confirm = MessageBox.Show("Are you sure you want to delete this category?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _repo.Delete(id);
                    LoadGrid();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete category.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object? sender, EventArgs e) => ClearInputs();

        private void dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgv.Rows.Count)
            {
                var row = dgv.Rows[e.RowIndex];
                if (row.DataBoundItem is Category c)
                {
                    txtId.Text = c.CategoryID.ToString();
                    txtName.Text = c.CategoryName;
                    txtDescription.Text = c.Description ?? "";
                    picPicture.Image = c.Picture != null ? Image.FromStream(new MemoryStream(c.Picture)) : null;
                }
            }
        }

        private void btnLoadImage_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog { Filter = "Images|*.png;*.jpg;*.jpeg;*.gif;*.bmp" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                picPicture.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void btnClearImage_Click(object? sender, EventArgs e) => picPicture.Image = null;
    }
}
