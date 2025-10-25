using System.Data;
using MySql.Data.MySqlClient;

public class Operacoes
{
    private string connectionString = 
    @"server=phpmyadmin.uni9.marize.us;User ID=user_poo;password=S3nh4!F0rt3;database=user_poo;";

    public int Criar(Tarefa tarefa)
    {
        using (var conexao = new MySqlConnection(connectionString))
        {
            conexao.Open();
            string sql = @"INSERT INTO tarefa (nome, descricao, dataCriacao, status, dataExecucao) 
                           VALUES (@nome, @descricao, @dataCriacao, @status, @dataExecucao);
                           SELECT LAST_INSERT_ID();";
            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@nome", tarefa.Nome);
                cmd.Parameters.AddWithValue("@descricao", tarefa.Descricao);
                cmd.Parameters.AddWithValue("@dataCriacao", tarefa.DataCriacao);
                cmd.Parameters.AddWithValue("@status", tarefa.Status);
                cmd.Parameters.AddWithValue("@dataExecucao", tarefa.DataExecucao);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    public Tarefa Buscar(int id)
    {
        using (var conexao = new MySqlConnection(connectionString))
        {
            conexao.Open();
            string sql = @"SELECT id, nome, descricao, dataCriacao, dataExecucao, status 
                           FROM tarefa WHERE id = @id;";
            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var tarefa = new Tarefa();
                        tarefa.Id = reader.GetInt32("id");
                        tarefa.Nome = reader.GetString("nome");
                        tarefa.Descricao = reader.GetString("descricao");
                        tarefa.DataCriacao = reader.GetDateTime("dataCriacao");
                        tarefa.DataExecucao = reader.IsDBNull(reader.GetOrdinal("dataExecucao"))
                            ? (DateTime?)null
                            : reader.GetDateTime("dataExecucao");

                        return tarefa;
                    }
                    return null;
                }
            }
        }
    }

    public IList<Tarefa> Listar()
    {
        var tarefas = new List<Tarefa>();
        using (var conexao = new MySqlConnection(connectionString))
        {
            conexao.Open();
            string sql = @"SELECT id, nome, descricao, dataCriacao, dataExecucao, status FROM tarefa;";
            using (var cmd = new MySqlCommand(sql, conexao))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tarefa = new Tarefa();
                    tarefa.Id = reader.GetInt32("id");
                    tarefa.Nome = reader.GetString("nome");
                    tarefa.Descricao = reader.GetString("descricao");
                    tarefa.DataCriacao = reader.GetDateTime("dataCriacao");
                    tarefa.DataExecucao = reader.IsDBNull(reader.GetOrdinal("dataExecucao"))
                        ? (DateTime?)null
                        : reader.GetDateTime("dataExecucao");

                    tarefas.Add(tarefa);
                }
            }
        }
        return tarefas;
    }

    public void Alterar(Tarefa tarefa)
    {
        using (var conexao = new MySqlConnection(connectionString))
        {
            conexao.Open();
            string sql = @"UPDATE tarefa 
                           SET nome = @nome, descricao = @descricao, dataExecucao = @dataExecucao, status = @status 
                           WHERE id = @id;";
            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@id", tarefa.Id);
                cmd.Parameters.AddWithValue("@nome", tarefa.Nome);
                cmd.Parameters.AddWithValue("@descricao", tarefa.Descricao);
                cmd.Parameters.AddWithValue("@dataExecucao", tarefa.DataExecucao);
                cmd.Parameters.AddWithValue("@status", tarefa.Status);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Excluir(int id)
    {
        using (var conexao = new MySqlConnection(connectionString))
        {
            conexao.Open();
            string sql = @"DELETE FROM tarefa WHERE id = @id;";
            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
