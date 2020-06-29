using IHC.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace IHC.Web.Controllers
{
    public class Conexao
    {
        public static void Insert(object o)
        {
            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);

            var Tabela = o.GetType().Name;
            var Propriedades = o.GetType().GetProperties();

            string comando = "INSERT INTO " + Tabela + " (";

            foreach (var p in Propriedades)
            {
                if (p.Name != "Id")
                {
                    comando += p.Name + ", ";
                }
            }
            comando = comando.Remove(comando.Length - 2, 2);
            comando += ") VALUES (";
            foreach (var p in Propriedades)
            {
                if (p.Name != "Id")
                {
                    comando += "@" + p.Name + ", ";
                }
            }
            comando = comando.Remove(comando.Length - 2, 2);
            comando += ")";
            command.CommandText = comando;

            int i = 0;
            foreach (var p in Propriedades)
            {
                var value = o.GetType().GetProperty(p.Name).GetValue(o, null);
                if (value == null)
                {
                    value = DBNull.Value;
                }
                if (p.Name != "Id")
                {

                    command.Parameters.AddWithValue(p.Name, value);
                    i++;
                }
            }

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
        }

        public static void Update(string query)
        {
            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteScalar();
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
        }

        public static List<Colaborador> ListColaborador(string query)
        {
            List<Colaborador> colaborador = new List<Colaborador>();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    colaborador.Add(
                    new Colaborador
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        CPF = reader["CPF"].ToString(),
                        Telefone = reader["Telefone"].ToString(),
                        Email = reader["Email"].ToString(),
                        IdTipoContratacao = Int32.Parse(reader["IdTipoContratacao"].ToString()),

                    });
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return colaborador;
        }

        public static Colaborador Colaborador(string query)
        {
            Colaborador colaborador = new Colaborador();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    colaborador =
                    new Colaborador
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        Nome = reader["Nome"].ToString(),
                        CPF = reader["CPF"].ToString(),
                        Telefone = reader["Telefone"].ToString(),
                        Email = reader["Email"].ToString(),
                        IdTipoContratacao = Int32.Parse(reader["IdTipoContratacao"].ToString()),

                    };
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return colaborador;
        }

        public static List<Empresa> ListEmpresa(string query)
        {
            List<Empresa> empresa = new List<Empresa>();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    empresa.Add(
                    new Empresa
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        RazaoSocial = reader["RazaoSocial"].ToString(),
                        CNPJ = reader["CNPJ"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefone = reader["Telefone"].ToString(),
                        Responsavel = reader["Responsavel"].ToString(),

                    });
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return empresa;
        }

        public static bool VerificaUsuario(string login, string senha)
        {
            string query = @"
IF EXISTS 
	(SELECT * FROM Usuario WHERE Login = '{0}' and Senha = '{1}')
	SELECT 'true'
ELSE
	SELECT 'false'
";


            query = string.Format(query, login, senha);


            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = (string)command.ExecuteScalar();
                if (reader == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }


        }

        public static bool VerificaLogin(string login)
        {

            string query = @"
IF EXISTS 
	(SELECT * FROM Usuario WHERE Login = '{0}')
	SELECT 'true'
ELSE
	SELECT 'false'
";


            query = string.Format(query, login);


            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = (string)command.ExecuteScalar();
                if (reader == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }



        }

        public static bool VerificaLoginColaborador(int idColaborador)
        {

            string query = @"
IF EXISTS 
	(SELECT * FROM Usuario WHERE idColaborador = '{0}')
	SELECT 'true'
ELSE
	SELECT 'false'
";


            query = string.Format(query, idColaborador);


            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = (string)command.ExecuteScalar();
                if (reader == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }



        }

        public static Usuario Usuario(string query)
        {
            Usuario usuario = new Usuario();



            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    usuario =
                    new Usuario
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        Login = reader["Login"].ToString(),
                        Perfil = reader["Perfil"].ToString(),
                        Permissao = reader["Permissao"].ToString(),
                        IdColaborador = Int32.Parse(reader["IdColaborador"].ToString() == "" ? "0" : reader["IdColaborador"].ToString()),
                        IdEmpresa = Int32.Parse(reader["IdEmpresa"].ToString() == "" ? "0" : reader["IdEmpresa"].ToString()),

                    };
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return usuario;
        }

        public static List<Convocacao> ListConvocacao(string query)
        {
            List<Convocacao> convocacao = new List<Convocacao>();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    convocacao.Add(
                    new Convocacao
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        IdEmpresa = Int32.Parse(reader["IdEmpresa"].ToString()),
                        IdColaborador = Int32.Parse(reader["IdColaborador"].ToString()),

                        Inicio = DateTime.Parse(reader["Inicio"].ToString()),
                        Termino = DateTime.Parse(reader["Termino"].ToString()),
                        EntradaHora = Int32.Parse(reader["EntradaHora"].ToString()),
                        EntradaMinuto = Int32.Parse(reader["EntradaMinuto"].ToString()),
                        SaidaHora = Int32.Parse(reader["SaidaHora"].ToString()),
                        SaidaMinuto = Int32.Parse(reader["SaidaMinuto"].ToString()),
                        Funcao = reader["Funcao"].ToString(),
                        Rua = reader["Rua"].ToString(),
                        Numero = reader["Numero"].ToString(),
                        Complemento = reader["Complemento"].ToString(),
                        Bairro = reader["Bairro"].ToString(),
                        CEP = reader["CEP"].ToString(),
                        Cidade = reader["Cidade"].ToString(),
                        Estado = reader["Estado"].ToString(),
                        ValorHora = Double.Parse(reader["ValorHora"].ToString()),
                        ConfirmarAte = DateTime.Parse(reader["ConfirmarAte"].ToString()),
                        Titulo = reader["Titulo"].ToString(),
                        Mensagem = reader["Mensagem"].ToString(),
                        Status = reader["Status"].ToString(),

                    });
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return convocacao;
        }

        public static Convocacao Convocacao(string query)
        {
            Convocacao convocacao = new Convocacao();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    convocacao =
                    new Convocacao
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        IdEmpresa = Int32.Parse(reader["IdEmpresa"].ToString()),

                        Inicio = DateTime.Parse(reader["Inicio"].ToString()),
                        Termino = DateTime.Parse(reader["Termino"].ToString()),
                        EntradaHora = Int32.Parse(reader["EntradaHora"].ToString()),
                        EntradaMinuto = Int32.Parse(reader["EntradaMinuto"].ToString()),
                        SaidaHora = Int32.Parse(reader["SaidaHora"].ToString()),
                        SaidaMinuto = Int32.Parse(reader["SaidaMinuto"].ToString()),
                        Funcao = reader["Funcao"].ToString(),
                        Rua = reader["Rua"].ToString(),
                        Numero = reader["Numero"].ToString(),
                        Complemento = reader["Complemento"].ToString(),
                        Bairro = reader["Bairro"].ToString(),
                        CEP = reader["CEP"].ToString(),
                        Cidade = reader["Cidade"].ToString(),
                        Estado = reader["Estado"].ToString(),
                        ValorHora = Double.Parse(reader["ValorHora"].ToString()),
                        ConfirmarAte = DateTime.Parse(reader["ConfirmarAte"].ToString()),
                        Titulo = reader["Titulo"].ToString(),
                        Mensagem = reader["Mensagem"].ToString(),
                        Status = reader["Status"].ToString(),

                    };
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return convocacao;
        }

        public static List<Convocacao> ListConvocacao_(string query)
        {
            List<Convocacao> convocacao = new List<Convocacao>();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    convocacao.Add(
                    new Convocacao
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        IdEmpresa = Int32.Parse(reader["IdEmpresa"].ToString()),

                        Inicio = DateTime.Parse(reader["Inicio"].ToString()),
                        Termino = DateTime.Parse(reader["Termino"].ToString()),
                        EntradaHora = Int32.Parse(reader["EntradaHora"].ToString()),
                        EntradaMinuto = Int32.Parse(reader["EntradaMinuto"].ToString()),
                        SaidaHora = Int32.Parse(reader["SaidaHora"].ToString()),
                        SaidaMinuto = Int32.Parse(reader["SaidaMinuto"].ToString()),
                        Funcao = reader["Funcao"].ToString(),
                        ValorHora = Double.Parse(reader["ValorHora"].ToString()),
                        ConfirmarAte = DateTime.Parse(reader["ConfirmarAte"].ToString()),


                    });
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return convocacao;
        }

        public static Empresa Empresa(string query)
        {
            Empresa empresa = new Empresa();



            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    empresa =
                    new Empresa
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        RazaoSocial = reader["RazaoSocial"].ToString(),
                        CNPJ = reader["CNPJ"].ToString(),
                        Telefone = reader["Telefone"].ToString(),
                        Responsavel = reader["Responsavel"].ToString(),
                        Email = reader["Email"].ToString(),

                    };
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return empresa;
        }

        public static List<Contrato> ListContrato(string query)
        {
            List<Contrato> contrato = new List<Contrato>();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    contrato.Add(
                    new Contrato
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        IdConvocacao = Int32.Parse(reader["IdConvocacao"].ToString()),
                        DataAceite = DateTime.Parse(reader["DataAceite"].ToString()),
                        Ativo = Boolean.Parse(reader["Ativo"].ToString()),
                        DataFim = DateTime.Parse(reader["DataFim"].ToString() == ""  ? DateTime.MaxValue.ToString() : reader["DataFim"].ToString()),
                    });
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return contrato;
        }

        //public static List<Convocacao> ListConvocacao_2(string query)
        //{
        //    List<Convocacao> convocacao = new List<Convocacao>();

        //    var command = new SqlCommand();
        //    string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
        //    command.Connection = new SqlConnection(connectionString);
        //    command.CommandText = query;

        //    if (command.Connection.State == System.Data.ConnectionState.Closed)
        //    {
        //        command.Connection.Open();
        //    }
        //    try
        //    {
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            convocacao.Add(
        //            new Convocacao
        //            {
        //                Id = Int32.Parse(reader["Id"].ToString()),
        //                IdEmpresa = Int32.Parse(reader["IdEmpresa"].ToString()),
        //                RazaoSocial = reader["RazaoSocial"].ToString(),
        //                Inicio = DateTime.Parse(reader["Inicio"].ToString()),
        //                Termino = DateTime.Parse(reader["Termino"].ToString()),
        //                EntradaHora = Int32.Parse(reader["EntradaHora"].ToString()),
        //                EntradaMinuto = Int32.Parse(reader["EntradaMinuto"].ToString()),
        //                SaidaHora = Int32.Parse(reader["SaidaHora"].ToString()),
        //                SaidaMinuto = Int32.Parse(reader["SaidaMinuto"].ToString()),
        //                Funcao = reader["Funcao"].ToString(),
        //                Rua = reader["Rua"].ToString(),
        //                Numero = reader["Numero"].ToString(),
        //                Complemento = reader["Complemento"].ToString(),
        //                Bairro = reader["Bairro"].ToString(),
        //                CEP = reader["CEP"].ToString(),
        //                Cidade = reader["Cidade"].ToString(),
        //                Estado = reader["Estado"].ToString(),
        //                ValorHora = Double.Parse(reader["ValorHora"].ToString()),
        //                ConfirmarAte = DateTime.Parse(reader["ConfirmarAte"].ToString()),
        //                Titulo = reader["Titulo"].ToString(),
        //                Mensagem = reader["Mensagem"].ToString(),
        //                Status = reader["Status"].ToString(),
        //                Status = reader["Nome"].ToString(),
        //            });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    finally
        //    {
        //        command.CommandText = null;
        //        command.Parameters.Clear();
        //        command.Connection.Close();
        //    }
        //    return convocacao;
        //}

        public static List<TipoContratacao> ListTipoContratacao(string query)
        {
            List<TipoContratacao> tipoContratacao = new List<TipoContratacao>();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tipoContratacao.Add(
                                        new TipoContratacao
                                        {
                                            Id = Int32.Parse(reader["Id"].ToString()),
                                            Descricao = reader["Descricao"].ToString(),

                                        });
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return tipoContratacao;
        }

        public static List<Colaborador> ComboColaborador(string query)
        {
            List<Colaborador> comboColaborador = new List<Colaborador>();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboColaborador.Add(
                                        new Colaborador
                                        {
                                           
                                            Nome = reader["Nome"].ToString(),

                                        });
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return comboColaborador;
        }

        public static List<Convocacao> ComboFuncao(string query)
        {
            List<Convocacao> funcao = new List<Convocacao>();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    funcao.Add(
                                        new Convocacao
                                        {

                                            Funcao = reader["Funcao"].ToString(),

                                        });
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return funcao;
        }

        public static List<TipoContratacao> ListaTipoContratacao(string query)
        {
            List<TipoContratacao> tipoContratacao = new List<TipoContratacao>();

            var command = new SqlCommand();
            string connectionString = ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            command.Connection = new SqlConnection(connectionString);
            command.CommandText = query;

            if (command.Connection.State == System.Data.ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tipoContratacao.Add(
                                        new TipoContratacao
                                        {
                                            Id = Int32.Parse(reader["Id"].ToString()),
                                            Descricao = reader["Descricao"].ToString(),
                                        });
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                command.CommandText = null;
                command.Parameters.Clear();
                command.Connection.Close();
            }
            return tipoContratacao;
        }

    }
}