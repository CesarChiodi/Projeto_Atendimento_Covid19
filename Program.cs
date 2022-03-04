using System;

namespace Projeto_Atendimento_Covid19
{
    internal class Program
    {
        public static int MenuPrincipal()
        {
            bool flag = true;
            string escolha;
            int opcaoEntradaMenu = 0;   

            do
            {
                //Console.WriteLine($"\t LEITOS DISPONIVEIS: {}");
                Console.WriteLine("\t_________________________________");
                Console.WriteLine("\t|++++++++++++| MENU |+++++++++++|");
                Console.WriteLine("\t|0| - ENCERRAR A APLICACAO      |");
                Console.WriteLine("\t|1| - GERAR NOVA SENHA          |");
                Console.WriteLine("\t|2| - CHAMAR PROXIMA SENHA      |");
                Console.WriteLine("\t|3| - EXECUTAR TESTAGEM         |");
                Console.WriteLine("\t|4| - SETOR DE INTERNACAO       |");
                Console.WriteLine("\t|5| - PACIENTE EM EMERGENCIA    |");
                Console.WriteLine("\t|6| - RECUPERAR ARQUIVO         |");
                Console.WriteLine("\t|_______________________________|");
                escolha = Console.ReadLine();
                int.TryParse(escolha, out opcaoEntradaMenu);
                if(opcaoEntradaMenu < 0 || opcaoEntradaMenu > 6)
                {
                    Console.WriteLine("OPCAO INVALIDA, PRESSIONE QUALQUER TECLA PARA TENTAR NOVAMENTE");
                }  
                else 
                    flag = false;

            } while (flag);

            return opcaoEntradaMenu;
        }

        public static int CamasDisponiveis()
        {
            
            int camasDisponiveis;
            Console.WriteLine("INFORME A QUANTIDADE DE LEITOS DISPONIVEIS");
            camasDisponiveis = int.Parse(Console.ReadLine());
            Console.Clear();
            return camasDisponiveis;
        }

        static void Main(string[] args)
        {
            Controle controleTotal = null;
            bool flag = true;
           
            int opcaoMenu, camas = 0;
            camas = CamasDisponiveis();
            Console.WriteLine($"A QUANTIDADE DE LEITOS DISPONIVEIS É: {camas}");
            controleTotal = new Controle(camas);
            opcaoMenu = MenuPrincipal();

            do
            {
                switch (opcaoMenu)
                {
                    case 0:
                        
                        flag = false;
                        break;

                    case 1:
                        Console.Clear();

                        controleTotal.GerarSenha();
                        Console.Clear();
                        opcaoMenu = MenuPrincipal();
                        break;

                    case 2:
                        Console.Clear();

                        controleTotal.ChamarSenha();
                        Console.Clear();
                        opcaoMenu = MenuPrincipal();
                        break;

                    case 3:
                        Console.Clear();

                        controleTotal.Testagem();
                        Console.Clear();
                        opcaoMenu = MenuPrincipal();
                        break;

                    case 4:
                        Console.Clear();

                        controleTotal.SetorInternacao();
                        Console.Clear();
                        opcaoMenu = MenuPrincipal();
                        break;

                    case 5:
                        
                        Console.Clear();
                        controleTotal.Emergencia();
                        Console.Clear();
                        opcaoMenu = MenuPrincipal();
                        break;

                    case 6:

                        Console.Clear();
                        controleTotal.UploadData();
                        Console.Clear();
                        opcaoMenu = MenuPrincipal();
                        break;

                    default:
                        Console.WriteLine("OPCAO INVALIDA, PRESSIONE QUALQUER TECLA PARA TENTAR NOVAMENTE");
                        opcaoMenu = MenuPrincipal();
                        break;
                }
                

            } while (flag);
            Console.WriteLine("PROGRAMA ENCERRADO");

        }
    }
}
