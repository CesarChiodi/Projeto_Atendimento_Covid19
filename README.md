# Projeto_Atendimento_Covid19
Utilize informaaçoes ficticeas porem proximo ao real na hora de executar o programa
Se o paciente estiver com as condicoes de saude estaveis ele nao poderá receber exame por exempo: paciente.Sintomas == 0 && paciente.Comorbidade == 0 && paciente.Saturacao > 90 || paciente.Temperatura < 37
se o paciente apresentar condicoes possiveis para covid ele sera destinado a testagem por exemplo: paciente.Sintomas >= 0 || paciente.Comorbidade >= 0) && (paciente.Saturacao <= 90 || paciente.Temperatura > 36
Se o paciente estiver com as condicoes de saude instaveis por exemplo: paciente.Sintomas >= 3 && paciente.Comorbidade >= 1 && paciente.Saturacao <= 88 && paciente.Temperatura > 36
