using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Tarefas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            //Crio a estrutura da conexão com o banco e passa os parametros
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
            conexaoBD.Server = "localhost";
            conexaoBD.Database = "banco_tarefas";
            conexaoBD.UserID = "root";
            conexaoBD.Password = "";
            //Realizo a conexão com o banco
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open(); //Abre a conexão com o banco
                //MessageBox.Show("Conexão Aberta!");

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); //Crio um comando SQL
                comandoMySql.CommandText = "INSERT INTO tarefas (dia_da_semana,titulo,descricao) " +
                    "VALUES('" + mtxtData.Text + "', '" + txtTitulo.Text + "', '" + txtDescricao.Text + "')";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close(); // Fecho a conexão com o banco
                MessageBox.Show("Inserido com sucesso"); //Exibo mensagem de aviso
                atualizarGrid();
                limparCampos();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Não foi possivel abrir a conexão! ");
                Console.WriteLine(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            atualizarGrid();
        }

        private void atualizarGrid()
        {
            //Crio a estrutura da conexão com o banco e passa os parametros
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
            conexaoBD.Server = "localhost";
            conexaoBD.Database = "banco_tarefas";
            conexaoBD.UserID = "root";
            conexaoBD.Password = "";
            //Realizo a conexão com o banco
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open();

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand();
                comandoMySql.CommandText = "SELECT * from tarefas ORDER BY dia_da_semana"; //Traz todo mundo da tabela tarefas
                MySqlDataReader reader = comandoMySql.ExecuteReader();

                dataGridView1.Rows.Clear();//FAZ LIMPAR A TABELA

                while (reader.Read())
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();//FAZ UM CAST E CLONA A LINHA DA TABELA
                    //DataGridViewRow row = new DataGridViewRow();
                    row.Cells[0].Value = reader.GetInt32(0);//ID
                    row.Cells[1].Value = reader.GetString(1);//DATA
                    row.Cells[2].Value = reader.GetString(2);//TITULO
                    row.Cells[3].Value = reader.GetString(3);//DESCRICAO
                    dataGridView1.Rows.Add(row);//ADICIONO A LINHA NA TABELA
                }

                realizaConexacoBD.Close();
                //MessageBox.Show("Removido com sucesso"); ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //MessageBox.Show("Can not open connection ! ");
                Console.WriteLine(ex.Message);
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                dataGridView1.CurrentRow.Selected = true;
                //preenche os textbox com as células da linha selecionada
                txtID.Text = dataGridView1.Rows[e.RowIndex].Cells["id_tarefas"].FormattedValue.ToString();
                mtxtData.Text = dataGridView1.Rows[e.RowIndex].Cells["dia_da_semana"].FormattedValue.ToString();
                txtTitulo.Text = dataGridView1.Rows[e.RowIndex].Cells["titulo"].FormattedValue.ToString();
                txtDescricao.Text = dataGridView1.Rows[e.RowIndex].Cells["descricao"].FormattedValue.ToString();
            }
        }

        private void limparCampos()
        {
            txtID.Clear();
            mtxtData.Clear();
            txtTitulo.Clear();
            txtDescricao.Clear();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tem certeza que quer deletar esta tarefa?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
                conexaoBD.Server = "localhost";
                conexaoBD.Database = "banco_tarefas";
                conexaoBD.UserID = "root";
                conexaoBD.Password = "";
                MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
                try
                {
                    realizaConexacoBD.Open(); //Abre a conexão com o banco

                    MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); //Crio um comando SQL
                    comandoMySql.CommandText = "DELETE FROM tarefas WHERE id_tarefas = " + txtID.Text + "";
                    comandoMySql.ExecuteNonQuery();

                    realizaConexacoBD.Close(); // Fecho a conexão com o banco
                    MessageBox.Show("Deletado com sucesso"); //Exibo mensagem de aviso
                    atualizarGrid();
                    limparCampos();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Não foi possivel abrir a conexão! ");
                    Console.WriteLine(ex.Message);
                }
            };  
            
           
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder conexaoBD = new MySqlConnectionStringBuilder();
            conexaoBD.Server = "localhost";
            conexaoBD.Database = "banco_tarefas";
            conexaoBD.UserID = "root";
            conexaoBD.Password = "";
            MySqlConnection realizaConexacoBD = new MySqlConnection(conexaoBD.ToString());
            try
            {
                realizaConexacoBD.Open(); //Abre a conexão com o banco

                MySqlCommand comandoMySql = realizaConexacoBD.CreateCommand(); //Crio um comando SQL
                comandoMySql.CommandText = "UPDATE tarefas SET dia_da_semana = '" + mtxtData.Text + "', " +
                    "titulo = '" + txtTitulo.Text + "', " +
                    "descricao = '" + txtDescricao.Text + "' WHERE id_tarefas = " + txtID.Text + "";
                comandoMySql.ExecuteNonQuery();

                realizaConexacoBD.Close(); // Fecho a conexão com o banco
                MessageBox.Show("Atualizado com sucesso"); //Exibo mensagem de aviso
                atualizarGrid();
                limparCampos();
                btnEditar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possivel abrir a conexão! ");
                Console.WriteLine(ex.Message);
            }
            
        }

        private void btnLimpar_Click_1(object sender, EventArgs e)
        {
            limparCampos();
        }

       
    }
}
