using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace StudentInformationManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GridleriDoldur()
        {
            gridOzluk.DataSource = SQL.SelectCalistir("SELECT * FROM TBL_OZLUK");
            gridDers.DataSource = SQL.SelectCalistir("SELECT * FROM TBL_DERS");
            gridNot.DataSource = SQL.SelectCalistir("SELECT * FROM TBL_Not");
        }

        private void Temizle()
        {
            txtTcNo.Clear();
            txtAdi.Clear();
            txtSoyad.Clear();
            txtDersAdi.Clear();
            txtPuan.Clear();
        }

        private void Bagla()
        {
            DataSet ds = new DataSet();

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TBL_OZLUK", SQL.baglanti);
            SqlDataAdapter da2 = new SqlDataAdapter("SELECT * FROM TBL_DERS", SQL.baglanti);
            SqlDataAdapter da3 = new SqlDataAdapter("SELECT * FROM TBL_Not", SQL.baglanti);

            DataTable dt = new DataTable("TBL_OZLUK");
            da.Fill(dt);
            DataTable dt2 = new DataTable("TBL_DERS");
            da2.Fill(dt2);
            DataTable dt3 = new DataTable("TBL_Not");
            da3.Fill(dt3);

            ds.Tables.Add(dt);
            ds.Tables.Add(dt2);
            ds.Tables.Add(dt3);

            DataRelation dr = new DataRelation("baglanti", dt.Columns["TcKimlik"], dt2.Columns["Tckimlik"]);
            DataRelation dr2 = new DataRelation("baglanti2", dt2.Columns["TcKimlik"], dt3.Columns["TcKimlik"]);

            ds.Relations.Add(dr);
            ds.Relations.Add(dr2);

            dataGrid1.DataSource = ds;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridleriDoldur();
            Bagla();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (SQL.InsertUpdateDeleteCalistir("INSERT INTO TBL_OZLUK VALUES ('" + txtTcNo.Text + "','" + txtAdi.Text + "','" + txtSoyad.Text + "')"))
                {
                    if (SQL.InsertUpdateDeleteCalistir("INSERT INTO TBL_DERS VALUES ('" + txtTcNo.Text + "','" + txtDersAdi.Text + "')"))
                    {
                        if (SQL.InsertUpdateDeleteCalistir("INSERT INTO TBL_Not VALUES ('" + txtTcNo.Text + "'," + Convert.ToDecimal(txtPuan.Text) + ")"))
                        {
                            MessageBox.Show("Kaydetme işlemi başarılı", "Bilgi Mesajı", MessageBoxButtons.OK);
                            GridleriDoldur();
                            Temizle();
                        }
                        else
                        {
                            SQL.InsertUpdateDeleteCalistir("DELETE FROM TBL_OZLUK WHERE TcKimlik='" + txtTcNo.Text + "'");
                            SQL.InsertUpdateDeleteCalistir("DELETE FROM TBL_DERS WHERE TcKimlik='" + txtTcNo.Text + "'");
                        }
                    }
                    else
                    {
                        SQL.InsertUpdateDeleteCalistir("DELETE FROM TBL_OZLUK WHERE TcKimlik='" + txtTcNo.Text + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA:" + ex.Message.ToString(), "Hata Mesajı", MessageBoxButtons.OK);
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTcNo.Text != "")
                {
                    DataTable dataTable;
                    string adi, soyadi, dersAdi;

                    dataTable = SQL.SelectCalistir("SELECT * FROM TBL_OZLUK WHERE TcKimlik='" + txtTcNo.Text + "'");
                    adi = dataTable.Rows[0]["Adi"].ToString();
                    soyadi = dataTable.Rows[0]["Soyad"].ToString();

                    dataTable = SQL.SelectCalistir("SELECT * FROM TBL_DERS WHERE TcKimlik='" + txtTcNo.Text + "'");
                    dersAdi = dataTable.Rows[0]["Ders_Adi"].ToString();

                    if (SQL.InsertUpdateDeleteCalistir("DELETE FROM TBL_OZLUK WHERE TcKimlik='" + txtTcNo.Text + "'"))
                    {
                        if (SQL.InsertUpdateDeleteCalistir("DELETE FROM TBL_DERS WHERE TcKimlik='" + txtTcNo.Text + "'"))
                        {
                            if (SQL.InsertUpdateDeleteCalistir("DELETE FROM TBL_Not WHERE TcKimlik='" + txtTcNo.Text + "'"))
                            {
                                MessageBox.Show("Silme işlemi başarılı", "Bilgi Mesajı", MessageBoxButtons.OK);
                                GridleriDoldur();
                                Temizle();
                            }
                            else
                            {
                                SQL.InsertUpdateDeleteCalistir("INSERT INTO TBL_OZLUK VALUES ('" + txtTcNo.Text + "','" + adi + "','" + soyadi + "')");
                                SQL.InsertUpdateDeleteCalistir("INSERT INTO TBL_DERS VALUES ('" + txtTcNo.Text + "','" + dersAdi + "')");
                            }
                        }
                        else
                        {
                            SQL.InsertUpdateDeleteCalistir("INSERT INTO TBL_OZLUK VALUES ('" + txtTcNo.Text + "','" + adi + "','" + soyadi + "')");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Tc Kimlik alanı boş geçilemez", "Hata Mesajı", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA:" + ex.Message.ToString(), "Hata Mesajı", MessageBoxButtons.OK);
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTcNo.Text != "")
                {
                    if (SQL.InsertUpdateDeleteCalistir("UPDATE TBL_OZLUK SET Adi='" + txtAdi.Text + "',Soyad='" + txtSoyad.Text + "' WHERE TcKimlik='" + txtTcNo.Text + "'"))
                    {
                        if (SQL.InsertUpdateDeleteCalistir("UPDATE TBL_DERS SET Ders_Adi = '" + txtDersAdi.Text + "' WHERE TcKimlik='" + txtTcNo.Text + "'"))
                        {
                            if (SQL.InsertUpdateDeleteCalistir("UPDATE TBL_Not SET Puan = " + Convert.ToDecimal(txtPuan.Text) + " WHERE TcKimlik='" + txtTcNo.Text + "'"))
                            {
                                MessageBox.Show("Güncelleme işlemi başarılı", "Bilgi Mesajı", MessageBoxButtons.OK);
                                GridleriDoldur();
                                Temizle();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Tc Kimlik alanı boş geçilemez", "Hata Mesajı", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA:" + ex.Message.ToString(), "Hata Mesajı", MessageBoxButtons.OK);
            }
        }
    }
}
