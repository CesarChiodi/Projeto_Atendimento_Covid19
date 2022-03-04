using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Atendimento_Covid19
{
    internal class Internacao
    {
        public int Camas { get; set; }
        public Paciente HeadCama { get; set; }
        public Paciente TailCama { get; set; }
        public Paciente Head { get; set; }
        public Paciente Tail { get; set; }

        public Internacao(int camas)
        {
            Camas = camas;
            Head = null;
            Tail = null;
            HeadCama = null;
            TailCama = null;

        }
        public bool Vazia()
        {
            if ((Head == null) && (Tail == null))
            {
                return true;
            }
            return false;
        }
        //
        public bool VaziaCama()
        {
            if ((HeadCama == null) && (TailCama == null))
            {
                return true;
            }
            return false;
        }


        public void PushIntern(Paciente paciente)
        {
            if (Camas > 0)
            {
                PushCama(paciente);
            }
            else if (Vazia())
            {
                Head = paciente;
                Tail = paciente;
                Camas--;
            }
            else
            {
                Tail.Proximo = paciente;
                Tail = paciente;
                Camas--;
            }
        }
        //
        public void PushCama(Paciente paciente)
        {

            if (VaziaCama())
            {
                HeadCama = paciente;
                TailCama = paciente;
            }
            else
            {
                TailCama.Proximo = paciente;
                TailCama = paciente;
            }
        }


        public Paciente PopIntern()
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
            if (Head == null)
            {
                Head = Tail = null;
            }
            return aux;
        }
        //
        public Paciente PopCama(Paciente paciente)
        {
            Paciente auxCama;

            if (VaziaCama())
            {
                auxCama = null;
            }
            else
            {
                auxCama = HeadCama;
                HeadCama = HeadCama.Proximo;

            }
            if (HeadCama == null)
            {
                HeadCama = TailCama = null;
            }
            return auxCama;
        }

        public int ContadorCama()
        {
            Paciente paciente = HeadCama;
            int cont = 0;
            if (VaziaCama())
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

        public int ContadorPessoa()
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
