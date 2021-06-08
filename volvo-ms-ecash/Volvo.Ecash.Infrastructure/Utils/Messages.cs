using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Application.Utils
{
    public static class ErrorMessage
    {
        public const string MSG001 = "Campo {0} está inválido(a).";
        public const string MSG002 = "Dados para {0} não estão preenchidos";
        public const string MSG003 = "Campo {0} deve ser informado.";
        public const string MSG004 = "Acesso Negado!";
        public const string MSG005 = "Token Inválido!";
        public const string MSG006 = "{0} não localizado";
        public const string MSG007 = "{0} não foi inserido(a)";
        public const string MSG008 = "{0} não foi atualizado(a)";
        public const string MSG009 = "O valor para {0} deve esta entre {1} a {2}";
        public const string MSG012 = "Já existe registro com as mesmas informações";
        public const string MSG013 = "Houve um erro inesperado, favor tentar novamente!";
        public const string MSG014 = "O campo {0} não é correspondente ao {1} informado!";
        public const string MSG015 = "Já existe um(a) {0} cadastrado com este {1]!";
        public const string MSG016 = "Foto com tamanho {0} bytes e tamanho máxmimo é de {1} bytes!";
        public const string MSG017 = "Arquivo não permitido. Somente {0} são permitidos!";
        public const string MSG018 = "{0} já possui cadastrado!";
        public const string MSG019 = "{0} não foi excluido(a)!";
        public const string MSG020 = "{0} não tem período de vigência válido.";
        public const string MSG021 = "{0} inativo";
        public const string MSG022 = "Usuário ou senha inválido(s)";
        public const string MSG023 = "Usuário não cadastrado no sistema. Favor solicitar acesso ao administrador.";
        public const string MSG024 = "Senha incorreta.";
    }

    public static class SuccessMessage
    {
        public const string MSG001 = "Registro(s) salvo(s) com sucesso.";
        public const string MSG002 = "Cadastro realizado com sucesso!";
        public const string MSG003 = "Alteração realizada com sucesso!";
        public const string MSG004 = "Exclusão realizada com sucesso!";
        public const string MSG005 = "{0} foi dasativado!";
        public const string MSG006 = "Acesso autorizado";

    }
}
