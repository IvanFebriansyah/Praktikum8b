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

namespace Praktikum8b
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-ABD7H3A;Initial Catalog=restoran;Integrated Security=True");
        DataTable dt = new DataTable();
        BindingSource bS = new BindingSource();


        private void resetdata()
        {
            txtid.Text = "";
            txtnama.Text = "";
            txtharga.Text = "";
        }


        private void bindingdata()
        {
            //clear data table
            dt.Clear();

            //clear databindings pada textbox
            txtid.DataBindings.Clear();
            txtnama.DataBindings.Clear();
            txtharga.DataBindings.Clear();

            //buat data adapter dan sqlbuilder
            SqlDataAdapter da = new SqlDataAdapter("select * from menu", con);
            SqlCommandBuilder scb = new SqlCommandBuilder(da);

            //fill data ke dalam datatable
            da.Fill(dt);

            //set bindingsource datasource dan tampilkan ke dalam grid
            bS.DataSource = dt;
            dgvmenu.DataSource = bS;

            //binding data ke dalam textbox yang ada
            txtid.DataBindings.Add("Text", bS, "idMenu");
            txtnama.DataBindings.Add("Text", bS, "namaMenu");
            txtharga.DataBindings.Add("Text", bS, "harga");
        }

        private void showdata()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from menu";
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "menu");
            dgvmenu.DataSource = ds;
            dgvmenu.DataMember = "menu";
            dgvmenu.ReadOnly = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'restoranDataSet1.menu' table. You can move, or remove it, as needed.
            showdata();
            resetdata();
            bindingdata();

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            bS.Filter = "namaMenu like '%" + txtfilter.Text + "%'";
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            bS.MoveNext();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            bS.MovePrevious();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            showdata();
            resetdata();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            resetdata();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(txtid.Text == "" | txtnama.Text == "" | txtharga.Text == "")
            {
                MessageBox.Show("Semua data menu harus diisi", "Peringatan");
            }

            int num;
            bool isNum = int.TryParse(txtharga.Text.ToString(), out num);

            if (!isNum)
            {
                MessageBox.Show("Isi harga harus angka", "Peringatan");
                goto berhenti;
            }

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE menu SET namaMenu= '" + txtnama.Text + "',harga=" + int.Parse(txtharga.Text) + "'";
            cmd.ExecuteNonQuery();
            con.Close();
            showdata();
            resetdata();

        berhenti:

            ;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if(txtid.Text == "")
            {
                MessageBox.Show("Isi id menu yang akan dihapus");
                goto berhenti;
            }

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from menu where idmenu = '" + txtid.Text + "'";
            cmd.ExecuteNonQuery();
            con.Close();
            showdata();
            resetdata();

        berhenti:
            ;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if(txtid.Text == "" | txtnama.Text == "" | txtharga.Text == "")
            {
                MessageBox.Show("Semua data menu harus diisi", "Peringatan");
                goto berhenti;
            }

            int num;
            bool isNum = int.TryParse(txtharga.Text.ToString(), out num);

            if (!isNum)
            {
                MessageBox.Show("Isi harus angka", "Peringatan");
                goto berhenti;
            }

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into menu values('" + txtid.Text + "','" + txtnama.Text + "'," + int.Parse(txtharga.Text) + ")";
            cmd.ExecuteNonQuery();
            con.Close();
            showdata();
            resetdata();

        berhenti:
            ;
        }
    }
}
