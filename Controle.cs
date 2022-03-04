using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projeto_Atendimento_Covid19
{
    internal class Controle
    {
        public int Senha { get; set; }
        public int Selecao { get; set; }
        public int Next { get; set; }
        public Internacao Internacao { get; set; }
        public Exame Exame { get; set; }
        public Preferencial Preferencial { get; set; }
        public Comum Comum { get; set; }
        public Controle(int camas)
        {
            Senha = 1;
            Selecao = 0;
            Internacao = new Internacao(camas);
            Exame = new Exame();
            Preferencial = new Preferencial();
            Comum = new Comum();
            Next = 1;
        }



        public void GerarSenha()
        {
            string nome, cPF, dataNasc, dia, mes, ano, idade, senha;
            Paciente espera;
            DateTime dta;
            string preferencial = @"C:\UBS5by5\Preferencial\", comum = @"C:\UBS5by5\Comum\";

            Senha++;

            Console.WriteLine("INFORME O NOME DO PACIENTE");
            nome = Console.ReadLine();

            Console.WriteLine("INFROME O CPF DO PACIENTE");
            cPF = Console.ReadLine();

            Console.WriteLine("INFORME A DATA DE NASCIMENTO DO PACIENTE DIA MES E ANO (DD/MM/YYYY)");
            Console.WriteLine("INFORME O DIA DE NASCIMENTO");
            dia = Console.ReadLine();
            Console.WriteLine("INFORME O MES DE NASCIMENTO");
            mes = Console.ReadLine();
            Console.WriteLine("INFORME O ANO DE NASCIMENTO");
            ano = Console.ReadLine();

            dta = new DateTime(int.Parse(ano), int.Parse(mes), int.Parse(dia));
            dataNasc = dta.ToString("dd/MM/yyyy"); 
            idade = Convert.ToString(Math.Floor(DateTime.Today.Subtract(dta).TotalDays / 365));

            if (int.Parse(idade) > 59)
            {
                senha = "PreF" + Senha;
                Console.WriteLine($"O PACIENTE: {nome}, DO CPF: {cPF} DEVE RECEBER SENHA PREFERENCIAL {senha}");

                espera = new Paciente(nome, cPF, idade, senha, dataNasc);

                Preferencial.PushPrefer(espera);  //para inserir em uma classe "inferior" eu utilizo um metodo da classe,, para passar dados a uma classe "superior" eu utilizo o retorno

                using (StreamWriter sw = new StreamWriter(preferencial + cPF + senha + ".txt"))
                {
                    sw.WriteLine(nome);
                    sw.WriteLine(cPF);
                    sw.WriteLine(dataNasc);
                    sw.WriteLine(idade);
                    sw.WriteLine(senha);
                }

            }
            else
            {
                senha = "Comum" + Senha;
                Console.Write($"O PACIENTE: {nome}, DO CPF: {cPF} DEVE RECEBER senha COMUM, {senha}");

                espera = new Paciente(nome, cPF, idade, senha, dataNasc);
                Comum.PushComum(espera);

                using (StreamWriter sw = new StreamWriter(comum + cPF + senha + ".txt"))
                {
                    sw.WriteLine(nome);
                    sw.WriteLine(cPF);
                    sw.WriteLine(dataNasc);
                    sw.WriteLine(idade);
                    sw.WriteLine(senha);
                }
            }
        }

        public void ChamarSenha()
        {
            bool flag = true;
            Paciente paciente = null;
            do
            {
                if (Comum.ContadorComum() == 0 && Preferencial.ContadorPrefer() == 0)
                {
                    Console.WriteLine("NAO TEM PROXIMOS NA CHAMADA");
                    flag = false;
                }
                else if (Preferencial.ContadorPrefer() > 0 && Next < 3)
                {
                    Console.WriteLine("CHAMADA PREFERENCIAL EXECUTADA");
                    paciente = Preferencial.PopPrefer();
                    Triagem(paciente);
                    DeleteData(paciente.CPF, paciente.Senha, "preferencial");
                    Next++;
                    flag = false;
                }
                else if (Comum.ContadorComum() > 0 && Next == 3)
                {
                    Console.WriteLine("CHAMADA COMUM EXECUTADA");
                    paciente = Comum.PopComum();
                    Triagem(paciente);
                    DeleteData(paciente.CPF, paciente.Senha, "comum");
                    Next = 1;
                    flag = false;
                }
                else if (Preferencial.ContadorPrefer() < 1)
                {
                    Next = 3;
                }

            } while (flag);
        }

        public void Triagem(Paciente paciente)
        {
            string respSint, respComorb, tipoSintoma, tipoComorbidade;
            int sintoma, comorbidade, diaSintomas;
            float saturacao, temperatura;

            Console.WriteLine("BEM VINDO AO CAMPO DE TRIAGEM!");

            Console.WriteLine("\nO PACIENTE POSSUI SINTOMAS DA DOENÇA? (Sim ou Nao)");
            respSint = Console.ReadLine();
            if (respSint.ToLower() == "sim")
            {
                Console.WriteLine("\nQUANTOS SINTOMAS DA DOENÇA?");
                sintoma = int.Parse(Console.ReadLine());
                Console.WriteLine("\nQUAIS SAO OS SINTOMAS DO PACIENTE?");
                tipoSintoma = Console.ReadLine();
                Console.WriteLine("\nQUANTOS DIAS SE PASSARAM APROXIMADAMENTE APOS O APARECIMENTO DOS SINTOMAS");
                diaSintomas = int.Parse(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("\nO PACIENTE PODE SER ASSINTOMATICO");
                sintoma = 0;
                diaSintomas = 0;
                tipoSintoma = null;
            }

            Console.WriteLine("\nO PACIENTE POSSUI ALGUM TIPO DE COMORBIDADAE? (Sim ou Nao)");
            respComorb = Console.ReadLine();
            if (respComorb.ToLower() == "sim")
            {
                Console.WriteLine("\nQUANTOS TIPOS DE COMORBIDADE?");
                comorbidade = int.Parse(Console.ReadLine());
                Console.WriteLine("\nQUAIS SAO AS COMORBIDADES?");
                tipoComorbidade = Console.ReadLine();
            }
            else
            {

                Console.WriteLine("O PACIENTE NAO SE ENCONTRA EM UM CASO DE POSSIVEIS AGRAVANTES");
                comorbidade = 0;
                tipoComorbidade = null;
            }

            Console.WriteLine("\nINFORME A TEMPERATURA DO PACIENTE");
            temperatura = int.Parse(Console.ReadLine());

            Console.WriteLine("\nINFORME A SATURACAO DO PACIENTE");
            saturacao = float.Parse(Console.ReadLine());

            paciente.Sintomas = sintoma;
            paciente.TipoSintoma = tipoSintoma;
            paciente.DiaSintomas = diaSintomas;
            paciente.Comorbidade = comorbidade;
            paciente.TipoComorbidade = tipoComorbidade;
            paciente.Temperatura = temperatura;
            paciente.Saturacao = saturacao;

            if (paciente.Sintomas == 0 && paciente.Comorbidade == 0 && paciente.Saturacao > 90 || paciente.Temperatura < 37)
            {

                Console.WriteLine("\nO PACIENTE NAO PODE SER TESTADO PELO HOSPITAL");
                RecordData(paciente, "historico");
            }
            else if (paciente.Sintomas >= 3 && paciente.Comorbidade >= 1 && paciente.Saturacao <= 88 && paciente.Temperatura > 36)
            {
                Internacao.PushIntern(paciente);
                RecordData(paciente, "internacao");
                Console.WriteLine("\nO PACIENTE DEVE SER DESTINADO A FILA DE INTERNACAO");
            }
            else if ((paciente.Sintomas >= 0 || paciente.Comorbidade >= 0) && (paciente.Saturacao <= 90 || paciente.Temperatura > 36))
            {
                Testagem(paciente);
                Exame.PushExame(paciente);
                RecordData(paciente, "exame");
                Console.WriteLine("\nO PACIENTE DEVE SER DESTIADO A FILA DE TESTAGEM");
            }
        }

        public void Testagem(Paciente paciente)
        {

            int escolhaTeste, resultTeste, estado;

            Console.WriteLine("BEM VINDO AO CAMPO DE TESTAGEM!");

            Console.WriteLine("EXISTEM 2 TIPOS DE TESTES DISPONIVEIS:\n1 - TESTE P.C.R.\n2 - TESTE RAPIDO (antigeno)");
            Console.WriteLine("INFORME (1 ou 2)  O TIPO DE EXAME QUE SERA REALIZADO");
            escolhaTeste = int.Parse(Console.ReadLine());

            if (escolhaTeste == 1)
            {
                Console.WriteLine($"SERA REALIZADO O TESTE P.C.R.");
                Console.WriteLine("AGUARDE O RESULTADO DO TESTE\nCASO O TESTE RETORNE [POSITIVO] - DIGITE 1\nCASO O TESTE REORNE [NEGATIVO] - DIGITE 2");
                resultTeste = int.Parse(Console.ReadLine());
                paciente.TipoExame = "TESTE P.C.R.";
                if (resultTeste == 2)
                {
                    paciente.ResultadoExame = "NEGATIVO";
                    Console.WriteLine("O PACIENTE ESTA LIVRE DO VIRUS");
                    RecordData(paciente, "historico");
                    DeleteData(paciente.CPF, paciente.Senha, "exame");

                }
                else if (resultTeste == 1)
                {
                    paciente.ResultadoExame = "POSITIVO";
                    Console.WriteLine("O PACIENTE SE ENCONTRA EM ESTADO GRAVE?");
                    Console.WriteLine("DIGITE 1 PARA CONFIRMAR ou 2 PARA NEGAR");
                    estado = int.Parse(Console.ReadLine());
                    if (estado == 1)
                    {
                        Console.WriteLine("O PACIENTE DEVE SER INTERNADO");
                        Internacao.PushIntern(paciente);
                        RecordData(paciente, "internacao");
                        DeleteData(paciente.CPF, paciente.Senha, "exame");

                        Exame.PopExame();

                    }
                    else
                    {
                        Console.WriteLine("O PACIENTE DEVE MANTER QUARENTENA E EM CASOS DE AGRAVANTES DEVE RETORNAR A U.B.S.");
                        RecordData(paciente, "Historico");
                        DeleteData(paciente.CPF, paciente.Senha, "exame");

                    }
                }
                else
                {
                    Console.WriteLine("O PACIENTE SE ENCONTRA APENAS COM SUSPEITA DA DOENCA E DEVE PROCURAR UM ESPECIALISTA RELACIONADO AO SINTOMA");
                }
            }
            else if (escolhaTeste == 2)
            {
                Console.WriteLine($"SERA REALIZADO O TESTE RAPIDO (antigeno)");
                Console.WriteLine("AGUARDE O RESULTADO DO TESTE\nCASO O TESTE RETORNE [POSITIVO] - DIGITE 1\nCASO O TESTE REORNE [NEGATIVO] - DIGITE 2");
                resultTeste = int.Parse(Console.ReadLine());
                paciente.TipoExame = "TESTE RAPIDO";

                if (resultTeste == 2)
                {
                    paciente.ResultadoExame = "NEGATIVO";
                    Console.WriteLine("O PACIENTE ESTA LIVRE DO VIRUS");
                    RecordData(paciente, "historico");
                    DeleteData(paciente.CPF, paciente.Senha, "exame");

                }
                else
                {
                    paciente.ResultadoExame = "POSITIVO";
                    Console.WriteLine("O PACIENTE SE ENCONTRA EM ESTADO GRAVE?");
                    Console.WriteLine("DIGITE 1 PARA CONFIRMAR ou 2 PARA NEGAR");
                    estado = int.Parse(Console.ReadLine());
                    if (estado == 1)
                    {
                        Console.WriteLine("O PACIENTE DEVE SER INTERNADO");
                        Internacao.PushIntern(paciente);
                        RecordData(paciente, "internacao");
                        DeleteData(paciente.CPF, paciente.Senha, "exame");

                        Exame.PopExame();
                    }
                    else
                    {
                        Console.WriteLine("O PACIENTE DEVE MANTER QUARENTENA E EM CASOS DE AGRAVANTES DEVE RETORNAR A U.B.S.");
                        RecordData(paciente, "historico");
                        DeleteData(paciente.CPF, paciente.Senha, "exame");

                    }
                }
            }
            else
            {
                Console.WriteLine("O PACIENTE SE ENCONTRA APENAS COM SUSPEITA DA DOENCA E DEVE PROCURAR UM ESPECIALISTA RELACIONADO AO SINTOMA");
            }
        }

        public void SetorInternacao()
        {
            Console.WriteLine("BEM VINDO AO CAMPO DE INTERNACAO");
            Paciente paciente = null;

            int numeroInternacao = Internacao.ContadorCama(), resposta;

            if (numeroInternacao < 1)
            {
                Console.WriteLine("NAO HEXISTEM INTERNACOES NO MOMENTO");
                Console.ReadKey();
            }
            else
            {
                paciente = Internacao.HeadCama;
                Console.WriteLine("AQUI É POSSIVEL MANIPULAR O PACIENTE: DAR ALTA OU MANTER INTERNADO");
                
                for (int i = 0; i < numeroInternacao; i++)
                {
                    Console.WriteLine(paciente.ToString());
                    Console.WriteLine("O PACIENTE DEVE RECEBER ALTA?\nSE SIM, DIGITE 1\nSE NAO, DIGITE2");
                    resposta = int.Parse(Console.ReadLine());
                   
                    if(resposta == 1)
                    {
                        Internacao.PopCama(paciente);
                        paciente.Internacao = "ALTA";
                        Console.ReadKey();
                        RecordData(paciente, "historico");
                        DeleteData(paciente.CPF, paciente.Senha, "internacao");



                        if (Internacao.ContadorPessoa() > 0)
                        {
                            paciente = Internacao.PopIntern();
                            Internacao.PushIntern(paciente);
                        }
                    }
                    paciente = paciente.Proximo;

                }

            }
        }
        public void Emergencia()
        {
            DateTime dta;
            string respSint, respComorb, tipoSintoma, tipoComorbidade, nome, cPF, dataNasc, dia, mes, ano, idade, senha;
            int sintoma, comorbidade, diaSintomas;
            float saturacao, temperatura;

            Console.WriteLine("BEM VINDO AO CAMPO EMERGENCIAL!");

            Senha++;

            Console.WriteLine("O PACIENTE SERA ALOCADO NO SETOR DE INTERNACAO");

            Console.WriteLine("INFORME O NOME DO PACIENTE");
            nome = Console.ReadLine();
            Console.WriteLine("INFROME O CPF DO PACIENTE");
            cPF = Console.ReadLine();
            senha = "Emeg" + Senha;
            Paciente paciente = new Paciente(nome, cPF, "", senha, "");
            Internacao.PushIntern(paciente);

            Console.WriteLine("O PACIENTE JA FOI ENCAMINHADO PARA A INTERNACAO\nPRESSIONE [ENTER] PARA PROSSEGUIR COM O CADASTRO");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine($"CPF: {cPF}");
            Console.WriteLine($"NOME: {nome}");

            Console.WriteLine("INFORME O DIA DE NASCIMENTO");
            dia = Console.ReadLine();
            Console.WriteLine("INFORME O MES DE NASCIMENTO");
            mes = Console.ReadLine();
            Console.WriteLine("INFORME O ANO DE NASCIMENTO");
            ano = Console.ReadLine();

            dta = new DateTime(int.Parse(ano), int.Parse(mes), int.Parse(dia));
            dataNasc = dta.ToString("dd/MM/yyyy");
            idade = Convert.ToString(Math.Floor(DateTime.Today.Subtract(dta).TotalDays / 365));



            Console.WriteLine("\nO PACIENTE POSSUI SINTOMAS DA DOENÇA? (Sim ou Nao)");
            respSint = Console.ReadLine();
            if (respSint.ToLower() == "sim")
            {
                Console.WriteLine("\nQUANTOS SINTOMAS DA DOENÇA?");
                sintoma = int.Parse(Console.ReadLine());
                Console.WriteLine("\nQUAIS SAO OS SINTOMAS DO PACIENTE?");
                tipoSintoma = Console.ReadLine();
                Console.WriteLine("\nQUANTOS DIAS SE PASSARAM APROXIMADAMENTE APOS O APARECIMENTO DOS SINTOMAS");
                diaSintomas = int.Parse(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("\nO PACIENTE PODE SER ASSINTOMATICO");
                sintoma = 0;
                diaSintomas = 0;
                tipoSintoma = null;
            }

            Console.WriteLine("\nO PACIENTE POSSUI ALGUM TIPO DE COMORBIDADAE? (Sim ou Nao)");
            respComorb = Console.ReadLine();
            if (respComorb.ToLower() == "sim")
            {
                Console.WriteLine("\nQUANTOS TIPOS DE COMORBIDADE?");
                comorbidade = int.Parse(Console.ReadLine());
                Console.WriteLine("\nQUAIS SAO AS COMORBIDADES?");
                tipoComorbidade = Console.ReadLine();
            }
            else
            {

                Console.WriteLine("O PACIENTE NAO SE ENCONTRA EM UM CASO DE POSSIVEIS AGRAVANTES");
                comorbidade = 0;
                tipoComorbidade = null;
            }

            Console.WriteLine("\nINFORME A TEMPERATURA DO PACIENTE");
            temperatura = int.Parse(Console.ReadLine());

            Console.WriteLine("\nINFORME A SATURACAO DO PACIENTE");
            saturacao = float.Parse(Console.ReadLine());

            paciente.Sintomas = sintoma;
            paciente.TipoSintoma = tipoSintoma;
            paciente.DiaSintomas = diaSintomas;
            paciente.Comorbidade = comorbidade;
            paciente.TipoComorbidade = tipoComorbidade;
            paciente.Temperatura = temperatura;
            paciente.Saturacao = saturacao;
            paciente.Idade = idade;

            RecordData(paciente, "internacao");
        }

        public void UploadData()
        {
            string exame = @"C:\UBS5by5\Exame\", internacao = @"C:\UBS5by5\Internacao\", historico = @"C:\UBS5by5\Historico\";
            string preferencial = @"C:\UBS5by5\Preferencial\", comum = @"C:\UBS5by5\Comum\";
            Paciente paciente = null;
            string[] data = new string[16];
            int i = -1;
            Console.WriteLine("RECUPERACAO DE ARQUIVOS");

            foreach (string file in Directory.GetFiles(preferencial))
            {
                i = -1;
                using (StreamReader sr = new StreamReader(file))
                {
                    do
                    {
                        i++;
                        data[i] = sr.ReadLine();
                    } while (data[i] != null);
                }
                paciente = new Paciente("","","","","");
                paciente.Nome = data[0];
                paciente.CPF = data[1];
                paciente.DataNascimento = data[2];
                paciente.Idade = data[3];
                paciente.Senha = data[4];
                Preferencial.PushPrefer(paciente);
            }

            foreach (string file in Directory.GetFiles(comum))
            {
                i = -1;
                using (StreamReader sr = new StreamReader(file))
                {
                    do
                    {
                        i++;
                        data[i] = sr.ReadLine();
                    } while (data[i] != null);
                }
                paciente = new Paciente("", "", "", "", "");
                paciente.Nome = data[0];
                paciente.CPF = data[1];
                paciente.DataNascimento = data[2];
                paciente.Idade = data[3];
                paciente.Senha = data[4];
                Preferencial.PushPrefer(paciente);
                Comum.PushComum(paciente);
            }

            foreach (string file in Directory.GetFiles(exame))
            {
                i = -1;
                using (StreamReader sr = new StreamReader(file))
                {
                    do
                    {
                        i++;
                        data[i] = sr.ReadLine();
                    } while (data[i] != null);
                }
                paciente = new Paciente("", "", "", "", "");
                paciente.Nome = data[0];
                paciente.CPF = data[1];
                paciente.DataNascimento = data[2];
                paciente.Idade = data[3];
                paciente.Senha = data[4];
                paciente.Sintomas = Convert.ToInt32(data[5]);
                paciente.TipoSintoma = data[6];
                paciente.DiaSintomas = Convert.ToInt32(data[7]);
                paciente.Comorbidade = Convert.ToInt32(data[8]);
                paciente.TipoComorbidade = data[9];
                paciente.Temperatura = float.Parse(data[10]);
                paciente.Saturacao = float.Parse(data[11]);
                Exame.PushExame(paciente);
            }

            foreach (string file in Directory.GetFiles(internacao))
            {
                i = -1;
                using (StreamReader sr = new StreamReader(file))
                {
                    do
                    {
                        i++;
                        data[i] = sr.ReadLine();
                    } while (data[i] != null);
                }
                paciente = new Paciente("", "", "", "", "");
                paciente.Nome = data[0];
                paciente.CPF = data[1];
                paciente.DataNascimento = data[2];
                paciente.Idade = data[3];
                paciente.Senha = data[4];
                paciente.Sintomas = Convert.ToInt32(data[5]);
                paciente.TipoSintoma = data[6];
                paciente.DiaSintomas = Convert.ToInt32(data[7]);
                paciente.Comorbidade = Convert.ToInt32(data[8]);
                paciente.TipoComorbidade = data[9];
                paciente.Temperatura = float.Parse(data[10]);
                paciente.Saturacao = float.Parse(data[11]);
                Internacao.PushIntern(paciente);
            }
            Console.WriteLine("\nerro de gravacao dos dados do paciente.");
            Console.WriteLine("\nPressione ENTER para voltar ao menu");
            Console.ReadKey();
        }

        public void DeleteData(string cpf, string pass, string sector)
        {
            string exame = @"C:\UBS5by5\Exame\", internacao = @"C:\UBS5by5\Internacao\", historico = @"C:\UBS5by5\Historico\";
            string preferencial = @"C:\UBS5by5\Preferencial\", comum = @"C:\UBS5by5\Comum\";
            string data = "";
            if (sector == "exame")
                data = exame + pass + cpf + ".txt";
            if (sector == "hosp")
                data = internacao + pass + cpf + ".txt";
            if (sector == "preferencial")
                data = preferencial + pass + cpf + ".txt";
            if (sector == "comum")
                data = comum + pass + cpf + ".txt";
            File.Delete(data);
        }

        public void RecordData(Paciente pessoa, string sector)
        {
            string exame = @"C:\UBS5by5\Exame\", internacao = @"C:\UBS5by5\Internacao\", historico = @"C:\UBS5by5\Historico\";
            string preferencial = @"C:\UBS5by5\Preferencial\", comum = @"C:\UBS5by5\Comum\";
            if (sector == "historico")
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(historico + pessoa.Senha + pessoa.CPF + ".txt"))
                    {
                        sw.WriteLine(pessoa.Nome);
                        sw.WriteLine(pessoa.CPF);
                        sw.WriteLine(pessoa.DataNascimento);
                        sw.WriteLine(pessoa.Idade);
                        sw.WriteLine(pessoa.Senha);
                        sw.WriteLine(pessoa.Temperatura);
                        sw.WriteLine(pessoa.Saturacao);
                        sw.WriteLine(pessoa.Comorbidade);
                        sw.WriteLine(pessoa.Sintomas);
                        sw.WriteLine(pessoa.DiaSintomas);
                        sw.WriteLine(pessoa.TipoExame);
                        sw.WriteLine(pessoa.ResultadoExame);
                        sw.WriteLine(pessoa.Internacao);
                    }
                }
                catch
                {
                    Console.WriteLine("\nerro de gravacao dos dados do paciente.");
                    Console.WriteLine("\nPressione ENTER para voltar ao menu");
                    Console.ReadKey();
                }
            }
            else if (sector == "internacao")
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(internacao + pessoa.Senha + pessoa.CPF + ".txt"))
                    {
                        sw.WriteLine(pessoa.Nome);
                        sw.WriteLine(pessoa.CPF);
                        sw.WriteLine(pessoa.DataNascimento);
                        sw.WriteLine(pessoa.Idade);
                        sw.WriteLine(pessoa.Senha);
                        sw.WriteLine(pessoa.Temperatura);
                        sw.WriteLine(pessoa.Saturacao);
                        sw.WriteLine(pessoa.Comorbidade);
                        sw.WriteLine(pessoa.Sintomas);
                        sw.WriteLine(pessoa.DiaSintomas);
                        sw.WriteLine(pessoa.TipoExame);
                        sw.WriteLine(pessoa.ResultadoExame);
                        sw.WriteLine(pessoa.Internacao);
                    }
                }
                catch
                {
                    Console.WriteLine("\nerro de gravacao dos dados do paciente.");
                    Console.WriteLine("\nPressione ENTER para voltar ao menu");
                    Console.ReadKey();
                }
            }
            else if (sector == "exame")
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(exame + pessoa.Senha + pessoa.CPF + ".txt"))
                    {
                        sw.WriteLine(pessoa.Nome);
                        sw.WriteLine(pessoa.CPF);
                        sw.WriteLine(pessoa.DataNascimento);
                        sw.WriteLine(pessoa.Idade);
                        sw.WriteLine(pessoa.Senha);
                        sw.WriteLine(pessoa.Temperatura);
                        sw.WriteLine(pessoa.Saturacao);
                        sw.WriteLine(pessoa.Comorbidade);
                        sw.WriteLine(pessoa.Sintomas);
                        sw.WriteLine(pessoa.DiaSintomas);
                        sw.WriteLine(pessoa.TipoExame);
                        sw.WriteLine(pessoa.ResultadoExame);
                        sw.WriteLine(pessoa.Internacao);
                    }
                }
                catch
                {
                    Console.WriteLine("\nerro de gravacao dos dados do paciente.");
                    Console.WriteLine("\nPressione ENTER para voltar ao menu");
                    Console.ReadKey();
                }
            }
        }
    }
}
