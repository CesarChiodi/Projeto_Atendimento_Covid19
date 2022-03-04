using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Atendimento_Covid19
{
    internal class Preferencial
    {
        public Paciente Head { get; set; }
        public Paciente Tail { get; set; }

        public Preferencial()
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

        public void PushPrefer(Paciente espera)
        {
            if (Vazia())
            {
                Head = espera;
                Tail = espera;
            }
            else
            {
                Tail.Proximo = espera;
                Tail = espera;
            }
        }

        public Paciente PopPrefer()
        {
            Paciente aux;

            if (Vazia())
            {
                aux = null;
            }
            else
            {
                aux = Head;
                Head = Head.Proximo;

            }
            if(Head == null)
            {
                Head = Tail = null;
            }
            return aux;
        }

        public int ContadorPrefer()
        {
            Paciente paciente = Head;
            int cont = 0;
            if (Vazia())
            {
                return 0;
            }
            else
            {
                do
                {
                    cont++;
                    paciente = paciente.Proximo;

                } while (paciente != null);

                return cont;
            }
        }
    }
}
