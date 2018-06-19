using System;
using Xunit;
using MyFinance.Models;

namespace ProjetoTestes
{
    public class UnitTest1
    {
        [Fact]
        public void TestLoginUsuario()
        {
            UsuarioModel usuarioModel = new UsuarioModel();
            usuarioModel.Email = "viniciusflores379@gmail.com";
            usuarioModel.Senha = "123";
            bool result = usuarioModel.ValidarLogin();
            Assert.True(result);

            
        }

        [Fact]
        public void TestRegistrarUsuario()
        {
            UsuarioModel usuarioModel = new UsuarioModel();
            usuarioModel.Nome = "Teste";
            usuarioModel.Data_Nascimento = "1977/02/05";
            usuarioModel.Email = "teste@mail.com";
            usuarioModel.Senha = "tst";
            usuarioModel.RegistrarUsuario();
            bool result = usuarioModel.ValidarLogin();
            Assert.True(result);

        }
    }
}
