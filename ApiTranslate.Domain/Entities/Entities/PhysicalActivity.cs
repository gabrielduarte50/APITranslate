using System;
using System.Collections.Generic;
using System.Text;

namespace ApiTranslate.Domain.Entities.Entities
{
    class PhysicalActivity
    {



    }
}
        //A estrutura aqui é um JSON com o seguinte corpo:
//        {
//          "resourceType" : "[name]",
//          // from Resource: id, meta, implicitRules, and language
//          "text" : { Narrative
//                                }, // Text summary of the resource, for human interpretation
//          "contained" : [{ Resource }], // Contained, inline Resources
//        }

//       // o CONTAINED possuirá os outros elementos que compoe a nova estrutura
//       // é do tipo Resource:
//       {
//          "resourceType" : "[name]",
//          "id" : "<string>", // Logical id of this artifact
//          "meta" : { Meta }, // Metadata about the resource
//          "implicitRules" : "<uri>", // A set of rules under which this content was created
//          "language" : "<code>" // Language of the resource content
//        }
//// nesse caso, META tem o formato:
//        { 
//        "versionId" : "<id>", // Version specific identifier
//          "lastUpdated" : "<instant>", // When the resource version last changed
//          "source" : "<uri>", // Identifies where the resource comes from
//          "profile" : [{ canonical(StructureDefinition) }], // Profiles this resource claims to conform to
//          "security" : [{ Coding }], // Security Labels applied to this resource
//          "tag" : [{ Coding }] // Tags applied to this resource
//        }

//    } 

// esse codigo tem um exemplo de construcao
// https://github.com/SALUD-AR/Open-RSD/blob/8312f85874ea4787962e81c68fa9c5b9e486c17c/Msn.InteropDemo.Fhir.Implementacion/ImmunizationManager.cs
// https://github.com/dhes/cqf-exercises/blob/b7c7a682a9469695147db21aadad24f6ee27d917/input/resources/Patient-example.json
// esse de cima tem o JSON do Resource Patient, portanto é ter essa base de estrutura
//https://nrces.in/ndhm/fhir/r4/index.html
//procurar como faz para usar um guide pronto - talvez tem o physical activity
//http://www.healthintersections.com.au/?p=2487
//esse uktimo usa so o observation para enviar os dados
//}
