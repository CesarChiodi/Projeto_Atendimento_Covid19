using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Atendimento_Covid19
{
    internal class Exame
    {
        public Paciente Head { get; set; }
        public Paciente Tail { get; set; }

        public Exame()
        {
            Head = null;
            Tail = null;
        }

        public bool Vazia()
        {
            if ((Head == null) && (Tail == null))
            {
                return true;
            }
            return false;
        }

        public void PushExame(Paciente paciente)
        {
            if (Vazia())
            {
                Head = paciente;
                Tail = paciente;
            }
            else
            {
                Tail.Proximo = paciente;
                Tail = paciente;
            }
        }

        public Paciente PopExame()
        {
            Paciente aux = null;

            if (Vazia())
            {
                aux = null;
            }
            else
            {
                aux = Head;
                Head = Head.Proximo;

            }
            if (Head == null)
            {
                Head = Tail = null;
            }
            return aux;
        }
    }
}
