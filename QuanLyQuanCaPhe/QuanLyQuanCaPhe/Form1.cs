using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyQuanCaPhe
{
    public partial class FormMain : Form
    {
        string strCon = "Data Source=LAPTOP-4TEMH196;Initial Catalog=QuanLyQuanCaPhe;Integrated Security=True";
        SqlConnection sqlCon = null;

        bool checkNum(string s)
        {
            try
            {
                int num = int.Parse(s);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void LoadData()
        {
            string sqlSelect = "select * from MenuCafe";
            SqlDataAdapter daMenu = new SqlDataAdapter(sqlSelect, sqlCon);
            DataTable dtMenu = new DataTable();
            dtMenu.Clear();
            daMenu.Fill(dtMenu);
            dataGridView1.DataSource = dtMenu;
        }
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                sqlCon = new SqlConnection(strCon);
                sqlCon.Open();
                LoadData();
            }catch (Exception)
            {
                MessageBox.Show("Không kết nối đươc với SQL.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMa.Text != "")
                {
                    string execute = "INSERT INTO MenuCafe values (N'" + txtMa.Text + "', N'" + txtTen.Text
                        + "', N'" + cbbLoaidouong.Text + "', " + txtGia.Text + ", " + nudSoLuong.Value + ")";
                    SqlCommand cmd = new SqlCommand(execute, sqlCon);
                    cmd.ExecuteNonQuery();
                    LoadData();
                    MessageBox.Show("Thêm đồ uống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Bạn chưa thêm đồ uống vào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("ID bi trung", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (txtMa.Text != "")
            {
                string execute = "Update MenuCafe set [Mã đồ uống] = N'" + txtMa.Text + "', [Tên đồ uống] = N'" 
                    + txtTen.Text + "', [Loại đồ uống] = N'" + cbbLoaidouong.Text + "', [Giá đồ uống] = N'" 
                    + txtGia.Text + "', [Số lượng] = N'" + nudSoLuong.Value+ "' where [Mã đồ uống] = N'" + txtMa.Text + "'";
                SqlCommand cmd = new SqlCommand(execute, sqlCon);
                cmd.ExecuteNonQuery();
                LoadData();
                MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Bạn chưa có đồ uống nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            else
            {
                string execute = "Delete from MenuCafe where [Mã đồ uống] = N'" + txtMa.Text + "'";
                SqlCommand cmd = new SqlCommand(execute, sqlCon);
                cmd.ExecuteNonQuery();
                LoadData();
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[index];
                txtMa.Text = row.Cells[0].Value.ToString();
                txtTen.Text = row.Cells[1].Value.ToString();
                cbbLoaidouong.Text = row.Cells[2].Value.ToString();
                txtGia.Text = row.Cells[3].Value.ToString();
                nudSoLuong.Value = int.Parse(row.Cells[4].Value.ToString());
                txtMa.ReadOnly = true;
            }
            catch(Exception)
            {
                nudSoLuong.Value = 0;
            }
        }

        private void txtSeach_TextChanged(object sender, EventArgs e)
        {
            string execute = "select * from MenuCafe where [Mã đồ uống] like '%" + txtSearch.Text.Trim() + "%'";
            SqlDataAdapter daMenu = new SqlDataAdapter(execute, sqlCon);
            DataTable dtMenu = new DataTable();
            dtMenu.Clear();
            daMenu.Fill(dtMenu);
            dataGridView1.DataSource = dtMenu;
            /*if (txtSearch.Text == "")
            {
                LoadData();
            }*/
        }

        private void txtGia_TextChanged(object sender, EventArgs e)
        {
            if(!checkNum(txtGia.Text) && txtGia.Text != "")
            {
                MessageBox.Show("Bạn nhập sai kiểu dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            string execute = "select * from MenuCafe where [Mã đồ uống] like '%" + txtSearch.Text.Trim() + "%'";
            SqlCommand cmd = new SqlCommand(execute, sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                reader.Close();
                MessageBox.Show("Không tìm thấy kết quả.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                reader.Close();
                MessageBox.Show("Tìm kiếm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            /*string select = " select * from MenuCafe where [Mã đồ uống] = '" + txtSearch.Text.Trim() + "'";
            SqlCommand cmd = new SqlCommand(select, sqlCon);
            cmd.ExecuteNonQuery();
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.Read())
            {
                MessageBox.Show("Không tìm thấy kết quả", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                dr.Close();
                SqlDataAdapter adSearch = new SqlDataAdapter(cmd);
                DataTable dtSearch = new DataTable();
                adSearch.Fill(dtSearch);
                dataGridView1.DataSource = dtSearch;
            }*/
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sqlCon.Close();
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtMa.ReadOnly = false;
            txtMa.Text = "";
            txtTen.Text = "";
            txtGia.Text = "";
            cbbLoaidouong.Text = "";
            nudSoLuong.Value = 0;
        }
    }
}
