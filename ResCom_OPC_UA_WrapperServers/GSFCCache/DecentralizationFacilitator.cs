using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSFCCache
{
    public class DecentralizationFacilitator
    {
        private Dictionary<string, ResourceDetails> dictRegisteredResources;

        public DecentralizationFacilitator()
        {
            dictRegisteredResources = new Dictionary<string, ResourceDetails>();
        }

        public bool AddResource(ResourceDetails resourceDetails)
        {
            if (!resourceDetails.IsOnline)
            {
                KeyValuePair<string, ResourceDetails> Resource = dictRegisteredResources.
                    Where(resource => resource.Value.ResourceUrl.Equals(resourceDetails.ResourceUrl)).
                    FirstOrDefault();

                if (!Resource.Equals(default(KeyValuePair<string, ResourceDetails>)))
                {
                    Resource.Value.IsOnline = false;
                }
                return false;
            }

            if (!dictRegisteredResources.ContainsKey(resourceDetails.Name))
            {
                dictRegisteredResources.Add(resourceDetails.Name, resourceDetails);
                return true;
            }
            else
            {
                dictRegisteredResources[resourceDetails.Name] = resourceDetails;
            }
            return false;
        }

        public List<string> GetRegisteredResourceUrls(string Capability)
        {
            return dictRegisteredResources.Values.
                Where(resource => resource.Capability.Equals(Capability)).
                Select(resources => resources.ResourceUrl).ToList();
        }

        public ResourceDetails ResourceDetermination(string Capability)
        {

            return dictRegisteredResources.Values.
                     Where(resource => ((resource.Capability.Equals(Capability)) && (resource.State != ResourceState.OnHold)))
                    .OrderBy(resource => resource.TrustworthinessFactor)
                    .FirstOrDefault();
        }

        public bool SetResourceDetails(string ResourceUrl, ResourceState resourceState)
        {
            ResourceDetails resourceDetails = this.dictRegisteredResources.Values.
                Where(resource => resource.ResourceUrl.Equals(ResourceUrl)).
                FirstOrDefault();

            if (resourceDetails != null)
            {
                resourceDetails.State = resourceState;
                return true;
            }
            return false;
        }

        private bool RefreshResourceDetails()
        {
            foreach (KeyValuePair<string, ResourceDetails> Resource in dictRegisteredResources)
            {

            }
            return true;
        }
    }
}
