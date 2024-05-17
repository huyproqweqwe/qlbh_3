using QuanLiBanHang.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiBanHang
{
   
    public partial class frmHoaDon : Form
    {
        public string TimKiem { get; set; }
        DataTable tblCTHoaDon;
        int chucNangDaChon = ChucNang.NONE;
        public frmHoaDon()
        {
            InitializeComponent();
        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
            btnLuu.Enabled = false;
            LoadComboBoxMaHoaDon();
        }

        private void LoadDataGridView()
        {
            txtMaHD.Enabled = false;
            txtTenKH.Enabled = false;
            txtDiaChi.Enabled = false;
            cboMaKH.Enabled = false;
            mtbDienThoai.Enabled = false;
            cboMaDC.Enabled = false;
            txtSoLuong.Enabled = false;
            txtTenDC.Enabled = false;
            txtDonGia.Enabled = false;
            txtThanhTien.Enabled = false;
            dtpNgayLapHD.Enabled = false;

            //string sql = "SELECT * FROM ChiTietHoaDon";
            //tblCTHoaDon = Class.Functions.GetDataToTable(sql);
            //dgvHDBanHang.DataSource = tblCTHoaDon;
            string qr = "SELECT MaKhachHang FROM KhachHang";
            DataTable dtMaNhaCungCap = Class.Functions.GetDataToTable(qr);

            dgvHDBanHang.DataSource = tblCTHoaDon;
            cboMaKH.DisplayMember = "MaKhachHang";  // Hiển thị mã nhà cung cấp
            cboMaKH.ValueMember = "MaKhachHang";   // Giá trị của mỗi mục là mã nhà cung cấp

            dgvHDBanHang.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvHDBanHang.AllowUserToAddRows = false;
        }

        private void btnPhieuGhiNo_Click(object sender, EventArgs e)
        {
            frmPhieuTraNo frmPhieuTraNo = new frmPhieuTraNo();
            this.Hide();
            frmPhieuTraNo.ShowDialog();
            this.Show();
        }

        private void btnPhieuHen_Click(object sender, EventArgs e)
        {
            frmPhieuHen frm = new frmPhieuHen();
            this.Hide();
            frm.ShowDialog();
            this.Show();
        }

        private void ResetValue()
        {
            //txtMaHD.Text = "";
            txtTenKH.Text = "";
            cboMaKH.Text = "";
            mtbDienThoai.Text = "";
            txtDiaChi.Text = "";
            
            cboMaDC.Text = "";
            txtTenDC.Text = "";
            txtThanhTien.Text = "";
            txtSoLuong.Text = "";
            txtDonGia.Text = "";
        }

        private void SetStateControl(bool trangThai)
        {
            btnThem.Enabled = trangThai;
            btnXoa.Enabled = trangThai;
            btnLuu.Enabled = !trangThai;
            btnHuy.Enabled = !trangThai;
            btnExit.Enabled = trangThai;
        }

        private void SwitchMode(int chucNang)
        {
            chucNangDaChon = chucNang;
            switch (chucNang)
            {
                case ChucNang.ADD:
                    SetStateControl(false);
                    txtMaHD.Text = Class.Functions.CreateKey("HDB");
                    cboMaDC.Enabled = true;
                    txtSoLuong.Enabled = true;
                    txtDiaChi.Enabled = true;
                    txtTenKH.Enabled = true;
                    cboMaKH.Enabled = true;
                    dtpNgayLapHD.Enabled = true;
                    mtbDienThoai.Enabled = true;
                    txtDonGia.Enabled = true;
                    ResetValue();
                    break;

                case ChucNang.UPDATE:
                    SetStateControl(false);
                    txtTenKH.Enabled = true;
                    txtDiaChi.Enabled = true;
                    mtbDienThoai.Enabled = true;
                    break;

                case ChucNang.NONE:
                    SetStateControl(true);
                    txtTenKH.Enabled = false;
                    txtDiaChi.Enabled = false;
                    mtbDienThoai.Enabled = false;
                    break;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (chucNangDaChon == ChucNang.NONE)
                SwitchMode(ChucNang.ADD);
            else
                SwitchMode(ChucNang.NONE);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

        }
        private void LoadComboBoxMaHoaDon()
        {
            // Kết nối cơ sở dữ liệu
            string connectionString = "Data Source=LAPTOP-G77V1054\\DANGHUY;Initial Catalog=QuanLiDoChoi;Integrated Security=True"; // Thay YOUR_CONNECTION_STRING_HERE bằng chuỗi kết nối của bạn
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Thực hiện truy vấn để lấy dữ liệu từ bảng HoaDon
                string query = "SELECT MaHoaDon FROM HoaDon";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                // Xóa dữ liệu cũ trong ComboBox
                cboMaHoaDon.Items.Clear();

                // Đọc dữ liệu từ SqlDataReader và thêm vào ComboBox
                while (reader.Read())
                {
                    string maHoaDon = reader.GetString(0); // Lấy giá trị ở cột MaHoaDon
                    cboMaHoaDon.Items.Add(maHoaDon);
                }

                // Đóng kết nối và SqlDataReader
                reader.Close();
                connection.Close();
            }
        }
        private void btnTim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn cần tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimKiem = cboMaHoaDon.Text.Trim();

            // Tạo instance của frmTimKiemHD và truyền form hiện tại (frmHoaDon) vào constructor
            frmTimKiemHD timKiemHDForm = new frmTimKiemHD(this);

            // Hiển thị form frmTimKiemHD
            timKiemHDForm.ShowDialog();
        }
    }
}
