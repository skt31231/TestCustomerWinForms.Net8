using System;
using System.Windows.Forms;

namespace TestCustomerWinForms.Net8
{
    public partial class MainForm : Form
    {
        private readonly CustomerRepository _repo = new CustomerRepository();

        public MainForm()
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
            txtName.Focus();
        }

        private void btnLoad_Click(object? sender, EventArgs e) => LoadGrid();

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            try
            {
                var name = (txtName.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Customer Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var id = _repo.Add(new Customer { CustomerName = name });
                txtId.Text = id.ToString();
                LoadGrid();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.DataBoundItem is Customer c && c.CustomerID == id)
                    {
                        row.Selected = true;
                        dgv.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add customer.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("Select a customer to update.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var name = (txtName.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Customer Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _repo.Update(new Customer { CustomerID = id, CustomerName = name });
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update customer.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("Select a customer to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var confirm = MessageBox.Show("Are you sure you want to delete this customer?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _repo.Delete(id);
                    LoadGrid();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete customer.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object? sender, EventArgs e) => ClearInputs();

        private void dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgv.Rows.Count)
            {
                var row = dgv.Rows[e.RowIndex];
                if (row.DataBoundItem is Customer c)
                {
                    txtId.Text = c.CustomerID.ToString();
                    txtName.Text = c.CustomerName;
                }
            }
        }
    }
}