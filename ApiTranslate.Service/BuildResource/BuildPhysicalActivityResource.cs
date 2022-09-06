using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;

namespace ApiTranslate.Service.BuildResource
{
    public class BuildPhysicalActivityResource //tentar aplicar, talvez, o desing pattern Builder
    {
        public BuildPhysicalActivityResource(){}

        public string BuildDomainResource()
        {
            // Method intentionally left empty.
            return "B";
        }

        private Identifier BuildIdentifier()
        {
            var resourceIdentifier = new Identifier
            {
                Use = Hl7.Fhir.Model.Identifier.IdentifierUse.Usual,
                System = "teste",
                Value = "teste"
            };

            return resourceIdentifier;
        }

        private Observation BuildObservation()
        {
            var resourceObs = new Observation
            {
                
            };
            
            return resourceObs;
        }

        private Quantity BuildQuantity()
        {
            var resourceQuantity = new Quantity
            {

            };

            return resourceQuantity;
        }
        private ResourceReference BuildReference()
        {
            var resourceObs = new ResourceReference
            {

            };

            return resourceObs;
        }

        private string BuildBackboneElement()
        {
            
            return "A";
        }


    }
}
