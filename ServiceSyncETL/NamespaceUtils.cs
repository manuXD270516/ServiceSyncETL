using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSyncETL
{
    public class NamespaceUtils
    {

        private const string StringToRemoveClass = ".NamespaceUtils";
        public string GetNamespace1()
        {
            return ToString().Replace(StringToRemoveClass, string.Empty);
        }

        public string GetNamespace2()
        {
            var type = typeof(NamespaceUtils);
            return type.Namespace;
        }

        public string GetNamespace3()
        {
            var type = GetType();
            return type.Namespace;
        }
    }
}
