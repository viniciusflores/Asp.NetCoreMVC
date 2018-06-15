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

            string id_usuario_logado = HttpContextAccessor.HttpContext.Session.GetString("IdUsuarioLogado");
            string sql = "select t.id, t.data,t.tipo,t.valor,t.descricao as historico, t.conta_id,c.nome as conta, " +
                        " t.plano_contas_id, p.descricao as plano_conta from transacao as t inner join conta as c " +
                        " on t.conta_id = c.id inner join plano_contas as p on t.plano_contas_id = p.id " +
                        $"where t.usuario_id = {id_usuario_logado} ORDER BY t.data DESC LIMIT 10";

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

        public void Insert()
        {
            
        }

        public TransacaoModel CarregarRegistro(int? id)
        {
            TransacaoModel item = new TransacaoModel();
            return item;
        }
    }
}
