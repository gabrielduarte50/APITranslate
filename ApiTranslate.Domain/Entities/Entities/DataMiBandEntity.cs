using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTranslate.Domain.Entities.Entities
{
    public class DataMiBandEntity //corpo final dos elementos da miband - vincular cada elemento da miBnad e do sportData
    {
        public int totalSteps { get; set; }
        public DateTime date_time { get; set; }
        public string walk_distance { get; set; }
        public string walk_speed { get; set; }
        public int rhr { get; set; }
        public string light_sleep { get; set; }
        public string deep_sleep { get; set; }
        public string rem_sleep { get; set; }
        public int totalPai { get; set; }
        public int totalCal { get; set; } //field with cal and runCal -> understand de field Stage and yoursElements

    }
}




//O valor PAI foi introduzido por Huami em seus dispositivos Amazfit e 
//    basicamente podemos defini-lo como um indicador pessoal de atividade fisiológica. 
//    Em outras palavras, é um valor que informa o quão saudável você está em termos de 
//    atividade física ou se precisa melhorar
