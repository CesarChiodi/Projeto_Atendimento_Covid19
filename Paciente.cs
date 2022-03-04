using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Atendimento_Covid19
{
    internal class Paciente
    {
        public string Nome { get; set; }
        public string CPF { get; set;}
        public string Idade { get; set; }
        public string DataNascimento { get; set;}
        public string Senha { get; set; }
        public int Comorbidade { get; set; }
        public string Internacao { get; set; }
        public string TipoComorbidade { get; set; }
        public int Sintomas { get; set; }
        public string TipoSintoma { get; set; }
        public float Temperatura { get; set; }
        public float Saturacao { get; set; }
        public int DiaSintomas { get; set; }
        public string  TipoExame { get; set; }
        public string ResultadoExame { get; set; }
        //public string emergencia { get; set; }
        public Paciente Proximo { get; set; }

        public Paciente(string nome, string cPF, string idade, string senha, string dataNasc)
        {
            Nome = nome;
            CPF = cPF;
            Idade = idade;
            Senha = senha;
            DataNascimento = dataNasc;
        }

        public override string ToString()
        {
            return $"Sintomas: {Sintomas}\nTipo de sintoma: {TipoSintoma}\nDia de sintomas: {DiaSintomas}\nComorbidade: {Comorbidade}\nTipo de Comorbidade: {TipoComorbidade}\nTemperatura: {Temperatura}\nSaturacao: {Saturacao}\nIdade: {Idade}" ;
        }

    }

}
