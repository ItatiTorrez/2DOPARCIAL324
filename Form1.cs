using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mysqlx.Resultset;

namespace _0513formulario
{
    public partial class Form1 : Form
    {
        int cR, cG, cB;
        private ConexionMySQL conexionMySQL;
        private DataTable dataTable;

        public Form1()
        {
            InitializeComponent();
            conexionMySQL = new ConexionMySQL();
            dataTable = new DataTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "archivos jpg |*.jpg";
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            int sR, sG, sB;
            sR = 0;
            sG = 0;
            sB = 0;
            for (int i = e.X; i < e.X + 10; i++)
                for (int j = e.Y; j < e.Y + 10; j++)
                {
                    c = bmp.GetPixel(i, j);
                    sR = sR + c.R;
                    sG = sG + c.G;
                    sB = sB + c.B;
                }
            sR = sR / 100;
            sG = sG / 100;
            sB = sB / 100;
            cR = sR;
            cG = sG;
            cB = sB;
            textBox1.Text = sR.ToString();
            textBox2.Text = sG.ToString();
            textBox3.Text = sB.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(pictureBox1.Image);
            Color c = new Color();

            for (int i=0; i<bmp.Width; i++)
            {
                for (int j=0; j<bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                    if (((80<=c.R)&&(c.R<=160)) && ((84 <= c.G) && (c.G <= 154)) && ((74 <= c.B) && (c.B <= 150)))
                    {
                        bmp2.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        bmp2.SetPixel(i, j, Color.FromArgb(c.R, c.G, c.B));
                    }
                }
            }
            pictureBox1.Image = bmp2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            int sR, sG, sB;
            for (int i = 0; i < bmp.Width - 10; i = i + 10)
                for (int j = 0; j < bmp.Height - 10; j = j + 10)
                {
                    sR = 0; sG = 0; sB = 0;
                    for (int ip = i; ip < i + 10; ip++)
                        for (int jp = j; jp < j + 10; jp++)
                        {
                            c = bmp.GetPixel(ip, jp);
                            sR = sR + c.R;
                            sG = sG + c.G;
                            sB = sB + c.B;
                        }
                    sR = sR / 100;
                    sG = sG / 100;
                    sB = sB / 100;

                    if (((cR - 20 <= sR) && (sR <= cR + 20)) && ((cG - 20 <= sG) && (sG <= cG + 20)) && ((cB - 20 <= sB) && (sB <= cB + 20)))
                    {
                        for (int ip = i; ip < i + 10; ip++)
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                bmp2.SetPixel(ip, jp, Color.Black);
                            }
                    }
                    else
                    {
                        for (int ip = i; ip < i + 10; ip++)
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                c = bmp.GetPixel(ip, jp);
                                bmp2.SetPixel(ip, jp, Color.FromArgb(c.R, c.G, c.B));
                            }
                    }

                }
            pictureBox1.Image = bmp2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CargarDatos();
            conexionMySQL.CerrarConexion();
        }

        private void CargarDatos()
        {
            MySqlConnection conexion = conexionMySQL.AbrirConexion();
            if (conexion != null)
            {
                string consulta = "SELECT * FROM Colores";
                MySqlDataAdapter adaptador = new MySqlDataAdapter(consulta, conexion);
                adaptador.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                conexionMySQL.CerrarConexion();
            }
        }

        private void ObtenerRGB(string nombreColor)
        {
            MySqlConnection conexion = conexionMySQL.AbrirConexion();
            if (conexion != null)
            {
                // Consulta SQL para seleccionar los valores de R, G, B para un color específico
                String nombre_Color = "Verde";
                string consulta = $"SELECT R, G, B FROM Colores WHERE nombre_color = '{nombre_Color}'";
                MySqlCommand comando = new MySqlCommand(consulta, conexion);

                try
                {
                    // Ejecutar la consulta y obtener un lector de datos
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obtener los valores de R, G, B y guardarlos en variables
                            int r = Convert.ToInt32(reader["R"]);
                            int g = Convert.ToInt32(reader["G"]);
                            int b = Convert.ToInt32(reader["B"]);

                            // Aquí puedes usar los valores r, g, b como desees
                            // Por ejemplo, puedes mostrarlos en etiquetas de tu formulario
                            /*lblR.Text = $"Valor de R: {r}";
                            lblG.Text = $"Valor de G: {g}";
                            lblB.Text = $"Valor de B: {b}";*/
                        }
                        else
                        {
                            MessageBox.Show("El color especificado no se encontró en la base de datos.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los valores RGB: " + ex.Message);
                }
                finally
                {
                    conexionMySQL.CerrarConexion();
                }
            }
        }

        /*private void Form1_Load(object sender, EventArgs e)
        {
            //MySqlConnection conexion = conexionMySQL.AbrirConexion();
            CargarDatos();
        }*/

        private void button5_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.RowCount - 1;
            for (int i = 0; i < n; i++)
            {
                int red = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
                int green = Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                int blue = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                String colorp = Convert.ToString(dataGridView1.Rows[i].Cells[5].Value);
                String objeto = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value);
                pintar(red, green, blue, colorp, objeto);
            }
        }
        public void pintar(int cR2, int cG2, int cB2, string pintar, String objeto)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            Color c2 = Color.FromName(pintar);
            int encontrado = 0;
            int sR, sG, sB;
            for (int i = 0; i < bmp.Width - 10; i = i + 10)
                for (int j = 0; j < bmp.Height - 10; j = j + 10)
                {
                    sR = 0; sG = 0; sB = 0;
                    for (int ip = i; ip < i + 10; ip++)
                        for (int jp = j; jp < j + 10; jp++)
                        {
                            c = bmp.GetPixel(ip, jp);
                            sR = sR + c.R;
                            sG = sG + c.G;
                            sB = sB + c.B;
                        }
                    sR = sR / 100;
                    sG = sG / 100;
                    sB = sB / 100;

                    if (((cR2 - 20 <= sR) && (sR <= cR2 + 20)) && ((cG2 - 20 <= sG) && (sG <= cG2 + 20)) && ((cB2 - 20 <= sB) && (sB <= cB2 + 20)))
                    {
                        encontrado++;
                        for (int ip = i; ip < i + 10; ip++)
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                bmp2.SetPixel(ip, jp, c2);
                            }
                    }
                    else
                    {
                        for (int ip = i; ip < i + 10; ip++)
                            for (int jp = j; jp < j + 10; jp++)
                            {
                                c = bmp.GetPixel(ip, jp);
                                bmp2.SetPixel(ip, jp, Color.FromArgb(c.R, c.G, c.B));
                            }
                    }

                }
            pictureBox1.Image = bmp2;
            if (encontrado > 0)
            {
                label8.Text = label8.Text + "\n" + "Encontrados el objeto " + objeto + " lo pintamos de color " + pintar;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            String imagen = textBox4.Text.ToString();
            String pinta_color=textBox5.Text.ToString();

            MySqlConnection conexion = conexionMySQL.AbrirConexion();
            if (conexion != null)
            {
                // Consulta SQL para seleccionar los valores de R, G, B para un color específico
                string consulta = $"INSERT INTO colores(R, G, B, imagen, pinta_color) VALUES({cR}, {cG}, {cB}, '{imagen}','{pinta_color}')";
                MySqlCommand comando = new MySqlCommand(consulta, conexion);

                try
                {
                    // Ejecutar la consulta
                    int filasAfectadas = comando.ExecuteNonQuery();
                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("Color insertado correctamente.");
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                       // label5.Text = "Color insertado correctamente.";
                    }
                    else
                    {
                        MessageBox.Show("haga click en la imagen");
                        //label5.Text = "No se pudo insertar el color.";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los valores RGB: " + ex.Message);
                }
                finally
                {
                    conexionMySQL.CerrarConexion();
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            label8.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
        }
    }
}
