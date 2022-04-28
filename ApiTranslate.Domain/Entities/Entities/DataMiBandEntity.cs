using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTranslate.Domain.Entities.Entities
{

    public class DataMiBandEntity //corpo final dos elementos da miband
    {
            public int v { get; set; }
            public int goal { get; set; }
            public string tz { get; set; }
            public string sn { get; set; }
            public int byteLength { get; set; }
            public int sync { get; set; }
    }
 }




//O valor PAI foi introduzido por Huami em seus dispositivos Amazfit e 
//    basicamente podemos defini-lo como um indicador pessoal de atividade fisiológica. 
//    Em outras palavras, é um valor que informa o quão saudável você está em termos de 
//    atividade física ou se precisa melhorar
