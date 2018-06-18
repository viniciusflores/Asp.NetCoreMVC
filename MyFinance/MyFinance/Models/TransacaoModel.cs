using Microsoft.AspNetCore.Http;
using MyFinance.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Models
{
    public class TransacaoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe a data")]
        public string Data { get; set; }

        public string DataFinal { get; set; }//utilizada para filtros

        public string Tipo { get; set; }
        public double Valor { get; set; }
        [Required(ErrorMessage = "Informe a descrição")]
        public string Descricao { get; set; }
        public int Conta_Id { get; set; }
        public int Plano_Contas_Id { get; set; }
        public int Usuario_Id { get; set; }
        public string NomeConta { get; set; }
        public string DescricaoPlanoConta { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public TransacaoModel()
        {
        }

        public TransacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<TransacaoModel> ListaTransacao()
        {
            List<TransacaoModel> lista = new List<TransacaoModel>();
            TransacaoModel item;

            //utlizado pela view extrato
            string filtro = "";
            if((Data != null) && (DataFinal != null))
            {
                filtro += $" and t.Data >='{DateTime.Parse(Data).ToString("yyyy/MM/dd")}' ans t.data <= '{DateTime.Parse(Data).ToString("yyyy/MM/dd")}'";
            }

            if (Tipo != null)
            {
                if(Tipo != "A")
                {
                    filtro += $" and t.tipo = '{Tipo}'";
                }
            }

            if (Conta_Id != 0)
            {
                filtro += $" and t.Conta_Id = '{Conta_Id}'";
            }
            
            //FIM


            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "select t.id, t.data,t.tipo,t.valor,t.descricao as historico, t.conta_id,c.nome as conta, " +
                        " t.plano_contas_id, p.descricao as plano_conta from transacao as t inner join conta as c " +
                        " on t.conta_id = c.id inner join plano_contas as p on t.plano_contas_id = p.id " +
                        $"where t.usuario_id = {id_usuario_logado} {filtro} ORDER BY t.data DESC LIMIT 10";

            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new TransacaoModel();
                item.Id = int.Parse(dt.Rows[i]["ID"].ToString());
                item.Data = DateTime.Parse(dt.Rows[i]["DATA"].ToString()).ToString("dd/MM/yyyy");
                item.Tipo = dt.Rows[i]["TIPO"].ToString();
                item.Valor = double.Parse(dt.Rows[i]["VALOR"].ToString());
                item.Descricao = dt.Rows[i]["HISTORICO"].ToString();
                item.Conta_Id = int.Parse(dt.Rows[i]["CONTA_ID"].ToString());
                item.NomeConta = dt.Rows[i]["CONTA"].ToString();
                item.Plano_Contas_Id = int.Parse(dt.Rows[i]["PLANO_CONTAS_ID"].ToString());
                item.DescricaoPlanoConta = dt.Rows[i]["PLANO_CONTA"].ToString();

                lista.Add(item);
            }

            return lista;
        }
        
        public TransacaoModel CarregarRegistro(int? id)
        {
            TransacaoModel item;
            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "select t.id, t.data,t.tipo,t.valor,t.descricao as historico, t.conta_id,c.nome as conta, " +
                        " t.plano_contas_id, p.descricao as plano_conta from transacao as t inner join conta as c " +
                        " on t.conta_id = c.id inner join plano_contas as p on t.plano_contas_id = p.id " +
                        $"where t.usuario_id = {id_usuario_logado} and t.id='{id}'";

            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            item = new TransacaoModel();
            item.Id = int.Parse(dt.Rows[0]["ID"].ToString());
            item.Data = DateTime.Parse(dt.Rows[0]["DATA"].ToString()).ToString("dd/MM/yyyy");
            item.Tipo = dt.Rows[0]["TIPO"].ToString();
            item.Valor = double.Parse(dt.Rows[0]["VALOR"].ToString());
            item.Descricao = dt.Rows[0]["HISTORICO"].ToString();
            item.Conta_Id = int.Parse(dt.Rows[0]["CONTA_ID"].ToString());
            item.NomeConta = dt.Rows[0]["CONTA"].ToString();
            item.Plano_Contas_Id = int.Parse(dt.Rows[0]["PLANO_CONTAS_ID"].ToString());
            item.DescricaoPlanoConta = dt.Rows[0]["PLANO_CONTA"].ToString();

            return item;
        }

        public void Insert()
        {
            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "";

            if (Id == 0)
            {
                sql = $"INSERT INTO TRANSACAO (DATA, TIPO, DESCRICAO, VALOR, CONTA_ID, PLANO_CONTAS_ID, USUARIO_ID) " +
                    $"VALUES ('{DateTime.Parse(Data).ToString("yyyy/MM/dd")}', '{Tipo}','{Descricao}', '{Valor}'," +
                    $"'{Conta_Id}','{Plano_Contas_Id}','{id_usuario_logado}')";

            }
            else
            {
                sql = $"UPDATE TRANSACAO SET DATA={DateTime.Parse(Data).ToString("yyyy/MM/dd")}', " +
                    $" TIPO='{Tipo}', " +
                    $" DESCRICAO ='{Descricao}'" +
                    $" VALOR = {Valor}" +
                    $" CONTA_ID = {Conta_Id}" +
                    $" PLANO_CONTAS_ID = {Plano_Contas_Id}" +
                    $" WHERE USUARIO_ID='{id_usuario_logado}' AND ID = '{Id}'";

            }
            DAL objDAL = new DAL();
            objDAL.ExecutarComandoSQL(sql);
        }

        public void Excluir(int id)
        {
            new DAL().ExecutarComandoSQL("DELETE FROM TRANSACAO WHERE ID = " + id);


        }

        
    }

    public class Dashboard
    {
        public double Total { get; set; }
        public string Plano_conta { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public Dashboard()
        {
        }

        public Dashboard(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public List<Dashboard> RetornarDadosGraficoPie()
        {
            List<Dashboard> lista = new List<Dashboard>();
            Dashboard item;
            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = $"SELECT sum(t.Valor) as Total, p.descricao FROM transacao as t " +
                $"inner join plano_contas as p on t.plano_contas_id = p.id " +
                $"where t.tipo = 'D' and t.usuario_id={id_usuario_logado} group by p.descricao";
            DAL objDAL = new DAL();
            DataTable dt = new DataTable();
            dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new Dashboard();
                item.Total = double.Parse(dt.Rows[i]["Total"].ToString());
                item.Plano_conta = dt.Rows[i]["Descricao"].ToString();
                lista.Add(item);
            }
            return lista;

        }
    }
}
